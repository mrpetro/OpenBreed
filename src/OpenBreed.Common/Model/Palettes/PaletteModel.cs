using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using OpenBreed.Common.Builders.Palettes;

namespace OpenBreed.Common.Model.Palettes
{
    public class PaletteModel
    {

        #region Private Fields

        private static PaletteModel m_NullPalette = null;
        private Color[] m_Data = null;

        #endregion Private Fields

        #region Public Constructors

        public PaletteModel(PaletteBuilder builder)
        {
            Name = builder.Name;
            m_Data = builder.Colors;
        }

        public PaletteModel()
        {
            LoadFromDefault();
        }

        #endregion Public Constructors

        #region Public Properties

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

        public Color[] Data { get { return m_Data; } }
        public int Length
        {
            get
            {
                return Data.Length;
            }
        }

        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        #endregion Public Properties

        #region Public Indexers

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

        #endregion Public Indexers

        #region Public Methods

        public static Color[] DefaultPalette()
        {
            Color[] colors = new Color[256];

            for (int colorIndex = 0; colorIndex < colors.Length; colorIndex++)
                colors[colorIndex] = Color.FromArgb(colorIndex, colorIndex, colorIndex);

            return colors;
        }

        public void LoadFromDefault()
        {
            m_Data = DefaultPalette();
            Name = "Default";
        }
        public override string ToString()
        {
            return Name;
        }

        #endregion Public Methods

    }
}
