using Microsoft.Extensions.Logging;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OpenTK.Windowing.Common;
using System.Net;
using OpenBreed.Input.Abstractions;
using System.Windows.Controls;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Abstractions.Events;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Gui.Abstractions;
using System.Diagnostics;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Extensions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Constants;

namespace RendererTest.Wpf.App.VM
{
    public class RendererVm : BaseViewModel
    {
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;
        private readonly IInteractionFactory interactionFactory;
        private readonly IInteractionRenderer interactionRenderer;

        private bool checkboxTest;

        public bool CheckboxTest {
            get
            {
                return checkboxTest;
            }

            set
            {
                checkboxTest = value;

                logger.LogInformation($"Checked {checkboxTest}");
            }
        }


        #region Private Fields

        private readonly Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> renderContextProvider;
        private Vector2i cursorPos;
        private Vector2i cursorDelta;
        private IRenderView cursorView;
        private bool cursorScroll;

        private IRenderContext renderContext;
        private IRenderView renderView;
        private IRenderView renderView2;

        #endregion Private Fields

        #region Public Constructors

        public RendererVm(IEventsMan eventsMan, 
            ILogger logger,
            IInteractionFactory interactionCore,
            IInteractionRenderer interactionRenderer,
            Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> renderContextProvider)
        {
            this.eventsMan = eventsMan;
            this.logger = logger;
            this.interactionFactory = interactionCore;
            this.interactionRenderer = interactionRenderer;
            this.renderContextProvider = renderContextProvider;

            InitFunc = OnInitialize;
        }

        #endregion Public Constructors

        #region Public Properties

        public Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> InitFunc { get; }

        #endregion Public Properties

        #region Private Methods

        private IElement CreateButtonCtrlTest(IInteractionFactory factory)
        {
            return factory.CreateDockPanel((builder) =>
            {
                builder.SetPosition(0.0f, 300.0f);
                builder.SetSize(300, 300);
                builder.SetMovable(true);
                builder.SetMargin(0);

                builder.SetTag("ButtonTestPanel");

                factory.CreateDockPanel((builder) =>
                {
                    builder.SetPosition(0.0f, 0.0f);
                    builder.SetSize(100, 50);
                    builder.SetMovable(true);
                    builder.SetMargin(0);

                    factory.CreateButton((builder) =>
                    {
                        builder.SetTag("Ok");
                        builder.SetClickCallback((element, cursor, key) =>
                        {
                            logger.LogTrace("Button '{ElementTag}' clicked.", element.Tag);
                        });
                        builder.SetDockMode(ElementDockMode.Fill);
                    });

                    factory.CreateLabelField((builder) =>
                    {
                        builder.SetHitTestable(false);
                        builder.SetHorizontalAlignment(HorizontalAlignment.Center);
                        builder.SetVerticalAlignment(VerticalAlignment.Center);
                        builder.SetText("Ok");
                        builder.SetDockMode(ElementDockMode.Fill);
                    });
                });
            });
        }

        private void CreateCheckboxCtrlTest(IInteractionFactory factory)
        {
            factory.CreateCheckbox((builder) =>
            {
                builder.SetTag("MyCheckbox");
                builder.SetPosition(300.0f, 0.0f);
                builder.SetSize(300, 300);
                builder.SetLabel("This is my checkbox");
                builder.BindValue(PropertyBinding<bool>.Create(this, (obj) => obj.CheckboxTest));

            });

            factory.CreateCheckbox((builder) =>
            {
                builder.SetTag("MyCheckbox");
                builder.SetPosition(300.0f, 40.0f);
                builder.SetSize(300, 300);
                builder.SetLabel("This is my checkbox 3");
                builder.BindValue(PropertyBinding<bool>.Create(this, (obj) => obj.CheckboxTest));

            });
        }

        private static void CreateLabelCtrlTest(IInteractionFactory factory)
        {
            factory.CreateDockPanel((builder) =>
            {
                builder.SetTag("LabelTest");
                builder.SetSize(300, 300);
                builder.SetPosition(-300.0f, 0.0f);

                factory.CreateLabelField((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Left);
                    builder.SetVerticalAlignment(VerticalAlignment.Top);
                    builder.SetText("Left-Top");
                    builder.SetTag("LT");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                factory.CreateLabelField((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Center);
                    builder.SetVerticalAlignment(VerticalAlignment.Top);
                    builder.SetText("Center-Top");
                    builder.SetTag("CT");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                factory.CreateLabelField((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Right);
                    builder.SetVerticalAlignment(VerticalAlignment.Top);
                    builder.SetText("Right-Top");
                    builder.SetTag("RT");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                factory.CreateLabelField((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Left);
                    builder.SetVerticalAlignment(VerticalAlignment.Center);
                    builder.SetText("Left-Center");
                    builder.SetTag("LC");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                factory.CreateLabelField((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Center);
                    builder.SetVerticalAlignment(VerticalAlignment.Center);
                    builder.SetText("Center-Center");
                    builder.SetTag("CC");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                factory.CreateLabelField((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Right);
                    builder.SetVerticalAlignment(VerticalAlignment.Center);
                    builder.SetText("Right-Center");
                    builder.SetTag("RC");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                factory.CreateLabelField((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Left);
                    builder.SetVerticalAlignment(VerticalAlignment.Bottom);
                    builder.SetText("Left-Bottom");
                    builder.SetTag("LB");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                factory.CreateLabelField((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Center);
                    builder.SetVerticalAlignment(VerticalAlignment.Bottom);
                    builder.SetText("Center-Bottom");
                    builder.SetTag("CB");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                factory.CreateLabelField((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Right);
                    builder.SetVerticalAlignment(VerticalAlignment.Bottom);
                    builder.SetText("Right-Bottom");
                    builder.SetTag("RB");
                    builder.SetDockMode(ElementDockMode.Fill);
                });
            });
        }

        private IRenderContext OnInitialize(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter)
        {
            renderContext = renderContextProvider.Invoke(graphicsContext, hostCoordinateSystemConverter);

            renderView = renderContext.CreateView(0.0f, 0.0f, 1.0f, 1.0f);


            var element = interactionFactory.CreateDesktop(builder =>
            {
                interactionFactory.CreateDockPanel(builder =>
                {
                    builder.SetTag("1");
                    builder.SetDockMode(ElementDockMode.Left);
                    builder.SetPosition(0.0f, 0.0f);
                    builder.SetSize(450, 450);
                    builder.SetMovable(true);
                    builder.SetPadding(10);
                    builder.SetMargin(10);
                    builder.SetClickCallback(ButtonClicked);



                    interactionFactory.CreateDockPanel(builder =>
                    {
                        builder.SetTag("2");
                        builder.SetDockMode(ElementDockMode.Left);
                        builder.SetPosition(0.0f, 0.0f);
                        builder.SetSize(225, 225);
                        builder.SetMovable(true);
                        builder.SetPadding(10);
                        builder.SetMargin(10);
                        builder.SetClickCallback(ButtonClicked);



                        interactionFactory.CreateDockPanel(builder =>
                        {
                            builder.SetTag("3");
                            builder.SetDockMode(ElementDockMode.Left);
                            builder.SetPosition(0.0f, 0.0f);
                            builder.SetSize(110, 110);
                            builder.SetMovable(true);
                            builder.SetPadding(10);
                            builder.SetMargin(10);
                            builder.SetClickCallback(ButtonClicked);

                        });

                    });
                });


                interactionFactory.CreateDockPanel(builder =>
                {
                    builder.SetTag("4");
                    builder.SetDockMode(ElementDockMode.Fill);
                    builder.SetPosition(0.0f, 0.0f);
                    builder.SetSize(110, 110);
                    builder.SetMovable(true);
                    builder.SetPadding(10);
                    builder.SetMargin(10);
                    builder.SetClickCallback(ButtonClicked);


                    interactionFactory.CreateDockPanel(builder =>
                    {
                        builder.SetTag("5");
                        builder.SetDockMode(ElementDockMode.Left);
                        builder.SetPosition(0.0f, 0.0f);
                        builder.SetSize(110, 110);
                        builder.SetMovable(true);
                        builder.SetPadding(10);
                        builder.SetMargin(10);
                        builder.SetClickCallback(ButtonClicked);

                    });

                    interactionFactory.CreateDockPanel(builder =>
                    {
                        builder.SetTag("6");
                        builder.SetDockMode(ElementDockMode.Fill);
                        builder.SetPosition(0.0f, 0.0f);
                        builder.SetSize(110, 110);
                        builder.SetMovable(true);
                        builder.SetPadding(10);
                        builder.SetMargin(10);
                        builder.SetClickCallback(ButtonClicked);

                    });


                });

                //CreateGridPanelTest(builder);

                //CreateButtonCtrlTest(builder);

                //CreateCheckboxCtrlTest(builder);

                //CreateLabelCtrlTest(builder);

            });

            return renderContext;
        }

        private static IElement CreateGridPanelTest(IInteractionFactory factory)
        {
            return factory.CreateGridPanel(builder =>
            {
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
                builder.SetMaximumSize(500, 400);

                builder.SetMovable(true);

                factory.CreateButton((builder) =>
                {
                    builder.SetMovable(true);

                    builder.SetTag("ResizeRightBottom");
                    builder.SetMoveCallback((element, offset) =>
                    {
                    if (element.IsMovable &&  element is IButton button && button.IsPressed)
                        {
                            var form = element.GetAncestor("Form");

                            if (form is not null)
                            {
                                form.ResizeBy(offset, ElementResizeAnchor.RightBottom);
                            }
                        }
                    });
                    builder.SetGridPosition(2, 0);
                });

                factory.CreateButton((builder) =>
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
                });

                factory.CreateButton((builder) =>
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
                });


                factory.CreateButton((builder) =>
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
                });

                factory.CreateButton((builder) =>
                {
                    builder.SetMovable(true);

                    builder.SetTag("ResizeCenter");
                    builder.SetMoveCallback((element, offset) =>
                    {
                        if (element.IsMovable && element is IButton button && button.IsPressed)
                        {
                            var form = element.GetAncestor("Form");

                            if (form is not null)
                            {
                                form.ResizeBy(offset, ElementResizeAnchor.Center);
                            }
                        }
                    });
                    builder.SetGridPosition(1, 1);
                });

                factory.CreateButton((builder) =>
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
                });


                factory.CreateButton((builder) =>
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
                });

                factory.CreateButton((builder) =>
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
                });

                factory.CreateButton((builder) =>
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
                });



            });
        }

        private void ButtonClicked(IElement interactiveElement, IInteractionCursor cursor, CursorKey cursorKey)
        {


            Debug.WriteLine($"Interactive element '{interactiveElement.Tag}' clicked.");
        }

        #endregion Private Methods
    }
}