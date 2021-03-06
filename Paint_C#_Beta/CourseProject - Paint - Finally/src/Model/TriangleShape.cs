using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    [Serializable]
    class TriangleShape : Shape
    {

        #region Constructor

        public TriangleShape(RectangleF tri) : base(tri)
        {

        }

        public TriangleShape(TriangleShape triangle) : base(triangle)
        {

        }

        #endregion

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
                return true;
            else
                return false;
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            base.RotateShape(grfx);

            Point[] p =
            {
                new Point((int)Rectangle.X + ((int)Rectangle.Width / 2), (int)Rectangle.Y),
                new Point((int)Rectangle.X, (int)(Rectangle.Y + Rectangle.Height)),
                new Point((int)(Rectangle.X + Rectangle.Width), (int)(Rectangle.Y + Rectangle.Height))
            };
            grfx.FillPolygon(new SolidBrush(Color.FromArgb(Opacity, FillColor)), p);
            grfx.DrawPolygon(new Pen(BorderColor), p);

            grfx.ResetTransform();
        }
    }
}
