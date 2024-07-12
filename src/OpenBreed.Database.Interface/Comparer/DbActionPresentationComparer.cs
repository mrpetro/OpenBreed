using OpenBreed.Database.Interface.Items.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Comparer
{
    public class DbActionPresentationComparer : IEqualityComparer<IDbActionPresentation>
    {
        public static IEqualityComparer<IDbActionPresentation> Instance { get; } = new DbActionPresentationComparer();

        public bool Equals(IDbActionPresentation x, IDbActionPresentation y)
        {
            if (!Equals(x.Color, y.Color))
                return false;

            if (!Equals(x.Image, y.Image))
                return false;

            if (!Equals(x.Visibility, y.Visibility))
                return false;

            return true;
        }

        public int GetHashCode([DisallowNull] IDbActionPresentation obj)
        {
            return obj.Color.GetHashCode() ^
                obj.Image.GetHashCode() ^
                obj.Visibility.GetHashCode();
        }
    }
}
