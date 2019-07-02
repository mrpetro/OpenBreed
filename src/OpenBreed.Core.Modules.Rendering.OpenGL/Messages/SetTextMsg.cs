using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Rendering.Messages
{
    public struct SetTextMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "SET_TEXT";

        #endregion Public Fields

        #region Public Constructors

        public SetTextMsg(string text)
        {
            Text = text;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Type { get { return TYPE; } }
        public object Data { get { return Text; } }
        public string Text { get; }

        #endregion Public Properties
    }
}