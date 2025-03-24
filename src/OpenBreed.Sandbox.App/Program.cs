using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Windows.Extensions;
using OpenBreed.Core;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model;
using OpenBreed.Model.Extensions;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Sprites;

using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Extensions;

using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using OpenBreed.Rendering.Interface.Extensions;
using OpenBreed.Core.Interface;
using OpenBreed.Core.Interface.Managers;

using OpenBreed.Common.Interface.Tools;
using OpenBreed.Sandbox.App.Extensions;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Extensions;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Gui.Abstractions.Elements;
using System.Diagnostics;
using System.Windows.Input;
using OpenBreed.Input.Generic.Extensions;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Gui.Abstractions.Builders;

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

            hostBuilder.SetupDataGridFactory();
            hostBuilder.SetupDefaultLogger();
            hostBuilder.ConfigureLogConsolePrinter();
            hostBuilder.SetupCoreManagers();
            hostBuilder.SetupOpenGLManagers();
            hostBuilder.ConfigureInteraction();
            hostBuilder.SetupGameWindowInputMan();

            hostBuilder.SetupGameWindow(640, 480, $"{appName} v{infoVersion}");
            hostBuilder.SetupGLWindow();
            hostBuilder.SetupWindowsDrawingContext();

            hostBuilder.SetupDataLoaderFactory((dataLoaderFactory, sp) =>
            {
                dataLoaderFactory.RegisterGraphicsDataLoader(sp);
            });

            var host = hostBuilder.Build();

            return new Program(host);
        }

        #endregion Public Methods
    }

    public class Program : CoreBase
    {
        #region Private Fields

        private readonly IWindow window;
        private readonly IEventsMan eventsMan;
        private readonly IInputsMan inputsMan;
        private readonly IInteractionFactoryProvider interactionFactoryProvider;
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

            eventsMan.Subscribe<WindowUpdateEvent>((a) => OnUpdateFrame(a.Dt));
            eventsMan.Subscribe<WindowLoadEvent>(OnWindowLoad);
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

        private void OnUpdateFrame(float dt)
        {
            inputsMan.Update();
        }
        private Win WinRoot;
        private Win winA;
        private Win winB;
        private Win winC;
        private Win winD;
        private Vector2 posA = new Vector2(40, 40);
        private Vector2 posB = new Vector2(60, -20);
        private Vector2 posC = new Vector2(30, -40);

        private void OnRenderFrame(IRenderView view, Matrix4 transform, float dt)
        {
            if (inputsMan.IsKeyPressed((int)OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
            {
                Dumper.Trigger();
            }

            if (inputsMan.IsLeftMousePressed)
            {
                winA.Pos += inputsMan.CursorDelta * new Vector2(1, -1);
            }

            if (inputsMan.IsRightMousePressed)
            {
                winB.Pos += inputsMan.CursorDelta * new Vector2(1, -1);
            }

            if (inputsMan.IsMiddleMousePressed)
            {
                winC.Pos += inputsMan.CursorDelta * new Vector2(1, -1);
            }

            if (inputsMan.IsLeftMousePressed)
            {
                posA += inputsMan.CursorDelta * new Vector2(1, -1);
            }

            if (inputsMan.IsRightMousePressed)
            {
                posB += inputsMan.CursorDelta * new Vector2(1, -1);
            }

            if (inputsMan.IsMiddleMousePressed)
            {
                posC += inputsMan.CursorDelta * new Vector2(1, -1);
            }

            //view.RenderWinScissor(WinRoot);
            //view.RenderWinStencil(WinRoot);


            //view.RenderTest(posA, posB, posC);

            //interactionRenderer.Render(interactionCore, view);
        }

        private void ButtonClicked(IElement interactiveElement, IInteractionCursor cursor, CursorKey cursorKey)
        {


            Debug.WriteLine($"Interactive element '{interactiveElement.Tag}' clicked.");
        }

        private void OnWindowLoad(WindowLoadEvent e)
        {
            WinRoot = new Win(new Box2(0, 0, 500, 500), new Vector2(-5, -5), Color4.Red, Color.Yellow);

            winA = new Win(new Box2(200, 200, 600, 300), new Vector2(-5, -5), Color4.Green, Color.Pink)
            {
                Pos = new Vector2(40, 40)
            };


            winB = new Win(new Box2(0, 0, 300, 300), new Vector2(-5, -5), Color4.Blue, Color.Honeydew)
            {
                Pos = new Vector2(60, -20)
            };

            winC = new Win(new Box2(200, 200, 300, 300), new Vector2(-5, -5), Color4.Gray, Color.ForestGreen)
            {
                Pos = new Vector2(30, -40)
            };

            winD = new Win(new Box2(30, 30, 200, 200), new Vector2(-5, -5), Color4.DarkBlue, Color.Beige)
            {
                Pos = new Vector2(0, 0)
            };


            winB.Childs.Add(winC);
            winD.Childs.Add(winA);

            WinRoot.Childs.Add(winB);
            WinRoot.Childs.Add(winD);

            renderView = e.RenderContext.CreateView();

            var interactionFactory = interactionFactoryProvider.GetFactory(renderView);

            var desktop = interactionFactory.CreateDesktop(builder =>
            {


                //CreateButtonCtrlTest(builder);

                //CreateCheckboxCtrlTest(builder);

                //CreateLabelCtrlTest(builder);

            });

            desktop.AddChild(CreateGridPanelTest(interactionFactory));
        }

        private static IGridPanel CreateGridPanelTest(IInteractionFactory factory)
        {
            var grid = factory.CreateGridPanel(builder =>
            {
                builder.SetDockMode(ElementDockMode.Left);

                builder.SetMargin(5);

                builder.AddColumn(25);
                builder.AddColumn();
                builder.AddColumn(25);
                builder.AddRow(25);
                builder.AddRow();
                builder.AddRow(25);

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

            grid.AddChild(factory.CreateButton((builder) =>
                {
                    builder.SetMargin(5);

                    builder.SetMovable(true);

                    builder.SetTag("ResizeBottom");
                    builder.SetMoveCallback((element, offset) =>
                    {
                        if (element.IsMovable && element is IButton button && button.IsPressed)
                        {
                            var form = element.GetAncestor("Form");

                            if (form is not null)
                            {
                                form.ResizeBy(offset, ElementResizeAnchor.Bottom);
                            }
                        }
                    });
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


            grid.AddChild(factory.CreateButton((builder) =>
                {
                    builder.SetMovable(true);

                    builder.SetTag("ResizeRight");
                    builder.SetMoveCallback((element, offset) =>
                    {
                        if (element.IsMovable && element is IButton button && button.IsPressed)
                        {
                            var form = element.GetAncestor("Form");

                            if (form is not null)
                            {
                                form.ResizeBy(offset, ElementResizeAnchor.Right);
                            }
                        }
                    });
                    builder.SetGridPosition(2, 1);
                }));

            grid.AddChild(factory.CreateTextBox((builder) =>
                {
                    builder.SetFontSize(15);
                    builder.SetMovable(false);

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