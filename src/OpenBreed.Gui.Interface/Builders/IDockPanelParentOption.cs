using OpenBreed.Gui.Interface.Elements;

namespace OpenBreed.Gui.Interface.Builders
{
    public interface IDockPanelParentOption : IElementOption
    {
        #region Public Properties

        ElementDockMode Mode { get; }

        #endregion Public Properties
    }

    public class DockPanelParentOption : IDockPanelParentOption
    {
        #region Public Constructors

        public DockPanelParentOption(ElementDockMode mode)
        {
            Mode = mode;
        }

        #endregion Public Constructors

        #region Public Properties

        public ElementDockMode Mode { get; }

        #endregion Public Properties
    }
}