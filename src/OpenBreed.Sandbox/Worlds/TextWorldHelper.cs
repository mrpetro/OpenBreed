using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.FpsCounter;
using OpenTK;
using OpenTK.Input;
using System;
using System.Linq;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Commands;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Sandbox.Worlds
{
    public static class TextWorldHelper
    {

        #region Public Methods

        public static void Create(Program core)
        {


            var builder = core.GetManager<IWorldMan>().Create().SetName("TEXT");

            AddSystems(core, builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private static void AddSystems(Program core, WorldBuilder builder)
        {
            var systemFactory = core.GetManager<ISystemFactory>();

            //Input
            //builder.AddSystem(core.CreateWalkingControlSystem().Build());
            //builder.AddSystem(core.CreateAiControlSystem().Build());

            //Action
            //builder.AddSystem(core.CreateMovementSystem().Build());
            builder.AddSystem(core.CreateAnimationSystem().Build());

            ////Audio
            //builder.AddSystem(core.CreateSoundSystem().Build());

            //Video
            //builder.AddSystem(core.CreateTileSystem().SetGridSize(64, 64)
            //                               .SetLayersNo(1)
            //                               .SetTileSize(16)
            //                               .SetGridVisible(false)
            //                               .Build());

            //builder.AddSystem(core.CreateSpriteSystem().Build());
            builder.AddSystem(new TextInputSystem(core));
            builder.AddSystem(systemFactory.Create<TextPresenterSystem>());
        }

        private static void Setup(World world)
        {
            //((Program)world.Core).KeyDown += (s, a) => ProcessKey(world, a);
            //((Program)world.Core).KeyPress += (s, a) => AddChar(world, a);

            var windowClient = world.Core.GetManager<ICoreClient>();
            var cameraBuilder = new CameraBuilder(world.Core);
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetFov(windowClient.ClientRectangle.Width, windowClient.ClientRectangle.Height);
            var hudCamera = cameraBuilder.Build();
            hudCamera.Tag = "HudCamera";
            world.Core.Commands.Post(new AddEntityCommand(world.Id, hudCamera.Id));

            var caret = TextHelper.CreateText(world);
            var inputs = world.Core.GetManager<IInputsMan>();

            inputs.KeyDown += (s, a) => ProcessKey(caret, a);
            inputs.KeyPress += (s, a) => AddChar(caret, a);

            //caret.Subscribe<TextCaretPositionChanged>(OnTextCaretPositionChanged);
            //caret.Subscribe<TextDataChanged>(OnTextDataChanged);

            world.Core.Commands.Post(new AddEntityCommand(world.Id, caret.Id));


            var hudViewport = world.Core.GetManager<IEntityMan>().GetByTag(ScreenWorldHelper.TEXT_VIEWPORT).First();
            hudViewport.Get<ViewportComponent>().CameraEntityId = hudCamera.Id;

            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateCameraFov(hudCamera, a));
        }

        private static void OnTextCaretPositionChanged(object sender, TextCaretPositionChanged e)
        {
            var entity = sender as Entity;
            var dataCmp = entity.Get<TextDataComponent>();

            Console.Clear();

            var text = dataCmp.Data;
            text = text.Insert(e.Position, "|");

            Console.WriteLine($"{e.Position}: {text}");
        }

        private static void OnTextDataChanged(object sender, TextDataChanged e)
        {
            var entity = sender as Entity;

            var caretCmp = entity.Get<TextCaretComponent>();

            Console.Clear();

            var text = e.Text;
            text = text.Insert(caretCmp.Position, "|");

            Console.WriteLine($"{caretCmp.Position}: {text}");
        }

        private static void AddChar(Entity caret, KeyPressEventArgs a)
        {
            caret.Core.Commands.Post(new TextDataInsert(caret.Id, a.KeyChar.ToString()));
        }

        private static void ProcessKey(Entity entity, KeyboardKeyEventArgs a)
        {
            var caretCmp = entity.Get<TextCaretComponent>();

            switch (a.Key)
            {
                case Key.Left:
                    entity.Core.Commands.Post(new TextCaretSetPosition(entity.Id, caretCmp.Position - 1));
                    break;
                case Key.Right:
                    entity.Core.Commands.Post(new TextCaretSetPosition(entity.Id, caretCmp.Position + 1));
                    break;
                case Key.Enter:
                    entity.Core.Commands.Post(new TextDataInsert(entity.Id, "\r\n"));
                    break;
                default:
                    break;
            }

            if(a.Key == Key.BackSpace)
                entity.Core.Commands.Post(new TextDataBackspace(entity.Id));
        }

        private static char KeyToChar(KeyboardKeyEventArgs e)
        {
            var str = e.Key.ToString();

            if (str.Length == 1)
            {
                return e.Shift ? str[0] : str.ToLower()[0];
            }
            else if (str.StartsWith("Number") || str.StartsWith("Keypad") && str.Length == 7)
            {
                return str[6];
            }

            switch (e.Key)
            {
                case Key.BackSlash:
                    return '\\';
                case Key.BracketLeft:
                    return '(';
                case Key.BracketRight:
                    return ')';
                case Key.Comma:
                    return ',';
                case Key.Space:
                    return ' ';
            }

            return '\0';
        }

        private static void UpdateCameraFov(Entity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.Get<CameraComponent>().Width = a.Width;
            cameraEntity.Get<CameraComponent>().Height = a.Height;
        }

        #endregion Private Methods

    }
}