using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ELEMENTS.Elements
{
    public class Image<T> : BaseElement<T> where T : Image<T>
    {
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
            LoadImageAsync(textureTask);
        }

        public async void LoadImageAsync(Task<Texture2D> textureTask)
        {
            try
            {
                var texture = await textureTask;
                ((UnityEngine.UIElements.Image)VisualElement).image = texture;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    public class Image : Image<Image>
    {
        public Image(Texture2D image) : base(image) { }
        public Image(string imagePath) : base(imagePath) { }
        public Image(Task<Texture2D> textureTask) : base(textureTask) { }
    }
}