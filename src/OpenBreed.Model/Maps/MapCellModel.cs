using System;

namespace OpenBreed.Model.Maps
{
    public struct MapCellModel : IEquatable<MapCellModel>
    {
        #region Public Fields

        public readonly int[] Values;

        #endregion Public Fields

        #region Public Constructors

        public MapCellModel(int x, int y, int[] values)
        {
            X = x;
            Y = y;
            Values = values;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Y { get; }
        public int X { get; }

        #endregion Public Properties

        #region Public Methods

        public static bool operator !=(MapCellModel obj1, MapCellModel obj2)
        {
            return !(obj1 == obj2);
        }

        public static bool operator ==(MapCellModel obj1, MapCellModel obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;

            if (ReferenceEquals(obj1, null))
                return false;

            if (ReferenceEquals(obj2, null))
                return false;

            return (obj1.Y == obj2.Y
                    && obj1.X == obj2.X);
        }

        public bool Equals(MapCellModel other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Y.Equals(other.Y)
                   && X.Equals(other.X);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((MapCellModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Y.GetHashCode();
                hashCode = (hashCode * 397) ^ X.GetHashCode();
                return hashCode;
            }
        }

        #endregion Public Methods
    }
}