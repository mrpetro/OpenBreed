using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Maps.Builders
{
    public class MapBodyLayerBuilder<T>
    {
        #region Internal Fields

        internal T[] Cells = null;
        internal string Name;
        internal Size Size;

        #endregion Internal Fields

        #region Public Methods

        public static MapBodyLayerBuilder<T> NewMapBodyLayerModel()
        {
            return new MapBodyLayerBuilder<T>();
        }

        public MapBodyLayerModel<T> Build()
        {
            return new MapBodyLayerModel<T>(this);
        }

        public MapBodyLayerBuilder<T> SetCell(int index, T value)
        {
            Cells[index] = value;

            return this;
        }

        public MapBodyLayerBuilder<T> SetName(string name)
        {
            Name = name;

            return this;
        }

        public MapBodyLayerBuilder<T> SetSize(int sizeX, int sizeY)
        {
            Size = new Size(sizeX, sizeY);
            Cells = new T[sizeX * sizeY];

            return this;
        }

        #endregion Public Methods
    }
}
