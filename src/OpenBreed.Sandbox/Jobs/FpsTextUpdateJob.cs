using OpenBreed.Core;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Rendering.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Entities;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems.Rendering.Extensions;

namespace OpenBreed.Sandbox.Jobs
{
    public class FpsTextUpdateJob : IJob
    {
        private readonly ICore core;
        #region Private Fields

        private Entity entity;

        #endregion Private Fields

        #region Public Constructors

        public FpsTextUpdateJob(ICore core, Entity entity)
        {
            this.core = core;
            this.entity = entity;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Execute()
        {
            //entity.PostMsg(new TextSetMsg(entity, $"FPS: {entity.Core.Rendering.Fps.ToString("0.00")}"));
        }

        public void Update(float dt)
        {
            entity.SetText(0, $"FPS: {core.GetManager<IRenderingMan>().Fps.ToString("0.00")}");
        }

        public void Dispose()
        {
        }

        #endregion Public Methods

    }
}
