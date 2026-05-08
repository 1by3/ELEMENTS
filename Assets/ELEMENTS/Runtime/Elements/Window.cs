using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UIElements;
using Component = ELEMENTS.Component;
using Button = ELEMENTS.Elements.Button;
using Label = ELEMENTS.Elements.Label;

namespace ELEMENTS.Elements
{
    public abstract class Window : Component
    {
        [Flags]
        private enum ResizeEdge
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 4,
            Bottom = 8
        }

        public readonly ReactiveProperty<string> Title = new("Window");
        public readonly ReactiveProperty<Vector2> Position = new(new Vector2(50, 50));
        public readonly ReactiveProperty<Vector2> Size = new(new Vector2(320, 200));
        public Vector2 MinSize { get; set; } = new(160, 80);
        public Vector2 MaxSize { get; set; } = new(float.PositiveInfinity, float.PositiveInfinity);
        public Vector2 StartingSize { get; set; } = new(320, 200);
        public WindowStartPosition StartingPosition { get; set; } = WindowStartPosition.Center;
        public bool OptOutOfPlacementMemory { get; set; }

        private const float SnapThreshold = 8f;

        private WindowManager manager;
        private string fullId;

        internal string FullId => fullId;

        private VerticalGroup root;
        private HorizontalGroup titleBar;
        private Button closeButton;

        private bool isDragging;
        private Vector2 dragOffset;

        public Window WithMinSize(float x, float y)
        {
            MinSize = new Vector2(x, y);
            return this;
        }

        public Window WithMaxSize(float x, float y)
        {
            MaxSize = new Vector2(x, y);
            return this;
        }

        public Window WithStartingSize(float x, float y)
        {
            StartingSize = new Vector2(x, y);
            return this;
        }

        public Window StartAt(WindowStartPosition pos)
        {
            StartingPosition = pos;
            return this;
        }

        public Window WithoutPlacementMemory()
        {
            OptOutOfPlacementMemory = true;
            return this;
        }

        public Window BindTitle(Observable<string> title)
        {
            Disposables.Add(title.Subscribe(t => Title.Value = t));
            return this;
        }

        internal void Attach(WindowManager windowManager, string id)
        {
            manager = windowManager;
            fullId = id;
        }

        internal void ApplyPlacement(Vector2 position, Vector2 size)
        {
            Position.Value = position;
            Size.Value = ClampSize(size);
        }

        private Vector2 ClampSize(Vector2 size)
        {
            return new Vector2(
                Mathf.Clamp(size.x, MinSize.x, MaxSize.x),
                Mathf.Clamp(size.y, MinSize.y, MaxSize.y));
        }

        public void BringToFront() => manager?.BringToFront(this);

        public void Close() => manager?.CloseWindow(fullId);

        internal VisualElement GetWindowVisualElement()
        {
            return root?.GetVisualElement();
        }

        protected override IElement Render()
        {
            HorizontalGroup leftHandle = null, rightHandle = null, topHandle = null, bottomHandle = null;
            HorizontalGroup tlHandle = null, trHandle = null, blHandle = null, brHandle = null;

            root = new VerticalGroup(
                BuildTitleBar().Ref(ref titleBar),
                new VerticalGroup(RenderContent()).ClassName("elements-window-content"),
                new HorizontalGroup().ClassName("elements-window-resize-handle elements-window-resize-l").Ref(ref leftHandle),
                new HorizontalGroup().ClassName("elements-window-resize-handle elements-window-resize-r").Ref(ref rightHandle),
                new HorizontalGroup().ClassName("elements-window-resize-handle elements-window-resize-t").Ref(ref topHandle),
                new HorizontalGroup().ClassName("elements-window-resize-handle elements-window-resize-b").Ref(ref bottomHandle),
                new HorizontalGroup().ClassName("elements-window-resize-handle elements-window-resize-tl").Ref(ref tlHandle),
                new HorizontalGroup().ClassName("elements-window-resize-handle elements-window-resize-tr").Ref(ref trHandle),
                new HorizontalGroup().ClassName("elements-window-resize-handle elements-window-resize-bl").Ref(ref blHandle),
                new HorizontalGroup().ClassName("elements-window-resize-handle elements-window-resize-br").Ref(ref brHandle)
            ).ClassName("elements-window");

            var rootVe = root.GetVisualElement();

            Disposables.Add(Position.Subscribe(p =>
            {
                rootVe.style.left = p.x;
                rootVe.style.top = p.y;
            }));
            Disposables.Add(Size.Subscribe(s =>
            {
                var clamped = ClampSize(s);
                rootVe.style.width = clamped.x;
                rootVe.style.height = clamped.y;
            }));

            rootVe.RegisterCallback<PointerDownEvent>(_ => BringToFront());

            WireDrag(titleBar.GetVisualElement(), closeButton.GetVisualElement());

            WireResize(leftHandle.GetVisualElement(), ResizeEdge.Left, "elements-window-hover-l");
            WireResize(rightHandle.GetVisualElement(), ResizeEdge.Right, "elements-window-hover-r");
            WireResize(topHandle.GetVisualElement(), ResizeEdge.Top, "elements-window-hover-t");
            WireResize(bottomHandle.GetVisualElement(), ResizeEdge.Bottom, "elements-window-hover-b");
            WireResize(tlHandle.GetVisualElement(), ResizeEdge.Top | ResizeEdge.Left, "elements-window-hover-tl");
            WireResize(trHandle.GetVisualElement(), ResizeEdge.Top | ResizeEdge.Right, "elements-window-hover-tr");
            WireResize(blHandle.GetVisualElement(), ResizeEdge.Bottom | ResizeEdge.Left, "elements-window-hover-bl");
            WireResize(brHandle.GetVisualElement(), ResizeEdge.Bottom | ResizeEdge.Right, "elements-window-hover-br");

            return root;
        }

        private HorizontalGroup BuildTitleBar()
        {
            var children = RenderTitleBarChildren();
            var withClose = new IElement[children.Length + 1];
            Array.Copy(children, withClose, children.Length);
            withClose[children.Length] =
                new Button(new Label("X"))
                    .ClassName("elements-window-close-button")
                    .OnClick(_ => Close())
                    .Ref(ref closeButton);
            return new HorizontalGroup(withClose).ClassName("elements-window-title-bar");
        }

        protected virtual IElement[] RenderTitleBarChildren()
        {
            return new IElement[]
            {
                new Label(Title).ClassName("elements-window-title")
            };
        }

        protected abstract IElement RenderContent();

        private void WireDrag(VisualElement bar, VisualElement closeBtn)
        {
            bar.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button != 0) return;
                if (closeBtn.Contains((VisualElement)evt.target)) return;

                isDragging = true;
                dragOffset = (Vector2)evt.position - Position.Value;
                bar.CapturePointer(evt.pointerId);
            });
            bar.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (!isDragging) return;
                var candidate = (Vector2)evt.position - dragOffset;
                Position.Value = SnapDrag(candidate);
            });
            bar.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (!isDragging) return;
                isDragging = false;
                bar.ReleasePointer(evt.pointerId);
                manager?.PersistPlacement(this);
            });
            bar.RegisterCallback<PointerCaptureOutEvent>(_ => isDragging = false);
        }

        private void WireResize(VisualElement handle, ResizeEdge edges, string hoverClass)
        {
            var active = false;
            var hovered = false;
            var startPos = Vector2.zero;
            var startSize = Vector2.zero;
            var startPointer = Vector2.zero;
            var rootVe = root.GetVisualElement();
            var buddies = new List<ResizeBuddy>();

            void UpdateHover()
            {
                if (hovered || active) rootVe.AddToClassList(hoverClass);
                else rootVe.RemoveFromClassList(hoverClass);
            }

            handle.RegisterCallback<PointerEnterEvent>(_ =>
            {
                hovered = true;
                UpdateHover();
            });
            handle.RegisterCallback<PointerLeaveEvent>(_ =>
            {
                hovered = false;
                UpdateHover();
            });

            handle.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button != 0) return;
                active = true;
                startPos = Position.Value;
                startSize = Size.Value;
                startPointer = (Vector2)evt.position;
                buddies.Clear();
                CollectResizeBuddies(edges, buddies);
                handle.CapturePointer(evt.pointerId);
                UpdateHover();
            });
            handle.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (!active) return;

                var delta = (Vector2)evt.position - startPointer;
                var newPos = startPos;
                var newSize = startSize;

                if ((edges & ResizeEdge.Right) != 0)
                {
                    newSize.x = Mathf.Clamp(startSize.x + delta.x, MinSize.x, MaxSize.x);
                }
                if ((edges & ResizeEdge.Bottom) != 0)
                {
                    newSize.y = Mathf.Clamp(startSize.y + delta.y, MinSize.y, MaxSize.y);
                }
                if ((edges & ResizeEdge.Left) != 0)
                {
                    var w = Mathf.Clamp(startSize.x - delta.x, MinSize.x, MaxSize.x);
                    newPos.x = startPos.x + (startSize.x - w);
                    newSize.x = w;
                }
                if ((edges & ResizeEdge.Top) != 0)
                {
                    var h = Mathf.Clamp(startSize.y - delta.y, MinSize.y, MaxSize.y);
                    newPos.y = startPos.y + (startSize.y - h);
                    newSize.y = h;
                }

                var snapped = SnapResize(newPos, newSize, edges);
                newPos = snapped.pos;
                newSize = snapped.size;

                // Clamp by buddies' MinSize so the seam can't push them below their floor.
                foreach (var b in buddies)
                {
                    ClampAxis(ref newPos.x, ref newSize.x, startPos.x, startSize.x, b.StartSize.x, b.Window.MinSize.x, b.MyXEdge, b.TheirXEdge);
                    ClampAxis(ref newPos.y, ref newSize.y, startPos.y, startSize.y, b.StartSize.y, b.Window.MinSize.y, b.MyYEdge, b.TheirYEdge);
                }

                Position.Value = newPos;
                Size.Value = newSize;

                var rightDelta = newPos.x + newSize.x - (startPos.x + startSize.x);
                var leftDelta = newPos.x - startPos.x;
                var bottomDelta = newPos.y + newSize.y - (startPos.y + startSize.y);
                var topDelta = newPos.y - startPos.y;

                foreach (var b in buddies)
                {
                    var bPos = b.StartPos;
                    var bSize = b.StartSize;

                    if (b.MyXEdge != ResizeEdge.None)
                    {
                        var myDelta = b.MyXEdge == ResizeEdge.Right ? rightDelta : leftDelta;
                        ApplyBuddyAxis(ref bPos.x, ref bSize.x, b.StartPos.x, b.StartSize.x, myDelta, b.TheirXEdge);
                    }
                    if (b.MyYEdge != ResizeEdge.None)
                    {
                        var myDelta = b.MyYEdge == ResizeEdge.Bottom ? bottomDelta : topDelta;
                        ApplyBuddyAxis(ref bPos.y, ref bSize.y, b.StartPos.y, b.StartSize.y, myDelta, b.TheirYEdge);
                    }

                    b.Window.Position.Value = bPos;
                    b.Window.Size.Value = bSize;
                }
            });
            handle.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (!active) return;
                active = false;
                handle.ReleasePointer(evt.pointerId);
                UpdateHover();
                manager?.PersistPlacement(this);
                foreach (var b in buddies) manager?.PersistPlacement(b.Window);
            });
            handle.RegisterCallback<PointerCaptureOutEvent>(_ =>
            {
                active = false;
                UpdateHover();
            });
        }

        private struct ResizeBuddy
        {
            public Window Window;
            public Vector2 StartPos;
            public Vector2 StartSize;
            // Per-axis rule: which of MY edges drives the buddy on this axis,
            // and which of THEIR edges follows. None means no rule on that axis.
            public ResizeEdge MyXEdge;
            public ResizeEdge TheirXEdge;
            public ResizeEdge MyYEdge;
            public ResizeEdge TheirYEdge;
        }

        private static void ApplyBuddyAxis(ref float bPosA, ref float bSizeA, float startPosA, float startSizeA, float myDelta, ResizeEdge theirEdge)
        {
            // Both Top and Left are "min" edges; Bottom and Right are "max" edges.
            var theirIsMin = theirEdge == ResizeEdge.Left || theirEdge == ResizeEdge.Top;
            if (theirIsMin)
            {
                bPosA = startPosA + myDelta;
                bSizeA = startSizeA - myDelta;
            }
            else
            {
                bSizeA = startSizeA + myDelta;
            }
        }

        private static void ClampAxis(ref float newPosA, ref float newSizeA, float startPosA, float startSizeA, float bStartSizeA, float bMinSize, ResizeEdge myEdge, ResizeEdge theirEdge)
        {
            if (myEdge == ResizeEdge.None) return;
            var myIsMax = myEdge == ResizeEdge.Right || myEdge == ResizeEdge.Bottom;
            var theirIsMin = theirEdge == ResizeEdge.Left || theirEdge == ResizeEdge.Top;

            var oldEdge = myIsMax ? startPosA + startSizeA : startPosA;
            var newEdge = myIsMax ? newPosA + newSizeA : newPosA;
            var bound = oldEdge + (theirIsMin ? bStartSizeA - bMinSize : bMinSize - bStartSizeA);

            if (theirIsMin)
            {
                if (newEdge > bound)
                {
                    if (myIsMax) newSizeA = bound - newPosA;
                    else { var diff = newEdge - bound; newPosA = bound; newSizeA += diff; }
                }
            }
            else
            {
                if (newEdge < bound)
                {
                    if (myIsMax) newSizeA = bound - newPosA;
                    else { var diff = bound - newEdge; newPosA = bound; newSizeA -= diff; }
                }
            }
        }

        private void CollectResizeBuddies(ResizeEdge edges, List<ResizeBuddy> result)
        {
            if (manager == null) return;

            const float touchEpsilon = 1f;
            var myPos = Position.Value;
            var mySize = Size.Value;
            var myL = myPos.x;
            var myR = myPos.x + mySize.x;
            var myT = myPos.y;
            var myB = myPos.y + mySize.y;

            foreach (var w in manager.Windows)
            {
                if (w == this) continue;
                var p = w.Position.Value;
                var s = w.Size.Value;
                var oL = p.x;
                var oR = p.x + s.x;
                var oT = p.y;
                var oB = p.y + s.y;

                var myXEdge = ResizeEdge.None;
                var theirXEdge = ResizeEdge.None;
                var myYEdge = ResizeEdge.None;
                var theirYEdge = ResizeEdge.None;

                // X axis: find which of my resize edges connects to which of theirs.
                // Opposite-edge match (docked across the line) is preferred over same-line (column-aligned).
                if ((edges & ResizeEdge.Right) != 0)
                {
                    if (Mathf.Abs(myR - oL) < touchEpsilon) { myXEdge = ResizeEdge.Right; theirXEdge = ResizeEdge.Left; }
                    else if (Mathf.Abs(myR - oR) < touchEpsilon) { myXEdge = ResizeEdge.Right; theirXEdge = ResizeEdge.Right; }
                }
                if (myXEdge == ResizeEdge.None && (edges & ResizeEdge.Left) != 0)
                {
                    if (Mathf.Abs(myL - oR) < touchEpsilon) { myXEdge = ResizeEdge.Left; theirXEdge = ResizeEdge.Right; }
                    else if (Mathf.Abs(myL - oL) < touchEpsilon) { myXEdge = ResizeEdge.Left; theirXEdge = ResizeEdge.Left; }
                }

                if ((edges & ResizeEdge.Bottom) != 0)
                {
                    if (Mathf.Abs(myB - oT) < touchEpsilon) { myYEdge = ResizeEdge.Bottom; theirYEdge = ResizeEdge.Top; }
                    else if (Mathf.Abs(myB - oB) < touchEpsilon) { myYEdge = ResizeEdge.Bottom; theirYEdge = ResizeEdge.Bottom; }
                }
                if (myYEdge == ResizeEdge.None && (edges & ResizeEdge.Top) != 0)
                {
                    if (Mathf.Abs(myT - oB) < touchEpsilon) { myYEdge = ResizeEdge.Top; theirYEdge = ResizeEdge.Bottom; }
                    else if (Mathf.Abs(myT - oT) < touchEpsilon) { myYEdge = ResizeEdge.Top; theirYEdge = ResizeEdge.Top; }
                }

                if (myXEdge == ResizeEdge.None && myYEdge == ResizeEdge.None) continue;

                result.Add(new ResizeBuddy
                {
                    Window = w,
                    StartPos = p,
                    StartSize = s,
                    MyXEdge = myXEdge,
                    TheirXEdge = theirXEdge,
                    MyYEdge = myYEdge,
                    TheirYEdge = theirYEdge,
                });
            }
        }

        private Vector2 SnapDrag(Vector2 candidate)
        {
            if (manager == null) return candidate;
            var size = Size.Value;
            var myTop = candidate.y;
            var myBottom = candidate.y + size.y;
            var myLeft = candidate.x;
            var myRight = candidate.x + size.x;
            var dx = ComputeBestDelta(myLeft, myRight, GetXTargets(myTop, myBottom));
            var dy = ComputeBestDelta(myTop, myBottom, GetYTargets(myLeft, myRight));
            return candidate + new Vector2(dx, dy);
        }

        private (Vector2 pos, Vector2 size) SnapResize(Vector2 pos, Vector2 size, ResizeEdge edges)
        {
            if (manager == null) return (pos, size);

            var myTop = pos.y;
            var myBottom = pos.y + size.y;
            var myLeft = pos.x;
            var myRight = pos.x + size.x;

            if ((edges & ResizeEdge.Right) != 0)
            {
                var d = ComputeEdgeDelta(myRight, GetXTargets(myTop, myBottom));
                var newW = size.x + d;
                if (newW >= MinSize.x && newW <= MaxSize.x) size.x = newW;
            }
            if ((edges & ResizeEdge.Left) != 0)
            {
                var d = ComputeEdgeDelta(myLeft, GetXTargets(myTop, myBottom));
                var newW = size.x - d;
                if (newW >= MinSize.x && newW <= MaxSize.x) { pos.x += d; size.x = newW; }
            }
            if ((edges & ResizeEdge.Bottom) != 0)
            {
                var d = ComputeEdgeDelta(myBottom, GetYTargets(myLeft, myRight));
                var newH = size.y + d;
                if (newH >= MinSize.y && newH <= MaxSize.y) size.y = newH;
            }
            if ((edges & ResizeEdge.Top) != 0)
            {
                var d = ComputeEdgeDelta(myTop, GetYTargets(myLeft, myRight));
                var newH = size.y - d;
                if (newH >= MinSize.y && newH <= MaxSize.y) { pos.y += d; size.y = newH; }
            }

            return (pos, size);
        }

        private static float ComputeBestDelta(float minEdge, float maxEdge, IEnumerable<float> targets)
        {
            var bestDelta = 0f;
            var bestDist = SnapThreshold;
            foreach (var t in targets)
            {
                var dMin = t - minEdge;
                if (Mathf.Abs(dMin) < bestDist) { bestDist = Mathf.Abs(dMin); bestDelta = dMin; }
                var dMax = t - maxEdge;
                if (Mathf.Abs(dMax) < bestDist) { bestDist = Mathf.Abs(dMax); bestDelta = dMax; }
            }
            return bestDelta;
        }

        private static float ComputeEdgeDelta(float edge, IEnumerable<float> targets)
        {
            var bestDelta = 0f;
            var bestDist = SnapThreshold;
            foreach (var t in targets)
            {
                var d = t - edge;
                if (Mathf.Abs(d) < bestDist) { bestDist = Mathf.Abs(d); bestDelta = d; }
            }
            return bestDelta;
        }

        private IEnumerable<float> GetXTargets(float myTop, float myBottom)
        {
            foreach (var w in manager.Windows)
            {
                if (w == this) continue;
                var p = w.Position.Value;
                var s = w.Size.Value;
                if (myBottom < p.y - SnapThreshold) continue;
                if (myTop > p.y + s.y + SnapThreshold) continue;
                yield return p.x;
                yield return p.x + s.x;
            }
            var bounds = manager.GetContainerBounds();
            if (bounds.width > 0)
            {
                yield return 0f;
                yield return bounds.width;
            }
        }

        private IEnumerable<float> GetYTargets(float myLeft, float myRight)
        {
            foreach (var w in manager.Windows)
            {
                if (w == this) continue;
                var p = w.Position.Value;
                var s = w.Size.Value;
                if (myRight < p.x - SnapThreshold) continue;
                if (myLeft > p.x + s.x + SnapThreshold) continue;
                yield return p.y;
                yield return p.y + s.y;
            }
            var bounds = manager.GetContainerBounds();
            if (bounds.height > 0)
            {
                yield return 0f;
                yield return bounds.height;
            }
        }
    }
}
