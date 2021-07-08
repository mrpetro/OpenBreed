using System.Collections.ObjectModel;

namespace OpenBreed.Database.Interface.Items.Animations
{
    public interface IAnimationFrame
    {
        #region Public Properties

        float Time { get; set; }
        int ValueIndex { get; set; }

        #endregion Public Properties
    }
}