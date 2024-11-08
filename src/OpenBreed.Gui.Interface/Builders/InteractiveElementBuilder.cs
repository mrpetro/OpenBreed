using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Interface.Elements;

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

        public IInteractiveElementBuilder AddLabel(Action<IInteractiveLabelBuilder> setter)
        {
            var builder = new InteractiveLabelBuilder(interactionCore, this);

            setter.Invoke(builder);

            return builder.FinishElement();
        }

        public IInteractiveElementBuilder AddPanel(Action<IInteractivePanelBuilder> setter)
        {
            var builder = new InteractivePanelBuilder(interactionCore, this);

            setter.Invoke(builder);

            return builder.FinishElement();
        }

        public void SetClickCallback(Action<IInteractiveElement> callback)
        {
            ClickCallback = callback;
        }

        public void SetPosition(float x, float y)
        {
            CenterX = x;
            CenterY = y;
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void SetTag(string tag)
        {
            Tag = tag;
        }

        #endregion Public Methods

        #region Internal Methods

        internal abstract InteractiveElement InternalBuild();

        #endregion Internal Methods

        #region Private Methods

        private IInteractiveElementBuilder FinishElement()
        {
            parentBuilder.childBuilders.Add(this);
            return parentBuilder;
        }

        #endregion Private Methods
    }
}