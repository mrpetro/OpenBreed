using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using System.Linq;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapMissionPropertiesEditorVM : BaseViewModel
    {
        #region Private Fields

        private int _unkn1;
        private int _unkn2;
        private int _unkn3;
        private int _unkn4;
        private int _time;
        private int _unkn6;
        private int _unkn7;
        private int _unkn8;
        private int _exc1;
        private int _exc0;
        private int _exc2;
        private int _exc3;
        private int _m1ty;
        private int _m1he;
        private int _m1sp;
        private int _unkn16;
        private int _unkn17;
        private int _m2ty;
        private int _m2he;
        private int _m2sp;
        private int _unkn21;
        private int _unkn22;

        #endregion Private Fields

        #region Public Constructors

        public MapMissionPropertiesEditorVM()
        {
        }


        #endregion Public Constructors

        #region Public Properties

        public int UNKN1
        {
            get { return _unkn1; }
            set { SetProperty(ref _unkn1, value); }
        }

        public int UNKN2
        {
            get { return _unkn2; }
            set { SetProperty(ref _unkn2, value); }
        }

        public int UNKN3
        {
            get { return _unkn3; }
            set { SetProperty(ref _unkn3, value); }
        }

        public int UNKN4
        {
            get { return _unkn4; }
            set { SetProperty(ref _unkn4, value); }
        }

        public int TIME
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }

        public int UNKN6
        {
            get { return _unkn6; }
            set { SetProperty(ref _unkn6, value); }
        }

        public int UNKN7
        {
            get { return _unkn7; }
            set { SetProperty(ref _unkn7, value); }
        }

        public int UNKN8
        {
            get { return _unkn8; }
            set { SetProperty(ref _unkn8, value); }
        }



        public int EXC0
        {
            get { return _exc0; }
            set { SetProperty(ref _exc0, value); }
        }

        public int EXC1
        {
            get { return _exc1; }
            set { SetProperty(ref _exc1, value); }
        }

        public int EXC2
        {
            get { return _exc2; }
            set { SetProperty(ref _exc2, value); }
        }

        public int EXC3
        {
            get { return _exc3; }
            set { SetProperty(ref _exc3, value); }
        }

        public int M1TY
        {
            get { return _m1ty; }
            set { SetProperty(ref _m1ty, value); }
        }

        public int M1HE
        {
            get { return _m1he; }
            set { SetProperty(ref _m1he, value); }
        }

        public int M1SP
        {
            get { return _m1sp; }
            set { SetProperty(ref _m1sp, value); }
        }

        public int UNKN16
        {
            get { return _unkn16; }
            set { SetProperty(ref _unkn16, value); }
        }

        public int UNKN17
        {
            get { return _unkn17; }
            set { SetProperty(ref _unkn17, value); }
        }

        public int M2TY
        {
            get { return _m2ty; }
            set { SetProperty(ref _m2ty, value); }
        }

        public int M2HE
        {
            get { return _m2he; }
            set { SetProperty(ref _m2he, value); }
        }

        public int M2SP
        {
            get { return _m2sp; }
            set { SetProperty(ref _m2sp, value); }
        }

        public int UNKN21
        {
            get { return _unkn21; }
            set { SetProperty(ref _unkn21, value); }
        }

        public int UNKN22
        {
            get { return _unkn22; }
            set { SetProperty(ref _unkn22, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal void Load(MapModel map)
        {
            var missionBlock = map.Blocks.OfType<MapMissionBlock>().First(item => item.Name == "MISS");

            UNKN1 = missionBlock.UNKN1;
            UNKN2 = missionBlock.UNKN2;
            UNKN3 = missionBlock.UNKN3;
            UNKN4 = missionBlock.UNKN4;
            TIME = missionBlock.TIME;
            UNKN6 = missionBlock.UNKN6;
            UNKN7 = missionBlock.UNKN7;
            UNKN8 = missionBlock.UNKN8;
            EXC0 = missionBlock.EXC1;
            EXC1 = missionBlock.EXC2;
            EXC2 = missionBlock.EXC3;
            EXC3 = missionBlock.EXC4;
            M1TY = missionBlock.M1TY;
            M1HE = missionBlock.M1HE;
            M1SP = missionBlock.M1SP;
            UNKN16 = missionBlock.UNKN16;
            UNKN17 = missionBlock.UNKN17;
            M2TY = missionBlock.M2TY;
            M2HE = missionBlock.M2HE;
            M2SP = missionBlock.M2SP;
            UNKN21 = missionBlock.UNKN21;
            UNKN22 = missionBlock.UNKN22;
        }

        #endregion Internal Methods
    }
}