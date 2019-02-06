using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Common.Assets;
using System.ComponentModel;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Actions;
using OpenBreed.Common;

namespace OpenBreed.Editor.VM.Actions
{
    public class ActionSetVM : EditableEntryVM
    {

        #region Private Fields

        private const int PROP_SIZE = 32;

        #endregion Private Fields

        #region Public Constructors

        public ActionSetVM()
        {
            Items = new BindingList<ActionVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<ActionVM> Items { get; }

        #endregion Public Properties

        #region Public Methods

        public ActionVM NewItem()
        {
            return new ActionVM(this);
        }

        public void DrawProperty(Graphics gfx, int id, float x, float y, int tileSize)
        {
            if (id >= Items.Count)
                return;

            var propertyData = Items[id];

            if (!propertyData.Visibility)
                return;

            var image = propertyData.Icon;

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

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IActionSetEntry)entry);
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IActionSetEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void ToEntry(IActionSetEntry entry)
        {
            entry.Items.Clear();

            foreach (var item in Items)
            {
                var newAction = entry.NewItem();
                item.ToModel(newAction);
                entry.Items.Add(newAction);
            }
        }

        private void FromEntry(IActionSetEntry entry)
        {
            Items.UpdateAfter(() =>
            {
                Items.Clear();

                foreach (var item in entry.Items)
                {
                    var newAction = NewItem();
                    newAction.FromModel(item);
                    Items.Add(newAction);
                }
            });
        }

        #endregion Private Methods
    }
}
