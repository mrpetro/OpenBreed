using OpenBreed.Game.Rendering.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Rendering
{
    public class TextureMan
    {
        private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public Texture Load(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var fullPath = Path.GetFullPath(filePath);

            if (!File.Exists(fullPath))
                throw new InvalidOperationException($"File '{fullPath}' doesn't exist.");

            Texture texture = null;

            if (textures.TryGetValue(fullPath, out texture))
                return texture;

            using (var bitmap = new Bitmap(fullPath))
            {
                texture = Texture.CreateFromBitmap(bitmap);

                textures.Add(fullPath, texture);

                return texture;
            }
        }

        public void UnloadAll()
        {
            foreach (var texture in textures.Values)
                texture.Dispose();

            textures.Clear();
        }
    }
}
