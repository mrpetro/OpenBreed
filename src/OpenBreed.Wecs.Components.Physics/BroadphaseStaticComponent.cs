using OpenBreed.Common.Interface;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Physics
{
    public interface IBroadphaseStaticComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        /// <summary>
        /// Width of tile grid
        /// </summary>
        int Width { get; set; }
        /// <summary>
        /// Height of tile grid
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Tile grid cell size
        /// </summary>
        int CellSize { get; set; }

        #endregion Public Properties
    }

    public class BroadphaseStaticComponent : IEntityComponent
    {
        #region Public Constructors

        public BroadphaseStaticComponent(BroadphaseStaticComponentBuilder builder)
        {
            Grid = builder.Grid;
        }

        #endregion Public Constructors

        #region Public Properties

        public IBroadphaseStatic Grid { get; }

        #endregion Public Properties
    }

    public class BroadphaseStaticComponentBuilder : IBuilder<BroadphaseStaticComponent>
    {
        #region Private Fields

        private readonly IBroadphaseFactory broadphaseFactory;

        #endregion Private Fields

        #region Internal Constructors

        internal BroadphaseStaticComponentBuilder(IBroadphaseFactory broadphaseFactory)
        {
            this.broadphaseFactory = broadphaseFactory;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IBroadphaseStatic Grid { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public BroadphaseStaticComponent Build()
        {
            return new BroadphaseStaticComponent(this);
        }

        public BroadphaseStaticComponentBuilder SetGrid(int width, int height, int cellSize)
        {
            Grid = broadphaseFactory.CreateStatic(width, height, cellSize);
            return this;
        }

        #endregion Public Methods
    }

    public sealed class BroadphaseStaticComponentFactory : ComponentFactoryBase<IBroadphaseStaticComponentTemplate>
    {
        #region Private Fields

        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Public Constructors

        public BroadphaseStaticComponentFactory(IBuilderFactory builderFactory)
        {
            this.builderFactory = builderFactory;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IBroadphaseStaticComponentTemplate template)
        {
            var builder = builderFactory.GetBuilder<BroadphaseStaticComponentBuilder>();

            builder.SetGrid(
                template.Width,
                template.Height,
                template.CellSize);

            return builder.Build();
        }

        #endregion Protected Methods
    }
}