using OpenBreed.Common.Formats;

namespace OpenBreed.Common.Extensions
{
    public static class Setup
    {
        #region Public Methods

        public static void SetupABFormats(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<DataFormatMan>(() =>
            {
                var formatMan = new DataFormatMan();
                formatMan.RegisterFormat("ABSE_MAP", new ABSEMAPFormat());
                formatMan.RegisterFormat("ABHC_MAP", new ABHCMAPFormat());
                formatMan.RegisterFormat("ABTA_MAP", new ABTAMAPFormat());
                formatMan.RegisterFormat("ABTABLK", new ABTABLKFormat());
                formatMan.RegisterFormat("ABTASPR", new ABTASPRFormat());
                formatMan.RegisterFormat("ACBM_TILE_SET", new ACBMTileSetFormat());
                formatMan.RegisterFormat("ACBM_IMAGE", new ACBMImageFormat());
                formatMan.RegisterFormat("IFF_IMAGE", new IFFImageFormat());
                formatMan.RegisterFormat("BINARY", new BinaryFormat());
                formatMan.RegisterFormat("PCM_SOUND", new PCMSoundFormat());
                formatMan.RegisterFormat("TEXT", new TextFormat());
                return formatMan;
            });
        }

        #endregion Public Methods
    }
}