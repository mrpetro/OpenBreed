using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Models
{
    public class AnimationEditorModel : IAnimationEditorModel
    {
        #region Private Fields

        private readonly IDataLoaderFactory dataLoaderFactory;

        private readonly IDbAnimation dbAnimation;

        #endregion Private Fields

        #region Public Constructors

        public AnimationEditorModel(IDataLoaderFactory dataLoaderFactory, IDbAnimation dbAnimation)
        {
            this.dataLoaderFactory = dataLoaderFactory ?? throw new ArgumentNullException(nameof(dataLoaderFactory));
            this.dbAnimation = dbAnimation ?? throw new ArgumentNullException(nameof(dbAnimation));
        }

        #endregion Public Constructors

        #region Public Properties

        public float ClipLength
        {
            get => dbAnimation.Length;
            set
            {
                if (dbAnimation.Length == value)
                {
                    return;
                }

                dbAnimation.Length = value;

                Load(reload: true);
            }
        }

        public IReadOnlyCollection<IDbAnimationTrack> Tracks => dbAnimation.Tracks;

        public IDbAnimationTrack EditedTrack { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Edit(IDbAnimationTrack dbTrack)
        {
            EditedTrack = dbTrack;
        }

        public IReadOnlyClip<IEntity> Load(bool reload = false)
        {
            var clipLoader = dataLoaderFactory.GetLoader<IAnimationClipDataLoader<IEntity>>();

            return clipLoader.Load(dbAnimation, reload);
        }

        #endregion Public Methods
    }
}