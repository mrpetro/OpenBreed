namespace OpenBreed.Common
{
    public abstract class ApplicationBase : IApplication
    {
        #region Private Fields

        protected readonly IManagerCollection managerCollection;

        #endregion Private Fields

        #region Protected Constructors

        protected ApplicationBase(IManagerCollection managerCollection)
        {
            this.managerCollection = managerCollection;
        }


        #endregion Protected Constructors
    }
}