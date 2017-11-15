using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Projectile
    {
        /// <summary>
        /// Unique projectile ID
        /// </summary>
        private int proj;


        /// <summary>
        /// Vector2D representing the location
        /// </summary>
        private Vector2D loc;

        /// <summary>
        /// Vector2D representing the orientation
        /// </summary>
        private Vector2D dir;

        /// <summary>
        /// a bool representing if the projectile is active or not
        /// </summary>
        private bool alive;

        /// <summary>
        /// an int representing the ID of the ship that created the projectile.
        /// </summary>
        private int owner;

        public Projectile(int proj, int owner)
        {
            this.proj = proj;
            this.owner = owner;
            this.loc = new Vector2D(); //TODO front location of whever the ship is.
            this.dir = new Vector2D(); //TODO direction of ship
            this.alive = false;
        }

        public int getID { get => proj; }
        public int getOwner { get => owner; }
        public Vector2D getLoc { get => loc; }
        public Vector2D getDir { get => dir; }
        public bool Alive { get => alive; }
    }
}
