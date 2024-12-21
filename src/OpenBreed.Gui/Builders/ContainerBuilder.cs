using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Builders
{
    internal abstract class ContainerBuilder : ElementBuilder, IContainerBuilder
    {
        #region Public Constructors

        public ContainerBuilder(IElementBuilder parentBuilder) : base(parentBuilder)
        {
        }

        #endregion Public Constructors

        #region Internal Properties

        internal List<ElementBuilder> ChildBuilders { get; } = new List<ElementBuilder>();

        #endregion Internal Properties

        #region Public Methods

        public IElementBuilder AddChild(IElementBuilder childBuilder)
        {
            ChildBuilders.Add((ElementBuilder)childBuilder);
            return this;
        }

        #endregion Public Methods
    }
}