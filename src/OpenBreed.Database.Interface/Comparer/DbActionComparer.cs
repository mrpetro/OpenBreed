using OpenBreed.Database.Interface.Items.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OpenBreed.Database.Interface.Comparer
{
    public class DbActionComparer : IEqualityComparer<IDbAction>
    {
        #region Public Properties

        public static IEqualityComparer<IDbAction> Instance { get; } = new DbActionComparer();

        #endregion Public Properties

        #region Public Methods

        public bool Equals(IDbAction x, IDbAction y)
        {
            if (!Equals(x.Id, y.Id))
                return false;

            if (!Equals(x.Name, y.Name))
                return false;

            if (!Equals(x.Description, y.Description))
                return false;

            if (!Equals(x.Presentation, y.Presentation))
                return false;

            if (!Equals(x.Triggers, y.Triggers))
                return false;

            return true;
        }

        public int GetHashCode([DisallowNull] IDbAction obj)
        {
            return obj.Id.GetHashCode() ^
                obj.Name.GetHashCode() ^
                obj.Description.GetHashCode() ^
                obj.Presentation.GetHashCode() ^
                obj.Triggers.GetHashCode();
        }

        #endregion Public Methods
    }
}