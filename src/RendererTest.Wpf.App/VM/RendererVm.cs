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
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.Interface;
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
using OpenBreed.Input.Interface;
using System.Windows.Controls;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Events;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Gui.Abstractions;
using System.Diagnostics;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Extensions;
using OpenBreed.Gui.Abstractions.Builders;

namespace RendererTest.Wpf.App.VM
{
    public class RendererVm : BaseViewModel
    {
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;
        private readonly IInteractionCore interactionCore;
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
            IInteractionCore interactionCore,
            IInteractionRenderer interactionRenderer,
            Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> renderContextProvider)
        {
            this.eventsMan = eventsMan;
            this.logger = logger;
            this.interactionCore = interactionCore;
            this.interactionRenderer = interactionRenderer;
            this.renderContextProvider = renderContextProvider;

            InitFunc = OnInitialize;


            cursorInput = interactionCore.CreateCursorInput();


            eventsMan.Subscribe<ViewCursorMoveEvent>(OnCursorMove);
            eventsMan.Subscribe<ViewCursorDownEvent>(OnCursorDown);
            eventsMan.Subscribe<ViewCursorUpEvent>(OnCursorUp);
            eventsMan.Subscribe<ViewCursorEnterEvent>(OnCursorEnter);
            eventsMan.Subscribe<ViewCursorLeaveEvent>(OnCursorLeave);
            eventsMan.Subscribe<ViewCursorWheelEvent>(OnCursorWheel);
        }

        #endregion Public Constructors

        #region Public Properties




        public Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> InitFunc { get; }

        private readonly ICursorInputHandler cursorInput;

        #endregion Public Properties

        #region Private Methods

        private void CreateButtonCtrlTest(IContainerBuilder builder)
        {
            builder.AddDockPanel((builder) =>
            {
                builder.SetPosition(0.0f, 300.0f);
                builder.SetSize(300, 300);
                builder.SetMovable(true);
                builder.SetMargin(0);

                builder.SetTag("ButtonTestPanel");

                builder.AddDockPanel((builder) =>
                {
                    builder.SetPosition(0.0f, 0.0f);
                    builder.SetSize(100, 50);
                    builder.SetMovable(true);
                    builder.SetMargin(0);

                    builder.AddButton((builder) =>
                    {
                        builder.SetTag("Ok");
                        builder.SetClickCallback((element) =>
                        {
                            logger.LogTrace("Button '{ElementTag}' clicked.", element.Tag);
                        });
                        builder.SetDockMode(ElementDockMode.Fill);
                    });

                    builder.AddLabel((builder) =>
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

        private void CreateCheckboxCtrlTest(IContainerBuilder builder)
        {
            builder.AddCheckbox((builder) =>
            {
                builder.SetTag("MyCheckbox");
                builder.SetPosition(300.0f, 0.0f);
                builder.SetSize(300, 300);
                builder.SetLabel("This is my checkbox");
                builder.BindIsChecked(PropertyBinding<bool>.Create(this, (obj) => obj.CheckboxTest));

            });

            builder.AddCheckbox((builder) =>
            {
                builder.SetTag("MyCheckbox");
                builder.SetPosition(300.0f, 40.0f);
                builder.SetSize(300, 300);
                builder.SetLabel("This is my checkbox 3");
                builder.BindIsChecked(PropertyBinding<bool>.Create(this, (obj) => obj.CheckboxTest));

            });
        }

        private static void CreateLabelCtrlTest(IContainerBuilder builder)
        {
            builder.AddDockPanel((builder) =>
            {
                builder.SetTag("LabelTest");
                builder.SetSize(300, 300);
                builder.SetPosition(-300.0f, 0.0f);

                builder.AddLabel((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Left);
                    builder.SetVerticalAlignment(VerticalAlignment.Top);
                    builder.SetText("Left-Top");
                    builder.SetTag("LT");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                builder.AddLabel((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Center);
                    builder.SetVerticalAlignment(VerticalAlignment.Top);
                    builder.SetText("Center-Top");
                    builder.SetTag("CT");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                builder.AddLabel((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Right);
                    builder.SetVerticalAlignment(VerticalAlignment.Top);
                    builder.SetText("Right-Top");
                    builder.SetTag("RT");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                builder.AddLabel((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Left);
                    builder.SetVerticalAlignment(VerticalAlignment.Center);
                    builder.SetText("Left-Center");
                    builder.SetTag("LC");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                builder.AddLabel((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Center);
                    builder.SetVerticalAlignment(VerticalAlignment.Center);
                    builder.SetText("Center-Center");
                    builder.SetTag("CC");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                builder.AddLabel((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Right);
                    builder.SetVerticalAlignment(VerticalAlignment.Center);
                    builder.SetText("Right-Center");
                    builder.SetTag("RC");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                builder.AddLabel((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Left);
                    builder.SetVerticalAlignment(VerticalAlignment.Bottom);
                    builder.SetText("Left-Bottom");
                    builder.SetTag("LB");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                builder.AddLabel((builder) =>
                {
                    builder.SetHorizontalAlignment(HorizontalAlignment.Center);
                    builder.SetVerticalAlignment(VerticalAlignment.Bottom);
                    builder.SetText("Center-Bottom");
                    builder.SetTag("CB");
                    builder.SetDockMode(ElementDockMode.Fill);
                });

                builder.AddLabel((builder) =>
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

            renderView = renderContext.CreateView(OnRender1, 0.0f, 0.0f, 1.0f, 1.0f);


            var element = interactionCore.CreateDockPanel(builder =>
            {
                builder.SetTag("Form");
                builder.SetPosition(0.0f, 0.0f);
                builder.SetSize(900, 900);
                builder.SetMovable(true);
                builder.SetPadding(10);
                builder.SetMargin(10);


                builder.AddDockPanel(builder =>
                {
                    builder.SetTag("1");
                    builder.SetDockMode(ElementDockMode.Left);
                    builder.SetPosition(0.0f, 0.0f);
                    builder.SetSize(450, 450);
                    builder.SetMovable(true);
                    builder.SetPadding(10);
                    builder.SetMargin(10);


                    builder.AddDockPanel(builder =>
                    {
                        builder.SetTag("2");
                        builder.SetDockMode(ElementDockMode.Left);
                        builder.SetPosition(0.0f, 0.0f);
                        builder.SetSize(225, 225);
                        builder.SetMovable(true);
                        builder.SetPadding(10);
                        builder.SetMargin(10);


                        builder.AddDockPanel(builder =>
                        {
                            builder.SetTag("3");
                            builder.SetDockMode(ElementDockMode.Left);
                            builder.SetPosition(0.0f, 0.0f);
                            builder.SetSize(110, 110);
                            builder.SetMovable(true);
                            builder.SetPadding(10);
                            builder.SetMargin(10);
                        });

                    });
                });


                builder.AddDockPanel(builder =>
                {
                    builder.SetTag("4");
                    builder.SetDockMode(ElementDockMode.Fill);
                    builder.SetPosition(0.0f, 0.0f);
                    builder.SetSize(110, 110);
                    builder.SetMovable(true);
                    builder.SetPadding(10);
                    builder.SetMargin(10);

                    builder.AddDockPanel(builder =>
                    {
                        builder.SetTag("5");
                        builder.SetDockMode(ElementDockMode.Left);
                        builder.SetPosition(0.0f, 0.0f);
                        builder.SetSize(110, 110);
                        builder.SetMovable(true);
                        builder.SetPadding(10);
                        builder.SetMargin(10);
                    });

                    builder.AddDockPanel(builder =>
                    {
                        builder.SetTag("6");
                        builder.SetDockMode(ElementDockMode.Fill);
                        builder.SetPosition(0.0f, 0.0f);
                        builder.SetSize(110, 110);
                        builder.SetMovable(true);
                        builder.SetPadding(10);
                        builder.SetMargin(10);
                    });


                });

                //CreateGridPanelTest(builder);

                //CreateButtonCtrlTest(builder);

                //CreateCheckboxCtrlTest(builder);

                //CreateLabelCtrlTest(builder);

            }).Build();

            interactionCore.Root = element;

            return renderContext;
        }

        private static void CreateGridPanelTest(IDockPanelBuilder builder)
        {
            var element = builder.AddGridPanel(builder =>
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

                builder.AddButton((builder) =>
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

                builder.AddButton((builder) =>
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

                builder.AddButton((builder) =>
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


                builder.AddButton((builder) =>
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

                builder.AddButton((builder) =>
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

                builder.AddButton((builder) =>
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


                builder.AddButton((builder) =>
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

                builder.AddButton((builder) =>
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

                builder.AddButton((builder) =>
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

        private void ButtonClicked(IElement interactiveElement)
        {
            Debug.WriteLine($"Interactive element '{interactiveElement.Tag}' clicked.");
        }

        private void OnCursorMove(ViewCursorMoveEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            cursorView = e.View;

            cursorDelta = e.Position - cursorPos;
            cursorPos = e.Position;

            if (cursorScroll)
            {
                e.View.View *= Matrix4.CreateTranslation(cursorDelta.X, cursorDelta.Y, 0.0f);
            }

            var cPos = e.View.GetViewToWorldCoords(cursorPos);
            cursorInput.Move(e.CursorId, new Vector2(cPos.X, cPos.Y));
            //interactionCore.Move(e.CursorId, cPos.X, cPos.Y);
        }

        private void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            var cPos = e.View.GetViewToWorldCoords(cursorPos);

            cursorInput.Down(e.CursorId, e.Key);
            //interactionCore.Down(e.CursorId, cPos.X, cPos.Y, e.Key);

            if (e.Key == CursorKey.Right)
            {
                cursorScroll = true;
            }
        }

        private void OnCursorEnter(ViewCursorEnterEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            cursorInput.Enter(e.CursorId);

            var cPos = e.View.GetViewToWorldCoords(cursorPos);

            //interactionCore.Enter(e.CursorId, cPos.X, cPos.Y);

        }

        private void OnCursorWheel(ViewCursorWheelEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            cursorInput.Wheel(e.CursorId, e.WheelDelta);

            var cPos = e.View.GetViewToWorldCoords(cursorPos);
            //interactionCore.Wheel(e.CursorId, cPos.X, cPos.Y, e.WheelDelta);
        }

        private void OnCursorLeave(ViewCursorLeaveEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            cursorInput.Leave(e.CursorId);

            //interactionCore.Leave(e.CursorId);
        }

        private void OnCursorUp(ViewCursorUpEvent e)
        {
            if (e.View.Context != renderContext)
            {
                return;
            }

            var cPos = e.View.GetViewToWorldCoords(cursorPos);
            cursorInput.Up(e.CursorId, e.Key);

            //interactionCore.Up(e.CursorId, cPos.X, cPos.Y, e.Key);

            if (e.Key == CursorKey.Right)
            {
                cursorScroll = false;
            }
        }

        private void DrawCursor(IRenderView view, float dt)
        {
            var cPos = view.GetViewToWorldCoords(cursorPos);
            var cSize = 10;
            view.Context.Primitives.DrawCircle(view, new Vector2(cPos.X, cPos.Y), cSize, Color4.Red, filled: false);
            view.Context.Primitives.DrawPoint(view, new Vector2(cPos.X, cPos.Y), Color4.Red, PointType.Cross, cSize);
            view.Context.Fonts.Render(view, new Box2(view.Box.Min, view.Box.Max), RenderTexts);
        }

        private void OnRender1(IRenderView view, Matrix4 transform, float dt)
        {
            interactionRenderer.Render(interactionCore, view);

            if (cursorView == view)
            {
                DrawCursor(view, dt);
            }
        }


        private void RenderTexts(IRenderView view, Box2 clipBox)
        {
            var cPos = view.GetViewToWorldCoords(cursorPos);

            var font = view.Context.Fonts.GetOSFont("ARIAL", 12);

            view.Context.Fonts.RenderStart(view, new Vector2(cPos.X, cPos.Y));
            view.Context.Fonts.RenderPart(view, font.Id, $"({cPos.X},{cPos.Y})", Vector2.Zero, Color4.Green, 100, clipBox);
            view.Context.Fonts.RenderEnd(view);
        }

        #endregion Private Methods
    }
}