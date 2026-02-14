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
        private readonly HashSet<IUpdatable> _updatables = new();
        private IUpdatable[] _updatableSnapshot = Array.Empty<IUpdatable>();
        private bool _updatableDirty;

        // IFixedUpdatable registrations
        private readonly HashSet<IFixedUpdatable> _fixedUpdatables = new();
        private IFixedUpdatable[] _fixedUpdatableSnapshot = Array.Empty<IFixedUpdatable>();
        private bool _fixedUpdatableDirty;

        // Action<float> update callbacks
        private readonly HashSet<Action<float>> _updateCallbacks = new();
        private Action<float>[] _updateCallbackSnapshot = Array.Empty<Action<float>>();
        private bool _updateCallbackDirty;

        // Action<float> fixed update callbacks
        private readonly HashSet<Action<float>> _fixedUpdateCallbacks = new();
        private Action<float>[] _fixedUpdateCallbackSnapshot = Array.Empty<Action<float>>();
        private bool _fixedUpdateCallbackDirty;

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
            _updatables.Clear();
            _updatableSnapshot = Array.Empty<IUpdatable>();
            _updatableDirty = false;

            _fixedUpdatables.Clear();
            _fixedUpdatableSnapshot = Array.Empty<IFixedUpdatable>();
            _fixedUpdatableDirty = false;

            _updateCallbacks.Clear();
            _updateCallbackSnapshot = Array.Empty<Action<float>>();
            _updateCallbackDirty = false;

            _fixedUpdateCallbacks.Clear();
            _fixedUpdateCallbackSnapshot = Array.Empty<Action<float>>();
            _fixedUpdateCallbackDirty = false;

            if (Current == this) Current = null;
        }

        private void Update()
        {
            if (_updatables.Count == 0 && _updateCallbacks.Count == 0) return;

            var dt = Time.deltaTime;

            if (_updatableDirty)
            {
                _updatableSnapshot = new IUpdatable[_updatables.Count];
                _updatables.CopyTo(_updatableSnapshot);
                _updatableDirty = false;
            }

            for (var i = 0; i < _updatableSnapshot.Length; i++)
            {
                var u = _updatableSnapshot[i];
                if (_updatables.Contains(u)) u.OnUpdate(dt);
            }

            if (_updateCallbackDirty)
            {
                _updateCallbackSnapshot = new Action<float>[_updateCallbacks.Count];
                _updateCallbacks.CopyTo(_updateCallbackSnapshot);
                _updateCallbackDirty = false;
            }

            for (var i = 0; i < _updateCallbackSnapshot.Length; i++)
            {
                var cb = _updateCallbackSnapshot[i];
                if (_updateCallbacks.Contains(cb)) cb(dt);
            }
        }

        private void FixedUpdate()
        {
            if (_fixedUpdatables.Count == 0 && _fixedUpdateCallbacks.Count == 0) return;

            var fdt = Time.fixedDeltaTime;

            if (_fixedUpdatableDirty)
            {
                _fixedUpdatableSnapshot = new IFixedUpdatable[_fixedUpdatables.Count];
                _fixedUpdatables.CopyTo(_fixedUpdatableSnapshot);
                _fixedUpdatableDirty = false;
            }

            for (var i = 0; i < _fixedUpdatableSnapshot.Length; i++)
            {
                var u = _fixedUpdatableSnapshot[i];
                if (_fixedUpdatables.Contains(u)) u.OnFixedUpdate(fdt);
            }

            if (_fixedUpdateCallbackDirty)
            {
                _fixedUpdateCallbackSnapshot = new Action<float>[_fixedUpdateCallbacks.Count];
                _fixedUpdateCallbacks.CopyTo(_fixedUpdateCallbackSnapshot);
                _fixedUpdateCallbackDirty = false;
            }

            for (var i = 0; i < _fixedUpdateCallbackSnapshot.Length; i++)
            {
                var cb = _fixedUpdateCallbackSnapshot[i];
                if (_fixedUpdateCallbacks.Contains(cb)) cb(fdt);
            }
        }

        public static IDisposable Register(IUpdatable updatable)
        {
            Current._updatables.Add(updatable);
            Current._updatableDirty = true;
            return new Registration(() =>
            {
                if (Current == null) return;
                Current._updatables.Remove(updatable);
                Current._updatableDirty = true;
            });
        }

        public static IDisposable Register(IFixedUpdatable fixedUpdatable)
        {
            Current._fixedUpdatables.Add(fixedUpdatable);
            Current._fixedUpdatableDirty = true;
            return new Registration(() =>
            {
                if (Current == null) return;
                Current._fixedUpdatables.Remove(fixedUpdatable);
                Current._fixedUpdatableDirty = true;
            });
        }

        public static IDisposable RegisterUpdate(Action<float> callback)
        {
            Current._updateCallbacks.Add(callback);
            Current._updateCallbackDirty = true;
            return new Registration(() =>
            {
                if (Current == null) return;
                Current._updateCallbacks.Remove(callback);
                Current._updateCallbackDirty = true;
            });
        }

        public static IDisposable RegisterFixedUpdate(Action<float> callback)
        {
            Current._fixedUpdateCallbacks.Add(callback);
            Current._fixedUpdateCallbackDirty = true;
            return new Registration(() =>
            {
                if (Current == null) return;
                Current._fixedUpdateCallbacks.Remove(callback);
                Current._fixedUpdateCallbackDirty = true;
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
            private Action _onDispose;

            public Registration(Action onDispose)
            {
                _onDispose = onDispose;
            }

            public void Dispose()
            {
                _onDispose?.Invoke();
                _onDispose = null;
            }
        }
    }
}
