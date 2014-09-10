﻿//2014 Apache2, WinterDev
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
 
namespace LayoutFarm.Presentation
{ 
    public abstract class VisualLayer
    {

#if DEBUG
        public int dbug_layer_id;
        static int dbug_layer_id_count = 0;
        public int dbug_InvalidateCount = 0;
        public int dbug_ValidateCount = 0;
#endif

        protected int layerFlags;

        protected const int IS_LAYER_HIDDEN = 1 << (14 - 1);
        protected const int IS_GROUND_LAYER = 1 << (15 - 1);
        protected const int MAY_HAS_OTHER_OVERLAP_CHILD = 1 << (16 - 1);

        protected const int DOUBLE_BACKCANVAS_WIDTH = 1 << (18 - 1);
        protected const int DOUBLE_BACKCANVAS_HEIGHT = 1 << (19 - 1);
        protected const int CONTENT_DRAWING = 1 << (22 - 1);
        protected const int ARRANGEMENT_VALID = 1 << (23 - 1);
        protected const int HAS_CALCULATE_SIZE = 1 << (24 - 1);

        protected const int FLOWLAYER_HAS_MULTILINE = 1 << (25 - 1);


        public readonly RenderElement ownerVisualElement;

        int postCalculateContentWidth;
        int postCalculateContentHeight;




        protected VisualLayer(MultiLayerRenderBox owner)
        {
            this.ownerVisualElement = owner;

#if DEBUG
            this.dbug_layer_id = dbug_layer_id_count;
            ++dbug_layer_id_count;
#endif
        }

        public abstract void Clear();

        protected RootWindowRenderBox WinRoot
        {
            get
            {
                return ownerVisualElement.WinRoot;
            }
        }
        protected bool HasWinRoot
        {
            get
            {

                return ownerVisualElement.WinRoot != null;
            }
        }

        public bool IsOwnerInArrangeQueue
        {
            get
            {
                return ownerVisualElement.IsInLayoutQueue;
            }
        }
        public bool IsOwnenerInSuspendingMode
        {
            get
            {

                return ownerVisualElement.IsInLayoutSuspendMode;
            }
        }

        public void InvalidateContentArrangementFromContainerSizeChanged()
        {
            layerFlags &= ~ARRANGEMENT_VALID;
#if DEBUG
            this.dbug_InvalidateCount++;
#endif
        }
        public RenderElement InvalidateArrangement()
        {
#if DEBUG
            this.dbug_InvalidateCount++;
#endif
            layerFlags &= ~ARRANGEMENT_VALID;
            return this.ownerVisualElement;
        }

        public bool Visible
        {
            get
            {
                return (layerFlags & IS_LAYER_HIDDEN) == 0;
            }
            set
            {
                if (value)
                {
                    layerFlags &= ~IS_LAYER_HIDDEN;
                }
                else
                {
                    layerFlags |= IS_LAYER_HIDDEN;
                }
            }
        }

        public ArtAlignment ContentAlignment
        {
            get
            {
                return (ArtAlignment)((layerFlags >> 11) & 0x3);
            }
            set
            {
                layerFlags = (layerFlags & ~(0x3 << 11)) | ((int)value << 11);
            }
        }


        public Size PostCalculateContentSize
        {
            get
            {
                return new Size(postCalculateContentWidth, postCalculateContentHeight);
            }
        }

#if DEBUG
        public dbugRootElement dbugVRoot
        {
            get
            {
                return LayoutFarm.Presentation.dbugRootElement.dbugCurrentGlobalVRoot;
            }
        }
#endif

        protected void BeginDrawingChildContent()
        {
            layerFlags |= CONTENT_DRAWING;
        }
        protected void FinishDrawingChildContent()
        {
            layerFlags &= ~CONTENT_DRAWING;
        }

        public bool DoubleBackCanvasWidth
        {
            get
            {
                return (layerFlags & DOUBLE_BACKCANVAS_WIDTH) != 0;
            }
            set
            {
                if (value)
                {
                    layerFlags |= DOUBLE_BACKCANVAS_WIDTH;
                }
                else
                {
                    layerFlags &= ~DOUBLE_BACKCANVAS_WIDTH;
                }
            }
        }

        public bool DoubleBackCanvasHeight
        {
            get
            {
                return (layerFlags & DOUBLE_BACKCANVAS_HEIGHT) != 0;
            }
            set
            {
                if (value)
                {
                    layerFlags |= DOUBLE_BACKCANVAS_HEIGHT;
                }
                else
                {
                    layerFlags &= ~DOUBLE_BACKCANVAS_HEIGHT;
                }
            }
        }
        protected void SetDoubleCanvas(bool useWithWidth, bool useWithHeight)
        {
            DoubleBackCanvasWidth = useWithWidth;
            DoubleBackCanvasHeight = useWithHeight;
        }
        protected void SetPostCalculateLayerContentSize(int width, int height)
        {
            ValidateCalculateContentSize();
            postCalculateContentWidth = width;
            postCalculateContentHeight = height;
        }

        protected void SetPostCalculateLayerContentSize(Size s)
        {
            ValidateCalculateContentSize();
            postCalculateContentWidth = s.Width;
            postCalculateContentHeight = s.Height;

        }

        public abstract bool HitTestCore(HitPointChain artHitResult); 
        public abstract void TopDownReCalculateContentSize(VisualElementArgs vinv); 
        public abstract void TopDownReArrangeContent(VisualElementArgs vinv);
        
        public abstract IEnumerable<RenderElement> GetVisualElementIter();
        public abstract IEnumerable<RenderElement> GetVisualElementReverseIter();
        

        protected void ValidateArrangement()
        {
#if DEBUG
            this.dbug_ValidateCount++;
#endif

            layerFlags |= ARRANGEMENT_VALID;
        }

        public abstract void DrawChildContent(CanvasBase canvasPage, InternalRect updateArea);
        public abstract bool PrepareDrawingChain(VisualDrawingChain chain);

        public void BeginLayerGraphicUpdate(VisualElementArgs vinv)
        {
            ownerVisualElement.BeginGraphicUpdate(vinv);
        }
        public void EndLayerGraphicUpdate(VisualElementArgs vinv)
        {
            ownerVisualElement.EndGraphicUpdate(vinv);
        }

        public bool NeedReArrangeContent
        {
            get
            {

                return (layerFlags & ARRANGEMENT_VALID) == 0;
            }
        }
        public bool HasCalculateContentSize
        {
            get
            {
                return (layerFlags & HAS_CALCULATE_SIZE) != 0;
            }
        }
        void ValidateCalculateContentSize()
        {
            this.layerFlags |= HAS_CALCULATE_SIZE;
        }
        void InvalidateCalculateContentSize()
        {
            this.layerFlags &= ~HAS_CALCULATE_SIZE;
        }

#if DEBUG
        public string dbugLayerState
        {
            get
            {
                if (NeedReArrangeContent)
                {
                    return "[A]";
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        public abstract void dbug_DumpElementProps(dbugLayoutMsgWriter writer);

#endif
        public abstract void AddTop(RenderElement ve);
        public abstract IEnumerable<RenderElement> GetDrawingIter();

    }


}