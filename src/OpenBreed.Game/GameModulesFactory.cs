﻿using OpenBreed.Core;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game
{
    internal class GameModulesFactory : ICoreModulesFactory
    {
        public IAudioModule CreateAudioModule(ICore core)
        {
            return new OpenALModule(core);
        }

        public IInputModule CreateInputModule(ICore core)
        {
            throw new NotImplementedException();
        }

        public IPhysicsModule CreatePhysicsModule(ICore core)
        {
            throw new NotImplementedException();
        }

        public IScriptingModule CreateScriptingModule(ICore core)
        {
            throw new NotImplementedException();
        }

        public IRenderModule CreateVideoModule(ICore core)
        {
            return new OpenGLModule(core);
        }
    }
}
