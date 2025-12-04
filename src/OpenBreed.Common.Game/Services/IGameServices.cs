using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Common.Data;
using OpenBreed.Common.Game.Managers;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Services
{
    public interface IGameServices
    {
        ITriggerMan Triggers { get; }
        ILogger Logger { get; }
        IClipMan<IEntity> Clips { get; }
        IWorldMan Worlds { get; }   
        IEntityMan Entities { get; }
        IShapeMan Shapes { get; }
        IScriptMan Scripts { get; }
        IDataLoaderFactory DataLoaderFactory { get; }
        TextsDataProvider Texts { get; }
        ISoundMan Sounds { get; }
        IStampMan Stamps { get; }
        ItemsMan Items { get; }
    }
}
