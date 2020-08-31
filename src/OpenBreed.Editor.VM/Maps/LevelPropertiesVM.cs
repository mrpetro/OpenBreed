using OpenBreed.Common.Maps;
using OpenBreed.Common.Model.Maps;
using OpenBreed.Editor.VM.Base;
using System;
using System.Linq;

namespace OpenBreed.Editor.VM.Maps
{
    public class LevelPropertiesVM : BaseViewModel
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

        public LevelPropertiesVM(MapVM map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            Map = map;

            Map.PropertyChanged += Map_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public MapVM Map { get; private set; }
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
            //XBLK = map.Properties.XBLK;
            //YBLK = map.Properties.YBLK;
            //XOFC = map.Properties.XOFC;
            //YOFC = map.Properties.YOFC;
            //XOFM = map.Properties.XOFM;
            //YOFM = map.Properties.YOFM;
            //XOFA = map.Properties.XOFA;
            //YOFA = map.Properties.YOFA;
            //XOFC = map.Properties.XOFC;
            //YOFC = map.Properties.YOFC;
            //IFFP = map.Properties.IFFP;
            //ALTP = map.Properties.ALTP;
            //ALTM = map.Properties.ALTM;
            //CCCI = map.Properties.CCCI?.ToArray();
            //CCIN = map.Properties.CCIN?.ToArray();
            //CSIN = map.Properties.CSIN?.ToArray();
        }

        #endregion Internal Methods

        #region Private Methods

        private void Map_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Map.Title):
                    Title = "Map properties - " + Map.Title;
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}