using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Xml.Items.Sprites;
using OpenBreed.Database.Xml.Items.Texts;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyTextsRepository : XmlReadonlyRepositoryBase<IDbText>
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

        public override IEnumerable<IDbEntry> Entries
        { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbTextEmbedded);
                yield return typeof(XmlDbTextFromMap);
                yield return typeof(XmlDbTextFromFile);
            }
        }

        public override string Name
        { get { return "Texts"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbText GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbText entry)
        {
            return context.Items.FindIndex(item => item.Id == entry.Id);
        }

        #endregion Protected Methods
    }

    public class XmlTextsRepository : XmlRepositoryBase<IDbText>
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

        public override IEnumerable<IDbEntry> Entries
        { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbTextEmbedded);
                yield return typeof(XmlDbTextFromMap);
                yield return typeof(XmlDbTextFromFile);
            }
        }

        public override string Name
        { get { return "Texts"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IDbText newEntry)
        {
            context.Items.Add((XmlDbText)newEntry);
        }

        public override bool Remove(IDbText entry)
        {
            return context.Items.Remove((XmlDbText)entry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IDbText GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbText entry)
        {
            return context.Items.FindIndex(item => item.Id == entry.Id);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbText newEntry)
        {
            context.Items[index] = (XmlDbText)newEntry;
        }

        #endregion Protected Methods
    }
}