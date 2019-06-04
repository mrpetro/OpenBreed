using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public class ViewportMan : IViewportMan
    {
        #region Private Fields

        private readonly List<IViewport> items = new List<IViewport>();
        private readonly List<IViewport> toAdd = new List<IViewport>();
        private readonly List<IViewport> toRemove = new List<IViewport>();

        #endregion Private Fields

        #region Public Constructors

        public ViewportMan()
        {
            Items = new ReadOnlyCollection<IViewport>(items);
        }

        #endregion Public Constructors

        #region Public Properties

        public ReadOnlyCollection<IViewport> Items { get; }

        #endregion Public Properties

        #region Public Methods

        public void Remove(IViewport viewport)
        {
            if (toRemove.Contains(viewport))
                throw new InvalidOperationException("Viewport already pending removing.");

            if (!items.Contains(viewport))
                throw new InvalidOperationException("Viewport is not added.");

            items.Remove(viewport);
        }

        public void Add(IViewport viewport)
        {
            if (toAdd.Contains(viewport))
                throw new InvalidOperationException("Viewport already pending adding.");

            if (items.Contains(viewport))
                throw new InvalidOperationException("Viewport already added.");

            toAdd.Add(viewport);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Draw(float dt)
        {
            for (int i = 0; i < items.Count; i++)
                items[i].Draw();
        }

        internal void Cleanup()
        {
            if (toRemove.Any())
            {
                //Process entities to remove
                for (int i = 0; i < toRemove.Count; i++)
                {
                    items.Remove(toRemove[i]);
                }

                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process entities to add
                for (int i = 0; i < toAdd.Count; i++)
                {
                    items.Add(toAdd[i]);
                }

                toAdd.Clear();
            }
        }

        #endregion Internal Methods
    }
}