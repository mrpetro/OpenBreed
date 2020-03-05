//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OpenBreed.Core.Systems
//{
//    class TimerSystem : WorldSystem
//    {
//        #region Private Fields

//        private readonly List<IEntity> entities = new List<IEntity>();
//        private readonly List<GroupPart> groupPartComps = new List<GroupPart>();

//        #endregion Private Fields

//        #region Public Constructors

//        public TimerSystem(ICore core) : base(core)
//        {
//            //Require<GroupPart>();
//        }

//        #endregion Public Constructors

//        #region Protected Methods

//        protected override void RegisterEntity(IEntity entity)
//        {
//            entities.Add(entity);
//            groupPartComps.Add(entity.GetComponent<GroupPart>().First());
//        }

//        protected override void UnregisterEntity(IEntity entity)
//        {
//            var index = entities.IndexOf(entity);

//            if (index < 0)
//                throw new InvalidOperationException("Entity not found in this system.");

//            entities.RemoveAt(index);
//            groupPartComps.RemoveAt(index);
//        }

//        public IEnumerable<IEntity> GetGroup(IEntity leader)
//        {
//            for (int i = 0; i < groupPartComps.Count; i++)
//            {
//                if (groupPartComps[i].EntityId == leader.Id)
//                    yield return entities[i];
//            }
//        }

//        public override bool HandleMsg(object sender, IMsg message)
//        {
//            foreach (var item in entities.Where(item => item.Id == ((IEntityMsg)message).Entity.Id))
//                item.PostMsg((IEntityMsg)message);

//            return true;
//        }

//        #endregion Protected Methods
//    }
//}
