using OpenBreed.Core;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using System;

namespace OpenBreed.Sandbox.Jobs
{
    public class FpsTextUpdateJob : IJob
    {
        #region Private Fields

        private readonly IRenderingMan renderingMan;

        private Entity entity;

        #endregion Private Fields

        #region Public Constructors

        public FpsTextUpdateJob(IRenderingMan renderingMan, Entity entity)
        {
            this.renderingMan = renderingMan;
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
            entity.SetText(0, $"FPS: {renderingMan.Fps.ToString("0.00")}");
        }

        public void Dispose()
        {
        }

        #endregion Public Methods
    }
}