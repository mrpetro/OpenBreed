using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Xml.Items.Scripts;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyScriptsRepository : XmlReadonlyRepositoryBase<IDbScript>
    {
        #region Private Fields

        private readonly XmlDbScriptTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyScriptsRepository(XmlDbScriptTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbScriptEmbedded);
                yield return typeof(XmlDbScriptFromFile);
            }
        }

        public override string Name { get { return "Scripts"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbScript GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbScript entry)
        {
            return context.Items.IndexOf((XmlDbScript)entry);
        }

        #endregion Protected Methods
    }

    public class XmlScriptsRepository : XmlRepositoryBase<IDbScript>
    {
        #region Private Fields

        private readonly XmlDbScriptTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlScriptsRepository(XmlDbScriptTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbScriptEmbedded);
                yield return typeof(XmlDbScriptFromFile);
            }
        }

        public override string Name { get { return "Scripts"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbScript GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbScript entry)
        {
            return context.Items.IndexOf((XmlDbScript)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbScript newEntry)
        {
            context.Items[index] = (XmlDbScript)newEntry;
        }

        public override void Add(IDbScript newEntry)
        {
            context.Items.Add((XmlDbScript)newEntry);
        }

        #endregion Protected Methods
    }
}