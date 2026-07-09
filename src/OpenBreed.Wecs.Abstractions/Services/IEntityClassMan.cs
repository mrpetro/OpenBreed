using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Abstractions.Services
{
    public interface IEntityClass
    {
        #region Public Properties

        int Id { get; }
        string Name { get; }
        IEntityClass Parent { get; }
        IEnumerable<IEntityClass> Childs { get; }

        #endregion Public Properties
    }

    public interface IEntityClassMan
    {
        #region Public Properties

        IEntityClass RootClass { get; }

        #endregion Public Properties

        #region Public Methods

        IEntityClass GetById(int classId);

        IEntityClass GetByName(string name);

        #endregion Public Methods
    }
}