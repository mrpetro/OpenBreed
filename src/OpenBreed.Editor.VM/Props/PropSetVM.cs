using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Common.Sources;
using System.ComponentModel;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Database.Items.Props;

namespace OpenBreed.Editor.VM.Props
{
    public class PropSetVM : BaseViewModel
    {

        #region Private Fields

        private const int PROP_SIZE = 32;
        private string _name;

        #endregion Private Fields

        #region Public Constructors

        public PropSetVM(EditorVM root)
        {
            Root = root;
            Items = new BindingList<PropVM>();
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<PropVM> Items { get; private set; }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


        public EditorVM Root { get; }

        #endregion Public Properties

        #region Public Methods

        public PropVM CreateProp(PropertyDef propDef)
        {
            return new PropVM(this);
        }

        public void DrawProperty(Graphics gfx, int id, float x, float y, int tileSize)
        {
            if (id >= Items.Count)
                return;

            var propertyData = Items[id];

            if (!propertyData.Visibility)
                return;

            var image = propertyData.Presentation;

            var opqPen = new Pen(Color.FromArgb(128, 255, 255, 255), 10);
            var otranspen = new Pen(Color.FromArgb(128, 255, 255, 255), 10);
            var ototTransPen = new Pen(Color.FromArgb(40, 0, 255, 0), 10);

            //ColorMatrix cm = new ColorMatrix();
            //cm.Matrix33 = 0.55f;
            //ImageAttributes ia = new ImageAttributes();
            //ia.SetColorMatrix(cm);
            //gfx.DrawImage(image, new Rectangle((int)x, (int)y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);

            gfx.DrawImage(image, x, y, tileSize, tileSize);

        }

        #endregion Public Methods

        #region Internal Methods

        internal void Load(PropertySetDef propSetDef)
        {
            Name = propSetDef.Name;

            foreach (var propDef in propSetDef.PropertyDefs)
            {
                var newProp = CreateProp(propDef);
                newProp.Load(propDef);
                Items.Add(newProp);
            }
        }

        #endregion Internal Methods

    }
}
