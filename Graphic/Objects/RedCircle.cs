using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic.Objects
{
    internal class RedCircle : BaseObject
    {
        private static Random rnd = new Random();
        public Action<Player> OnPlayerOverlap;
        private int radius = 1;
        public RedCircle(float x, float y, float angle) : base(x, y, angle)
        {
        }
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.MistyRose), -this.radius, -this.radius, this.radius * 2, this.radius * 2);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-this.radius, -this.radius, this.radius * 2, this.radius * 2);
            return path;
        }
        public void Increase()
        {
            ++radius;
        }
        public void Change(float paintWidth)
        {
            radius = 1;
            X = paintWidth / 2 - 200 + rnd.Next() % 400;
            Y = paintWidth / 2 - 200 + rnd.Next() % 400;
            Angle = 0;
        }
        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);
            if (obj is Player)
            {
                OnPlayerOverlap(obj as Player);
            }
        }
    }
}
