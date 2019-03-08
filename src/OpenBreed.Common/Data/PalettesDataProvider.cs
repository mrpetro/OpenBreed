using OpenBreed.Common.Maps;
using OpenBreed.Common.Maps.Blocks;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Palettes.Builders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    public class PalettesDataProvider
    {
        #region Public Constructors

        public PalettesDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties


        public static PaletteModel Create(MapPaletteBlock paletteBlock)
        {
            var paletteBuilder = PaletteBuilder.NewPaletteModel();
            paletteBuilder.SetName(paletteBlock.Name);
            paletteBuilder.CreateColors();
            for (int i = 0; i < paletteBlock.Value.Length; i++)
            {
                var colorData = paletteBlock.Value[i];
                paletteBuilder.SetColor(i, Color.FromArgb(255, colorData.R, colorData.G, colorData.B));
            }

            return paletteBuilder.Build();
        }

        private PaletteModel GetDataImpl(IPaletteDataFromMap paletteData)
        {
            var map = Provider.Datas.GetData(paletteData.DataRef) as MapModel;

            if (map == null)
                return null;

            var paletteBlock = map.Blocks.OfType<MapPaletteBlock>().FirstOrDefault(item => item.Name == paletteData.BlockName);

            if (paletteBlock == null)
                return null;

            return Create(paletteBlock);
        }

        private PaletteModel GetDataImpl(IPaletteDataFromBinary paletteData)
        {
            return Provider.Datas.GetData(paletteData.DataRef) as PaletteModel;
        }

        private PaletteModel GetData(dynamic paletteData)
        {
            return GetDataImpl(paletteData);

            //if (paletteData is IPaletteDataFromBinary)
            //    return GetData((IPaletteDataFromBinary)paletteData);
            //else if (paletteData is IPaletteDataFromMap)
            //    return GetData((IPaletteDataFromMap)paletteData);
            //else
            //    throw new InvalidOperationException();
        }

        public PaletteModel GetPalette(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<IPaletteEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Palette error: " + id);

            if (entry.Data == null)
                return null;

            return GetData(entry.Data);
        }
    }
}
