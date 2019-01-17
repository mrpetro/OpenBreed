﻿using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTableEditorConnector : VMConnectorBase<DbTableEditorVM>
    {
        #region Public Constructors

        public DbTableEditorConnector(DbTableEditorVM source) : base(source)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void ConnectTo(DbTableSelectorVM dbTableSelector)
        {
            dbTableSelector.PropertyChanged += DbTableSelector_PropertyChanged;
        }

        #endregion Public Methods

        #region Private Methods

        private void DbTableSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var tableSelector = sender as DbTableSelectorVM;

            switch (e.PropertyName)
            {
                case nameof(tableSelector.CurrentItem):
                    OnCurrentTableChanged(tableSelector.CurrentItem);
                    break;
                default:
                    break;
            }
        }

        private void OnCurrentTableChanged(DbTableVM currentTable)
        {
            if (currentTable != null)
                UpdateWithCurrentTableItems(currentTable);
            else
                UpdateWithNoItems();
        }

        private void UpdateWithCurrentTableItems(DbTableVM currentTable)
        {
            Source.Items.UpdateAfter(() =>
            {
                Source.Items.Clear();
                foreach (var item in currentTable.GetItems())
                    Source.Items.Add(item);
            });
        }

        private void UpdateWithNoItems()
        {
            Source.Items.UpdateAfter(() =>
            {
                Source.Items.Clear();
            });
        }

        #endregion Private Methods

    }
}
