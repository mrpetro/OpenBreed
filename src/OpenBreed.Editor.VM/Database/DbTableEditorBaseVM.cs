using OpenBreed.Common;
using OpenBreed.Editor.VM.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public abstract class DbTableEditorBaseVM<R, VM> : DbTableEditorVM where VM : DbTableVM, new() where R : IRepository
    {
        //private IRepository<E> _repository;
    }
}
