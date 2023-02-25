namespace OpenBreed.Wecs.Components.Rendering
{
    public struct PaletteColor
    {
        #region Public Fields

        public byte R;
        public byte G;
        public byte B;

        #endregion Public Fields

        #region Public Constructors

        public PaletteColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public override string ToString()
        {
            return $"{R}, {G}, {B}";
        }


        #endregion Public Constructors
    }

    public class PaletteComponent : IEntityComponent
    {
        #region Public Constructors

        public PaletteComponent()
        {
            Colors = new PaletteColor[256];
        }

        #endregion Public Constructors

        #region Public Properties

        public PaletteColor[] Colors { get; }

        public int PaletteId { get; set; }

        #endregion Public Properties
    }
}