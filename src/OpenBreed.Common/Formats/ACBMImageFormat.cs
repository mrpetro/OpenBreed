using OpenBreed.Common.DataSources;
using OpenBreed.Common.Images.Readers.ACBM;
using OpenBreed.Common.Model.Images.Builders;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenBreed.Common.Formats
{
    public class ACBMImageFormat : IDataFormatType
    {
        #region Public Constructors

        public ACBMImageFormat()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public static ACBMImageReader.ACBMPaletteMode ToACBMPaletteMode(string paletteMode)
        {
            switch (paletteMode)
            {
                case "NONE":
                    return ACBMImageReader.ACBMPaletteMode.NONE;

                case "16BIT":
                    return ACBMImageReader.ACBMPaletteMode.PALETTE_16BIT;

                case "32BIT":
                    return ACBMImageReader.ACBMPaletteMode.PALETTE_32BIT;

                default:
                    throw new InvalidOperationException(paletteMode);
            }
        }

        public object Load(DataSourceBase ds, List<FormatParameter> parameters)
        {
            var width = (int)parameters.FirstOrDefault(item => item.Name == "WIDTH").Value;
            var height = (int)parameters.FirstOrDefault(item => item.Name == "HEIGHT").Value;
            var bitPlanesNo = (int)parameters.FirstOrDefault(item => item.Name == "BIT_PLANES_NO").Value;
            var paletteStr = (string)parameters.FirstOrDefault(item => item.Name == "PALETTE_MODE").Value;

            var paletteMode = ToACBMPaletteMode(paletteStr);

            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var imageBuilder = ImageBuilder.NewImage();
            var reader = new ACBMImageReader(imageBuilder, width, height, bitPlanesNo, paletteMode);
            return reader.Read(ds.Stream);
        }

        public void Save(DataSourceBase ds, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException("ACBMImage Write");
        }

        #endregion Public Methods
    }
}