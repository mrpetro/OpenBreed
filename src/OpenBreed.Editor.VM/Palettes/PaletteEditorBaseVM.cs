using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Logging;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Model.Palettes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Palettes
{
    public abstract class PaletteEditorBaseVM<TDbPalette> : EntrySpecificEditorVM<IDbPalette> where TDbPalette : IDbPalette
    {
        #region Protected Fields

        protected readonly PalettesDataProvider palettesDataProvider;
        protected readonly IModelsProvider dataProvider;

        #endregion Protected Fields

        #region Private Fields

        private int _currentColorIndex = -1;

        #endregion Private Fields

        #region Public Constructors

        public PaletteEditorBaseVM(
            ILogger logger,
            PalettesDataProvider palettesDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(logger, workspaceMan, dialogProvider)
        {
            this.palettesDataProvider = palettesDataProvider;
            this.dataProvider = dataProvider;
            Colors = new ObservableCollection<ColorSelectionVM>();

            ColorEditor = new ColorEditorVM(Colors);

            Initialize();

            Colors.CollectionChanged += (s, a) => OnPropertyChanged(nameof(Colors));
        }

        #endregion Public Constructors

        #region Public Properties

        public ColorEditorVM ColorEditor { get; }

        public ObservableCollection<ColorSelectionVM> Colors { get; }

        public int CurrentColorIndex
        {
            get { return _currentColorIndex; }
            set { SetProperty(ref _currentColorIndex, value); }
        }

        #endregion Public Properties

        #region Protected Methods

        protected abstract void UpdateEntry(TDbPalette target);

        protected abstract void UpdateVM(TDbPalette source);

        protected override void UpdateVM(IDbPalette source)
        {
            base.UpdateVM(source);

            UpdateVM((TDbPalette)source);
        }

        protected override void UpdateEntry(IDbPalette target)
        {
            UpdateEntry((TDbPalette)target);

            var model = palettesDataProvider.GetPalette(target.Id);

            for (int i = 0; i < model.Length; i++)
                model.Data[i] = Colors[i].Color;

            base.UpdateEntry(target);
        }

        protected void UpdateVMColors(PaletteModel model)
        {
            for (int i = 0; i < model.Data.Length; i++)
            {
                Colors[i] = new ColorSelectionVM(model.Data[i], OnColorSelected);
            }

            CurrentColorIndex = 0;
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentColorIndex):
                    ColorEditor.Index = CurrentColorIndex;
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnColorSelected(ColorSelectionVM colorSelection)
        {
            CurrentColorIndex = Colors.IndexOf(colorSelection);
        }

        private void Initialize()
        {
            for (int i = 0; i < 256; i++)
            {
                Colors.Add(new ColorSelectionVM(MyColor.FromArgb(255, (byte)i, (byte)i, (byte)i), OnColorSelected));
            }

            CurrentColorIndex = 0;
        }

        #endregion Private Methods
    }
}