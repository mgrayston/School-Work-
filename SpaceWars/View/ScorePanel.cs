using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    class ScorePanel : Panel
    {
        // World this DrawingPanel draws
        private World theWorld;

        /// <summary>
        /// Constructor for DrawingPanel
        /// </summary>
        /// <param name="w">World that is being drawn on this DrawingPanel.</param>
        public ScorePanel(World w)
        {
            this.DoubleBuffered = true;
            theWorld = w;
        }

        private void ScoreboardDrawer(PaintEventArgs e)
        {
            int yLoc = 0;
            int xLoc = theWorld.WorldSize + 2;
            // Draw the scoreboard
            foreach (Ship player in theWorld.GetShips())
            {
                yLoc += 20; //mode down for each iteration
                e.Graphics.DrawString(player.Name + ": " + player.Score, new Font("Arial", 16), new SolidBrush(Color.Red), new Point(xLoc, yLoc));
            }
        }

        private Color GetTextColor(int ID)
        {
            Color retColor = Color.Black;
            switch (ID)
            {
                case 1:
                    retColor =  Color.Blue;
                    break;
                case 2:
                    retColor = Color.Brown;
                    break;
                case 3:
                    retColor = Color.Green;
                    break;
                case 4:
                    retColor = Color.Gray;
                    break;
                case 5:
                    retColor = Color.Red;
                    break;
                case 6:
                    retColor = Color.Violet;
                    break;
                case 7:
                    retColor = Color.Black;
                    break;
                case 0:
                    retColor = Color.Yellow;
                    break;
            }
            return retColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int yLoc = 0;
            int xLoc = 0;
            // Draw the scoreboard
            foreach (Ship player in theWorld.GetShips())
            {
                yLoc += 20;
                e.Graphics.DrawString(player.Name + ": " + player.Score, new Font("Arial", 16), new SolidBrush(GetTextColor(player.id)), new Point(xLoc, yLoc));
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }

    }
}
