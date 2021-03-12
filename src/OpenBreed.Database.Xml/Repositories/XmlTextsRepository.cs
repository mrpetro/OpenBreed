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
    public class XmlReadonlyTextsRepository : XmlReadonlyRepositoryBase<ITextEntry>
    {

        #region Private Fields

        private readonly XmlDbTextTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyTextsRepository(XmlDbTextTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }
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

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        protected override ITextEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(ITextEntry entry)
        {
            return context.Items.IndexOf((XmlTextEntry)entry);
        }

        #endregion Public Methods

    }

    public class XmlTextsRepository : XmlRepositoryBase<ITextEntry>
    {

        #region Private Fields

        private readonly XmlDbTextTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlTextsRepository(XmlDbTextTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }
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

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        protected override ITextEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(ITextEntry entry)
        {
            return context.Items.IndexOf((XmlTextEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, ITextEntry newEntry)
        {
            context.Items[index] = (XmlTextEntry)newEntry;
        }

        public override void Add(ITextEntry newEntry)
        {
            context.Items.Add((XmlTextEntry)newEntry);
        }

        #endregion Public Methods

    }
}
