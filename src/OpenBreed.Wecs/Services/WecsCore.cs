using OpenBreed.Core.Abstractions.Managers;
using System;

namespace OpenBreed.Wecs.Services
{
    internal class WecsCore : IWecsCore
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public WecsCore(IWorldMan worldMan, IEntityMan entityMan, IComponentsMan componentsMan, IEntityClassMan entityClassMan)
        {
            Worlds = worldMan ?? throw new ArgumentNullException(nameof(worldMan));
            Entities = entityMan ?? throw new ArgumentNullException(nameof(entityMan));
            Components = componentsMan ?? throw new ArgumentNullException(nameof(componentsMan));
            Classes = entityClassMan ?? throw new ArgumentNullException(nameof(entityClassMan));
        }

        #endregion Public Constructors

        #region Public Properties

        public IWorldMan Worlds { get; }

        public IEntityMan Entities { get; }

        public IComponentsMan Components { get; }

        public IEntityClassMan Classes { get; }

        #endregion Public Properties

        #region Private Methods

        public void Update(float dt)
        {
            dt = Math.Min(1.0f / 30.0f, dt);

            Worlds.Update(dt);
            Entities.Cleanup();
        }

        #endregion Private Methods
    }
}