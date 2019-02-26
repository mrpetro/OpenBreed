//using OpenBreed.Common.Maps.Builders;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OpenBreed.Common.Maps
//{
//    public interface IMapLayerModel
//    {

//        #region Public Properties

//        Type DataType { get; }
//        string Name { get; }

//        MapLayoutModel Owner { get; set; }
//        Size Size { get; }

//        #endregion Public Properties
//    }

//    public struct CellPos
//    {

//        #region Public Constructors

//        public CellPos(int x, int y)
//        {
//            X = x;
//            Y = y;
//        }

//        #endregion Public Constructors

//        #region Public Properties

//        public int X { get; private set; }
//        public int Y { get; private set; }

//        #endregion Public Properties
//    }
//    public class MapLayerModel<T> : IMapLayerModel
//    {

//        #region Public Constructors

//        public MapLayerModel(MapLayoutLayerBuilder<T> builder)
//        {
//            Name = builder.Name;
//            Size = builder.Size;
//            Cells = builder.Cells;
//        }

//        #endregion Public Constructors

//        #region Public Events

//        public event Action<CellPos> CellValueChanged;

//        #endregion Public Events

//        #region Public Properties

//        public T[] Cells { get; private set; }

//        public Type DataType { get { return typeof(T); } }
//        public string Name { get; private set; }
//        public MapLayoutModel Owner { get; set; }
//        public Size Size { get; private set; }

//        #endregion Public Properties

//        #region Public Indexers

//        public T this[CellPos pos]
//        {
//            get
//            {
//                return Cells[GetCellIndex(pos)];
//            }

//            set
//            {
//                int cellIndex = GetCellIndex(pos);

//                if (EqualityComparer<T>.Default.Equals(Cells[cellIndex], value))
//                    return;

//                Cells[cellIndex] = value;

//                if (CellValueChanged != null)
//                    CellValueChanged(pos);
//            }
//        }

//        #endregion Public Indexers

//        #region Internal Methods

//        internal int GetCellIndex(CellPos pos)
//        {
//            return pos.Y * Size.Width + pos.X;
//        }

//        #endregion Internal Methods

//    }
//}
