using OpenBreed.Core;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Jobs
{
    public class FpsTextUpdateJob : IJob
    {
        #region Private Fields

        private IEntity entity;

        #endregion Private Fields

        #region Public Constructors

        public FpsTextUpdateJob(IEntity entity)
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
            entity.PostMsg(new TextSetMsg(entity.Id, $"FPS: {entity.Core.Rendering.Fps.ToString("0.00")}"));
        }

        public void Dispose()
        {
        }

        #endregion Public Methods

    }
}
