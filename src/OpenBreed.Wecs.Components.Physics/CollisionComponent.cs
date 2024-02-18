using OpenBreed.Wecs.Components;
using System.Collections.Generic;
using System.Data;
using OpenTK;
using OpenTK.Mathematics;
using OpenBreed.Common.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Physics.Interface;

namespace OpenBreed.Wecs.Components.Physics
{
    public interface ICollisionComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        #endregion Public Properties
    }

    public class CollisionComponent : IEntityComponent
    {
        #region Public Constructors

        public CollisionComponent(CollisionComponentBuilder builder)
        {
            Dynamic = builder.Dynamic;
            Grid = builder.Grid;
            ContactPairs = new List<ContactPair>();
        }

        #endregion Public Constructors

        #region Public Properties

        public IBroadphaseDynamic Dynamic { get; }
        public IBroadphaseStatic Grid { get; }

        public List<ContactPair> ContactPairs { get; }

        #endregion Public Properties
    }

    public class CollisionComponentBuilder : IBuilder<CollisionComponent>
    {
        #region Private Fields

        private readonly IBroadphaseFactory broadphaseFactory;

        #endregion Private Fields

        #region Internal Constructors

        internal CollisionComponentBuilder(IBroadphaseFactory broadphaseFactory)
        {
            this.broadphaseFactory = broadphaseFactory;


        }

        public CollisionComponentBuilder SetGrid(int width, int height, int cellSize)
        {
            Grid = broadphaseFactory.CreateStatic(width, height, cellSize);
            return this;
        }

        public CollisionComponentBuilder Set()
        {
            Dynamic = broadphaseFactory.CreateDynamic();
            return this;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IBroadphaseDynamic Dynamic { get; private set; }
        internal IBroadphaseStatic Grid { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public CollisionComponent Build()
        {
            return new CollisionComponent(this);
        }

        #endregion Public Methods
    }

    public sealed class CollisionComponentFactory : ComponentFactoryBase<ICollisionComponentTemplate>
    {
        #region Private Fields

        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Public Constructors

        public CollisionComponentFactory(IBuilderFactory builderFactory)
        {
            this.builderFactory = builderFactory;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ICollisionComponentTemplate template)
        {
            var builder = builderFactory.GetBuilder<CollisionComponentBuilder>();

            builder.Set();


            return builder.Build();
        }

        #endregion Protected Methods
    }
}