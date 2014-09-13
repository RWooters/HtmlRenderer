﻿//2014 Apache2, WinterDev
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LayoutFarm.Text
{

    public abstract class VisualPointInfo
    {

        int lineCharIndex;

        EditableVisualTextRun onVisualElement; int onTextRunCharOffset; int caretXPos; int onTextRunPixelOffset;
        public VisualPointInfo(int lineCharIndex)
        {

            this.lineCharIndex = lineCharIndex;
        }

        public void SetAdditionVisualInfo(EditableVisualTextRun onTextRun, int onTextRunCharOffset, int caretXPos, int textRunPixelOffset)
        {
            this.caretXPos = caretXPos;
            this.onVisualElement = onTextRun;
            this.onTextRunCharOffset = onTextRunCharOffset;
            this.onTextRunPixelOffset = textRunPixelOffset;
        }
        public int LineCharIndex
        {
            get
            {
                return lineCharIndex;
            }
        }
        public int TextRunCharOffset
        {
            get
            {
                return onTextRunCharOffset;
            }
        }
        public EditableVisualTextRun TextRun
        {
            get
            {
                return onVisualElement;
            }
        }
        public bool IsOnTheBeginOfLine
        {
            get
            {
                return LocalSelectedIndex == -1;
            }
        }
        public abstract int LineId
        {
            get;

        }
        public abstract int LineTop
        {
            get;
        }
        public abstract int ActualLineHeight
        {
            get;
        }
        public abstract int LineNumber
        {
            get;
        }
        public abstract int CurrentWidth
        {
            get;
        }
        public int LocalSelectedIndex
        {
            get
            {
                return lineCharIndex - onTextRunCharOffset;
            }
        }
        public int X
        {
            get
            {
                return caretXPos;
            }
        }
        public int TextRunPixelOffset
        {
            get
            {
                return onTextRunPixelOffset;
            }
        }

#if DEBUG
        public override string ToString()
        {
            if (onVisualElement == null)
            {
                return "null " + " ,local[" + LocalSelectedIndex + "]";
            }
            else
            {
                return onVisualElement.ToString() + " ,local[" + LocalSelectedIndex + "]";
            }

        }
#endif

    }


    public class EditableVisualPointInfo : VisualPointInfo
    {
        EditableVisualElementLine line;
        public EditableVisualPointInfo(EditableVisualElementLine line, int index)
            : base(index)
        {
            this.line = line;
        }
        public EditableVisualElementLine Line
        {
            get
            {
                return this.line;
            }
        }
        public EditableVisualElementLine EditableLine
        {
            get
            {
                return this.line;
            }
        }
        public override int LineTop
        {
            get { return this.line.LineTop; }
        }
        public override int CurrentWidth
        {
            get { return this.line.CurrentWidth; }
        }
        public override int ActualLineHeight
        {
            get
            {
                return line.ActualLineHeight;
            }
        }
        public override int LineNumber
        {
            get
            {
                return this.line.LineNumber;
            }
        }
        public override int LineId
        {
            get
            {
                return this.line.LineNumber;
            }
        }
    }
}
