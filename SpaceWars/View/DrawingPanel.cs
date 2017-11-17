using Model;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    class DrawingPanel : Panel
    {
        private World theWorld;
        public DrawingPanel(Model.World w)
        {
            DoubleBuffered = true;
            theWorld = w;
        }

        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w)
        {
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
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
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

        private void ShipDrawer(object o, PaintEventArgs e)
        {
            int shipWidth = 35;
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
            using (System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green))
            {
                Rectangle r = new Rectangle(-(shipWidth / 2), -(shipWidth / 2), shipWidth, shipWidth);
                if (s.ID == 1)
                    e.Graphics.FillRectangle(greenBrush, r);
                else if (s.ID == 2)
                    e.Graphics.FillRectangle(blueBrush, r);
                else if (s.ID == 3)
                    e.Graphics.FillRectangle(redBrush, r);
                else if (s.ID == 4)
                    e.Graphics.FillRectangle(purpleBrush, r);
                else if (s.ID == 5)
                    e.Graphics.FillRectangle(whiteBrush, r);
                else if (s.ID == 6)
                    e.Graphics.FillRectangle(orangeBrush, r);
                else if (s.ID == 7)
                    e.Graphics.FillRectangle(yellowBrush, r);
                else if (s.ID == 8)
                    e.Graphics.FillRectangle(grayBrush, r);
                else if (s.ID > 8)
                    e.Graphics.FillRectangle(magentaBrush, r);
            }
        }

        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            int projWidth = 2;
            int projHeight = 6;
            Projectile p = o as Projectile;

            using (System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))
            {
                Rectangle r = new Rectangle(-(projWidth / 2), -(projHeight / 2), projWidth, projHeight);
                e.Graphics.FillRectangle(redBrush, r);
            }
        }

        private void StarDrawer(object o, PaintEventArgs e)
        {
            Star s = o as Star;
            int width = 10;
            int height = 10;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (System.Drawing.Bitmap starImg = new System.Drawing.Bitmap(Image.FromFile("..\\..\\Resources\\Graphics\\star.jpg"), new Size(width, height)))
            {
                Point pnt = new Point(-(width / 2), -(height / 2));

                e.Graphics.DrawImage(starImg, pnt);     
            }
        }
    }
}
