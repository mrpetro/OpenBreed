using OpenBreed.Core.Abstractions;
using OpenTK.Mathematics;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenBreed.Core
{
    internal class DefaultDataGrid<TObject> : IDataGrid<TObject>
    {
        #region Private Fields

        private readonly Func<int, int, TObject> cellInitializer;
        private TObject[] datas;

        #endregion Private Fields

        #region Public Constructors

        public DefaultDataGrid(int width, int height, Func<int, int, TObject> cellInitializer)
        {
            Width = width;
            Height = height;
            this.cellInitializer = cellInitializer ?? throw new ArgumentNullException(nameof(cellInitializer));
            datas = new TObject[Width * Height];
        }

        #endregion Public Constructors

        #region Public Properties

        public int Height { get; }
        public int Width { get; }

        #endregion Public Properties

        #region Public Methods

        public TObject Get(Vector2i pos)
        {
            var cellId = GetId(pos);
            return Get(cellId);
        }

        public TObject Get(int id)
        {
            return datas[id];
        }

        public bool IsValidId(int id)
        {
            return id >= 0 && id < datas.Length;
        }

        public bool IsValid(Vector2i pos)
        {
            if (pos.X < 0 || pos.X >= Width)
            {
                return false;
            }

            if (pos.Y < 0 || pos.Y >= Height)
            {
                return false;
            }

            return true;
        }

        public bool TryGet(Vector2i pos, out TObject value)
        {
            if (!IsValid(pos))
            {
                value = default;
                return false;
            }

            var dataIndex = GetId(pos);
            value = datas[dataIndex];
            return true;
        }

        public void Set(Vector2i pos, TObject data)
        {
            var dataIndex = GetId(pos);
            datas[dataIndex] = data;
        }

        public void SetAll(Action<TObject> cellAction)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                cellAction.Invoke(datas[i]);
            }
        }

        public void Set(Vector2i pos, Action<TObject> cellAction)
        {
            var dataIndex = GetId(pos);
            cellAction.Invoke(datas[dataIndex]);
        }

        public void ClearAll()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    datas[x + Width * y] = cellInitializer.Invoke(x,y);
                }
            }
        }

        public int GetId(Vector2i pos)
        {
            return pos.X + Width * pos.Y;
        }

        public Vector2i GetIndex(int id)
        {
            var x = id % Width;
            var y = id / Width;
            return new Vector2i(x, y);
        }

        public bool TryGetIdByOffset(int id, Vector2i offset, out int resultId)
        {
            var position = GetIndex(id) + offset;

            if (!IsValid(position))
            {
                resultId = -1;
                return false;
            }

            resultId = GetId(position);
            return true;
        }

        #endregion Public Methods
    }
}