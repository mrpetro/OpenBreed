using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Models
{
    public interface IClipEditorModel : IEditorModel
    {
        #region Public Properties

        float ClipLength { get; }

        IReadOnlyCollection<IDbAnimationTrack> Tracks { get; }

        IDbAnimationTrack Track { get; }

        #endregion Public Methods
    }
}