using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Xml.Items.Images;
using OpenBreed.Database.Xml.Items.Maps;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyMapsRepository : XmlReadonlyRepositoryBase<IDbMap>
    {
        #region Private Fields

        private readonly XmlDbMapTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyMapsRepository(XmlDbMapTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }

        public override string Name { get { return "Maps"; } }

        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbMap); } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override IDbMap GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbMap entry)
        {
            return context.Items.IndexOf((XmlDbMap)entry);
        }

        #endregion Protected Methods
    }

    public class XmlMapsRepository : XmlRepositoryBase<IDbMap>
    {
        #region Private Fields

        private readonly XmlDbMapTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlMapsRepository(XmlDbMapTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }

        public override string Name { get { return "Maps"; } }

        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbMap); } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IDbMap newEntry)
        {
            context.Items.Add((XmlDbMap)newEntry);
        }

        public override bool Remove(IDbMap entry)
        {
            return context.Items.Remove((XmlDbMap)entry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IDbMap GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbMap entry)
        {
            return context.Items.IndexOf((XmlDbMap)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbMap newEntry)
        {
            context.Items[index] = (XmlDbMap)newEntry;
        }

        #endregion Protected Methods
    }
}