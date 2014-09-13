﻿//2014 Apache2, WinterDev
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using LayoutFarm;
using LayoutFarm.UI;
using LayoutFarm.Text;


namespace LayoutFarm.SampleControls
{

    public class UIMultiLineTextBox : UIBox
    {

        TextEditRenderBox visualTextEdit;

        bool _multiline;
        public UIMultiLineTextBox(int width, int height, bool multiline)
            : base(width, height)
        {
            this._multiline = multiline;
        }
        protected override bool HasReadyRenderElement
        {
            get { return this.visualTextEdit != null; }
        }
        protected override RenderElement CurrentPrimaryRenderElement
        {
            get { return this.visualTextEdit; }
        }
        public override RenderElement GetPrimaryRenderElement(RootGraphic rootgfx)
        {
            if (visualTextEdit == null)
            {
                visualTextEdit = new TextEditRenderBox(this.Width, this.Height, _multiline);
                RenderElement.DirectSetVisualElementLocation(visualTextEdit, this.Left, this.Top);

                visualTextEdit.HasSpecificSize = true;

                visualTextEdit.SetController(this);
                RegisterNativeEvent(
                  1 << UIEventIdentifier.NE_MOUSE_DOWN
                  | 1 << UIEventIdentifier.NE_LOST_FOCUS
                  | 1 << UIEventIdentifier.NE_SIZE_CHANGED
                  );
            }
            return visualTextEdit;
        }

        protected override void OnKeyPress(UIKeyPressEventArgs e)
        {
            visualTextEdit.OnKeyPress(e);
        }
        protected override void OnKeyDown(UIKeyEventArgs e)
        {
            visualTextEdit.OnKeyDown(e);

        }
        protected override void OnKeyUp(UIKeyEventArgs e)
        {

        }
        protected override bool OnProcessDialogKey(UIKeyEventArgs e)
        {
            return visualTextEdit.OnProcessDialogKey(e);

        }
        protected override void OnMouseDown(UIMouseEventArgs e)
        {

            visualTextEdit.OnMouseDown(e);
        }
        protected override void OnMouseUp(UIMouseEventArgs e)
        {
            visualTextEdit.OnMouseUp(e);
        }

        protected override void OnDoubleClick(UIMouseEventArgs e)
        {
            visualTextEdit.OnDoubleClick(e);
        }
        protected override void OnDragDrop(UIDragEventArgs e)
        {
        }
        protected override void OnDragStart(UIDragEventArgs e)
        {
            visualTextEdit.OnDragStart(e);
        }
        protected override void OnDragging(UIDragEventArgs e)
        {
            visualTextEdit.OnDrag(e);
        }
        protected override void OnDragStop(UIDragEventArgs e)
        {
            visualTextEdit.OnDragStop(e);
        }
        public override void InvalidateGraphic()
        {
            if (visualTextEdit != null)
                visualTextEdit.InvalidateGraphic();
        }
    }
}