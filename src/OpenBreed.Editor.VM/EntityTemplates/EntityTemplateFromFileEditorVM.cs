using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Texts;

namespace OpenBreed.Editor.VM.EntityTemplates
{
    public class EntityTemplateFromFileEditorVM : EntityTemplateEditorBaseVM<IDbEntityTemplateFromFile>
    {
        #region Private Fields

        private bool _editEnabled;

        private string dataRef;

        private string entityTemplate;
        private readonly EntityTemplatesDataProvider entityTemplatesDataProvider;
        private readonly IModelsProvider dataProvider;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public EntityTemplateFromFileEditorVM(
            ILogger logger,
            EntityTemplatesDataProvider entityTemplatesDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(logger, workspaceMan, dialogProvider)
        {
            this.entityTemplatesDataProvider = entityTemplatesDataProvider;
            this.dataProvider = dataProvider;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool EditEnabled
        {
            get { return _editEnabled; }
            set { SetProperty(ref _editEnabled, value); }
        }

        public string DataRef
        {
            get { return dataRef; }
            set { SetProperty(ref dataRef, value); }
        }

        public string EntityTemplate
        {
            get { return entityTemplate; }
            set { SetProperty(ref entityTemplate, value); }
        }

        public override string EditorName => "Entity Template File Editor";

        #endregion Public Properties

        #region Public Methods

        protected override void UpdateEntry(IDbEntityTemplateFromFile entry)
        {
            var model = dataProvider.GetModel<IDbEntityTemplateFromFile, TextModel>(entry);
            model.Text = EntityTemplate;
            entry.DataRef = DataRef;
        }

        protected override void UpdateVM(IDbEntityTemplateFromFile entry)
        {
            try
            {
                var model = entityTemplatesDataProvider.GetEntityTemplate(entry.Id);

                if (model != null)
                    EntityTemplate = model.EntityTemplate;

                DataRef = entry.DataRef;
            }
            catch (System.Exception ex)
            {
                logger.LogCritical(ex.ToString());
            }
        }

        #endregion Public Methods

        #region Internal Methods

        #endregion Internal Methods

        #region Private Methods

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

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(DataRef))
                return false;

            return true;
        }

        #endregion Private Methods
    }
}