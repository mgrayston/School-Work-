using SpaceWars;
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

        public Projectile(int proj, int owner) {
            this.ID = proj;
            this.owner = owner;
            this.loc = new Vector2D(); //TODO front location of whever the ship is.
            this.dir = new Vector2D(); //TODO direction of ship
            this.alive = false;
        }

        public int id { get => ID; }
        public int Owner { get => owner; }
        public Vector2D Loc { get => loc; }
        public Vector2D Dir { get => dir; }
        public bool Alive { get => alive; }
    }
}
