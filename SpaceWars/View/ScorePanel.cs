using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View {
    class ScorePanel : Panel {
        // World this ScorePanel draws
        private World theWorld;

        /// <summary>
        /// Constructor for ScorePanel
        /// </summary>
        /// <param name="w">World that is being drawn on this ScorePanel.</param>
        public ScorePanel(World w) {
            this.DoubleBuffered = true;
            theWorld = w;
        }

        /// <summary>
        /// Returns a color based on the ship ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private Color GetPenColor(int ID) {
            switch (ID % 8) {
                case 1:
                    return Color.Blue;
                case 2:
                    return Color.Brown;
                case 3:
                    return Color.Green;
                case 4:
                    return Color.Gray;
                case 5:
                    return Color.Red;
                case 6:
                    return Color.Violet;
                case 7:
                    return Color.Silver;
                case 0:
                    return Color.Yellow;
            }
            return Color.Black;
        }

        /// <summary>
        /// Draws the scoreboard and health bars.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e) {
            int yLoc = 0;
            SolidBrush textBrush = new SolidBrush(Color.Black);
            Font textFont = new Font(FontFamily.GenericSerif, 16);
            Pen rectPen = new Pen(Color.Black);

            // Draw the scoreboard
            foreach (Ship ship in theWorld.GetShips()) {
                e.Graphics.DrawString(ship.Name + ": " + ship.Score, textFont, textBrush, new Point(0, yLoc));
                e.Graphics.FillRectangle(new SolidBrush(GetPenColor(ship.id)), 95, yLoc, 30 * ship.HP, 20);

                // Draws bars to separate the HP of the ship (five rectangles)
                for (int barChunk = 0; barChunk < 5; barChunk++) {
                    e.Graphics.DrawRectangle(rectPen, 95 + 30 * barChunk, yLoc, 30, 20);
                }
                yLoc += 20;
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
