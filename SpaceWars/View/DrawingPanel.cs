using Model;
using SpaceWars;
using System.Drawing;
using System.Windows.Forms;

namespace View {
    class DrawingPanel : Panel {
        private World theWorld;
        private static int starSize = 60;
        Point starPnt = new Point(-(starSize / 2), -(starSize / 2));
        Bitmap starImg = new Bitmap(Properties.Resources.star, new Size(starSize, starSize));

        public DrawingPanel(World w) {
            this.DoubleBuffered = true;
            theWorld = w;
        }

        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w) {
            return (int)w + size / 2;
        }

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer) {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, worldX);
            int y = WorldSpaceToImageSpace(worldSize, worldY);
            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }

        private void ShipDrawer(object o, PaintEventArgs e) {
            int shipWidth = 20;
            Ship s = o as Ship;

            // first eight ships are different colors. After eight they are the same.
            //TODO - Could add a radomizing of color after the eighth ship?
            //TODO - need to add ship spirtes.
            using (System.Drawing.SolidBrush magentaBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Magenta))
            using (System.Drawing.SolidBrush grayBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray))
            using (System.Drawing.SolidBrush yellowBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow))
            using (System.Drawing.SolidBrush orangeBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Orange))
            using (System.Drawing.SolidBrush whiteBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White))
            using (System.Drawing.SolidBrush purpleBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Purple))
            using (System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))
            using (System.Drawing.SolidBrush blueBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue))
            using (System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green)) {
                Rectangle r = new Rectangle(-(shipWidth / 2), -(shipWidth / 2), shipWidth, shipWidth);
                if (s.id == 1)
                    e.Graphics.FillRectangle(greenBrush, r);
                else if (s.id == 2)
                    e.Graphics.FillRectangle(blueBrush, r);
                else if (s.id == 3)
                    e.Graphics.FillRectangle(redBrush, r);
                else if (s.id == 4)
                    e.Graphics.FillRectangle(purpleBrush, r);
                else if (s.id == 5)
                    e.Graphics.FillRectangle(whiteBrush, r);
                else if (s.id == 6)
                    e.Graphics.FillRectangle(orangeBrush, r);
                else if (s.id == 7)
                    e.Graphics.FillRectangle(yellowBrush, r);
                else if (s.id == 8)
                    e.Graphics.FillRectangle(grayBrush, r);
                else if (s.id > 8)
                    e.Graphics.FillRectangle(magentaBrush, r);
            }
        }

        private void ProjectileDrawer(object o, PaintEventArgs e) {
            int projWidth = 6;
            int projHeight = 6;
            Projectile p = o as Projectile;

            using (System.Drawing.Bitmap starImg = new System.Drawing.Bitmap(Properties.Resources.star, new Size(projWidth, projHeight))) {
                Point pnt = new Point(-(projWidth / 2), -(projHeight / 2));

                e.Graphics.DrawImage(starImg, pnt);
            }
        }

        private void StarDrawer(object o, PaintEventArgs e) {
            Star s = o as Star;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(starImg, starPnt);
        }

        /// <summary>
        /// Returns a color based on the ID. There are 8 unique colors.
        /// </summary>
        /// <param name="ID"> Unique Identification number</param>
        /// <returns>A color based on ID</returns>

        // TODO change so that it returns the sprite instead maybe?
        private Color getIDColor(int ID) {
            Color retColor;
            int modResult = ID % 10;
            switch (modResult) {
                case 0:
                    retColor = Color.Blue; break;
                case 1:
                    retColor = Color.Brown; break;
                case 2:
                    retColor = Color.Green; break;
                case 3:
                    retColor = Color.Gray; break;
                case 4:
                    retColor = Color.Red; break;
                case 5:
                    retColor = Color.Violet; break;
                case 6:
                    retColor = Color.White; break;
                case 7:
                    retColor = Color.Yellow; break;
                case 8:
                    retColor = Color.Blue; break;
                case 9:
                    retColor = Color.Red; break;
                default:
                    retColor = Color.Green; break;
            }
            return retColor;
        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e) {
            lock (theWorld) {
                // Draw the stars
                foreach (Star star in theWorld.GetStars()) {
                    DrawObjectWithTransform(e, star, theWorld.WorldSize, star.Loc.GetX(), star.Loc.GetY(), 0, StarDrawer);
                }

                // Draw the ships
                foreach (Ship ship in theWorld.GetShips()) {
                    Vector2D loc = ship.Loc;
                    Vector2D dir = ship.Dir;
                    DrawObjectWithTransform(e, ship, theWorld.WorldSize, loc.GetX(), loc.GetY(), dir.ToAngle(), ShipDrawer);
                }

                // Draw the projectiles
                foreach (Projectile projectile in theWorld.GetProjectiles()) {
                    DrawObjectWithTransform(e, projectile, theWorld.WorldSize, projectile.Loc.GetX(), projectile.Loc.GetY(), projectile.Dir.ToAngle(), ProjectileDrawer);
                }
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
