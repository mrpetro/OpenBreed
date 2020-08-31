using OpenBreed.Common.Model.Scripts.Builders;

namespace OpenBreed.Common.Model.Scripts
{
    public class ScriptModel
    {
        #region Internal Constructors

        internal ScriptModel(ScriptBuilder builder)
        {
            Script = builder.Script;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        public string Script { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Public Methods
    }
}