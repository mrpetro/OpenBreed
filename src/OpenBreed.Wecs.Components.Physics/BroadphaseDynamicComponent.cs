using OpenBreed.Common.Interface;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Physics
{
    public interface IBroadphaseDynamicComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        #endregion Public Properties
    }

    public class BroadphaseDynamicComponent : IEntityComponent
    {
        #region Public Constructors

        public BroadphaseDynamicComponent(BroadphaseDynamicComponentBuilder builder)
        {
            Dynamic = builder.Dynamic;
            ContactPairs = new List<ContactPair>();
        }

        #endregion Public Constructors

        #region Public Properties

        public IBroadphaseDynamic Dynamic { get; }

        public List<ContactPair> ContactPairs { get; }

        #endregion Public Properties
    }

    public class BroadphaseDynamicComponentBuilder : IBuilder<BroadphaseDynamicComponent>
    {
        #region Private Fields

        private readonly IBroadphaseFactory broadphaseFactory;

        #endregion Private Fields

        #region Internal Constructors

        internal BroadphaseDynamicComponentBuilder(IBroadphaseFactory broadphaseFactory)
        {
            this.broadphaseFactory = broadphaseFactory;


        }

        public BroadphaseDynamicComponentBuilder Set()
        {
            Dynamic = broadphaseFactory.CreateDynamic();
            return this;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IBroadphaseDynamic Dynamic { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public BroadphaseDynamicComponent Build()
        {
            return new BroadphaseDynamicComponent(this);
        }

        #endregion Public Methods
    }

    public sealed class BroadphaseDynamicComponentFactory : ComponentFactoryBase<IBroadphaseDynamicComponentTemplate>
    {
        #region Private Fields

        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Public Constructors

        public BroadphaseDynamicComponentFactory(IBuilderFactory builderFactory)
        {
            this.builderFactory = builderFactory;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IBroadphaseDynamicComponentTemplate template)
        {
            var builder = builderFactory.GetBuilder<BroadphaseDynamicComponentBuilder>();

            builder.Set();

            return builder.Build();
        }

        #endregion Protected Methods
    }
}