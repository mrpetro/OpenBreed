using OpenBreed.Wecs.Abstractions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components
{
    public interface IClassComponentTemplate : IComponentTemplate
    {
        string Name { get; }
    }

    [ComponentName("Class")]
    public class ClassComponent : IEntityComponent
    {
        public ClassComponent(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
