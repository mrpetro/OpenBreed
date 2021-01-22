using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Ecsw;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using OpenTK.Graphics;
using System;

namespace OpenBreed.Rendering.Interface
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

        void Cleanup();

        void Draw(float dt);

        void OnClientResized(float width, float height);
        void DrawUnitRectangle();
        void DrawRectangle(Box2 clipBox);
        void DrawBox(Box2 clipBox);
        void DrawUnitBox();

        #endregion Public Methods
    }
}