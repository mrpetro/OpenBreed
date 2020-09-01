using OpenBreed.Common.Builders.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Model.Texts
{
    public class TextModel
    {

        #region Public Constructors

        internal TextModel(TextBuilder builder)
        {
            Text = builder.Text;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; set; }
        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        public string Text { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Public Methods

    }
}
