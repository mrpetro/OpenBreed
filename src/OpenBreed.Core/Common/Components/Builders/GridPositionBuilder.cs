//using OpenBreed.Core.Common.Systems.Components;

//namespace OpenBreed.Core.Common.Components.Builders
//{
//    public class GridPositionBuilder : ComponentBuilder
//    {
//        #region Public Fields

//        public const string TYPE = "GridPosition";

//        #endregion Public Fields

//        #region Internal Fields

//        internal int X;
//        internal int Y;
//        internal int OffsetX;
//        internal int OffsetY;

//        #endregion Internal Fields

//        #region Private Constructors

//        private GridPositionBuilder(ICore core) : base(core)
//        {

//        }

//        #endregion Private Constructors

//        #region Public Properties

//        public override string Type { get { return TYPE; } }

//        #endregion Public Properties

//        #region Public Methods

//        public static ComponentBuilder Create(ICore core)
//        {
//            return new GridPositionBuilder(core);
//        }

//        public void SetOffset(int x, int y)
//        {
//            OffsetX = x;
//            OffsetY = y;
//        }

//        public void SetPosition(int x, int y)
//        {
//            X = x;
//            Y = y;
//        }

//        public override IEntityComponent Build()
//        {
//            return new GridPosition(this);
//        }

//        #endregion Public Methods
//    }
//}