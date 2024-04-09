using Graphic.Objects;
using System.Diagnostics.Eventing.Reader;

namespace Graphic
{
    public partial class Form1 : Form
    {
        public static Random rnd = new Random();

        private Player player;
        private Marker marker;
        private GreenCircle greenCircle;
        private GreenCircle greenCircle1;
        private RedCircle redCircle;
        private List<BaseObject> objects = new();
        private int scoresCnt = 0;
        public Form1()
        {
            InitializeComponent();
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            //scoresCnt = 0;
            redCircle = new RedCircle(pbMain.Width / 2 - 200 + rnd.Next() % 400, pbMain.Height / 2 - 200 + rnd.Next() % 400, 0);
            
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnGreenCircleOverlap += (m) =>
            {
                scoresCnt++;
                scores.Text = "Очки: " + scoresCnt;
                objects.Remove(m);
                objects.Add(new GreenCircle(pbMain.Width / 2 - 200 + rnd.Next() % 400, pbMain.Width / 2 - 200 + rnd.Next() % 400, 0));
            };

            redCircle.OnPlayerOverlap += (m) =>
            {
                --scoresCnt;
                scores.Text = "Очки: " + scoresCnt;
                redCircle.radius = 1;
                redCircle.X = pbMain.Width / 2 - 200 + rnd.Next() % 400;
                redCircle.Y = pbMain.Width / 2 - 200 + rnd.Next() % 400;
                redCircle.Angle = 0;

            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);
            

            objects.Add(player);
            objects.Add(marker);
            objects.Add(redCircle);

            for (int i = 0; i < 2; i++)
            {
                objects.Add(new GreenCircle(pbMain.Width / 2 - 200 + rnd.Next() % 400, pbMain.Height / 2 - 200 + rnd.Next() % 400, 0));
            }
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            updatePlayer();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    //obj.Overlap(player);
                }
                if (redCircle.Overlaps(obj, g) && obj!=redCircle)
                {
                    redCircle.Overlap(obj);
                    obj.Overlap(redCircle);
                }
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //updatePlayer();

            pbMain.Invalidate();
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }
            marker.X = e.X;
            marker.Y = e.Y;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            foreach (var obj in objects.ToList())
            {
                if ((obj is GreenCircle) && ((GreenCircle)obj).IsZero())
                {
                    objects.Remove(obj);
                    objects.Add(new GreenCircle(pbMain.Width / 2 - 200 + rnd.Next() % 400, pbMain.Height / 2 - 200 + rnd.Next() % 400, 0));
                }
                else if ((obj is GreenCircle) && !((GreenCircle)obj).IsZero())
                {
                    ((GreenCircle)obj).Reduce();
                }
            }

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            foreach (var obj in objects.ToList())
            {
                if ((obj is RedCircle) && ((RedCircle)obj) != null)
                {
                    ((RedCircle)obj).Increase();
                }
            }
        }
    }
}
