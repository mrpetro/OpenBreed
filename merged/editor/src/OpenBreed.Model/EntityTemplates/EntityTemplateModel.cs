
namespace OpenBreed.Model.EntityTemplates
{
    public class EntityTemplateModel
    {
        #region Internal Constructors

        internal EntityTemplateModel(EntityTemplateBuilder builder)
        {
            EntityTemplate = builder.EntityTemplate;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        public string EntityTemplate { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Public Methods
    }
}