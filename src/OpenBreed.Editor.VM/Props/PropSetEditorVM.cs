﻿using OpenBreed.Common.Props;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Props
{
    public class PropSetEditorVM : EntryEditorBaseVM<IPropSetEntity, PropSetVM>
    {

        #region Public Constructors

        public PropSetEditorVM(EditorVM root) : base(root)
        {
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Property Set Editor"; } }

        public int SelectedIndex { get; private set; }

        #endregion Public Properties

        #region Internal Methods

        internal void Connect()
        {

        }

        #endregion Internal Methods

        #region Protected Methods

        protected override IPropSetEntity GetModel(string name)
        {
            return Root.DataProvider.GetPropSet(name);
        }

        protected override void UpdateModel(PropSetVM source, IPropSetEntity target)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateVM(IPropSetEntity source, PropSetVM target)
        {
            foreach (var property in source.Items)
            {
                var newProp = target.CreateProp(property);
                newProp.Load(property);
                target.Items.Add(newProp);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Editable):
                    SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}
