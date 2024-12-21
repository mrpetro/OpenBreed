using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Enums;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Abstractions.Helpers;
using OpenBreed.Gui.Builders;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Elements
{
    internal class GridPanel : Container, IGridPanel
    {
        #region Private Fields

        private readonly List<float> columns = new List<float>();

        private readonly List<float> rows = new List<float>();

        #endregion Private Fields

        #region Public Constructors

        public GridPanel(GridPanelBuilder builder) : base(builder)
        {
            ColumnDefinitions = builder.Columns;
            RowDefinitions = builder.Rows;

            Recalculate();
        }

        #endregion Public Constructors

        #region Public Properties

        public IReadOnlyList<float> Columns => columns;

        public IReadOnlyList<float> Rows => rows;

        public IReadOnlyList<IGridPartDimension> ColumnDefinitions { get; }

        public IReadOnlyList<IGridPartDimension> RowDefinitions { get; }

        #endregion Public Properties

        #region Protected Methods

        protected override void Recalculate()
        {
            SetupDimensions(ColumnDefinitions, LocalBox.Size.X, columns);
            SetupDimensions(RowDefinitions, LocalBox.Size.Y, rows);

            RecalculateChilds();

            base.Recalculate();
        }

        #endregion Protected Methods

        #region Private Methods

        private static void SetupDimensions(IReadOnlyList<IGridPartDimension> definitions, float totalSize, List<float> sizes)
        {
            sizes.Clear();

            var usedWidth = 0.0f;

            var remaining = 0;

            for (int i = 0; i < definitions.Count; i++)
            {
                var definition = definitions[i];

                if (definition.Value == -1)
                {
                    remaining++;
                    sizes.Add(0.0f);
                    continue;
                }

                if (definition.Type == DimmensionType.Size)
                {
                    sizes.Add(definition.Value);
                    usedWidth += definition.Value;
                }
            }

            if (remaining > 0)
            {
                var remainingWidth = totalSize - usedWidth;

                var columnWidth = remainingWidth / remaining;

                for (int i = 0; i < definitions.Count; i++)
                {
                    var size = definitions[i];

                    if (size.Value != -1)
                    {
                        continue;
                    }

                    sizes[i] = columnWidth;
                }
            }
        }

        private void RecalculateChilds()
        {
            foreach (var child in Childs)
            {
                RecalculateChild(child);
            }
        }

        private void RecalculateChild(IElement child)
        {
            var gridPosition = child.GetGridPosition();

            var x = Columns.Take(gridPosition.Column).Sum();
            var y = Rows.Take(gridPosition.Row).Sum();

            var sizeX = Columns.Skip(gridPosition.Column).Take(gridPosition.ColumnSpan).Sum();
            var sizeY = Rows.Skip(gridPosition.Row).Take(gridPosition.RowSpan).Sum();

            child.Position.X = x - LocalBox.HalfSize.X + sizeX / 2.0f;
            child.Position.Y = y - LocalBox.HalfSize.Y + sizeY / 2.0f;


            var limitedSize = MyMathHelper.Clamp(new Vector2(sizeX, sizeY), child.MinimumSize, child.MaximumSize);
            child.Resize(limitedSize);
        }

        #endregion Private Methods
    }
}