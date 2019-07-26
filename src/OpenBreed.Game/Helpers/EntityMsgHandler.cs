//using OpenBreed.Core;
//using OpenBreed.Core.Common.Helpers;
//using OpenBreed.Core.Entities;
//using System;
//using System.Diagnostics;

//namespace OpenBreed.Game.Helpers
//{
//    internal class EntityMsgHandler : IMsgHandler
//    {
//        #region Private Fields

//        private ICore core;

//        #endregion Private Fields

//        #region Internal Constructors

//        internal EntityMsgHandler(ICore core)
//        {
//            this.core = core;
//        }

//        #endregion Internal Constructors

//        #region Public Methods

//        public bool HandleMsg(object sender, IMsg message)
//        {
//            return HandleMsg(sender, (IEntityMsg)message);
//        }

//        private bool HandleMsg(object sender, IEntityMsg message)
//        {
//            Debug.Assert(message != null);
//            Debug.Assert(message.Entity != null);
//            Debug.Assert(message.Entity.World != null);

//            message.Entity.World.Me;
//        }

//        #endregion Public Methods
//    }
//}