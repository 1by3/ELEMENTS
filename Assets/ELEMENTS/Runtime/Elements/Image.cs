using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ELEMENTS.Elements
{
    public class Image<T> : BaseElement<T> where T : Image<T>
    {
        private CancellationTokenSource cts;

        public Image(Texture2D image)
        {
            VisualElement = new UnityEngine.UIElements.Image
            {
                image = image
            };
        }

        public Image(string imagePath)
        {
            var styleSheet = Resources.Load<Texture2D>(imagePath);
            if (styleSheet == null) throw new Exception($"StyleSheet not found at path: {imagePath}");
            VisualElement = new UnityEngine.UIElements.Image { image = styleSheet };
        }

        public Image(Task<Texture2D> textureTask)
        {
            VisualElement = new UnityEngine.UIElements.Image();
            cts = new CancellationTokenSource();
            LoadImageAsync(textureTask, cts.Token);
        }

        public async void LoadImageAsync(Task<Texture2D> textureTask, CancellationToken cancellationToken = default)
        {
            try
            {
                var texture = await textureTask;
                if (cancellationToken.IsCancellationRequested) return;
                ((UnityEngine.UIElements.Image)VisualElement).image = texture;
            }
            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested) return;
                Debug.LogException(ex);
            }
        }

        public override void Dispose()
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = null;
            base.Dispose();
        }
    }

    public class Image : Image<Image>
    {
        public Image(Texture2D image) : base(image) { }
        public Image(string imagePath) : base(imagePath) { }
        public Image(Task<Texture2D> textureTask) : base(textureTask) { }
    }
}