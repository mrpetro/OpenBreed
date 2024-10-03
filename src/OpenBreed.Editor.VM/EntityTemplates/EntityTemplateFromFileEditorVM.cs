using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Texts;

namespace OpenBreed.Editor.VM.EntityTemplates
{
    public class EntityTemplateFromFileEditorVM : EntityTemplateEditorBaseVM<IDbEntityTemplateFromFile>
    {
        #region Private Fields

        private readonly EntityTemplatesDataProvider entityTemplatesDataProvider;
        private readonly IModelsProvider dataProvider;
        private readonly ILogger logger;
        private bool editEnabled;

        private string dataRef;

        private string entityTemplate;

        #endregion Private Fields

        #region Public Constructors

        public EntityTemplateFromFileEditorVM(
            IDbEntityTemplateFromFile dbEntry,
            ILogger logger,
            EntityTemplatesDataProvider entityTemplatesDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
            this.entityTemplatesDataProvider = entityTemplatesDataProvider;
            this.dataProvider = dataProvider;
            this.logger = logger;

            IgnoreProperty(nameof(EditEnabled));
        }

        #endregion Public Constructors

        #region Public Properties

        public bool EditEnabled
        {
            get { return editEnabled; }
            set { SetProperty(ref editEnabled, value); }
        }

        public string DataRef
        {
            get { return Entry.DataRef; }
            set { SetProperty(Entry, x => x.DataRef, value); }
        }

        public string EntityTemplate
        {
            get { return entityTemplate; }
            set { SetProperty(ref entityTemplate, value); }
        }

        public override string EditorName => "Entity Template File Editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void ProtectedUpdateEntry()
        {
            var model = dataProvider.GetModel<IDbEntityTemplateFromFile, TextModel>(Entry);
            model.Text = EntityTemplate;
        }

        protected override void ProtectedUpdateVM()
        {
            try
            {
                var model = entityTemplatesDataProvider.GetEntityTemplate(Entry.Id);

                if (model != null)
                {
                    EntityTemplate = model.EntityTemplate;
                }
            }
            catch (System.Exception ex)
            {
                logger.LogCritical(ex.ToString());
            }
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(DataRef):
                    EditEnabled = ValidateSettings();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(DataRef))
                return false;

            return true;
        }

        #endregion Private Methods
    }
}