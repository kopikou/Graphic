using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic.Objects
{
    internal class GreenCircle : BaseObject
    {
        //public Action<GreenCircle> ReductionToZero;
        private int radius = 30;
        public GreenCircle(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render(Graphics g)
        { 
            g.FillEllipse(new SolidBrush(Color.Green), -this.radius, -this.radius, this.radius*2, this.radius * 2);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-this.radius, -this.radius, this.radius * 2, this.radius * 2);
            return path;
        }

        public void Reduce()
        {
            --radius;
        }
        public bool IsZero()
        {
            return radius == 0;
        }
    }
}
