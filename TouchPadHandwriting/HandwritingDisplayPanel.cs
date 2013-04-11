using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TouchPadHandwriting
{
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    internal class HandwritingDisplayPanel : Panel
    {
        public HandwritingDisplayPanel()
        {
            base.DoubleBuffered = true;
            this.ForeColor = base.ForeColor;
        }

        List<List<Point>> strokes = new List<List<Point>>();
        List<Point> currentStroke = null;

        Pen myPen;
        int penWidth = 20;

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                Pen penOld = this.myPen;
                this.myPen = new Pen(value, (float)this.penWidth);
                this.myPen.StartCap = LineCap.Round;
                this.myPen.EndCap = LineCap.Round;
                this.myPen.LineJoin = LineJoin.Round;
                if (penOld != null)
                {
                    penOld.Dispose();
                }
                base.ForeColor = value;
            }
        }

        public int PenWidth
        {
            get
            {
                return this.penWidth;
            }
            set
            {
                this.penWidth = value;
                Pen penOld = this.myPen;
                this.myPen = new Pen(base.ForeColor, (float)this.penWidth);
                this.myPen.StartCap = LineCap.Round;
                this.myPen.EndCap = LineCap.Round;
                this.myPen.LineJoin = LineJoin.Round;
                if (penOld != null)
                {
                    penOld.Dispose();
                }
                this.Invalidate();
            }
        }

        double aspectRatio = 1.5;

        [Browsable(true),
        DefaultValue(1.5),
        Description("The aspect ratio of this panel, given by width divided by height.")]
        public double AspectRatio
        {
            get
            {
                return this.aspectRatio;
            }
            set
            {
                if (value == this.aspectRatio)
                    return;
                if (value <= double.Epsilon)
                    throw new ArgumentException();
                if (strokes.Count > 0)
                    throw new InvalidOperationException();
                this.updateAspectRatio(value);
            }
        }

        delegate void updateAspectRatioDelegate(double aspectRatio);

        void updateAspectRatio(double aspectRatio)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((updateAspectRatioDelegate)this.updateAspectRatio, aspectRatio);
            }
            else
            {
                this.aspectRatio = aspectRatio;
                if (this.aspectRatio <= 1.0)
                {
                    this.Width = (int)((double)this.Height * aspectRatio);
                }
                else
                {
                    this.Height = (int)((double)this.Width / aspectRatio);
                }
            }
        }

        /*protected override void OnSizeChanged(EventArgs e)
        {
            if (this.aspectRatio <= 1.0)
            {
                this.Height = (int)((double)this.Width / aspectRatio);
            }
            else
            {
                this.Width = (int)((double)this.Height * aspectRatio);
            }
            base.OnSizeChanged(e);
        }*/

        [Browsable(false)]
        protected override bool DoubleBuffered
        {
            get
            {
                return base.DoubleBuffered;
            }
            set
            {
                base.DoubleBuffered = true;
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (specified == BoundsSpecified.Height)
            {
                width = (int)((double)height * aspectRatio);
            }
            else
            {
                height = (int)((double)width / aspectRatio);
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Matrix m = e.Graphics.Transform;
            float scale;
            if (this.aspectRatio <= 1.0)
            {
                scale = (float)((double)this.Height / 1024.0);
            }
            else
            {
                scale = (float)((double)this.Width / 1024.0);
            }
            e.Graphics.ScaleTransform(scale, scale);
            foreach (List<Point> stroke in this.strokes)
            {
                if (stroke.Count >= 2)
                {
                    e.Graphics.DrawLines(this.myPen, stroke.ToArray());
                }
            }
            e.Graphics.Transform = m;
        }

        private delegate void InvalidateDelegate();

        internal void AddStrokePoint(double x, double y)
        {
            if (this.currentStroke == null)
            {
                this.currentStroke = new List<Point>();
                this.strokes.Add(this.currentStroke);
            }
            if (this.aspectRatio <= 1.0)
            {
                this.currentStroke.Add(new Point((int)(x * (1024.0 * this.aspectRatio)), (int)(y * 1024.0)));
            }
            else
            {
                this.currentStroke.Add(new Point((int)(x * 1024.0), (int)(y * (1024.0 / this.aspectRatio))));
            }
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new InvalidateDelegate(this.Invalidate));
            }
            else
            {
                this.Invalidate();
            }
        }

        internal Size HandWritingBox
        {
            get
            {
                if (this.aspectRatio <= 1.0)
                {
                    return new Size((int)(1024.0 * this.aspectRatio), 1024);
                }
                else
                {
                    return new Size(1024, (int)(1024.0 / this.aspectRatio));
                }
            }
        }

        internal Point[] EndStroke()
        {
            Point[] stroke = this.currentStroke.ToArray();
            this.currentStroke = null;
            return stroke;
        }

        internal void ClearStrokes()
        {
            this.strokes.Clear();
            this.currentStroke = null;
            if (this.InvokeRequired)
            {
                this.Invoke(new InvalidateDelegate(this.Invalidate));
            }
            else
            {
                this.Invalidate();
            }
        }

        internal void ShowExample()
        {
            object result = new System.Xml.Serialization.XmlSerializer(typeof(List<List<Point>>)).Deserialize(new System.IO.StringReader(Resources.HandwritingExampleCharacter.Strokes));
            List<List<Point>> strokes = result as List<List<Point>>;
            if (strokes != null)
            {
                this.strokes = strokes;
            }
        }
    }
}
