﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public interface IDrawingContextProvider
    {
        IDrawingContext FromImage(IBitmap bitmap);
    }
}
