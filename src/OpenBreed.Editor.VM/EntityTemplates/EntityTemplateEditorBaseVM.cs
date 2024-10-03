using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Scripts;

namespace OpenBreed.Editor.VM.EntityTemplates
{
    public abstract class EntityTemplateEditorBaseVM<TDbEnityTemplate> : EntrySpecificEditorVM<IDbEntityTemplate> where TDbEnityTemplate : IDbEntityTemplate
    {
        #region Public Constructors

        public EntityTemplateEditorBaseVM(
            TDbEnityTemplate dbEntry,
            ILogger logger,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider) : base(dbEntry, logger, workspaceMan, dialogProvider)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public new TDbEnityTemplate Entry => (TDbEnityTemplate)base.Entry;

        #endregion Public Properties
    }
}