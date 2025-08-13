using System;
using System.Collections.Generic;
using R3;
using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public class Dialog<T> : Group<T> where T : Dialog<T>
    {
        private VisualElement lastBuilt;
        private List<Action> onCloseActions = new();
        private bool openBound;

        public Dialog(params IElement[] children) : base(children)
        {
            RendersInPortal = true;
            VisualElement.AddToClassList("elements-dialog");
        }

        public T Open(bool open = true)
        {
            if (!open) Close();
            else if (!openBound) ClassName("open");
            return (T)this;
        }

        public T Close()
        {
            if (!HasClassName("open")) return (T)this;
            if (!openBound) ClassName("open", false);
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

        public override void Dispose()
        {
            onCloseActions.Clear();
            onCloseActions = null;
            base.Dispose();
        }
    }

    public class Dialog : Dialog<Dialog>
    {
        public Dialog(params IElement[] children) : base(children)
        {
        }
    }
}