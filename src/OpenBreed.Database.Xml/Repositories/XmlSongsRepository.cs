using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Songs;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Xml.Items.Songs;
using OpenBreed.Database.Xml.Items.Sounds;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlySongsRepository : XmlReadonlyRepositoryBase<IDbSong>
    {
        #region Private Fields

        private readonly XmlDbSongTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlySongsRepository(XmlDbSongTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbSong); } }
        public override string Name { get { return "Songs"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbSong GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbSong entry)
        {
            return context.Items.IndexOf((XmlDbSong)entry);
        }

        #endregion Protected Methods
    }

    public class XmlSongsRepository : XmlRepositoryBase<IDbSong>
    {
        #region Private Fields

        private readonly XmlDbSongTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlSongsRepository(XmlDbSongTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbSong); } }
        public override string Name { get { return "Songs"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbSong GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbSong entry)
        {
            return context.Items.IndexOf((XmlDbSong)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbSong newEntry)
        {
            context.Items[index] = (XmlDbSong)newEntry;
        }

        public override void Add(IDbSong newEntry)
        {
            context.Items.Add((XmlDbSong)newEntry);
        }

        #endregion Protected Methods
    }
}