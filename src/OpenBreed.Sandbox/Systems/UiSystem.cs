using OpenBreed.Core;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Systems
{
    public class UiSystem : WorldSystem
    {


        public UiSystem(ICore core) : base(core)
        {
        }

        protected override void RegisterEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
