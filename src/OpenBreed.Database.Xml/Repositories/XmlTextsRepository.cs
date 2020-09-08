using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Xml.Items.Texts;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlTextsRepository : XmlRepositoryBase<ITextEntry>
    {

        #region Private Fields

        private readonly XmlDbTextTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlTextsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetTable<XmlDbTextTableDef>();
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlTextEmbeddedEntry);
                yield return typeof(XmlTextFromMapEntry);
                yield return typeof(XmlTextFromFileEntry);
            }
        }
        public override string Name { get { return "Texts"; } }

        public override int Count => _table.Items.Count;

        #endregion Public Properties

        #region Public Methods

        protected override ITextEntry GetEntryWithIndex(int index)
        {
            return _table.Items[index];
        }

        protected override int GetIndexOf(ITextEntry entry)
        {
            return _table.Items.IndexOf((XmlTextEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, ITextEntry newEntry)
        {
            _table.Items[index] = (XmlTextEntry)newEntry;
        }

        public override void Add(ITextEntry newEntry)
        {
            _table.Items.Add((XmlTextEntry)newEntry);
        }

        #endregion Public Methods

    }
}
