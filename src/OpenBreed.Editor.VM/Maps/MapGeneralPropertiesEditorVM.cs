using OpenBreed.Model.Maps;
using OpenBreed.Editor.VM.Base;
using System;
using System.Linq;
using OpenBreed.Model.Maps.Blocks;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapGeneralPropertiesEditorVM : BaseViewModel
    {

        #region Private Fields

        private string _altm;
        private string _altp;
        private byte[] _ccci;
        private byte[] _ccin;
        private byte[] _csin;
        private string _iffp;
        private string _title;
        private int _xblk;
        private int _xofa;
        private int _xofc;
        private int _xofm;
        private int _yblk;
        private int _yofa;
        private int _yofc;
        private int _yofm;

        #endregion Private Fields

        #region Public Constructors

        public MapGeneralPropertiesEditorVM(MapEditorVM parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            Parent = parent;

            Parent.PropertyChanged += Map_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public MapEditorVM Parent { get; private set; }
        public byte[] Header { get; set; }

        public string ALTM
        {
            get { return _altm; }
            set { SetProperty(ref _altm, value); }
        }

        public string ALTP
        {
            get { return _altp; }
            set { SetProperty(ref _altp, value); }
        }

        public byte[] CCCI
        {
            get { return _ccci; }
            set { SetProperty(ref _ccci, value); }
        }

        public byte[] CCIN
        {
            get { return _ccin; }
            set { SetProperty(ref _ccin, value); }
        }

        public byte[] CSIN
        {
            get { return _csin; }
            set { SetProperty(ref _csin, value); }
        }

        public string IFFP
        {
            get { return _iffp; }
            set { SetProperty(ref _iffp, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public int XBLK
        {
            get { return _xblk; }
            set { SetProperty(ref _xblk, value); }
        }

        public int XOFA
        {
            get { return _xofa; }
            set { SetProperty(ref _xofa, value); }
        }

        public int XOFC
        {
            get { return _xofc; }
            set { SetProperty(ref _xofc, value); }
        }

        public int XOFM
        {
            get { return _xofm; }
            set { SetProperty(ref _xofm, value); }
        }

        public int YBLK
        {
            get { return _yblk; }
            set { SetProperty(ref _yblk, value); }
        }

        public int YOFA
        {
            get { return _yofa; }
            set { SetProperty(ref _yofa, value); }
        }

        public int YOFC
        {
            get { return _yofc; }
            set { SetProperty(ref _yofc, value); }
        }

        public int YOFM
        {
            get { return _yofm; }
            set { SetProperty(ref _yofm, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal void Load(MapModel map)
        {
            XBLK = (int)map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == nameof(XBLK)).Value;
            YBLK = (int)map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == nameof(YBLK)).Value;
            XOFC = (int)map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == nameof(XOFC)).Value;
            YOFC = (int)map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == nameof(YOFC)).Value;
            XOFM = (int)map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == nameof(XOFM)).Value;
            YOFM = (int)map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == nameof(YOFM)).Value;
            XOFA = (int)map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == nameof(XOFA)).Value;
            YOFA = (int)map.Blocks.OfType<MapUInt32Block>().First(item => item.Name == nameof(YOFA)).Value;
        }

        #endregion Internal Methods

        #region Private Methods

        private void Map_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Parent.Title):
                    Title = "Map properties - " + Parent.Title;
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}