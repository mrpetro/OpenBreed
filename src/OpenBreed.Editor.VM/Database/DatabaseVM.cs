using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM;
using OpenBreed.Common.Assets;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Levels;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Levels.Tools;
using OpenBreed.Editor.VM.Base;
using System.IO;
using OpenBreed.Common;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Database.Items;
using OpenBreed.Common.XmlDatabase;
using System.ComponentModel;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.XmlDatabase.Tables.Sources;
using OpenBreed.Common.XmlDatabase.Items.Levels;
using OpenBreed.Common.XmlDatabase.Items.Images;
using OpenBreed.Common.XmlDatabase.Tables;
using OpenBreed.Common.XmlDatabase.Tables.Images;
using OpenBreed.Editor.VM.Database.Tables;
using OpenBreed.Common.XmlDatabase.Tables.Levels;
using OpenBreed.Common.XmlDatabase.Tables.Props;
using OpenBreed.Common.XmlDatabase.Items.Props;
using OpenBreed.Common.XmlDatabase.Items.Tiles;
using OpenBreed.Common.XmlDatabase.Tables.Tiles;
using OpenBreed.Common.XmlDatabase.Items.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Items.Palettes;
using OpenBreed.Common.XmlDatabase.Tables.Palettes;
using OpenBreed.Common.Images;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Props;
using OpenBreed.Common.Tiles;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Sounds;

namespace OpenBreed.Editor.VM.Database
{
    public enum ProjectState
    {
        Closed,
        New,
        Opened,
        Closing
    }

    public class DatabaseVM : BaseViewModel, IDisposable
    {

        #region Private Fields

        private string _name;
        private ProjectState _state;

        #endregion Private Fields

        #region Internal Constructors

        internal DatabaseVM()
        {
        }

        #endregion Internal Constructors

        #region Public Properties

        public bool IsModified { get; internal set; }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public ProjectState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public void Dispose()
        {
        }

        #endregion Public Properties

        #region Internal Methods

        internal IEnumerable<string> GetTableNames()
        {
            var unitOfWork = ServiceLocator.Instance.GetService<IUnitOfWork>();

            foreach (var repository in unitOfWork.Repositories)
                yield return repository.Name;
        }

        #endregion Internal Methods

    }
}
