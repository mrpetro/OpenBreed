namespace OpenBreed.Gui.Abstractions.Builders
{
    public interface IGridPanelParentOption : IElementOption
    {
        #region Public Properties

        int Column { get; }
        int Row { get; }
        int ColumnSpan { get; }
        int RowSpan { get; }

        #endregion Public Properties
    }

    public class GridPanelParentOption : IGridPanelParentOption
    {
        #region Public Constructors

        public GridPanelParentOption(int column, int row, int columnSpan = 1, int rowSpan = 1)
        {
            Column = column;
            Row = row;
            ColumnSpan = columnSpan;
            RowSpan = rowSpan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Column { get; }
        public int Row { get; }

        public int ColumnSpan { get; }

        public int RowSpan { get; }

        #endregion Public Properties
    }
}