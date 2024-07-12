using OpenTK.Mathematics;

namespace OpenBreed.Core
{
    internal class DefaultDataGrid<TObject> : IDataGrid<TObject>
    {
        #region Private Fields

        private TObject[] datas;

        #endregion Private Fields

        #region Public Constructors

        public DefaultDataGrid(int width, int height)
        {
            Width = width;
            Height = height;

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
            var dataIndex = pos.X + Width * pos.Y;
            return datas[dataIndex];
        }

        public void Set(Vector2i pos, TObject data)
        {
            var dataIndex = pos.X + Width * pos.Y;
            datas[dataIndex] = data;
        }

        #endregion Public Methods
    }
}