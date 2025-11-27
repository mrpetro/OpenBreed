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

using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Managers;
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
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;

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
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using OpenBreed.Rendering.Common.Extensions;

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
            hostBuilder.SetupCommonRenderingServices();
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

            eventsMan.Subscribe<WindowUpdateEvent>((a) => OnUpdateFrame(a.Dt));
            eventsMan.Subscribe<WindowLoadEvent>(OnWindowLoad);

            data = new Data(host.Services.GetRequiredService<ILogger>());
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

        private void OnWindowLoad(WindowLoadEvent e)
        {
            renderView = e.RenderContext.CreateView();

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