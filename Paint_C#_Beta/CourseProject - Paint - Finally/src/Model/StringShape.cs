using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    [Serializable]
    class StringShape : Shape
    {
        public StringShape(RectangleF line) : base(line)
        {
        }

        public StringShape(StringShape line) : base(line)
        {
        }

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

            Font drawFont = new Font("Arial", 16);
            grfx.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(Opacity, FillColor)), Rectangle.X, Rectangle.Y);

            grfx.ResetTransform();
        }
    }
}
