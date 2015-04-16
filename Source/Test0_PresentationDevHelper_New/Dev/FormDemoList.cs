﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LayoutFarm;
using PixelFarm.Drawing;
using LayoutFarm.UI;
namespace LayoutFarm.Dev
{
    public partial class FormDemoList : Form
    {
        static readonly PixelFarm.Drawing.GraphicsPlatform gdiPlatform = LayoutFarm.UI.GdiPlus.MyWinGdiPortal.Start();
        static readonly PixelFarm.Drawing.GraphicsPlatform openGLPlatform = LayoutFarm.UI.OpenGL.MyOpenGLPortal.Start();

        UIPlatform uiPlatformWinForm;

        public FormDemoList()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);
            this.uiPlatformWinForm = new LayoutFarm.UI.UIPlatformWinForm();


        }
        public TreeView SamplesTreeView
        {
            get { return this._samplesTreeView; }
        }
        void Form1_Load(object sender, EventArgs e)
        {

            this.lstDemoList.DoubleClick += new EventHandler(lstDemoList_DoubleClick);
        }

        void lstDemoList_DoubleClick(object sender, EventArgs e)
        {
            //load demo sample
            var selectedDemoInfo = this.lstDemoList.SelectedItem as DemoInfo;
            if (selectedDemoInfo == null) return;
            //------------------------------------------------------------ 
            DemoBase selectedDemo = (DemoBase)Activator.CreateInstance(selectedDemoInfo.DemoType);
            RunDemo(selectedDemo);

            //LayoutFarm.UI.UISurfaceViewportControl viewport; 
            //Form formCanvas;
            //CreateReadyForm(
            //    out viewport,
            //    out formCanvas);

            //selectedDemo.StartDemo(new SampleViewport(viewport));
            //viewport.TopDownRecalculateContent();
            ////==================================================  
            //viewport.PaintMe();
            //ShowFormLayoutInspector(viewport); 
        }
        public void RunDemo(DemoBase selectedDemo)
        {
            LayoutFarm.UI.UISurfaceViewportControl viewport;

            Form formCanvas;
            CreateReadyForm(out viewport, out formCanvas);

            selectedDemo.StartDemo(new SampleViewport(viewport));
            viewport.TopDownRecalculateContent();
            //==================================================  
            viewport.PaintMe();

            if (this.chkShowLayoutInspector.Checked)
            {
                ShowFormLayoutInspector(viewport);
            }

            if (this.chkShowFormPrint.Checked)
            {
                ShowFormPrint(viewport);
            }

        }
        static void ShowFormLayoutInspector(LayoutFarm.UI.UISurfaceViewportControl viewport)
        {

            var formLayoutInspector = new LayoutFarm.Dev.FormLayoutInspector();
            formLayoutInspector.Show();

            formLayoutInspector.FormClosed += (s, e2) =>
            {
                formLayoutInspector = null;
            };
            formLayoutInspector.Connect(viewport);
            formLayoutInspector.Show();

        }
        static void ShowFormPrint(LayoutFarm.UI.UISurfaceViewportControl viewport)
        {

            var formPrint = new LayoutFarm.Dev.FormPrint();
            formPrint.Show();

            formPrint.FormClosed += (s, e2) =>
            {
                formPrint = null;
            };
            formPrint.Connect(viewport);

        }
        void CreateReadyForm(
            out LayoutFarm.UI.UISurfaceViewportControl viewport,
            out Form formCanvas)
        {
            var workingArea = Screen.PrimaryScreen.WorkingArea;
            int w = workingArea.Width;
            int h = workingArea.Height;

            
            MyRootGraphic rootgfx = new MyRootGraphic(this.uiPlatformWinForm,
                this.chkUseGLCanvas.Checked ? openGLPlatform : gdiPlatform,
                w, h);

            var topRenderBox = rootgfx.TopWindowRenderBox;
            var userEventPortal = new UserEventPortal(topRenderBox);
            rootgfx.SetUserEventPortal(userEventPortal);

            formCanvas = FormCanvasHelper.CreateNewFormCanvas(rootgfx,userEventPortal,
                this.chkUseGLCanvas.Checked ? InnerViewportKind.GL : InnerViewportKind.GdiPlus,
                out viewport);

            formCanvas.Text = "FormCanvas 1";

            viewport.PaintMe();

            formCanvas.WindowState = FormWindowState.Maximized;
            formCanvas.Show();
        }

        public void ClearDemoList()
        {
            this.lstDemoList.Items.Clear();
        }

        public void LoadDemoList(Type sampleAssemblySpecificType)
        {


            var demoBaseType = typeof(DemoBase);
            var thisAssem = System.Reflection.Assembly.GetAssembly(sampleAssemblySpecificType);
            List<DemoInfo> demoInfoList = new List<DemoInfo>();
            foreach (var t in thisAssem.GetTypes())
            {
                if (demoBaseType.IsAssignableFrom(t) && t != demoBaseType && !t.IsAbstract)
                {
                    string demoTypeName = t.Name;
                    object[] notes = t.GetCustomAttributes(typeof(DemoNoteAttribute), false);
                    string noteMsg = null;
                    if (notes != null && notes.Length > 0)
                    {
                        //get one only
                        DemoNoteAttribute note = notes[0] as DemoNoteAttribute;
                        if (note != null)
                        {
                            noteMsg = note.Message;
                        }
                    }
                    demoInfoList.Add(new DemoInfo(t, noteMsg));
                }
            }
            demoInfoList.Sort((d1, d2) =>
            {
                if (d1.DemoNote != null && d2.DemoNote != null)
                {
                    return d1.DemoNote.CompareTo(d2.DemoNote);
                }
                else
                {
                    return d1.DemoType.Name.CompareTo(d2.DemoType.Name);
                }
            });
            foreach (var demo in demoInfoList)
            {
                this.lstDemoList.Items.Add(demo);
            }

        }

        private void chkShowLayoutInspector_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkShowFormPrint_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
