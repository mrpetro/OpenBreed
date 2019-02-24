using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [Bindable(true, BindingDirection.TwoWay)]
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
            }

        }

        #endregion Public Properties

        #region Private Methods

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
