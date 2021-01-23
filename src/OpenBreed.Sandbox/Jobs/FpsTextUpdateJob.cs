using OpenBreed.Core;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Rendering.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Sandbox.Jobs
{
    public class FpsTextUpdateJob : IJob
    {
        #region Private Fields

        private Entity entity;

        #endregion Private Fields

        #region Public Constructors

        public FpsTextUpdateJob(Entity entity)
        {
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
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, $"FPS: {entity.Core.GetModule<IRenderModule>().Fps.ToString("0.00")}"));
        }

        public void Dispose()
        {
        }

        #endregion Public Methods

    }
}
