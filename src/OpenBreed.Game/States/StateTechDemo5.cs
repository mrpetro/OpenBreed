﻿using OpenBreed.Core;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using OpenBreed.Core.Entities;
using OpenBreed.Game.Worlds;
using OpenBreed.Core.Modules.Animation.Systems.Control.Components;
using OpenBreed.Core.Modules.Animation.Systems.Movement.Components;
using OpenBreed.Game.Helpers;
using OpenBreed.Game.Entities.Actor;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Game.Entities.Door;

namespace OpenBreed.Game.States
{
    /// <summary>
    /// Tech Demo Class: Animation
    /// </summary>
    public class StateTechDemo5 : BaseState
    {
        #region Public Fields

        public const string ID = "TECH_DEMO_5";

        #endregion Public Fields

        #region Private Fields

        private static byte[] mapA = new byte[] {
            3,3,3,3,3,3,3,3,3,3,
            0,0,0,0,0,0,0,0,1,3,
            0,0,0,0,0,0,0,0,1,3,
            3,0,0,1,0,1,0,0,4,3,
            3,0,0,2,2,2,0,0,2,3,
            3,3,3,3,3,3,3,3,3,3
        };

        private ITexture tileTex;
        private ITexture spriteTex;
        private ITileAtlas tileAtlas;
        private ISpriteAtlas spriteAtlas;
        private Viewport gameViewport;

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo5(ICore core)
        {
            Core = core;

            InitializeWorld();
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public CameraEntity gameCamera { get; private set; }
        public CameraEntity hudCamera { get; private set; }

        public override string Id { get { return ID; } }

        #endregion Public Properties

        #region Public Methods

        public override void OnResize(Rectangle clientRectangle)
        {
            base.OnResize(clientRectangle);

            gameViewport.X = clientRectangle.X + 25;
            gameViewport.Y = clientRectangle.Y + 25;
            gameViewport.Width = clientRectangle.Width  - 50;
            gameViewport.Height = clientRectangle.Height - 50;
        }

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            Viewport hoverViewport = null;

            if (gameViewport.TestScreenCoords(Core.Inputs.CursorPos))
                hoverViewport = gameViewport;
            else
                hoverViewport = null;

            if (hoverViewport != null)
            {
                hoverViewport.Camera.Zoom = Tools.GetZoom(Core, hoverViewport.Camera.Zoom);

                if (mouseState.IsButtonDown(MouseButton.Middle))
                {
                    var transf = hoverViewport.Camera.GetTransform();
                    transf.Invert();
                    var delta4 = Vector4.Transform(transf, new Vector4(Core.Inputs.CursorDelta));
                    var delta2 = new Vector2(-delta4.X, -delta4.Y);
 
                    hoverViewport.Camera.Position.Value += delta2;
                }
             }
        }

        #endregion Public Methods

        #region Protected Methods

        private void Inputs_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            var pressedKey = e.Key.ToString();

            if (pressedKey.StartsWith("Number"))
            {
                pressedKey = pressedKey.Replace("Number", "");
                Core.StateMachine.SetNextState($"TECH_DEMO_{pressedKey}");
            }
        }

        protected override void OnEnter()
        {
            Core.Inputs.KeyDown += Inputs_KeyDown;

            Core.Rendering.Viewports.Add(gameViewport);

            Console.Clear();
            Console.WriteLine("---------- Entity groups --------");
            Console.WriteLine("This demo shows typical usage of fonts and texts on the screen.");
            Console.WriteLine("Constrols:");
            Console.WriteLine("RMB + Move mouse cursor = Camera control over hovered viewport");
            Console.WriteLine("Keyboard arrows  = Control arrow actor");
        }

        protected override void OnLeave()
        {
            Core.Rendering.Viewports.Remove(gameViewport);

            Core.Inputs.KeyDown -= Inputs_KeyDown;
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitializeWorld()
        {
            var gameWorld = new GameWorld(Core);

            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            tileTex = Core.Rendering.Textures.Create(@"Content\TileAtlasTest32bit.bmp");
            spriteTex = Core.Rendering.Textures.Create(@"Content\ArrowSpriteSet.png");
            tileAtlas = Core.Rendering.Tiles.Create(tileTex.Id, 16, 4, 4);
            spriteAtlas = Core.Rendering.Sprites.Create(spriteTex.Id, 32, 32, 8, 5);

            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            gameCamera = (CameraEntity)cameraBuilder.Build();
            gameWorld.AddEntity(gameCamera);


            gameViewport = (Viewport)Core.Rendering.Viewports.Create(50, 50, 540, 380);
            gameViewport.Camera = gameCamera;

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas(tileAtlas.Id);

            var actor = ActorHelper.CreateActor(Core, new Vector2(64, 288), spriteAtlas);
            actor.Add(new KeyboardControl(Key.Up, Key.Down, Key.Left, Key.Right));
            actor.Add(TextHelper.Create(Core, new Vector2(-10, 10), "Hero"));

   
            var actorSm = ActorHelper.CreateStateMachine(actor);
            actorSm.Initialize("Standing_Down");
            gameWorld.AddEntity(actor);

            var doorCollection = DoorHelper.CreateHorizontalDoor(Core, 3, 20, spriteAtlas, tileAtlas);

            foreach (var item in doorCollection)
                gameWorld.AddEntity(item);

            var rnd = new Random();

            var ymax = mapA.Length / 10;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < ymax; y++)
                {
                    var v = mapA[x + y * 10];

                    if (v > 0)
                    {
                        blockBuilder.SetIndices(x + 5, y + 5);
                        blockBuilder.SetTileId(v);
                        gameWorld.AddEntity(blockBuilder.Build());
                    }
                }
            }

            Core.Worlds.Add(gameWorld);
        }

        #endregion Private Methods
    }
}