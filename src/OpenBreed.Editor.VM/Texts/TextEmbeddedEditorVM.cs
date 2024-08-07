﻿using Microsoft.Extensions.Logging;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextEmbeddedEditorVM : TextEditorBaseVM<IDbTextEmbedded>
    {
        #region Private Fields

        private readonly TextsDataProvider textsDataProvider;

        private string text;

        private string dataRef;

        #endregion Private Fields

        #region Public Constructors

        public TextEmbeddedEditorVM(
            ILogger logger,
            TextsDataProvider textsDataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(logger, workspaceMan, dialogProvider)
        {
            this.textsDataProvider = textsDataProvider;
        }

        #endregion Public Constructors

        #region Public Properties

        public string DataRef
        {
            get { return dataRef; }
            set { SetProperty(ref dataRef, value); }
        }

        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        public override string EditorName => "Embedded Text Editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry(IDbTextEmbedded entry)
        {
            var model = textsDataProvider.GetText(entry);
            model.Text = Text;
        }

        protected override void UpdateVM(IDbTextEmbedded entry)
        {
            var model = textsDataProvider.GetText(entry);

            if (model != null)
            {
                Text = model.Text;
            }
        }

        #endregion Protected Methods
    }
}