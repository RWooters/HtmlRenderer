﻿//MIT, 2014-present, WinterDev

using System.Collections.Generic;

using PixelFarm.Drawing;
using PaintLab.Svg;
using LayoutFarm.Svg;
using LayoutFarm.UI;

namespace LayoutFarm
{
    [DemoNote("9.2 ShapeControls")]
    class DemoShapeControl : App
    {
        LayoutFarm.CustomWidgets.PolygonController polygonController = new CustomWidgets.PolygonController();
        LayoutFarm.CustomWidgets.RectBoxController rectBoxController = new CustomWidgets.RectBoxController();



        protected override void OnStart(AppHost host)
        {

            var spec = new Svg.SvgPathSpec() { FillColor = Color.Red };
            SvgRenderRootElement renderRoot = new SvgRenderRootElement();
            SvgRenderElement renderE = new SvgRenderElement(WellknownSvgElementName.Path, spec, renderRoot);
            VgRenderVx svgRenderVx = new VgRenderVx(renderE);
            using (VxsContext.Temp(out VertexStore vxs))
            {
                //red-triangle ***
                vxs.AddMoveTo(100, 20);
                vxs.AddLineTo(150, 50);
                vxs.AddLineTo(110, 80);
                vxs.AddCloseFigure();
                renderE._vxsPath = vxs.CreateTrim();
            }

            svgRenderVx.DisableBackingImage = true;
            var uiSprite = new UISprite(10, 10); //init size = (10,10), location=(0,0) 
            uiSprite.DisableBmpCache = true;
            uiSprite.LoadSvg(svgRenderVx);//
            host.AddChild(uiSprite);

            var spriteEvListener = new GeneralEventListener();
            uiSprite.AttachExternalEventListener(spriteEvListener);



            //box1 = new LayoutFarm.CustomWidgets.SimpleBox(50, 50);
            //box1.BackColor = Color.Red;
            //box1.SetLocation(10, 10);
            ////box1.dbugTag = 1;
            //SetupActiveBoxProperties(box1);
            //viewport.AddContent(box1);
            //-------- 
            rectBoxController.Init();
            //polygonController.Visible = false;
            host.AddChild(polygonController);
            //-------------------------------------------
            host.AddChild(rectBoxController);




            //foreach (var ui in rectBoxController.GetControllerIter())
            //{
            //    viewport.AddContent(ui);
            //}

            spriteEvListener.MouseDown += e1 =>
            {
                //mousedown on ui sprite
                polygonController.SetPosition((int)uiSprite.Left, (int)uiSprite.Top);
                polygonController.SetTargetUISprite(uiSprite);
                polygonController.UpdateControlPoints(renderE._vxsPath);

            };
            spriteEvListener.MouseMove += e1 =>
            {
                if (e1.IsDragging)
                {
                    //drag event on uisprite

                    int left = (int)uiSprite.Left;
                    int top = (int)uiSprite.Top;

                    int new_left = left + e1.DiffCapturedX;
                    int new_top = top + e1.DiffCapturedY;
                    uiSprite.SetLocation(new_left, new_top);
                    //-----
                    //also update controller position
                    polygonController.SetPosition(new_left, new_top);
                }
            };

        }
        void SetupActiveBoxProperties(LayoutFarm.CustomWidgets.Box box)
        {
            //1. mouse down         
            box.MouseDown += (s, e) =>
            {
                box.BackColor = KnownColors.FromKnownColor(KnownColor.DeepSkyBlue);
                e.MouseCursorStyle = MouseCursorStyle.Pointer;
                //--------------------------------------------
                e.SetMouseCapture(rectBoxController.ControllerBoxMain);
                rectBoxController.UpdateControllerBoxes(box);

            };
            //2. mouse up
            box.MouseUp += (s, e) =>
            {
                e.MouseCursorStyle = MouseCursorStyle.Default;
                box.BackColor = Color.LightGray;
                //controllerBox1.Visible = false;
                //controllerBox1.TargetBox = null;
            };
        }
    }



    [DemoNote("9.2.1 ShapeControls")]
    class DemoShapeControl2 : App
    {
        LayoutFarm.CustomWidgets.PolygonController _polygonController = new CustomWidgets.PolygonController();
        LayoutFarm.CustomWidgets.RectBoxController _rectBoxController = new CustomWidgets.RectBoxController();
        LayoutFarm.CustomWidgets.Box _rectBoundsWidgetBox;


        VgRenderVx CreateTestRenderVx()
        {
            //string svgfile = "../Test8_HtmlRenderer.Demo/Samples/Svg/others/cat_simple.svg";
            string svgfile = "../Test8_HtmlRenderer.Demo/Samples/Svg/others/cat_complex.svg";
            //string svgfile = "../Test8_HtmlRenderer.Demo/Samples/Svg/others/tiger.svg";
            //string svgfile = "../Test8_HtmlRenderer.Demo/Samples/Svg/freepik/dog1.svg";
            //string svgfile = "1f30b.svg";
            //string svgfile = "../Data/Svg/twemoji/1f30b.svg";
            //string svgfile = "../Data/1f30b.svg";
            //string svgfile = "../Data/Svg/twemoji/1f370.svg";
            return ReadSvgFile(svgfile);
        }
        VgRenderVx CreateTestRenderVx2()
        {
            var spec = new Svg.SvgPathSpec() { FillColor = Color.Red };
            SvgRenderRootElement renderRoot = new SvgRenderRootElement();
            SvgRenderElement renderE = new SvgRenderElement(WellknownSvgElementName.Path, spec, renderRoot);
            VgRenderVx svgRenderVx = new VgRenderVx(renderE);

            using (VxsContext.Temp(out VertexStore vxs))
            {
                //red-triangle ***
                vxs.AddMoveTo(100, 20);
                vxs.AddLineTo(150, 50);
                vxs.AddLineTo(110, 80);
                vxs.AddCloseFigure();
                renderE._vxsPath = vxs.CreateTrim();
            }

            return svgRenderVx;
        }


        bool _hitTestOnSubPath = false;

        protected override void OnStart(AppHost host)
        {
            VgRenderVx svgRenderVx = CreateTestRenderVx();

            svgRenderVx.DisableBackingImage = true;
            var _uiSprite = new UISprite(10, 10); //init size = (10,10), location=(0,0) 
            _uiSprite.DisableBmpCache = true;
            _uiSprite.LoadSvg(svgRenderVx);//
            host.AddChild(_uiSprite);

            var spriteEvListener = new GeneralEventListener();
            _uiSprite.AttachExternalEventListener(spriteEvListener);

            //
            _rectBoundsWidgetBox = new LayoutFarm.CustomWidgets.Box(50, 50); //visual rect box
            Color c = KnownColors.FromKnownColor(KnownColor.DeepSkyBlue);
            _rectBoundsWidgetBox.BackColor = Color.FromArgb(100, c);
            _rectBoundsWidgetBox.SetLocation(10, 10);
            //box1.dbugTag = 1;
            SetupActiveBoxProperties(_rectBoundsWidgetBox);
            host.AddChild(_rectBoundsWidgetBox);

            //box1 = new LayoutFarm.CustomWidgets.SimpleBox(50, 50);
            //box1.BackColor = Color.Red;
            //box1.SetLocation(10, 10);
            ////box1.dbugTag = 1;
            //SetupActiveBoxProperties(box1);
            //viewport.AddContent(box1);
            //-------- 
            _rectBoxController.Init();
            //polygonController.Visible = false;
            host.AddChild(_polygonController);
            //-------------------------------------------
            host.AddChild(_rectBoxController);

            //foreach (var ui in rectBoxController.GetControllerIter())
            //{
            //    viewport.AddContent(ui);
            //}

            spriteEvListener.MouseDown += e1 =>
            {
                //mousedown on ui sprite 
                //find exact part ... 


                if (_hitTestOnSubPath)
                {
                    SvgHitInfo hitInfo = _uiSprite.FindRenderElementAtPos(e1.X, e1.Y, true);
                    if (hitInfo.svg != null &&
                        hitInfo.svg._vxsPath != null)
                    {

                        PixelFarm.CpuBlit.RectD bounds =
                            PixelFarm.CpuBlit.VertexProcessing.BoundingRect.GetBoundingRect(
                                hitInfo.copyOfVxs);

                        _polygonController.UpdateControlPoints(hitInfo.copyOfVxs);

                        //move redbox and its controller
                        _rectBoundsWidgetBox.SetLocationAndSize(
                            (int)bounds.Left, (int)(bounds.Top - bounds.Height),
                            (int)bounds.Width, (int)bounds.Height);
                        _rectBoxController.UpdateControllerBoxes(_rectBoundsWidgetBox);

                        _rectBoundsWidgetBox.Visible = true;
                        _rectBoxController.Visible = true;
                        //show bounds
                    }
                    else
                    {
                        _rectBoundsWidgetBox.Visible = false;
                        _rectBoxController.Visible = false;
                    }
                }
                else
                {
                    //hit on sprite

                    _rectBoundsWidgetBox.SetLocationAndSize(
                          (int)_uiSprite.Left, (int)_uiSprite.Top,
                          (int)_uiSprite.Width, (int)_uiSprite.Height);
                    _rectBoxController.UpdateControllerBoxes(_rectBoundsWidgetBox);
                    _rectBoundsWidgetBox.Visible = true;
                    _rectBoxController.Visible = true;

                    //polygonController.SetPosition((int)uiSprite.Left, (int)uiSprite.Top);
                    //polygonController.SetTargetUISprite(uiSprite);
                    //polygonController.UpdateControlPoints(svgRenderVx._renderE._vxsPath);
                }


            };
            spriteEvListener.MouseMove += e1 =>
            {
                if (e1.IsDragging)
                {
                    //drag event on uisprite 
                    int left = (int)_uiSprite.Left;
                    int top = (int)_uiSprite.Top;

                    int new_left = left + e1.DiffCapturedX;
                    int new_top = top + e1.DiffCapturedY;
                    _uiSprite.SetLocation(new_left, new_top);
                    //-----
                    //also update controller position

                    _polygonController.SetPosition(new_left, new_top);
                    _rectBoundsWidgetBox.SetLocation(new_left, new_top);
                    _rectBoxController.SetPosition(new_left, new_top);

                    
                }
            };

        }
        void SetupActiveBoxProperties(LayoutFarm.CustomWidgets.Box box)
        {
            //1. mouse down         
            box.MouseDown += (s, e) =>
            {
                Color c = KnownColors.FromKnownColor(KnownColor.DeepSkyBlue);
                box.BackColor = Color.FromArgb(100, c);
                e.MouseCursorStyle = MouseCursorStyle.Pointer;
                //--------------------------------------------
                e.SetMouseCapture(_rectBoxController.ControllerBoxMain);
                _rectBoxController.UpdateControllerBoxes(box);

            };
            //2. mouse up
            box.MouseUp += (s, e) =>
            {
                e.MouseCursorStyle = MouseCursorStyle.Default;
                //box.BackColor = Color.LightGray;
                //controllerBox1.Visible = false;
                //controllerBox1.TargetBox = null;
            };
        }

        VgRenderVx ReadSvgFile(string filename)
        {

            string svgContent = System.IO.File.ReadAllText(filename);
            SvgDocBuilder docBuidler = new SvgDocBuilder();
            SvgParser parser = new SvgParser(docBuidler);
            WebLexer.TextSnapshot textSnapshot = new WebLexer.TextSnapshot(svgContent);
            parser.ParseDocument(textSnapshot);
            //TODO: review this step again
            SvgRenderVxDocBuilder builder = new SvgRenderVxDocBuilder();
            return builder.CreateRenderVx(docBuidler.ResultDocument, svgElem =>
            {

            });
        }
    }
}