using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    [Serializable]
    class GroupShape : Shape
    {
        public List<Shape> SubItems { get; set; }

        #region Constructor

        public GroupShape(RectangleF rect) : base(rect)
        {
            SubItems = new List<Shape>();
        }

        public GroupShape(GroupShape rectangle) : base(rectangle)
        {
            SubItems = new List<Shape>();
        }

        #endregion

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                foreach (var item in SubItems)
                {
                    if (item.Contains(point))
                        return true;
                }
                return true;
            }
            else
                return false;
        }


        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            foreach(var item in SubItems)
            {
                item.DrawSelf(grfx);
            }
        }

        public override void Move(float dx, float dy)
        {
            base.Move(dx, dy);
            foreach(var item in SubItems)
            {
                item.Move(dx, dy);
            }
        }

        public override Color FillColor
        {
            set
            {
                foreach(var item in SubItems)
                {
                    item.FillColor = value;
                }
            }
        }

        public override Color BorderColor
        {
            set
            {
                foreach(var item in SubItems)
                {
                    item.BorderColor = value;
                }
            }
        }

        public override void GroupOpacity(int opacity)
        {
            base.GroupOpacity(opacity);
            foreach(var item in SubItems)
            {
                item.Opacity = opacity;
            }    
        }

        public override void GroupRotate(float angle)
        {
            base.GroupRotate(angle);
            foreach(var item in SubItems)
            {
                item.Angle = angle;
            }
        }


    }
}
