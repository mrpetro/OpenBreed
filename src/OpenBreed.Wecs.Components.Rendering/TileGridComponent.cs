using OpenBreed.Common.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Rendering
{
    public interface ITileGridComponentTemplate : IComponentTemplate
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
        /// Tile grid layers number
        /// </summary>
        int LayersNo { get; set; }

        /// <summary>
        /// Tile grid cell size
        /// </summary>
        int CellSize { get; set; }

        #endregion Public Properties
    }

    public class TileGridComponent : IEntityComponent
    {
        #region Public Constructors

        public TileGridComponent(TileGridComponentBuilder builder)
        {
            Grid = builder.Grid;
        }

        #endregion Public Constructors

        #region Public Properties

        public ITileGrid Grid { get; }

        #endregion Public Properties
    }

    public class TileGridComponentBuilder : IBuilder<TileGridComponent>
    {
        #region Private Fields

        private readonly ITileGridFactory tileGridFactory;
        private readonly ITileMan tileMan;

        #endregion Private Fields

        #region Internal Constructors

        internal TileGridComponentBuilder(ITileMan tileMan, ITileGridFactory tileGridFactory)
        {
            this.tileMan = tileMan;
            this.tileGridFactory = tileGridFactory;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal ITileGrid Grid { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public TileGridComponent Build()
        {
            return new TileGridComponent(this);
        }

        public void SetGrid(int width, int height, int layersNo, int cellSize)
        {
            Grid = tileGridFactory.CreateGrid(width, height, layersNo, cellSize);
        }

        #endregion Public Methods
    }

    public sealed class TileGridComponentFactory : ComponentFactoryBase<ITileGridComponentTemplate>
    {
        #region Private Fields

        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Public Constructors

        public TileGridComponentFactory(IBuilderFactory builderFactory)
        {
            this.builderFactory = builderFactory;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ITileGridComponentTemplate template)
        {
            var builder = builderFactory.GetBuilder<TileGridComponentBuilder>();

            builder.SetGrid(
                template.Width,
                template.Height,
                template.LayersNo,
                template.CellSize);

            return builder.Build();
        }

        #endregion Protected Methods
    }
}