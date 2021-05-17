using OpenBreed.Database.Interface.Items.Sprites;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Database.Interface.Items.Animations
{
    public interface IAnimationEntry : IEntry
    {
        string AnimatorType { get; set; }

        ReadOnlyCollection<IArgument> AnimatorArguments { get; }

        ReadOnlyCollection<IAnimationFrame> Frames { get; }

        void AddFrame(int valueIndex, float frameTime);

        void ClearFrames();
    }
}