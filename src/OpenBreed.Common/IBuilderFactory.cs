using System;

namespace OpenBreed.Common
{
    public interface IBuilderFactory
    {
        #region Public Methods

        TBuilder GetBuilder<TBuilder>();

        void Register<TBuilder>(Func<IBuilder> initializer);

        #endregion Public Methods
    }
}