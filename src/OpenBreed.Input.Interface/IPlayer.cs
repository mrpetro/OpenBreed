﻿using OpenTK.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
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
        int Id { get; }
        string Name { get; }
        ReadOnlyCollection<IPlayerInput> Inputs { get; }

        void RegisterInput(IPlayerInput input);
        void AddKeyBinding(string controlType, string controlAction, Keys key);
    }
}
