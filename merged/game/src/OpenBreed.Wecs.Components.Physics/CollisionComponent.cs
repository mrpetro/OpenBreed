using OpenBreed.Wecs.Components;
using System.Collections.Generic;
using System.Data;
using OpenTK;
using OpenTK.Mathematics;
using OpenBreed.Common.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Physics.Interface;
using System.Runtime.CompilerServices;

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
            Broadphase = builder.GetBroadphase();

            Result = new BroadphaseResult()
            {
                Contacts = new List<CollisionContact>()
                //ContactPairs = new List<ContactPair>()
            };
        }

        #endregion Public Constructors

        #region Public Properties

        public IBroadphase Broadphase { get; }

        public BroadphaseResult Result;

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

        internal int Width;
        internal int Height;
        internal int CellSize;

        internal IBroadphase GetBroadphase()
        {
            return broadphaseFactory.CreateDynamic(Width, Height, CellSize);
        }

        public CollisionComponentBuilder SetStaticGrid(int width, int height, int cellSize)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            return this;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IBroadphase Dynamic { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public CollisionComponent Build()
        {
            return new CollisionComponent(this);
        }

        #endregion Public Methods
    }
}