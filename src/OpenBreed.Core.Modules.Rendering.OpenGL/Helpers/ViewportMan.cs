using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Modules.Rendering.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    internal class ViewportMan : IViewportMan
    {
        #region Private Fields

        private readonly List<IViewport> viewports = new List<IViewport>();
        private readonly List<IViewport> toAdd = new List<IViewport>();
        private readonly List<IViewport> toRemove = new List<IViewport>();

        #endregion Private Fields

        #region Public Constructors

        internal ViewportMan(OpenGLModule module)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
            Items = new ReadOnlyCollection<IViewport>(viewports);
        }

        #endregion Public Constructors

        #region Public Properties

        public ReadOnlyCollection<IViewport> Items { get; }

        #endregion Public Properties

        #region Internal Properties

        internal OpenGLModule Module { get; }

        #endregion Internal Properties

        #region Public Methods

        public IViewport Create(float x, float y, float width, float height, float order = 0.0f)
        {
            return new Viewport(Module.Core, x, y, width, height, order);
        }

        public void Remove(IViewport viewport)
        {
            if (toRemove.Contains(viewport))
                throw new InvalidOperationException("Viewport already pending removing.");

            if (!viewports.Contains(viewport))
                throw new InvalidOperationException("Viewport is not added.");

            viewports.Remove(viewport);
        }

        public void Add(IViewport viewport)
        {
            if (toAdd.Contains(viewport))
                throw new InvalidOperationException("Viewport already pending adding.");

            if (viewports.Contains(viewport))
                throw new InvalidOperationException("Viewport already added.");

            toAdd.Add(viewport);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Draw(float dt)
        {


            for (int i = 0; i < viewports.Count; i++)
                viewports[i].Render(dt);
        }

        internal void Cleanup()
        {
            if (toRemove.Any())
            {
                //Process entities to remove
                for (int i = 0; i < toRemove.Count; i++)
                {
                    viewports.Remove(toRemove[i]);
                }

                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process entities to add
                for (int i = 0; i < toAdd.Count; i++)
                    viewports.Add(toAdd[i]);

                viewports.Sort((a, b) => { return a.Order.CompareTo(b.Order); });

                toAdd.Clear();
            }
        }

        #endregion Internal Methods
    }
}