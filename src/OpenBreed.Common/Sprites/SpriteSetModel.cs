using OpenBreed.Common.Sprites.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Sprites
{
    public class SpriteSetModel
    {

        #region Public Constructors

        public SpriteSetModel(SpriteSetBuilder builder)
        {
            Sprites = builder.Sprites;
        }

        #endregion Public Constructors

        #region Public Properties

        public List<SpriteModel> Sprites { get; set; }

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        #endregion Public Properties
    }
}
