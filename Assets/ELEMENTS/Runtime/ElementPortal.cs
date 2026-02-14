using System;
using System.Collections.Generic;
using ELEMENTS.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace ELEMENTS
{
    [RequireComponent(typeof(UIDocument))]
    public class ElementPortal : MonoBehaviour
    {
        public static ElementPortal Current { get; private set; }

        private UIDocument uiDocument;

        // IUpdatable registrations
        private readonly HashSet<IUpdatable> updatables = new();
        private IUpdatable[] updatableSnapshot = Array.Empty<IUpdatable>();
        private bool updatableDirty;

        // IFixedUpdatable registrations
        private readonly HashSet<IFixedUpdatable> fixedUpdatables = new();
        private IFixedUpdatable[] fixedUpdatableSnapshot = Array.Empty<IFixedUpdatable>();
        private bool fixedUpdatableDirty;

        // Action<float> update callbacks
        private readonly HashSet<Action<float>> updateCallbacks = new();
        private Action<float>[] updateCallbackSnapshot = Array.Empty<Action<float>>();
        private bool updateCallbackDirty;

        // Action<float> fixed update callbacks
        private readonly HashSet<Action<float>> fixedUpdateCallbacks = new();
        private Action<float>[] fixedUpdateCallbackSnapshot = Array.Empty<Action<float>>();
        private bool fixedUpdateCallbackDirty;

        private void OnEnable()
        {
            Current = this;
            uiDocument = GetComponent<UIDocument>();
            uiDocument.rootVisualElement.pickingMode = PickingMode.Ignore;
            uiDocument.AddStyleSheet("ELEMENTS/DefaultStyles");
            uiDocument.AddStyleSheet("ELEMENTS/ExtendedStyles");
        }

        private void OnDisable()
        {
            updatables.Clear();
            updatableSnapshot = Array.Empty<IUpdatable>();
            updatableDirty = false;

            fixedUpdatables.Clear();
            fixedUpdatableSnapshot = Array.Empty<IFixedUpdatable>();
            fixedUpdatableDirty = false;

            updateCallbacks.Clear();
            updateCallbackSnapshot = Array.Empty<Action<float>>();
            updateCallbackDirty = false;

            fixedUpdateCallbacks.Clear();
            fixedUpdateCallbackSnapshot = Array.Empty<Action<float>>();
            fixedUpdateCallbackDirty = false;

            if (Current == this) Current = null;
        }

        private void Update()
        {
            if (updatables.Count == 0 && updateCallbacks.Count == 0) return;

            var dt = Time.deltaTime;

            if (updatableDirty)
            {
                updatableSnapshot = new IUpdatable[updatables.Count];
                updatables.CopyTo(updatableSnapshot);
                updatableDirty = false;
            }

            for (var i = 0; i < updatableSnapshot.Length; i++)
            {
                var u = updatableSnapshot[i];
                if (updatables.Contains(u)) u.OnUpdate(dt);
            }

            if (updateCallbackDirty)
            {
                updateCallbackSnapshot = new Action<float>[updateCallbacks.Count];
                updateCallbacks.CopyTo(updateCallbackSnapshot);
                updateCallbackDirty = false;
            }

            for (var i = 0; i < updateCallbackSnapshot.Length; i++)
            {
                var cb = updateCallbackSnapshot[i];
                if (updateCallbacks.Contains(cb)) cb(dt);
            }
        }

        private void FixedUpdate()
        {
            if (fixedUpdatables.Count == 0 && fixedUpdateCallbacks.Count == 0) return;

            var fdt = Time.fixedDeltaTime;

            if (fixedUpdatableDirty)
            {
                fixedUpdatableSnapshot = new IFixedUpdatable[fixedUpdatables.Count];
                fixedUpdatables.CopyTo(fixedUpdatableSnapshot);
                fixedUpdatableDirty = false;
            }

            for (var i = 0; i < fixedUpdatableSnapshot.Length; i++)
            {
                var u = fixedUpdatableSnapshot[i];
                if (fixedUpdatables.Contains(u)) u.OnFixedUpdate(fdt);
            }

            if (fixedUpdateCallbackDirty)
            {
                fixedUpdateCallbackSnapshot = new Action<float>[fixedUpdateCallbacks.Count];
                fixedUpdateCallbacks.CopyTo(fixedUpdateCallbackSnapshot);
                fixedUpdateCallbackDirty = false;
            }

            for (var i = 0; i < fixedUpdateCallbackSnapshot.Length; i++)
            {
                var cb = fixedUpdateCallbackSnapshot[i];
                if (fixedUpdateCallbacks.Contains(cb)) cb(fdt);
            }
        }

        public static IDisposable Register(IUpdatable updatable)
        {
            Current.updatables.Add(updatable);
            Current.updatableDirty = true;
            return new Registration(() =>
            {
                if (Current == null) return;
                Current.updatables.Remove(updatable);
                Current.updatableDirty = true;
            });
        }

        public static IDisposable Register(IFixedUpdatable fixedUpdatable)
        {
            Current.fixedUpdatables.Add(fixedUpdatable);
            Current.fixedUpdatableDirty = true;
            return new Registration(() =>
            {
                if (Current == null) return;
                Current.fixedUpdatables.Remove(fixedUpdatable);
                Current.fixedUpdatableDirty = true;
            });
        }

        public static IDisposable RegisterUpdate(Action<float> callback)
        {
            Current.updateCallbacks.Add(callback);
            Current.updateCallbackDirty = true;
            return new Registration(() =>
            {
                if (Current == null) return;
                Current.updateCallbacks.Remove(callback);
                Current.updateCallbackDirty = true;
            });
        }

        public static IDisposable RegisterFixedUpdate(Action<float> callback)
        {
            Current.fixedUpdateCallbacks.Add(callback);
            Current.fixedUpdateCallbackDirty = true;
            return new Registration(() =>
            {
                if (Current == null) return;
                Current.fixedUpdateCallbacks.Remove(callback);
                Current.fixedUpdateCallbackDirty = true;
            });
        }

        public void AddToPortal(VisualElement element)
        {
            uiDocument.rootVisualElement.Add(element);
        }

        public void RemoveFromPortal(VisualElement element)
        {
            if (!uiDocument.rootVisualElement.Contains(element)) return;
            uiDocument.rootVisualElement.Remove(element);
        }

        public Rect GetPortalBounds()
        {
            return uiDocument.rootVisualElement.worldBound;
        }

        public Vector2 ScreenToPanel(Vector2 screenPosition)
        {
            var panel = uiDocument.rootVisualElement.panel;
            return RuntimePanelUtils.ScreenToPanel(panel, screenPosition);
        }

        private sealed class Registration : IDisposable
        {
            private Action onDispose;

            public Registration(Action onDispose)
            {
                this.onDispose = onDispose;
            }

            public void Dispose()
            {
                onDispose?.Invoke();
                onDispose = null;
            }
        }
    }
}
