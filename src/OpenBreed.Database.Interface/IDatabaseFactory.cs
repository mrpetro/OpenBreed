using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface
{
    public interface IDatabaseFactory
    {
        IUnitOfWork CreateDatabase();
    }
}
