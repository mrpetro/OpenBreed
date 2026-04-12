using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Common;

using OpenBreed.Common.Extensions;


using OpenBreed.Common.Interface.Tools;

using OpenBreed.Common.Windows.Extensions;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Events;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Extensions;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Gui.Abstractions;

using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;

using OpenBreed.Gui.Extensions;
using OpenBreed.Input.Generic.Extensions;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Generic.Extensions;
using OpenBreed.Physics.Generic.Shapes;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Common.Extensions;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Sandbox.App.Extensions;
using OpenBreed.Scripting.Lua.Extensions;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;

using System.IO;

using System.Reflection;
using static OpenBreed.Wecs.Control.Components.AnimationPlayerComponent;


namespace OpenBreed.Sandbox
{
    public class ProgramFactory
    {
        #region Private Fields

        private readonly IHostBuilder hostBuilder;

        #endregion Private Fields

        #region Public Constructors

        public ProgramFactory(IHostBuilder hostBuilder)
        {
            this.hostBuilder = hostBuilder;
        }

        #endregion Public Constructors

        #region Public Methods

        public ICore Create()
        {
            var appName = ProgramTools.AppProductName;
            var infoVersion = ProgramTools.AppInfoVerion;

            hostBuilder.SetupDataHandlers();

            hostBuilder.SetupCollisionVisualizingOptions();


            hostBuilder.SetupDataGridFactory();
            hostBuilder.SetupDefaultLogger();
            //hostBuilder.SetupXmlReadonlyDatabase();
            hostBuilder.ConfigureLogConsolePrinter();
            hostBuilder.SetupCoreManagers();
            hostBuilder.SetupOpenALServices();
            hostBuilder.SetupOpenGLServices();
            hostBuilder.SetupCommonRenderingServices();
            hostBuilder.ConfigureInteraction();
            hostBuilder.SetupGameWindowInputMan();
            hostBuilder.SetupGameWindow(640, 480, $"{appName} v{infoVersion}");
            hostBuilder.SetupGLWindow();
            hostBuilder.SetupWindowsDrawingContext();
            hostBuilder.SetupGLRenderContextComponents();
            hostBuilder.SetupCommonServices();
            hostBuilder.SetupDataLoaderFactory((dataLoaderFactory, sp) =>
            {
                dataLoaderFactory.RegisterGraphicsDataLoader(sp);
            });

            hostBuilder.SetupSandboxWecsSystems();

            hostBuilder.SetupShapeMan((shapeMan, sp) =>
            {
                shapeMan.Register("Shapes/Point_14_14", new PointShape(14, 14));
                shapeMan.Register("Shapes/Point_0_0", new PointShape(0, 0));
                shapeMan.Register("Shapes/Box_0_0_16_16", new BoxShape(0, 0, 16, 16));
                shapeMan.Register("Shapes/Box_16_16_8_8", new BoxShape(16, 16, 8, 8));
                shapeMan.Register("Shapes/Box_0_0_16_32", new BoxShape(0, 0, 16, 32));
                shapeMan.Register("Shapes/Box_0_0_32_16", new BoxShape(0, 0, 32, 16));
                shapeMan.Register("Shapes/Box_0_0_32_32", new BoxShape(0, 0, 32, 32));
                shapeMan.Register("Shapes/Box_-24_-24_48_48", new BoxShape(-24, -24, 48, 48));
                shapeMan.Register("Shapes/Box_0_0_28_28", new BoxShape(0, 0, 28, 28));
                shapeMan.Register("Shapes/Box_-14_-14_28_28", new BoxShape(-14, -14, 28, 28));
                shapeMan.Register("Shapes/Circle_0_0_240", new CircleShape(new Vector2(0, 0), 240));
                shapeMan.Register("Shapes/Circle_0_0_120", new CircleShape(new Vector2(0, 0), 120));
                shapeMan.Register("Shapes/Circle_0_0_480", new CircleShape(new Vector2(0, 0), 480));
                shapeMan.Register("Shapes/Circle_0_0_40", new CircleShape(new Vector2(0, 0), 40));
                shapeMan.Register("Shapes/Circle_0_0_320", new CircleShape(new Vector2(0, 0), 320));
                shapeMan.Register("Shapes/Circle_0_0_160", new CircleShape(new Vector2(0, 0), 160));
            });

            hostBuilder.SetupCollisionChecker();

            hostBuilder.SetupCollisionMan<IEntity>((collisionMan, sp) =>
            {

            });

            hostBuilder.SetupBroadphaseFactory<IEntity>();
            hostBuilder.SetupFixtureMan((s, a) => { });

            hostBuilder.SetupClipMan<IEntity>();

            hostBuilder.SetupFrameUpdaterMan<IEntity>((frameUpdaterMan, sp) =>
            {
                //new SpriteComponentAnimator(frameUpdaterMan, sp.GetService<ISpriteMan>(), sp.GetRequiredService<IDataLoaderFactory>());
            });

            hostBuilder.SetupFsmManager((fsmMan, sp) =>
            {
            });


            hostBuilder.SetupLuaScripting((scriptMan, sp) =>
            {

            });

            hostBuilder.SetupDefaultActionCodeProvider((codeProvider, sp) =>
            {
                //codeProvider.Register(PlayerActions.Fire);
            });

            var host = hostBuilder.Build();

            return new Program(host);
        }

        #endregion Public Methods
    }

    public class Data
    {
        private readonly ILogger logger;
        private bool checkboxTest = true;
        private float scrollTestHorizontal = 0.5f;
        private float scrollTestVertical = 0.5f;
        private string textBoxTest = "HelloWorld";


        public Data(ILogger logger)
        {
            this.logger = logger;
        }

        public bool CheckboxTest
        {
            get => checkboxTest;

            set
            {
                checkboxTest = value;
                logger.LogInformation($"Check field changed: {checkboxTest}");
            }
        }

        public float ScrollTestHorizontal
        {
            get => scrollTestHorizontal;

            set
            {
                scrollTestHorizontal = value;
                logger.LogInformation($"Horizontal scrollbar value changed: {scrollTestHorizontal}");
            }
        }

        public float ScrollTestVertical
        {
            get => scrollTestVertical;

            set
            {
                scrollTestVertical = value;
                logger.LogInformation($"Vertical scrollbar value changed: {scrollTestVertical}");
            }
        }

        public string TextBoxTest
        {
            get => textBoxTest;

            set
            {
                textBoxTest = value;
                logger.LogInformation($"Text changed: {textBoxTest}");
            }
        }
    }

    public class Program : CoreBase
    {
        #region Private Fields

        private Data data;
        private readonly IWindow window;
        private readonly IEventsMan eventsMan;
        private readonly IInputsMan inputsMan;
        private readonly IInteractionFactoryProvider interactionFactoryProvider;
        private readonly IElementFactory elementFactory;
        private IRenderView renderView;

        #endregion Private Fields

        #region Public Constructors

        public Program(IHost host) :
            base(host)
        {
            host.RunAsync();

            window = host.Services.GetRequiredService<IWindow>();
            eventsMan = host.Services.GetRequiredService<IEventsMan>();
            inputsMan = host.Services.GetRequiredService<IInputsMan>();
            interactionFactoryProvider = host.Services.GetRequiredService<IInteractionFactoryProvider>();
            elementFactory = host.Services.GetRequiredService<IElementFactory>();

            eventsMan.Subscribe<WindowLoadEvent>(OnWindowLoad);
            eventsMan.Subscribe<WindowUpdateEvent>(OnWindowUpdate);

            data = new Data(host.Services.GetRequiredService<ILogger>());
        }

        private void OnWindowUpdate(WindowUpdateEvent e)
        {
            e.Context.ServiceProvider.GetRequiredService<IWecsCore>().Update(e.Dt);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Run()
        {
            window.Run();
        }

        public override void Exit()
        {
            window.Exit();
        }

        #endregion Public Methods

        #region Private Methods

        [STAThread]
        private static void Main(string[] args)
        {
            ThreadTools.Initialize();

            var hostBuilder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddEnvironmentVariables();
            });

            hostBuilder.SetupDefaultLogger();

            var asm = Assembly.GetExecutingAssembly();

            var programFactory = new ProgramFactory(hostBuilder);

            var program = programFactory.Create();

            program.Run();
        }

        private void OnWindowLoad(WindowLoadEvent e)
        {
            var wecsCore = e.RenderContext.ServiceProvider.GetRequiredService<IWecsCore>();
            var world = wecsCore.Worlds.CreateSandboxWorld();
            renderView = e.RenderContext.CreateView();
            world.AddToView(renderView);

            var interactionFactory = interactionFactoryProvider.GetFactory(renderView);

            var desktop = interactionFactory.CreateDesktop(builder =>
            {


                //CreateButtonCtrlTest(builder);

                //CreateCheckboxCtrlTest(builder);

                //CreateLabelCtrlTest(builder);

            });

            //var form = elementFactory.Create<IElement>("CheckBox", data);

            //desktop.AddChild(form);

            desktop.AddChild(interactionFactory.CreateScrollbar((builder) =>
            {
                builder.SetMovable(false);

                builder.SetMode(Gui.Abstractions.Constants.ScrollbarMode.Horizontal);
                builder.SetValue(-100.0f);
                builder.SetMaximumSize(float.MaxValue, 16.0f);
                builder.SetMinimumValue(-100.0f);
                builder.SetMaximumValue(200.0f);
                builder.SetValueUnit(25.0f);
                builder.SetDockMode(ElementDockMode.Top);
                //builder.BindValue(PropertyBinding<float>.Create(data, (obj) => obj.ScrollTestVertical));
                //builder.SetGridPosition(2, 1);
            }));

            desktop.AddChild(interactionFactory.CreateScrollbar((builder) =>
            {
                builder.SetMovable(false);

                builder.SetMode(Gui.Abstractions.Constants.ScrollbarMode.Vertical);
                builder.SetMaximumSize(16.0f, float.MaxValue);
                builder.SetValue(-100.0f);
                builder.SetMinimumValue(-100.0f);
                builder.SetMaximumValue(200.0f);
                builder.SetValueUnit(25.0f);
                builder.SetDockMode(ElementDockMode.Right);
                //builder.BindValue(PropertyBinding<float>.Create(data, (obj) => obj.ScrollTestVertical));
                //builder.SetGridPosition(2, 1);
            }));


            //desktop.AddChild(CreateGridPanelTest(interactionFactory, data));

            //desktop.AddChild(interactionFactory.CreateCheckbox((builder) =>
            //{
            //    builder.SetPosition(80, 0);
            //    builder.SetLabel("Test1");

            //    builder.BindValue(PropertyBinding<bool>.Create(data, (obj) => obj.CheckboxTest));
            //}));

            //desktop.AddChild(interactionFactory.CreateTextField((builder) =>
            //{
            //    builder.SetFontSize(15);
            //    //var text = File.ReadAllText(@"Data//SampleText.txt");

            //    builder.BindProperty(PropertyBinding<string>.Create(data, (obj) => obj.TextBoxTest));

            //    //builder.SetText(text);
            //}));

        }

        private static IGridPanel CreateGridPanelTest(IInteractionFactory factory, Data data)
        {
            var grid = factory.CreateGridPanel(builder =>
            {
                builder.SetDockMode(ElementDockMode.Left);

                builder.SetMargin(5);

                builder.AddColumn(16);
                builder.AddColumn();
                builder.AddColumn(16);
                builder.AddRow(16);
                builder.AddRow();
                builder.AddRow(16);

                builder.SetTag("Form");
                builder.SetPosition(-300.0f, 300.0f);

                builder.SetMinimumSize(100, 100);
                builder.SetSize(300, 300);
                //builder.SetMaximumSize(500, 400);

                builder.SetMovable(true);
            });

            grid.AddChild(factory.CreateButton((builder) =>
                {
                    builder.SetMovable(true);

                    builder.SetTag("ResizeRightBottom");
                    builder.SetMoveCallback((element, offset) =>
                    {
                        if (element.IsMovable && element is IButton button && button.IsPressed)
                        {
                            var form = element.GetAncestor("Form");

                            if (form is not null)
                            {
                                form.ResizeBy(offset, ElementResizeAnchor.RightBottom);
                            }
                        }
                    });
                    builder.SetGridPosition(2, 0);
                }));

            grid.AddChild(factory.CreateScrollbar((builder) =>
            {
                builder.SetMovable(false);

                builder.SetMode(Gui.Abstractions.Constants.ScrollbarMode.Horizontal);
                builder.SetValue(2.0f);
                builder.SetMinimumValue(2.0f);
                builder.SetMaximumValue(8.0f);
                builder.SetValueUnit(5.0f);
                builder.BindValue(PropertyBinding<float>.Create(data, (obj) => obj.ScrollTestHorizontal));
                builder.SetGridPosition(1, 0);
            }));

            grid.AddChild(factory.CreateButton((builder) =>
                {
                    builder.SetMargin(5);

                    builder.SetMovable(true);

                    builder.SetTag("ResizeBottomLeft");
                    builder.SetMoveCallback((element, offset) =>
                    {
                        if (element.IsMovable && element is IButton button && button.IsPressed)
                        {
                            var form = element.GetAncestor("Form");

                            if (form is not null)
                            {
                                form.ResizeBy(offset, ElementResizeAnchor.BottomLeft);
                            }
                        }
                    });
                    builder.SetGridPosition(0, 0);
                }));


            grid.AddChild(factory.CreateScrollbar((builder) =>
            {
                builder.SetMovable(false);

                builder.SetMode(Gui.Abstractions.Constants.ScrollbarMode.Vertical);
                builder.SetValue(-100.0f);
                builder.SetMinimumValue(-100.0f);
                builder.SetMaximumValue(200.0f);
                builder.SetValueUnit(25.0f);
                builder.BindValue(PropertyBinding<float>.Create(data, (obj) => obj.ScrollTestVertical));
                builder.SetGridPosition(2, 1);
            }));

            grid.AddChild(factory.CreateTextField((builder) =>
                {
                    var text = File.ReadAllText(@"Data//SampleText.txt");
                    builder.SetText(text);

                    builder.SetGridPosition(1, 1);
                }));

            grid.AddChild(factory.CreateButton((builder) =>
                {
                    builder.SetMovable(true);

                    builder.SetTag("ResizeLeft");
                    builder.SetMoveCallback((element, offset) =>
                    {
                        if (element.IsMovable && element is IButton button && button.IsPressed)
                        {
                            var form = element.GetAncestor("Form");

                            if (form is not null)
                            {
                                form.ResizeBy(offset, ElementResizeAnchor.Left);
                            }
                        }
                    });
                    builder.SetGridPosition(0, 1);
                }));


            grid.AddChild(factory.CreateButton((builder) =>
                {
                    builder.SetMovable(true);

                    builder.SetTag("ResizeTopRight");
                    builder.SetMoveCallback((element, offset) =>
                    {
                        if (element.IsMovable && element is IButton button && button.IsPressed)
                        {
                            var form = element.GetAncestor("Form");

                            if (form is not null)
                            {
                                form.ResizeBy(offset, ElementResizeAnchor.TopRight);
                            }
                        }
                    });
                    builder.SetGridPosition(2, 2);
                }));

            grid.AddChild(factory.CreateButton((builder) =>
                {
                    builder.SetMovable(true);

                    builder.SetTag("ResizeTop");
                    builder.SetMoveCallback((element, offset) =>
                    {
                        if (element.IsMovable && element is IButton button && button.IsPressed)
                        {
                            var form = element.GetAncestor("Form");

                            if (form is not null)
                            {
                                form.ResizeBy(offset, ElementResizeAnchor.Top);
                            }
                        }
                    });
                    builder.SetGridPosition(1, 2);
                }));

            grid.AddChild(factory.CreateButton((builder) =>
                {
                    builder.SetMovable(true);

                    builder.SetTag("ResizeLeftTop");
                    builder.SetMoveCallback((element, offset) =>
                    {
                        if (element.IsMovable && element is IButton button && button.IsPressed)
                        {
                            var form = element.GetAncestor("Form");

                            if (form is not null)
                            {
                                form.ResizeBy(offset, ElementResizeAnchor.LeftTop);
                            }
                        }
                    });
                    builder.SetGridPosition(0, 2);
                }));

            return grid;
        }

        #endregion Private Methods
    }
}