using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Animations
{
    public class ClipTrackItemVM : BaseViewModel
    {
        #region Private Fields

        private string label;

        #endregion Private Fields

        #region Public Constructors

        public ClipTrackItemVM(IDbAnimationTrack source)
        {
            Source = source;

            Refresh();
        }

        #endregion Public Constructors

        #region Public Properties

        public string Label
        {
            get { return label; }
            private set { SetProperty(ref label, value); }
        }

        #endregion Public Properties

        #region Internal Properties

        internal IDbAnimationTrack Source { get; }

        #endregion Internal Properties

        #region Public Methods

        public void Refresh()
        {
            Label = Source.Controller;
        }

        #endregion Public Methods
    }
}