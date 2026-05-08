using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Component = ELEMENTS.Component;

namespace ELEMENTS.Elements
{
    public class WindowManager : Component
    {
        public string PreferenceKeyPrefix { get; set; } = "ELEMENTS.Window";

        private const int CascadeStep = 24;
        private const string IndexSuffix = ".index";

        private readonly Dictionary<string, Window> windows = new();
        private readonly Dictionary<WindowStartPosition, int> spawnCountersByAnchor = new();
        private HorizontalGroup container;

        public T GetWindow<T>(string id = "Common") where T : Window, new()
        {
            var fullId = typeof(T).FullName + "_" + id;

            if (windows.TryGetValue(fullId, out var existing)) return (T)existing;

            var window = new T();
            window.Attach(this, fullId);
            windows[fullId] = window;

            Vector2? savedPos = null;
            Vector2? savedSize = null;
            int? cascadeIndex = null;

            if (!window.OptOutOfPlacementMemory && TryLoadPlacement(fullId, out var sp, out var ss))
            {
                savedPos = sp;
                savedSize = ss;
            }
            else
            {
                if (!spawnCountersByAnchor.TryGetValue(window.StartingPosition, out var i)) i = 0;
                cascadeIndex = i;
                spawnCountersByAnchor[window.StartingPosition] = i + 1;
            }

            // Always build the VisualElement here so we can wire deferred-placement callbacks.
            // (BuildVisualElement is not idempotent — calling it twice would re-run Render and leak subscriptions.)
            var ve = window.BuildVisualElement();
            container?.GetVisualElement().Add(ve);

            ApplyPlacementWhenReady(window, ve, savedPos, savedSize, cascadeIndex);

            return window;
        }

        private void ApplyPlacementWhenReady(Window window, VisualElement ve, Vector2? savedPos, Vector2? savedSize, int? cascadeIndex)
        {
            var startingSize = savedSize ?? window.StartingSize;

            if (savedPos.HasValue)
            {
                window.ApplyPlacement(savedPos.Value, startingSize);
                return;
            }

            // Anchored placement needs container bounds. Try now; otherwise wait for layout.
            var bounds = GetContainerBounds();
            if (bounds.width > 0 && bounds.height > 0)
            {
                var pos = ComputeAnchoredPosition(window.StartingPosition, bounds, startingSize, cascadeIndex.GetValueOrDefault());
                window.ApplyPlacement(pos, startingSize);
                return;
            }

            // Apply size now so the window doesn't render at the wrong dimensions, then defer position.
            window.ApplyPlacement(window.Position.Value, startingSize);

            EventCallback<GeometryChangedEvent> onGeo = null;
            onGeo = _ =>
            {
                var b = GetContainerBounds();
                if (b.width <= 0 || b.height <= 0) return;
                ve.UnregisterCallback(onGeo);
                var pos = ComputeAnchoredPosition(window.StartingPosition, b, startingSize, cascadeIndex.GetValueOrDefault());
                window.ApplyPlacement(pos, startingSize);
            };
            ve.RegisterCallback(onGeo);
        }

        private static Vector2 ComputeAnchoredPosition(WindowStartPosition anchor, Rect bounds, Vector2 size, int cascadeIndex)
        {
            var step = cascadeIndex * CascadeStep;
            switch (anchor)
            {
                case WindowStartPosition.TopLeft:
                    return new Vector2(0f + step, 0f + step);
                case WindowStartPosition.TopRight:
                    return new Vector2(bounds.width - size.x - step, 0f + step);
                case WindowStartPosition.BottomLeft:
                    return new Vector2(0f + step, bounds.height - size.y - step);
                case WindowStartPosition.BottomRight:
                    return new Vector2(bounds.width - size.x - step, bounds.height - size.y - step);
                case WindowStartPosition.Center:
                default:
                    return new Vector2((bounds.width - size.x) * 0.5f + step, (bounds.height - size.y) * 0.5f + step);
            }
        }

        internal void BringToFront(Window window)
        {
            window.GetWindowVisualElement()?.BringToFront();
        }

        internal IEnumerable<Window> Windows => windows.Values;

        internal Rect GetContainerBounds()
        {
            if (container == null) return default;
            var ve = container.GetVisualElement();
            return new Rect(0, 0, ve.resolvedStyle.width, ve.resolvedStyle.height);
        }

        internal void CloseWindow(string fullId)
        {
            if (!windows.Remove(fullId, out var window)) return;
            window.Dispose();
        }

        internal void PersistPlacement(Window window)
        {
            if (window.OptOutOfPlacementMemory) return;
            var data = new WindowPlacementData
            {
                x = window.Position.Value.x,
                y = window.Position.Value.y,
                w = window.Size.Value.x,
                h = window.Size.Value.y,
            };
            PlayerPrefs.SetString(KeyFor(window.FullId), JsonUtility.ToJson(data));
            AddToIndex(window.FullId);
            PlayerPrefs.Save();
        }

        public void ClearStoredPlacement(string fullId)
        {
            var key = KeyFor(fullId);
            if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
            RemoveFromIndex(fullId);
            PlayerPrefs.Save();
        }

        public void ClearAllStoredPlacements()
        {
            foreach (var fullId in LoadIndexData().ids)
            {
                PlayerPrefs.DeleteKey(KeyFor(fullId));
            }
            PlayerPrefs.DeleteKey(IndexKey);
            PlayerPrefs.Save();
        }

        private bool TryLoadPlacement(string fullId, out Vector2 pos, out Vector2 size)
        {
            var key = KeyFor(fullId);
            if (!PlayerPrefs.HasKey(key))
            {
                pos = default;
                size = default;
                return false;
            }
            try
            {
                var data = JsonUtility.FromJson<WindowPlacementData>(PlayerPrefs.GetString(key));
                pos = new Vector2(data.x, data.y);
                size = new Vector2(data.w, data.h);
                return true;
            }
            catch
            {
                pos = default;
                size = default;
                return false;
            }
        }

        private string KeyFor(string fullId) => $"{PreferenceKeyPrefix}.{fullId}";
        private string IndexKey => $"{PreferenceKeyPrefix}{IndexSuffix}";

        private void AddToIndex(string fullId)
        {
            var idx = LoadIndexData();
            if (idx.ids.Contains(fullId)) return;
            idx.ids.Add(fullId);
            PlayerPrefs.SetString(IndexKey, JsonUtility.ToJson(idx));
        }

        private void RemoveFromIndex(string fullId)
        {
            var idx = LoadIndexData();
            if (!idx.ids.Remove(fullId)) return;
            PlayerPrefs.SetString(IndexKey, JsonUtility.ToJson(idx));
        }

        private IndexData LoadIndexData()
        {
            if (!PlayerPrefs.HasKey(IndexKey)) return new IndexData();
            try
            {
                var data = JsonUtility.FromJson<IndexData>(PlayerPrefs.GetString(IndexKey));
                if (data == null) return new IndexData();
                if (data.ids == null) data.ids = new List<string>();
                return data;
            }
            catch
            {
                return new IndexData();
            }
        }

        protected override IElement Render()
        {
            var group = new HorizontalGroup()
                .ClassName("elements-window-manager");
            container = group;

            // Re-parent any windows that were opened before Render ran.
            // BuildVisualElement was already called for them in GetWindow, so just attach.
            var ve = container.GetVisualElement();
            foreach (var window in windows.Values)
            {
                var windowVe = window.GetWindowVisualElement();
                if (windowVe != null && windowVe.parent == null)
                {
                    ve.Add(windowVe);
                }
            }

            return group;
        }

        public override void Dispose()
        {
            foreach (var window in windows.Values)
            {
                window.Dispose();
            }
            windows.Clear();
            container = null;
            base.Dispose();
        }

        [Serializable]
        private class WindowPlacementData
        {
            public float x;
            public float y;
            public float w;
            public float h;
        }

        [Serializable]
        private class IndexData
        {
            public List<string> ids = new();
        }
    }
}
