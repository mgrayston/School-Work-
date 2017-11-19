using Model;
using SpaceWars;
using System.Drawing;
using System.Windows.Forms;

namespace View {
    class DrawingPanel : Panel {
        // World this DrawingPanel draws
        private World theWorld;

        // Info for drawing Stars
        private static int starSize = 60;
        Point starPnt = new Point(-(starSize / 2), -(starSize / 2));
        Bitmap starImg = new Bitmap(Properties.Resources.star, new Size(starSize, starSize));

        // Info for drawing Projectiles
        private static int projSize = 10;
        Point projPnt = new Point(-(projSize / 2), -(projSize / 2));

        Bitmap blueProj = new Bitmap(Properties.Resources.shot_blue, new Size(projSize, projSize));
        Bitmap brownProj = new Bitmap(Properties.Resources.shot_brown, new Size(projSize, projSize));
        Bitmap greenProj = new Bitmap(Properties.Resources.shot_green, new Size(projSize, projSize));
        Bitmap greyProj = new Bitmap(Properties.Resources.shot_grey, new Size(projSize, projSize));
        Bitmap redProj = new Bitmap(Properties.Resources.shot_red, new Size(projSize, projSize));
        Bitmap violetProj = new Bitmap(Properties.Resources.shot_violet, new Size(projSize, projSize));
        Bitmap whiteProj = new Bitmap(Properties.Resources.shot_white, new Size(projSize, projSize));
        Bitmap yellowProj = new Bitmap(Properties.Resources.shot_yellow, new Size(projSize, projSize));

        // Info for drawing Ships
        private static int shipSize = 25;
        Point shipPnt = new Point(-(shipSize / 2), -(shipSize / 2));

        Bitmap blueCoastImg = new Bitmap(Properties.Resources.ship_coast_blue, new Size(shipSize, shipSize));
        Bitmap blueThrustImg = new Bitmap(Properties.Resources.ship_thrust_blue, new Size(shipSize, shipSize));

        Bitmap brownCoastImg = new Bitmap(Properties.Resources.ship_coast_brown, new Size(shipSize, shipSize));
        Bitmap brownThrustImg = new Bitmap(Properties.Resources.ship_thrust_brown, new Size(shipSize, shipSize));

        Bitmap greenCoastImg = new Bitmap(Properties.Resources.ship_coast_green, new Size(shipSize, shipSize));
        Bitmap greenThrustImg = new Bitmap(Properties.Resources.ship_thrust_green, new Size(shipSize, shipSize));

        Bitmap greyCoastImg = new Bitmap(Properties.Resources.ship_coast_grey, new Size(shipSize, shipSize));
        Bitmap greyThrustImg = new Bitmap(Properties.Resources.ship_thrust_grey, new Size(shipSize, shipSize));

        Bitmap redCoastImg = new Bitmap(Properties.Resources.ship_coast_red, new Size(shipSize, shipSize));
        Bitmap redThrustImg = new Bitmap(Properties.Resources.ship_thrust_red, new Size(shipSize, shipSize));

        Bitmap violetCoastImg = new Bitmap(Properties.Resources.ship_coast_violet, new Size(shipSize, shipSize));
        Bitmap violetThrustImg = new Bitmap(Properties.Resources.ship_thrust_violet, new Size(shipSize, shipSize));

        Bitmap whiteCoastImg = new Bitmap(Properties.Resources.ship_coast_white, new Size(shipSize, shipSize));
        Bitmap whiteThrustImg = new Bitmap(Properties.Resources.ship_thrust_white, new Size(shipSize, shipSize));

        Bitmap yellowCoastImg = new Bitmap(Properties.Resources.ship_coast_yellow, new Size(shipSize, shipSize));
        Bitmap yellowThrustImg = new Bitmap(Properties.Resources.ship_thrust_yellow, new Size(shipSize, shipSize));

        /// <summary>
        /// Constructor for DrawingPanel
        /// </summary>
        /// <param name="w">World that is being drawn on this DrawingPanel.</param>
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

        /// <summary>
        /// Draws Ships. Color is picked based on ID.
        /// </summary>
        /// <param name="o">Object to draw</param>
        /// <param name="e">PaintEventArgs</param>
        private void ShipDrawer(object o, PaintEventArgs e) {
            Ship ship = o as Ship;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            switch (ship.id % 8) {
                case 1:
                    if (ship.HP > 0) {
                        if (ship.Thrust) {
                            e.Graphics.DrawImage(blueThrustImg, shipPnt);
                        }
                        else {
                            e.Graphics.DrawImage(blueCoastImg, shipPnt);
                        }
                    }
                    break;
                case 2:
                    if (ship.HP > 0) {
                        if (ship.Thrust) {
                            e.Graphics.DrawImage(brownThrustImg, shipPnt);
                        }
                        else {
                            e.Graphics.DrawImage(brownCoastImg, shipPnt);
                        }
                    }
                    break;
                case 3:
                    if (ship.HP > 0) {
                        if (ship.Thrust) {
                            e.Graphics.DrawImage(greenThrustImg, shipPnt);
                        }
                        else {
                            e.Graphics.DrawImage(greenCoastImg, shipPnt);
                        }
                    }
                    break;
                case 4:
                    if (ship.HP > 0) {
                        if (ship.Thrust) {
                            e.Graphics.DrawImage(greyThrustImg, shipPnt);
                        }
                        else {
                            e.Graphics.DrawImage(greyCoastImg, shipPnt);
                        }
                    }
                    break;
                case 5:
                    if (ship.HP > 0) {
                        if (ship.Thrust) {
                            e.Graphics.DrawImage(redThrustImg, shipPnt);
                        }
                        else {
                            e.Graphics.DrawImage(redCoastImg, shipPnt);
                        }
                    }
                    break;
                case 6:
                    if (ship.HP > 0) {
                        if (ship.Thrust) {
                            e.Graphics.DrawImage(violetThrustImg, shipPnt);
                        }
                        else {
                            e.Graphics.DrawImage(violetCoastImg, shipPnt);
                        }
                    }
                    break;
                case 7:
                    if (ship.HP > 0) {
                        if (ship.Thrust) {
                            e.Graphics.DrawImage(whiteThrustImg, shipPnt);
                        }
                        else {
                            e.Graphics.DrawImage(whiteCoastImg, shipPnt);
                        }
                    }
                    break;
                case 0:
                    if (ship.HP > 0) {
                        if (ship.Thrust) {
                            e.Graphics.DrawImage(yellowThrustImg, shipPnt);
                        }
                        else {
                            e.Graphics.DrawImage(yellowCoastImg, shipPnt);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Draws Projectiles. Color is based on Projectile
        /// owner's ID.
        /// </summary>
        /// <param name="o">Object to draw</param>
        /// <param name="e">PaintEventArgs</param>
        private void ProjectileDrawer(object o, PaintEventArgs e) {
            Projectile proj = o as Projectile;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            switch (proj.Owner % 8) {
                case 1:
                    if (proj.Alive) {
                        e.Graphics.DrawImage(blueProj, projPnt);
                    }
                    break;
                case 2:
                    if (proj.Alive) {
                        e.Graphics.DrawImage(brownProj, projPnt);
                    }
                    break;
                case 3:
                    if (proj.Alive) {
                        e.Graphics.DrawImage(greenProj, projPnt);
                    }
                    break;
                case 4:
                    if (proj.Alive) {
                        e.Graphics.DrawImage(greyProj, projPnt);
                    }
                    break;
                case 5:
                    if (proj.Alive) {
                        e.Graphics.DrawImage(redProj, projPnt);
                    }
                    break;
                case 6:
                    if (proj.Alive) {
                        e.Graphics.DrawImage(violetProj, projPnt);
                    }
                    break;
                case 7:
                    if (proj.Alive) {
                        e.Graphics.DrawImage(whiteProj, projPnt);
                    }
                    break;
                case 0:
                    if (proj.Alive) {
                        e.Graphics.DrawImage(yellowProj, projPnt);
                    }
                    break;
            }
        }

        /// <summary>
        /// Used to draw Stars.
        /// </summary>
        /// <param name="o">Object to draw</param>
        /// <param name="e">PaintEventArgs</param>
        private void StarDrawer(object o, PaintEventArgs e) {
            Star s = o as Star;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(starImg, starPnt);
        }

        /// <summary>
        /// This method is invoked when the DrawingPanel needs to be re-drawn 
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
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
