using System;
using OpenBreed.Core;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Animation.Components;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Systems.Movement.Components;
using OpenBreed.Core.Systems.Physics.Components;
using OpenBreed.Game.Components;

namespace OpenBreed.Game.Entities.Builders
{
    public class TextBuilder : WorldEntityBuilder
    {
        #region Internal Fields

        internal Position position;
        internal IText text;

        #endregion Internal Fields

        #region Public Constructors

        public TextBuilder(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override IEntity Build()
        {
            if (position == null)
                CoreTools.ThrowComponentRequiredException<Position>();

            if (text == null)
                CoreTools.ThrowComponentRequiredException<Direction>();

            return new TextEntity(this);
        }

        public void SetPosition(Position position)
        {
            this.position = position;
        }

        public void SetText(IText text)
        {
            this.text = text;
        }

        #endregion Public Methods
    }
}