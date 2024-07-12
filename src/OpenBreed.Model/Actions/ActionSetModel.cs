using OpenBreed.Common.Interface.Drawing;
using System.Collections.Generic;

namespace OpenBreed.Model.Actions
{
    public class ActionSetModel
    {
        #region Public Constructors

        public ActionSetModel()
        {
            Items = new List<ActionModel>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<ActionModel> Items { get; }

        #endregion Public Properties
    }

    public class ActionModel
    {
        #region Public Properties

        public int Id { get; set; }
        public string Description { get; set; }
        public IImage Icon { get; set; }
        public MyColor Color { get; set; }
        public bool Visibility { get; set; }
        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }

        #endregion Public Methods
    }
}