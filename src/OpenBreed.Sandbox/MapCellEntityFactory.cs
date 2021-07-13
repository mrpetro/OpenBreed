using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox
{
    internal class MapCellEntityFactory : IEntityFactory
    {
        public Entity Create(IEntityTemplate template)
        {
            throw new NotImplementedException();
        }

        public void RegisterComponentFactory<T>(IComponentFactory factory) where T : IComponentTemplate
        {
            throw new NotImplementedException();
        }
    }
}
