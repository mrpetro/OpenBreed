using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;

namespace OpenBreed.Common.Interface.Drawing
{
    public struct MyColor
    {
        #region Public Constructors

        static MyColor()
        {
            Black = MyColor.FromArgb(255, 0, 0, 0);
            White = MyColor.FromArgb(255, 255, 255, 255);
            Empty = MyColor.FromArgb(0, 0, 0, 0);
            LightGreen = FromHex("FF90EE90");
            LightBlue = FromHex("FFADD8E6");
            Red = FromHex("FFFF0000");
        }

        #endregion Public Constructors

        #region Private Constructors

        private MyColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        #endregion Private Constructors

        #region Public Properties

        public static MyColor Black { get; }
        public static MyColor White { get; }
        public static MyColor LightGreen { get; set; }
        public static MyColor LightBlue { get; set; }
        public static MyColor Red { get; set; }
        public static MyColor Empty { get; }

        public byte R { get; }

        public byte G { get; }

        public byte B { get; }

        public byte A { get; }

        #endregion Public Properties

        #region Public Methods

        public static MyColor FromArgb(byte a, byte r, byte g, byte b)
        {
            return new MyColor(a, r, g, b);
        }

        public static MyColor FromArgb(byte r, byte g, byte b)
        {
            return new MyColor(255, r, g, b);
        }

        public static bool operator ==(MyColor left, MyColor right) =>
            left.A == right.A
                && left.R == right.R
                && left.G == right.G
                && left.B == right.B;

        public static bool operator !=(MyColor left, MyColor right) => !(left == right);

        public static MyColor FromHex(string hex)
        {
            var c = new MyColor(0, 0, 0, 0);

            // empty color
            if (string.IsNullOrEmpty(hex))
            {
                return c;
            }

            if (hex.Length != 8)
            {
                if (hex.Length != 9 || hex[0] != '#')
                {
                    return c;
                }

                hex = hex.TrimStart('#');
            }

            c = MyColor.FromArgb(Convert.ToByte(hex.Substring(0, 2), 16),
                                Convert.ToByte(hex.Substring(2, 2), 16),
                                Convert.ToByte(hex.Substring(4, 2), 16),
                                Convert.ToByte(hex.Substring(6, 2), 16));

            return c;
        }

        public override bool Equals(object obj) => obj is MyColor other && Equals(other);

        public bool Equals(MyColor other) => this == other;

        public override int GetHashCode()
        {
            return HashCode.Combine(A.GetHashCode(), R.GetHashCode(), G.GetHashCode(), B.GetHashCode());
        }

        public int ToArgb()
        {
            return 0;
        }

        #endregion Public Methods
    }
}