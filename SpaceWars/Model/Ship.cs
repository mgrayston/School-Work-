using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Ship
    {
        /// <summary>
        /// Unique ship ID
        /// </summary>
        private int ship;

        /// <summary>
        /// Player's name
        /// </summary>
        private string name;

        /// <summary>
        /// Vector2D representing the ship's location
        /// </summary>
        private Vector2D loc;

        /// <summary>
        /// Vector2D representing the ship's orientation
        /// </summary>
        private Vector2D dir;

        /// <summary>
        /// bool representing whether or not the ship was firing engines on that frame. 
        /// This can be used by the client to draw a different representation of the ship, e.g., showing engine exhaust
        /// </summary>
        private bool thrust;

        /// <summary>
        /// Int representing the hit points of the ship. 
        /// This value ranges from 0 - 5. If it is 0, then this ship is temporarily destroyed, and waiting to respawn. 
        /// If the player controlling this ship disconnects, the server will discontinue sending this ship
        /// </summary>
        private int hp;

        /// <summary>
        /// int representing the ship's score
        /// </summary>
        private int score;

        public Ship(int ship, string name)
        {
            this.ship = ship;
            this.name = name;
            this.loc = new Vector2D(); //TODO randomize starting loc?
            this.dir = new Vector2D(); //TODO randomize
            this.thrust = false;
            this.hp = 5;
            this.score = 0;
        }

        public int getID{get => ship;}
        public string getName { get => name; }
        public Vector2D getLoc { get => loc; }
        public Vector2D getDir { get => dir; }
        public bool getThrust { get => thrust; }
        public int getHP { get => hp; }
        public int getScore { get => score; }

    }
}
