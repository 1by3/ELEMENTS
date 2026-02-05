using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public enum AnchorPosition
    {
        Below,
        Above,
        Left,
        Right
    }

    public class ContextMenu<T> : Group<T> where T : ContextMenu<T>
    {
        private readonly ElementPortal portal;
        private readonly VisualElement backdrop;
        private readonly List<Action> onCloseActions = new();
        private bool openBound;
        private bool ignoreNextPointerDown;

        public ContextMenu(ElementPortal portal, params IElement[] children) : base(children)
        {
            this.portal = portal;
            RenderInPortal = portal;
            VisualElement.AddToClassList("elements-context-menu");
            VisualElement.style.flexDirection = FlexDirection.Column;

            backdrop = new VisualElement();
            backdrop.AddToClassList("elements-context-menu-backdrop");
            backdrop.RegisterCallback<PointerDownEvent>(_ =>
            {
                if (ignoreNextPointerDown)
                {
                    ignoreNextPointerDown = false;
                    return;
                }
                Close();
            });
        }

        public T Open(bool open = true)
        {
            if (!open)
            {
                Close();
            }
            else if (!openBound)
            {
                // Ignore the next pointer down to prevent the opening click from immediately closing
                ignoreNextPointerDown = true;
                backdrop.AddToClassList("open");
                ClassName("open");
            }

            return (T)this;
        }

        public T Close()
        {
            if (!HasClassName("open")) return (T)this;

            if (!openBound)
            {
                backdrop.RemoveFromClassList("open");
                ClassName("open", false);
            }

            foreach (var action in onCloseActions) action.Invoke();
            return (T)this;
        }

        public bool GetOpen()
        {
            return HasClassName("open");
        }

        public T BindOpen(Observable<bool> open)
        {
            openBound = true;

            Disposables.Add(open.Subscribe(nv =>
            {
                ClassName("open", nv);
                backdrop.EnableInClassList("open", nv);
                if (nv) Open();
                else Close();
            }));

            return (T)this;
        }

        public T OnClose(Action onClose)
        {
            onCloseActions.Add(onClose);
            return (T)this;
        }

        public T Position(float x, float y)
        {
            VisualElement.style.left = x;
            VisualElement.style.top = y;
            return (T)this;
        }

        public T Position(Vector2 position)
        {
            return Position(position.x, position.y);
        }

        /// <summary>
        /// Opens the menu at the given screen position (e.g., Input.mousePosition).
        /// Handles coordinate conversion from screen space to panel space.
        /// </summary>
        public T OpenAtScreenPosition(Vector2 screenPosition)
        {
            var panelPosition = portal.ScreenToPanel(screenPosition);
            // Correct Y coordinate - ScreenToPanel doesn't fully convert for UI Toolkit's coordinate system
            // where Y=0 is at the top, not the bottom
            var portalBounds = portal.GetPortalBounds();
            panelPosition.y = portalBounds.height - panelPosition.y;
            Position(panelPosition);
            BuildVisualElement();
            Open();
            return (T)this;
        }

        /// <summary>
        /// Opens the menu at the current mouse cursor position.
        /// </summary>
        public T OpenAtCursor()
        {
            return OpenAtScreenPosition(Input.mousePosition);
        }

        public T AnchorTo(VisualElement anchor, AnchorPosition anchorPosition = AnchorPosition.Below)
        {
            VisualElement.RegisterCallbackOnce<GeometryChangedEvent>(_ => PositionRelativeToAnchor(anchor, anchorPosition));
            VisualElement.schedule.Execute(() => PositionRelativeToAnchor(anchor, anchorPosition));
            return (T)this;
        }

        private void PositionRelativeToAnchor(VisualElement anchor, AnchorPosition anchorPosition)
        {
            var anchorBounds = anchor.worldBound;
            var portalBounds = portal.GetPortalBounds();

            float x, y;

            switch (anchorPosition)
            {
                case AnchorPosition.Below:
                    x = anchorBounds.x - portalBounds.x;
                    y = anchorBounds.yMax - portalBounds.y;
                    break;
                case AnchorPosition.Above:
                    x = anchorBounds.x - portalBounds.x;
                    y = anchorBounds.y - portalBounds.y - VisualElement.resolvedStyle.height;
                    break;
                case AnchorPosition.Right:
                    x = anchorBounds.xMax - portalBounds.x;
                    y = anchorBounds.y - portalBounds.y;
                    break;
                case AnchorPosition.Left:
                    x = anchorBounds.x - portalBounds.x - VisualElement.resolvedStyle.width;
                    y = anchorBounds.y - portalBounds.y;
                    break;
                default:
                    x = anchorBounds.x - portalBounds.x;
                    y = anchorBounds.yMax - portalBounds.y;
                    break;
            }

            // Check bounds and flip if necessary
            var menuWidth = VisualElement.resolvedStyle.width;
            var menuHeight = VisualElement.resolvedStyle.height;

            if (x + menuWidth > portalBounds.width)
            {
                x = portalBounds.width - menuWidth;
            }

            if (x < 0) x = 0;

            if (y + menuHeight > portalBounds.height)
            {
                if (anchorPosition == AnchorPosition.Below)
                {
                    y = anchorBounds.y - portalBounds.y - menuHeight;
                }
                else
                {
                    y = portalBounds.height - menuHeight;
                }
            }

            if (y < 0) y = 0;

            Position(x, y);
        }

        public override VisualElement BuildVisualElement()
        {
            // Set parent menu reference on children
            foreach (var child in Children)
            {
                if (child is IMenuChild menuChild)
                {
                    menuChild.SetParentMenu(this);
                }
            }

            // Add backdrop to portal before menu
            portal.RemoveFromPortal(backdrop);
            portal.AddToPortal(backdrop);

            return base.BuildVisualElement();
        }

        public void CloseFromChild()
        {
            Close();
        }

        public override void Dispose()
        {
            onCloseActions.Clear();
            backdrop.RemoveFromHierarchy();
            base.Dispose();
        }
    }

    public class ContextMenu : ContextMenu<ContextMenu>
    {
        public ContextMenu(ElementPortal portal, params IElement[] children) : base(portal, children)
        {
        }
    }

    public interface IMenuChild
    {
        void SetParentMenu<T>(ContextMenu<T> menu) where T : ContextMenu<T>;
    }
}
