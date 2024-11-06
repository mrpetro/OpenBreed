using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Builders
{
    internal abstract class InteractiveElementBuilder : IInteractiveElementBuilder
    {
        #region Internal Fields

        internal readonly List<InteractiveElementBuilder> childBuilders = new List<InteractiveElementBuilder>();
        internal string Tag;
        internal int Width;
        internal int Height;
        internal float CenterX;
        internal float CenterY;
        internal Action<IInteractiveElement> ClickCallback;

        #endregion Internal Fields

        #region Protected Fields

        protected InteractionCore interactionCore;

        #endregion Protected Fields

        #region Private Fields

        private readonly InteractiveElementBuilder parentBuilder;

        #endregion Private Fields

        #region Public Constructors

        public InteractiveElementBuilder(InteractionCore interactionCore, InteractiveElementBuilder parentBuilder)
        {
            this.interactionCore = interactionCore;
            this.parentBuilder = parentBuilder;
        }

        #endregion Public Constructors

        #region Public Methods

        public IInteractiveElement Build()
        {
            if (parentBuilder is not null)
            {
                throw new InvalidOperationException($"Invalid level of calling {nameof(Build)}()");
            }

            return InternalBuild();
        }

        public IInteractiveLabelBuilder BeginLabel()
        {
            return new InteractiveLabelBuilder(interactionCore, this);
        }

        public IInteractivePanelBuilder BeginPanel()
        {
            return new InteractivePanelBuilder(interactionCore, this);
        }

        public IInteractiveElementBuilder FinishElement()
        {
            parentBuilder.childBuilders.Add(this);
            return parentBuilder;
        }

        public IInteractiveElementBuilder SetClickCallback(Action<IInteractiveElement> callback)
        {
            ClickCallback = callback;
            return this;
        }

        public IInteractiveElementBuilder SetPosition(float x, float y)
        {
            CenterX = x;
            CenterY = y;

            return this;
        }

        public IInteractiveElementBuilder SetSize(int width, int height)
        {
            Width = width;
            Height = height;

            return this;
        }

        public IInteractiveElementBuilder SetTag(string tag)
        {
            Tag = tag;
            return this;
        }

        #endregion Public Methods

        #region Internal Methods

        internal abstract InteractiveElement InternalBuild();

        #endregion Internal Methods
    }
}