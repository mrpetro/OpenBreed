using OpenBreed.Core.Entities;
using OpenBreed.Game.Entities.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities
{
    public class TextEntity : WorldEntity
    {
        #region Internal Constructors

        internal TextEntity(TextBuilder builder) : base(builder)
        {
            Components.Add(builder.position);
            Components.Add(builder.text);
        }

        #endregion Internal Constructors
    }
}
