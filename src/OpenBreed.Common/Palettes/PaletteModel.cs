using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Common.Palettes.Builders;

namespace OpenBreed.Common.Palettes
{
    public class PaletteModel
    {
        private static PaletteModel m_NullPalette = null;

        private Color[] m_Data = null;

        public string Name { get; set; }
        public Color[] Data { get { return m_Data; } }

        public static PaletteModel NullPalette
        {
            get
            {
                if (m_NullPalette == null)
                {
                    m_NullPalette = new PaletteModel();
                    m_NullPalette.LoadFromDefault();
                }

                return m_NullPalette;
            }
        }

        public void LoadFromDefault()
        {
            m_Data = DefaultPalette();
            Name = "Default";
        }

        public int Length
        {
            get
            {
                return Data.Length;
            }
        }

        public Color this[int index]
        {
            get
            {
                return Data[index];
            }

            set
            {
                Color oldValue = Data[index];

                if (oldValue == value)
                    return;

                Data[index] = value;
                //RaiseColorChanged(new PaletteColorChangedEventArgs(this,index, value));
            }
        }

        public PaletteModel(PaletteBuilder builder)
        {
            Name = builder.Name;
            m_Data = builder.Colors;
        }

        public PaletteModel()
        {
            LoadFromDefault();
        }

        public static Color[] DefaultPalette()
        {
            Color[] colors = new Color[256];

            for (int colorIndex = 0; colorIndex < colors.Length; colorIndex++)
                colors[colorIndex] = Color.FromArgb(colorIndex,colorIndex,colorIndex);

            return colors;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
