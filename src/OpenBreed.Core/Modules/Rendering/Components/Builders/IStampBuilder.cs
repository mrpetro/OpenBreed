using OpenBreed.Core.Modules.Rendering.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.Modules.Rendering.Components.Builders
{
    public interface IStampBuilder
    {
        ITileStamp Build();
        void SetName(string name);
        void SetSize(int width, int height);
        void SetOrigin(int originX, int originY);
        void ClearTiles();
        void AddTile(int x, int y, int tileId);
    }
}