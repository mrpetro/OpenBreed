using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Sources;
using OpenBreed.Editor.VM.Database.Sources;
using OpenBreed.Editor.VM.Project;
using OpenBreed.Common.Props;
using System.ComponentModel;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Props
{
    public class PropSetVM : BaseViewModel
    {
        #region Private Fields

        private const int PROP_SIZE = 32;
        private string _name;
        private PropertySetModel m_CurrentPropertySet = PropertySetModel.NullTileSet;

        #endregion Private Fields

        #region Public Constructors

        public PropSetVM(PropSetsVM owner)
        {
            Owner = owner;
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


        public PropSetsVM Owner { get; private set; }

        public BaseSource Source { get; private set; }

        #endregion Public Properties

        #region Public Methods

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

        internal static PropSetVM Create(PropSetsVM owner, BaseSource source)
        {
            var model = source.Load() as PropertySetModel;

            var newPropertySet = new PropSetVM(owner);
            newPropertySet.Source = source;
            newPropertySet.Name = source.Name;

            foreach (var propertyModel in model.Properties)
                newPropertySet.Items.Add(PropVM.Create(propertyModel));

            return newPropertySet;
        }

        #endregion Internal Methods

    }
}
