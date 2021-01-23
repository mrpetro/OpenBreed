using OpenBreed.Wecs.Entities;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Input.Interface
{
    public interface IPlayer
    {
        string Name { get; }
        ReadOnlyCollection<Entity> ControlledEntities { get; }
        ReadOnlyCollection<IPlayerInput> Inputs { get; }

        void RegisterInput(IPlayerInput input);
        void AddKeyBinding(string controlType, string controlAction, Key key);
        void AssumeControl(Entity entity);
    }
}
