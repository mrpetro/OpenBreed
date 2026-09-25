using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using System;
using OpenBreed.Common.Interface.Logging;
using Microsoft.Extensions.Logging;

namespace OpenBreed.Physics.Generic.Managers
{
    internal class CollisionMan<TObject> : ICollisionMan<TObject>
    {
        #region Private Fields

        private readonly Dictionary<string, int> aliases = new Dictionary<string, int>();

        private readonly Dictionary<(int, int), List<FixtureContactCallback<TObject>>> fixtureCallbacks = new Dictionary<(int, int), List<FixtureContactCallback<TObject>>>();
        private readonly IdMap<string> groupIds = new IdMap<string>();
        private readonly Dictionary<string, int> groupNames = new Dictionary<string, int>();
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public CollisionMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<string> GroupNames => groupNames.Keys;

        #endregion Public Properties

        #region Public Methods

        public int GetByName(string name)
        {
            int result;
            aliases.TryGetValue(name, out result);
            return result;
        }

        public int GetGroupId(string groupName)
        {
            if (!groupNames.TryGetValue(groupName, out int groupId))
                throw new InvalidOperationException($"Group name '{groupName}' not registered.");

            return groupId;
        }

        public void RegisterFixturePair(int fixtureA, int fixtureB, FixtureContactCallback<TObject> callback)
        {
            var abKey = (fixtureA, fixtureB);
            var baKey = (fixtureB, fixtureA);

            if (!fixtureCallbacks.TryGetValue(abKey, out var fixtureCallBacksAb))
            {
                fixtureCallBacksAb = new List<FixtureContactCallback<TObject>>();
                fixtureCallbacks.Add(abKey, fixtureCallBacksAb);
            }

            if (!fixtureCallBacksAb.Contains(callback))
            {
                fixtureCallBacksAb.Add(callback);
            }

            if (!fixtureCallbacks.TryGetValue(baKey, out var fixtureCallBacksBa))
            {
                fixtureCallBacksBa = new List<FixtureContactCallback<TObject>>();
                fixtureCallbacks.Add(baKey, fixtureCallBacksBa);
            }

            if (!fixtureCallBacksBa.Contains(callback))
            {
                fixtureCallBacksBa.Add(callback);
            }
        }

        public int RegisterGroup(string groupName)
        {
            if (groupNames.ContainsKey(groupName))
            {
                throw new InvalidOperationException($"Group name '{groupName}' already registered.");
            }

            var groupId = groupIds.Add(groupName);
            groupNames.Add(groupName, groupId);
            return groupId;
        }

        public void Resolve(TObject objA, TObject objB, float dt, List<Interface.Managers.CollisionContact> contacts)
        {
            foreach (var contact in contacts)
            {
                foreach (var groupIdA in contact.FixtureA.GroupIds)
                {
                    foreach (var groupIdB in contact.FixtureB.GroupIds)
                    {
                        if (fixtureCallbacks.TryGetValue((groupIdA, groupIdB), out var callbacks))
                        {
                            foreach (var callback in callbacks)
                            {
                                callback.Invoke(contact.FixtureA, objA, contact.FixtureB, objB, dt, contact.Projection);
                            }
                        }
                        else if (fixtureCallbacks.TryGetValue((groupIdB, groupIdA), out callbacks))
                        {
                            foreach (var callback in callbacks)
                            {
                                callback.Invoke(contact.FixtureB, objB, contact.FixtureA, objA, dt, contact.Projection);
                            }
                        }
                    }
                }
            }
        }

        #endregion Public Methods
    }
}