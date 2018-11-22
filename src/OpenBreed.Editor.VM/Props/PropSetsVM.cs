using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM.Database.Sources;
using OpenBreed.Common.Props;
using System.ComponentModel;
using OpenBreed.Editor.VM.Base;
using System.Drawing;
using OpenBreed.Common.Logging;

namespace OpenBreed.Editor.VM.Props
{
    public class PropSetsVM : BaseViewModel
    {
        #region Private Fields
        private string _title;
        private int _currentIndex;
        private PropSetVM _currentItem;

        #endregion Private Fields

        #region Public Constructors

        public PropSelectorVM PropSelector { get; private set; }


        public PropSetsVM(EditorVM root)
        {
            Root = root;

            PropSelector = new PropSelectorVM(this);

            Items = new BindingList<PropSetVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));
        }

        #endregion Public Constructors

        #region Public Properties

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                if (SetProperty(ref _currentIndex, value))
                    CurrentItem = Items[value];
            }
        }

        public PropSetVM CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (SetProperty(ref _currentItem, value))
                    CurrentIndex = Items.IndexOf(value);
            }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public BindingList<PropSetVM> Items { get; private set; }
        public EditorVM Root { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void AddPropertySet(string propertySetRef)
        {
            if (string.IsNullOrWhiteSpace(propertySetRef))
            {
                propertySetRef = "DefaultPropertySetDef.xml";
                LogMan.Instance.LogWarning("Property Set source not set. Getting default: DefaultPropertySetDef.xml");
            }

            var propertySetSourceDef = Root.CurrentDatabase.GetSourceDef(propertySetRef);

            if (propertySetSourceDef == null)
                throw new Exception("No PropertySetSource definition found with name: " + propertySetRef);

            var source = Root.Sources.GetSource(propertySetSourceDef);

            if (source == null)
                throw new Exception("PropertySet source error: " + propertySetSourceDef);

            Items.Add(PropSetVM.Create(this, source));

            CurrentItem = Items.LastOrDefault();
        }

        public void DrawProperty(Graphics gfx, int id, float x, float y, int tileSize)
        {
            if (CurrentItem != null)
                CurrentItem.DrawProperty(gfx, id, x, y, tileSize);
            //else
            //    DrawDefaultProperty(gfx, id, x, y, tileSize);
        }

        private void DrawDefaultProperty(Graphics gfx, int id, float x, float y, int tileSize)
        {
            if (id == 0)
                return;

            gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            Font font = new Font("Arial", 7);

            Rectangle rectangle = new Rectangle((int)x, (int)y, tileSize, tileSize);

            //Color c = Color.Red;
            Color c = Color.FromArgb(50, 255, 0, 0);// Color.Black;
            Pen tileColor = new Pen(c);
            Brush brush = new SolidBrush(c);

            gfx.FillRectangle(brush, rectangle);

            c = Color.White;
            tileColor = new Pen(c);
            brush = new SolidBrush(c);

            gfx.DrawRectangle(tileColor, rectangle);
            gfx.DrawString(string.Format("{0,2:D2}", id), font, brush, x + 1, y+ 3);


        }

        #endregion Public Methods
    }
}
