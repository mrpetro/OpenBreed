﻿using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Editor.VM.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileSetEditorVM : ParentEntryEditor<ITileSetEntry>
    {
        #region Public Constructors

        static TileSetEditorVM()
        {
            RegisterSubeditor<ITileSetFromBlkEntry>((parent) => new TileSetFromBlkEditorVM(parent));
        }

        public TileSetEditorVM(EditorApplication application, DataProvider dataProvider, IUnitOfWork unitOfWork) : base(application, dataProvider, unitOfWork, "Tile Set Editor")
        {
        }


        #endregion Public Constructors
    }
}