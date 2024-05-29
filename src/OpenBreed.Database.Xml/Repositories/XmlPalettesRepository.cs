using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Xml.Items.Maps;
using OpenBreed.Database.Xml.Items.Palettes;
using OpenBreed.Database.Xml.Tables;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyPalettesRepository : XmlReadonlyRepositoryBase<IDbPalette>
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

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbPaletteFromBinary);
                yield return typeof(XmlDbPaletteFromMap);
                yield return typeof(XmlDbPaletteFromLbm);
            }
        }

        public override string Name { get { return "Palettes"; } }

        public override int Count => context.Items.Count;


        #endregion Public Properties

        #region Public Methods

        protected override IDbPalette GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbPalette entry)
        {
            return context.Items.IndexOf((XmlDbPalette)entry);
        }

        #endregion Public Methods

    }

    public class XmlPalettesRepository : XmlRepositoryBase<IDbPalette>
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

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbPaletteFromBinary);
                yield return typeof(XmlDbPaletteFromMap);
            }
        }

        public override  string Name { get { return "Palettes"; } }

        public override int Count => context.Items.Count;


        #endregion Public Properties

        #region Public Methods

        protected override IDbPalette GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbPalette entry)
        {
            return context.Items.IndexOf((XmlDbPalette)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbPalette newEntry)
        {
            context.Items[index] = (XmlDbPalette)newEntry;
        }

        public override void Add(IDbPalette newEntry)
        {
            context.Items.Add((XmlDbPalette)newEntry);
        }

        public override bool Remove(IDbPalette entry)
        {
            return context.Items.Remove((XmlDbPalette)entry);
        }

        #endregion Public Methods

    }
}
