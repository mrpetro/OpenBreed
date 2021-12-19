using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using System;

namespace OpenBreed.Physics.Generic.Managers
{
    internal class CollisionMan<TObject> : ICollisionMan<TObject>
    {
        #region Private Fields

        private readonly Dictionary<string, int> aliases = new Dictionary<string, int>();

        private readonly Dictionary<string, int> groupNames = new Dictionary<string, int>();
        private readonly IdMap<string> groupIds = new IdMap<string>();

        private readonly ILogger logger;

        private readonly Dictionary<(int, int), FixtureContactCallback<TObject>> fixtureCallbacks = new Dictionary<(int, int), FixtureContactCallback<TObject>>();

        #endregion Private Fields

        #region Public Constructors

        public CollisionMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public int RegisterGroup(string groupName)
        {
            if (groupNames.ContainsKey(groupName))
                throw new InvalidOperationException($"Group name '{groupName}' already registered.");

            var groupId = groupIds.Add(groupName);
            groupNames.Add(groupName, groupId);
            return groupId;
        }

        public int GetGroupId(string groupName)
        {
            if (!groupNames.TryGetValue(groupName, out int groupId))
                throw new InvalidOperationException($"Group name '{groupName}' not registered.");

            return groupId;
        }

        public void RegisterFixturePair(int fixtureA, int fixtureB, FixtureContactCallback<TObject> callback)
        {
            fixtureCallbacks.Add((fixtureA, fixtureB), callback);
        }

        public int GetByName(string name)
        {
            int result;
            aliases.TryGetValue(name, out result);
            return result;
        }

        public void Resolve(TObject objA, TObject objB, List<Interface.Managers.CollisionContact> contacts)
        {
            foreach (var contact in contacts)
            {
                foreach (var groupIdA in contact.FixtureA.GroupIds)
                {
                    foreach (var groupIdB in contact.FixtureB.GroupIds)
                    {
                        if (fixtureCallbacks.TryGetValue((groupIdA, groupIdB), out FixtureContactCallback<TObject> fixtureCallback))
                            fixtureCallback.Invoke(contact.FixtureA, objA, contact.FixtureB, objB, contact.Projection);
                    }
                }
            }
        }

        #endregion Public Methods
    }
}