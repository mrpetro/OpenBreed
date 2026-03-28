using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Common.Builders;
using OpenBreed.Rendering.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Rendering.Common.Managers
{
    internal class PaletteMan : IPaletteMan
    {
        #region Private Fields

        private readonly List<Palette> items = new List<Palette>();
        private readonly Dictionary<string, Palette> names = new Dictionary<string, Palette>();
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public PaletteMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public IPaletteBuilder CreatePalette()
        {
            return new PaletteBuilder(this);
        }

        public IPalette GetById(int paletteId)
        {
            return InternalGetById(paletteId);
        }

        public bool Contains(string paletteName)
        {
            return names.ContainsKey(paletteName);
        }

        public string GetName(int paletteId)
        {
            //TODO: Very ineffective. Name should be part of ISpriteAtlas object.
            return names.First(pair => pair.Value == items[paletteId]).Key;
        }

        public bool TryGetByName(string paletteName, out IPalette palette)
        {
            if (names.TryGetValue(paletteName, out Palette result))
            {
                palette = result;
                return true;
            }

            palette = null;
            return false;
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Internal Methods

        internal Palette InternalGetById(int paletteId)
        {
            return items[paletteId];
        }

        internal int Register(string name, Palette palette)
        {
            items.Add(palette);
            names.Add(name, palette);

            logger.LogTrace("Palette '{0}' created with ID {1}.", name, items.Count - 1);

            return items.Count - 1;
        }

        #endregion Internal Methods
    }
}