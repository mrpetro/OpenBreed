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
using OpenBreed.Editor.UI.Mvc.Models;
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
    public class AnimationCurvesEditorToolsVM : BaseViewModel
    {
        #region Private Fields

        private readonly AnimationCurvesEditorController controller;
        private bool setAddKeyFramesModeEnabled;
        private bool setSelectionModeEnabled;

        #endregion Private Fields

        #region Public Constructors

        public AnimationCurvesEditorToolsVM(AnimationCurvesEditorController controller)
        {
            this.controller = controller ?? throw new ArgumentNullException(nameof(controller));
            SetAddKeyFramesModeCommand = new Command(() => SetAddKeyFramesMode());
            SetSelectionModeCommand = new Command(() => SetSelectionMode());
            FocusCommand = new Command(() => Focus());

            SetSelectionMode();
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand SetAddKeyFramesModeCommand { get; }

        public ICommand SetSelectionModeCommand { get; }

        public ICommand FocusCommand { get; }

        public bool SetAddKeyFramesModeEnabled
        {
            get { return setAddKeyFramesModeEnabled; }
            set { SetProperty(ref setAddKeyFramesModeEnabled, value); }
        }

        public bool SetSelectionModeEnabled
        {
            get { return setSelectionModeEnabled; }
            set { SetProperty(ref setSelectionModeEnabled, value); }
        }

        #endregion Public Properties

        #region Private Methods

        private void Focus()
        {
            controller.Focus();
        }

        private void SetSelectionMode()
        {
            controller.SetMode(AnimationCurvesEditorMode.SelectKeyFrames);
            SetAddKeyFramesModeEnabled = true;
            SetSelectionModeEnabled = false;
        }

        private void SetAddKeyFramesMode()
        {
            controller.SetMode(AnimationCurvesEditorMode.InsertKeyFrames);
            SetAddKeyFramesModeEnabled = false;
            SetSelectionModeEnabled = true;
        }

        #endregion Private Methods
    }
}