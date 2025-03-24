using OpenTK.Mathematics;

namespace OpenBreed.Gui.Abstractions.Elements
{
    public interface IDesktop : IContainer
    {
        #region Public Properties

        public IReadOnlyCollection<IInteractionCursor> Cursors { get; }

        #endregion Public Properties
    }
}