using OpenBreed.Common.Interface;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Components
{
    public interface IDataGridComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        /// <summary>
        /// Width of data grid
        /// </summary>
        int Width { get; set; }
        /// <summary>
        /// Height of data grid
        /// </summary>
        int Height { get; set; }

        #endregion Public Properties
    }

    public class DataGridComponent : IEntityComponent
    {
        #region Public Constructors

        public DataGridComponent(DataGridComponentBuilder builder)
        {
            Grid = builder.Grid;
        }

        #endregion Public Constructors

        #region Public Properties

        public IDataGrid<int> Grid { get; }

        #endregion Public Properties
    }

    public class DataGridComponentBuilder : IBuilder<DataGridComponent>
    {
        #region Private Fields

        private readonly IDataGridFactory dataGridFactory;

        #endregion Private Fields

        #region Internal Constructors

        internal DataGridComponentBuilder(IDataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IDataGrid<int> Grid { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public DataGridComponent Build()
        {
            return new DataGridComponent(this);
        }

        public DataGridComponentBuilder SetGrid(int width, int height)
        {
            Grid = dataGridFactory.Create<int>(width, height);
            return this;
        }

        #endregion Public Methods
    }
}
