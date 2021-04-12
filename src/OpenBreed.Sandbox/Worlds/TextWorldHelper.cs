using OpenBreed.Core;
using OpenBreed.Core.Events;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Input;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Worlds
{
    public class TextWorldHelper
    {
        private readonly ICore core;
        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly ISystemFactory systemFactory;
        private readonly IInputsMan inputsMan;
        private readonly IFontMan fontMan;
        private readonly ICommandsMan commandsMan;
        private readonly IViewClient viewClient;

        public TextWorldHelper(ICore core, IEntityMan entityMan, IWorldMan worldMan, ISystemFactory systemFactory, IInputsMan inputsMan, IFontMan fontMan, ICommandsMan commandsMan, IViewClient viewClient)
        {
            this.core = core;
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.systemFactory = systemFactory;
            this.inputsMan = inputsMan;
            this.fontMan = fontMan;
            this.commandsMan = commandsMan;
            this.viewClient = viewClient;
        }

        #region Public Methods

        public void Create()
        {


            var builder = worldMan.Create().SetName("TEXT");

            AddSystems(builder);

            Setup(builder.Build(core));
        }

        #endregion Public Methods

        #region Private Methods

        private void AddSystems(WorldBuilder builder)
        {
            //Input
            //builder.AddSystem(core.CreateWalkingControlSystem().Build());
            //builder.AddSystem(core.CreateAiControlSystem().Build());

            //Action
            //builder.AddSystem(core.CreateMovementSystem().Build());
            builder.AddSystem(systemFactory.Create<AnimationSystem>());

            ////Audio
            //builder.AddSystem(core.CreateSoundSystem().Build());

            //Video
            //builder.AddSystem(core.CreateTileSystem().SetGridSize(64, 64)
            //                               .SetLayersNo(1)
            //                               .SetTileSize(16)
            //                               .SetGridVisible(false)
            //                               .Build());

            builder.AddSystem(systemFactory.Create<TextInputSystem>());
            builder.AddSystem(systemFactory.Create<TextPresenterSystem>());
        }

        private void Setup(World world)
        {
            //((Program)world.Core).KeyDown += (s, a) => ProcessKey(world, a);
            //((Program)world.Core).KeyPress += (s, a) => AddChar(world, a);

            var cameraBuilder = core.GetManager<CameraBuilder>();
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetFov(viewClient.ClientRectangle.Width, viewClient.ClientRectangle.Height);
            var hudCamera = cameraBuilder.Build();
            hudCamera.Tag = "HudCamera";
            commandsMan.Post(new AddEntityCommand(world.Id, hudCamera.Id));

            var caret = TextHelper.CreateText(entityMan, fontMan, world);

            inputsMan.KeyDown += (s, a) => ProcessKey(caret, a);
            inputsMan.KeyPress += (s, a) => AddChar(caret, a);

            //caret.Subscribe<TextCaretPositionChanged>(OnTextCaretPositionChanged);
            //caret.Subscribe<TextDataChanged>(OnTextDataChanged);

            commandsMan.Post(new AddEntityCommand(world.Id, caret.Id));


            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.TEXT_VIEWPORT).First();
            hudViewport.Get<ViewportComponent>().CameraEntityId = hudCamera.Id;

            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateCameraFov(hudCamera, a));
        }

        private void OnTextCaretPositionChanged(object sender, TextCaretPositionChanged e)
        {
            var entity = sender as Entity;
            var dataCmp = entity.Get<TextDataComponent>();

            Console.Clear();

            var text = dataCmp.Data;
            text = text.Insert(e.Position, "|");

            Console.WriteLine($"{e.Position}: {text}");
        }

        private void OnTextDataChanged(object sender, TextDataChanged e)
        {
            var entity = sender as Entity;

            var caretCmp = entity.Get<TextCaretComponent>();

            Console.Clear();

            var text = e.Text;
            text = text.Insert(caretCmp.Position, "|");

            Console.WriteLine($"{caretCmp.Position}: {text}");
        }

        private void AddChar(Entity caret, KeyPressEventArgs a)
        {
            commandsMan.Post(new TextDataInsert(caret.Id, a.KeyChar.ToString()));
        }

        private void ProcessKey(Entity entity, KeyboardKeyEventArgs a)
        {
            var caretCmp = entity.Get<TextCaretComponent>();

            switch (a.Key)
            {
                case Key.Left:
                    commandsMan.Post(new TextCaretSetPosition(entity.Id, caretCmp.Position - 1));
                    break;
                case Key.Right:
                    commandsMan.Post(new TextCaretSetPosition(entity.Id, caretCmp.Position + 1));
                    break;
                case Key.Enter:
                    commandsMan.Post(new TextDataInsert(entity.Id, "\r\n"));
                    break;
                default:
                    break;
            }

            if(a.Key == Key.BackSpace)
                commandsMan.Post(new TextDataBackspace(entity.Id));
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