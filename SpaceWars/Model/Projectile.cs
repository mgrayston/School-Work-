﻿using SpaceWars;
using Newtonsoft.Json;

namespace Model {
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile {
        /// <summary>
        /// Unique projectile ID
        /// </summary>
        [JsonProperty(PropertyName = "proj")]
        private int ID;

        /// <summary>
        /// Vector2D representing the location
        /// </summary>
        [JsonProperty]
        private Vector2D loc;

        /// <summary>
        /// Vector2D representing the orientation
        /// </summary>
        [JsonProperty]
        private Vector2D dir;

        /// <summary>
        /// a bool representing if the projectile is active or not
        /// </summary>
        [JsonProperty]
        private bool alive;

        /// <summary>
        /// an int representing the ID of the ship that created the projectile.
        /// </summary>
        [JsonProperty]
        private int owner;

        public Projectile() : this(-1, -1, 0, 0, 0, 0) { }

        public Projectile(int ID, int owner, int locX, int locY, double dirX, double dirY) {
            this.ID = ID;
            this.owner = owner;
            this.loc = new Vector2D(locX, locY);
            this.dir = new Vector2D(dirX, dirY);
            this.alive = true;
        }

        public int id { get => ID; }
        public int Owner { get => owner; }
        public Vector2D Loc { get => loc; set => loc = value; }
        public Vector2D Dir { get => dir; }
        public bool Alive { get => alive; set => alive = value; }
    }
}
