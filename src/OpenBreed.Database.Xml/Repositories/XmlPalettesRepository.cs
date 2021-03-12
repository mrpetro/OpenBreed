using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Xml.Items.Palettes;
using OpenBreed.Database.Xml.Tables;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyPalettesRepository : XmlReadonlyRepositoryBase<IPaletteEntry>
    {

        #region Private Fields

        private readonly XmlDbPaletteTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyPalettesRepository(XmlDbPaletteTableDef context)
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
                yield return typeof(XmlPaletteFromBinaryEntry);
                yield return typeof(XmlPaletteFromMapEntry);
            }
        }

        public override string Name { get { return "Palettes"; } }

        public override int Count => context.Items.Count;


        #endregion Public Properties

        #region Public Methods

        protected override IPaletteEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IPaletteEntry entry)
        {
            return context.Items.IndexOf((XmlPaletteEntry)entry);
        }

        #endregion Public Methods

    }

    public class XmlPalettesRepository : XmlRepositoryBase<IPaletteEntry>
    {

        #region Private Fields

        private readonly XmlDbPaletteTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlPalettesRepository(XmlDbPaletteTableDef context)
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
                yield return typeof(XmlPaletteFromBinaryEntry);
                yield return typeof(XmlPaletteFromMapEntry);
            }
        }

        public override  string Name { get { return "Palettes"; } }

        public override int Count => context.Items.Count;


        #endregion Public Properties

        #region Public Methods

        protected override IPaletteEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IPaletteEntry entry)
        {
            return context.Items.IndexOf((XmlPaletteEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IPaletteEntry newEntry)
        {
            context.Items[index] = (XmlPaletteEntry)newEntry;
        }

        public override void Add(IPaletteEntry newEntry)
        {
            context.Items.Add((XmlPaletteEntry)newEntry);
        }

        #endregion Public Methods

    }
}
