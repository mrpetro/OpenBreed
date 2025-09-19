using OpenBreed.Animation.Interface;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Wecs.Entities;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Models
{
    public interface IAnimationEditorModel : IEditorModel
    {
        #region Public Properties

        float ClipLength { get; set; }

        IReadOnlyCollection<IDbAnimationTrack> Tracks { get; }

        IDbAnimationTrack EditedTrack { get; }

        #endregion Public Properties

        #region Public Methods

        void Edit(IDbAnimationTrack dbTrack);

        IReadOnlyClip<IEntity> Load(bool reload = false);

        #endregion Public Methods
    }
}