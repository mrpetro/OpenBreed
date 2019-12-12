//
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OpenBreed.Core.Common.Systems
//{
//    public class StateMachineSystem : WorldSystem, IMsgListener
//    {
//        #region Private Fields

//        private readonly List<IEntity> entities = new List<IEntity>();
//        private readonly List<Animator> animatorComps = new List<Animator>();

//        #endregion Private Fields

//        #region Public Constructors

//        public StateMachineSystem(ICore core) : base(core)
//        {
//            Require<Animator>();
//        }

//        #endregion Public Constructors

//        private MsgHandler msgHandler;

//        #region Public Methods

//        public override void Initialize(World world)
//        {
//            msgHandler = new MsgHandler(this);

//            base.Initialize(world);

//            World.MessageBus.RegisterHandler(SetAnimMsg.TYPE, msgHandler);
//            World.MessageBus.RegisterHandler(PlayAnimMsg.TYPE, msgHandler);
//            World.MessageBus.RegisterHandler(PauseAnimMsg.TYPE, msgHandler);
//            World.MessageBus.RegisterHandler(StopAnimMsg.TYPE, msgHandler);
//        }

//        public void Update(float dt)
//        {
//            msgHandler.PostEnqueued();

//            for (int i = 0; i < entities.Count; i++)
//                Animate(i, dt);
//        }


//        public override bool RecieveMsg(object sender, IMsg message)
//        {
//            switch (message.Type)
//            {
//                case SetAnimMsg.TYPE:
//                    return HandleSetAnimMsg(sender, (SetAnimMsg)message);

//                case PlayAnimMsg.TYPE:
//                    return HandlePlayAnimMsg(sender, (PlayAnimMsg)message);

//                case PauseAnimMsg.TYPE:
//                    return HandlePauseAnimMsg(sender, (PauseAnimMsg)message);

//                case StopAnimMsg.TYPE:
//                    return HandleStopAnimMsg(sender, (StopAnimMsg)message);

//                default:
//                    return false;
//            }
//        }

//        #endregion Public Methods

//        #region Protected Methods

//        protected override void RegisterEntity(IEntity entity)
//        {
//            entities.Add(entity);
//            animatorComps.Add(entity.Components.OfType<Animator>().First());
//        }

//        protected override void UnregisterEntity(IEntity entity)
//        {
//            var index = entities.IndexOf(entity);

//            if (index < 0)
//                throw new InvalidOperationException("Entity not found in this system.");

//            entities.RemoveAt(index);
//            animatorComps.RemoveAt(index);
//        }

//        #endregion Protected Methods
//    }
//}
