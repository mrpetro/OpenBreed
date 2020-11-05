using System.ComponentModel;
using System.Windows.Forms;

namespace OpenBreed.Common.UI.WinForms.Controls
{
    public class DataGridViewEx : DataGridView, INotifyPropertyChanged
    {
        #region Private Fields

        private int _currentRowIndex = -1;

        #endregion Private Fields

        #region Public Constructors

        public DataGridViewEx()
        {
            CurrentCellChanged += (s, a) => CurrentRowIndex = GetCurrentRowIndex();
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        [Bindable(BindableSupport.Yes, BindingDirection.TwoWay)]
        public int CurrentRowIndex
        {
            get
            {
                return _currentRowIndex;
            }

            set
            {
                if (_currentRowIndex == value)
                    return;

                _currentRowIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentRowIndex)));

                //Make sure current row will also be selected and focussed on
                if (_currentRowIndex != -1)
                {
                    Rows[_currentRowIndex].Selected = true;
                    EnsureVisibleRow(_currentRowIndex);
                }
            }
        }

        #endregion Public Properties

        #region Private Methods

        private void EnsureVisibleRow(int rowToShow)
        {
            if (rowToShow >= 0 && rowToShow < RowCount)
            {
                var countVisible = DisplayedRowCount(false);
                var firstVisible = FirstDisplayedScrollingRowIndex;
                if (rowToShow < firstVisible)
                {
                    FirstDisplayedScrollingRowIndex = rowToShow;
                }
                else if (rowToShow >= firstVisible + countVisible)
                {
                    FirstDisplayedScrollingRowIndex = rowToShow - countVisible + 1;
                }
            }
        }

        private int GetCurrentRowIndex()
        {
            if (CurrentRow == null)
                return -1;
            else
                return Rows.IndexOf(CurrentRow);
        }

        #endregion Private Methods
    }
}