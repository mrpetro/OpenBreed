using OpenBreed.Database.Interface.Items.Sprites;
using System.Collections.ObjectModel;

namespace OpenBreed.Database.Interface.Items.Animations
{
    public interface IAnimationEntry : IEntry
    {
        string ValueSetRef { get; set; }

        ReadOnlyCollection<IAnimationFrame> Frames { get; }

        void AddFrame(int valueIndex, float frameTime);

        void ClearFrames();
    }
}