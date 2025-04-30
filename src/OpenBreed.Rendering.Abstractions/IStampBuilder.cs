using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Rendering.Abstractions
{
    public interface IStampBuilder
    {
        ITileStamp Build();
        IStampBuilder SetName(string name);
        IStampBuilder SetSize(int width, int height);
        IStampBuilder SetOrigin(int originX, int originY);
        IStampBuilder ClearTiles();
        IStampBuilder AddTile(int x, int y, int atlasId, int tileId);
    }
}