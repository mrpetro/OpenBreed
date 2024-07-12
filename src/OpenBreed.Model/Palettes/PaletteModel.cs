using OpenBreed.Common.Interface.Drawing;
using System;
using System.Linq;

namespace OpenBreed.Model.Palettes
{
    public class PaletteModel
    {
        #region Public Constructors

        static PaletteModel()
        {
            NullPalette = new PaletteModel();
            NullPalette.LoadFromDefault();
        }

        public PaletteModel(PaletteBuilder builder)
        {
            Name = builder.Name;
            Data = builder.Colors;
        }

        #endregion Public Constructors

        #region Private Constructors

        private PaletteModel()
        {
            LoadFromDefault();
        }

        #endregion Private Constructors

        #region Public Properties

        public static PaletteModel NullPalette { get; }

        public MyColor[] Data { get; private set; }

        public int Length => Data.Length;

        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        #endregion Public Properties

        #region Public Indexers

        public MyColor this[int index]
        {
            get
            {
                return Data[index];
            }

            set
            {
                var oldValue = Data[index];

                if (oldValue == value)
                    return;

                Data[index] = value;
                //RaiseColorChanged(new PaletteColorChangedEventArgs(this,index, value));
            }
        }

        #endregion Public Indexers

        #region Public Methods

        public MyColor[] GetColors(int startIndex, int size)
        {
            return Data.Skip(startIndex).Take(size).ToArray();
        }

        public void SetColors(int startIndex, MyColor[] sourceArray)
        {
            if (startIndex + sourceArray.Length > Data.Length)
                throw new InvalidOperationException("Start index and colors array length are surpassing size of palette.");

            for (int i = 0; i < sourceArray.Length; i++)
            {
                Data[startIndex + i] = sourceArray[i];
            }
        }

        public static MyColor[] DefaultPalette()
        {
            var colors = new MyColor[256];

            for (int colorIndex = 0; colorIndex < colors.Length; colorIndex++)
                colors[colorIndex] = MyColor.FromArgb((byte)colorIndex, (byte)colorIndex, (byte)colorIndex);

            return colors;
        }

        public void LoadFromDefault()
        {
            Data = DefaultPalette();
            Name = "Default";
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion Public Methods
    }
}