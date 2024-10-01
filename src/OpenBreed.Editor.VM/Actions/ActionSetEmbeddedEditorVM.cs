using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Actions;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Actions
{
    public class ActionSetEmbeddedEditorVM : EntrySpecificEditorVM<IDbActionSet>
    {
        #region Private Fields

        private const int PROP_SIZE = 32;
        private readonly ActionSetsDataProvider actionSetsDataProvider;
        private readonly IDrawingFactory drawingFactory;
        private readonly IDrawingContextProvider drawingContextProvider;
        private ActionSetModel model;

        #endregion Private Fields

        #region Public Constructors

        public ActionSetEmbeddedEditorVM(
            IDbActionSet dbEntry,
            ILogger logger,
            ActionSetsDataProvider actionSetsDataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IDrawingFactory drawingFactory,
            IDrawingContextProvider drawingContextProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
            this.actionSetsDataProvider = actionSetsDataProvider;
            this.drawingFactory = drawingFactory;
            this.drawingContextProvider = drawingContextProvider;
            Items = new ObservableCollection<ActionVM>();
            Items.CollectionChanged += (s, a) => OnPropertyChanged(nameof(Items));
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<ActionVM> Items { get; }

        public int SelectedIndex { get; private set; }

        public override string EditorName => "Action Set Editor";

        #endregion Public Properties

        #region Public Methods

        public void DrawProperty(IDrawingContext gfx, int id, float x, float y, int tileSize)
        {
            if (id >= Items.Count)
            {
                return;
            }

            var propertyData = Items[id];

            if (!propertyData.IsVisible)
            {
                return;
            }

            var image = propertyData.Icon;

            var opqPen = drawingFactory.CreatePen(MyColor.FromArgb(128, 255, 255, 255), 10);
            var otranspen = drawingFactory.CreatePen(MyColor.FromArgb(128, 255, 255, 255), 10);
            var ototTransPen = drawingFactory.CreatePen(MyColor.FromArgb(40, 0, 255, 0), 10);

            //ColorMatrix cm = new ColorMatrix();
            //cm.Matrix33 = 0.55f;
            //ImageAttributes ia = new ImageAttributes();
            //ia.SetColorMatrix(cm);
            //gfx.DrawImage(image, new Rectangle((int)x, (int)y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);

            gfx.DrawImage(image, x, y, tileSize, tileSize);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateVM(IDbActionSet entry)
        {
            base.UpdateVM(entry);

            model = actionSetsDataProvider.GetActionSet(entry.Id);

            Items.Clear();

            foreach (var item in model.Items)
            {
                Items.Add(new ActionVM(drawingFactory, drawingContextProvider, item, OnItemPropertyChanged));
            }

            SelectedIndex = 0;
        }

        protected override void UpdateEntry(IDbActionSet entry)
        {
            entry.Actions.Clear();

            foreach (var item in model.Items)
            {
                var newAction = entry.NewItem();
                ActionSetsDataHelper.ToEntry(item, newAction);
                entry.Actions.Add(newAction);
            }

            base.UpdateEntry(entry);
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnItemPropertyChanged(ActionVM action)
        {
            OnPropertyChanged(nameof(Items));
        }

        #endregion Private Methods
    }
}