﻿using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenTK;
using OpenTK.Graphics;

namespace OpenBreed.Core.Modules.Rendering
{
    /// <summary>
    /// Core module interface specialized in graphical rendering related work
    /// </summary>
    public interface IRenderModule : ICoreModule
    {
        #region Public Properties

        /// <summary>
        /// Get current rendering frames per second
        /// </summary>
        float Fps { get; }

        /// <summary>
        /// Textures manager
        /// </summary>
        ITextureMan Textures { get; }

        /// <summary>
        /// Sprite manager
        /// </summary>
        ISpriteMan Sprites { get; }

        /// <summary>
        /// Tile manager
        /// </summary>
        ITileMan Tiles { get; }

        /// <summary>
        /// Stamp manager
        /// </summary>
        IStampMan Stamps { get; }

        /// <summary>
        /// Font manager
        /// </summary>
        IFontMan Fonts { get; }

        /// <summary>
        /// World from which rendering will start
        /// </summary>
        World ScreenWorld { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create wireframe render component
        /// </summary>
        /// <param name="thickness">Thickness of wireframe lines</param>
        /// <param name="color">Color of wireframe lines</param>
        /// <returns></returns>
        IWireframe CreateWireframe(float thickness, Color4 color);

        void Cleanup();

        void Draw(float dt);

        #endregion Public Methods
    }
}