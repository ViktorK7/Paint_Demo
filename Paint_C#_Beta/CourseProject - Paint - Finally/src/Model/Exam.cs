using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    public class ExamShape : Shape
    {
        #region Constructor

        public ExamShape(RectangleF circ) : base(circ)
        {

        }
        #endregion

        public override bool Contains(PointF point)
        {

            if (base.Contains(point))
                // Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
                // В случая на правоъгълник - директно връщаме true

                return true;
            else
                // Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
                return false;
        }
        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            base.RotateShape(grfx);


            grfx.DrawRectangle(new Pen(BorderColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            Point[] p = {
                new Point((int)Rectangle.X, (int)Rectangle.Y),
                new Point((int)(Rectangle.X + Rectangle.Width), (int)Rectangle.Y),
                new Point((int)Rectangle.X + ((int)Rectangle.Width / 2),(int)(Rectangle.Bottom - 50))};
            grfx.FillPolygon(new SolidBrush(FillColor), p);
            grfx.DrawPolygon(new Pen(BorderColor), p);
        }
    }
}
