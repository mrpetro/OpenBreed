﻿using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface.Renderers
{
    public interface IRenderer<TObject>
    {
        void Render(TObject obj, IRenderView view);
    }
}
