using OpenBreed.Common.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Wecs.Attributes;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Components.Rendering
{
    public interface IPaletteComponentTemplate : IComponentTemplate
    {
    }


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

    [ComponentName("Palette")]
    public class PaletteComponent : IEntityComponent
    {
        #region Public Constructors

        public PaletteComponent()
        {
            PaletteId = -1;

            Colors = new Color4[256];
        }

        #endregion Public Constructors

        #region Public Properties

        public int PaletteId { get; set; }


        public Color4[] Colors { get; }

        #endregion Public Properties
    }


    public sealed class PaletteComponentFactory : ComponentFactoryBase<IPaletteComponentTemplate>
    {
        #region Private Fields

        private readonly IBuilderFactory builderFactory;
        private readonly IDataLoaderFactory dataLoaderFactory;

        #endregion Private Fields

        #region Public Constructors

        public PaletteComponentFactory()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IPaletteComponentTemplate template)
        {
            return new PaletteComponent();
        }

        #endregion Protected Methods
    }
}