using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Rendering.Messages
{
    public struct TextSetMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "TEXT_SET";

        #endregion Public Fields

        #region Public Constructors

        public TextSetMsg(IEntity entity, string text)
        {
            Entity = entity;
            Text = text;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }
        public string Text { get; }

        #endregion Public Properties
    }
}