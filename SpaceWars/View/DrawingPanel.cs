﻿using Model;
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

        //todo - copied from canvas
        private void ShipDrawer(object o, PaintEventArgs e)
        {
            int shipWidth = 35;
            Ship s = o as Ship;
            //if (s.ID == ...)
            //    color = ...;
            //else if (...)
            //    color = ...;
            //TODO - need 7 more colors
            using (System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green))
            {
                Rectangle r = new Rectangle(-(shipWidth / 2), -(shipWidth / 2), shipWidth, shipWidth);
                e.Graphics.FillRectangle(greenBrush, r);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (KeyValuePair<int, Ship> s in theWorld.Ships)
            {
                Vector2D location = s.Value.Loc;
                Vector2D orientation = s.Value.Dir;
                DrawObjectWithTransform(e, s, theWorld.WorldSize, location.GetX(), location.GetY(), orientation.ToAngle(), ShipDrawer);
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
