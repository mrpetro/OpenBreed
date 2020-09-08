using OpenBreed.Common;
using OpenBreed.Common.Actions;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Xml.Items.Actions;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlActionSetsRepository : XmlRepositoryBase<IActionSetEntry>
    {

        #region Private Fields

        private readonly XmlDbActionSetTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlActionSetsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetTable<XmlDbActionSetTableDef>();
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlActionSetEntry); } }
        public override string Name { get { return "Action sets"; } }

        public override int Count => _table.Items.Count;

        #endregion Public Properties

        #region Public Methods

        protected override IActionSetEntry GetEntryWithIndex(int index)
        {
            return _table.Items[index];
        }

        protected override int GetIndexOf(IActionSetEntry entry)
        {
            return _table.Items.IndexOf((XmlActionSetEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IActionSetEntry newEntry)
        {
            _table.Items[index] = (XmlActionSetEntry)newEntry;
        }

        public override void Add(IActionSetEntry newEntry)
        {
            _table.Items.Add((XmlActionSetEntry)newEntry);
        }

        #endregion Public Methods

    }
}
