using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Model.Maps
{
    public struct TileRef : IEquatable<TileRef>
    {

        #region Public Constructors

        public TileRef(int tileSetId, int tileId)
        {
            TileSetId = tileSetId;
            TileId = tileId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TileId { get; private set; }
        public int TileSetId { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static bool operator !=(TileRef obj1, TileRef obj2)
        {
            return !(obj1 == obj2);
        }

        public static bool operator ==(TileRef obj1, TileRef obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;

            if (ReferenceEquals(obj1, null))
                return false;

            if (ReferenceEquals(obj2, null))
                return false;

            return (obj1.TileId == obj2.TileId
                    && obj1.TileSetId == obj2.TileSetId);
        }

        public bool Equals(TileRef other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return TileId.Equals(other.TileId)
                   && TileSetId.Equals(other.TileSetId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((TileRef)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = TileId.GetHashCode();
                hashCode = (hashCode * 397) ^ TileSetId.GetHashCode();
                return hashCode;
            }
        }


        #endregion Public Methods
    }
}
