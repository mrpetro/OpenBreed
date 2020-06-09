using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.FpsCounter;
using OpenTK;
using OpenTK.Input;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Worlds
{
    public static class TextWorldHelper
    {

        #region Public Methods

        public static void Create(Program core)
        {


            var builder = core.Worlds.Create().SetName("TEXT");

            AddSystems(core, builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private static void AddSystems(Program core, WorldBuilder builder)
        {
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
            builder.AddSystem(new TextPresenterSystem(core));
        }

        private static void Setup(World world)
        {
            //((Program)world.Core).KeyDown += (s, a) => ProcessKey(world, a);
            //((Program)world.Core).KeyPress += (s, a) => AddChar(world, a);


            var cameraBuilder = new CameraBuilder(world.Core);
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetFov(world.Core.ClientRectangle.Width, world.Core.ClientRectangle.Height);
            var hudCamera = cameraBuilder.Build();
            hudCamera.Tag = "HudCamera";
            world.PostCommand(new AddEntityCommand(world.Id, hudCamera.Id));

            var caret = TextHelper.CreateCaret(world);
            ((Program)world.Core).KeyDown += (s, a) => ProcessKey(caret, a);
            ((Program)world.Core).KeyPress += (s, a) => AddChar(caret, a);


            caret.Subscribe<TextCaretPositionChanged>(OnTextCaretPositionChanged);
            caret.Subscribe<TextDataChanged>(OnTextDataChanged);

            world.PostCommand(new AddEntityCommand(world.Id, caret.Id));

            //TextHelper.Create(world, "hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhT");

            var hudViewport = world.Core.Entities.GetByTag(ScreenWorldHelper.TEXT_VIEWPORT).First();
            hudViewport.GetComponent<ViewportComponent>().CameraEntityId = hudCamera.Id;

            //world.Core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s, a) => UpdateFpsPos(fpsTextEntity, (ClientResizedEventArgs)a));
            //world.Core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s, a) => UpdateCameraFov(hudCamera, (ClientResizedEventArgs)a));

            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateCameraFov(hudCamera, a));
        }

        private static void OnTextCaretPositionChanged(object sender, TextCaretPositionChanged e)
        {
            var entity = sender as IEntity;
            var dataCmp = entity.GetComponent<TextDataComponent>();

            Console.Clear();

            var text = dataCmp.Data;
            text = text.Insert(e.Position, "|");

            Console.WriteLine($"{e.Position}: {text}");
        }

        private static void OnTextDataChanged(object sender, TextDataChanged e)
        {
            var entity = sender as IEntity;

            var caretCmp = entity.GetComponent<TextCaretComponent>();

            Console.Clear();

            var text = e.Text;
            text = text.Insert(caretCmp.Position, "|");

            Console.WriteLine($"{caretCmp.Position}: {text}");
        }

        private static void AddChar(IEntity caret, KeyPressEventArgs a)
        {
            caret.PostCommand(new TextDataInsert(caret.Id, a.KeyChar.ToString()));
        }

        private static void ProcessKey(IEntity caret, KeyboardKeyEventArgs a)
        {
            var caretCmp = caret.GetComponent<TextCaretComponent>();

            switch (a.Key)
            {
                case Key.Left:
                    caret.PostCommand(new TextCaretSetPosition(caret.Id, caretCmp.Position - 1));
                    break;
                case Key.Right:
                    caret.PostCommand(new TextCaretSetPosition(caret.Id, caretCmp.Position + 1));
                    break;
                default:
                    break;
            }

            if(a.Key == Key.BackSpace)
                caret.PostCommand(new TextDataBackspace(caret.Id));
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

        private static void UpdateCameraFov(IEntity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.GetComponent<CameraComponent>().Width = a.Width;
            cameraEntity.GetComponent<CameraComponent>().Height = a.Height;
        }

        #endregion Private Methods

    }
}