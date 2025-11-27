using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc;
using OpenBreed.Editor.UI.Mvc.Controllers;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Messages;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Factories;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Entities;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Animations
{
    public class AnimationPlayerVM : BaseViewModel
    {
        #region Private Fields

        private readonly IAnimationSandbox animationSandbox;

        #endregion Private Fields

        #region Public Constructors

        public AnimationPlayerVM(IAnimationSandbox animationSandbox)
        {
            this.animationSandbox = animationSandbox;

            PlayCommand = new Command(() => Play());
            StopCommand = new Command(() => Stop());
            PauseCommand = new Command(() => Pause());
            FastRewindCommand = new Command(() => FastRewind());
            FastForwardCommand = new Command(() => FastForward());
            ToBeginCommand = new Command(() => ToBegin());
            ToEndCommand = new Command(() => ToEnd());
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand PlayCommand { get; }

        public ICommand StopCommand { get; }

        public ICommand PauseCommand { get; }

        public ICommand FastRewindCommand { get; }

        public ICommand FastForwardCommand { get; }

        public ICommand ToBeginCommand { get; }

        public ICommand ToEndCommand { get; }

        #endregion Public Properties

        #region Private Methods

        private void Stop()
        {
            animationSandbox.StopAnimation();
        }

        private void Play()
        {
            animationSandbox.PlayAnimation();
        }

        private void Pause()
        {
            animationSandbox.PauseAnimation();
        }

        private void ToEnd()
        {
            animationSandbox.ToEndAnimation();
        }

        private void ToBegin()
        {
            animationSandbox.ToBeginAnimation();
        }

        private void FastForward()
        {
            animationSandbox.FastForwardAnimation();
        }

        private void FastRewind()
        {
            animationSandbox.FastRewindAnimation();
        }


        #endregion Private Methods
    }
}