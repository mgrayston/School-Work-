using Newtonsoft.Json;
using SpaceWars;

namespace Model {
    [JsonObject(MemberSerialization.OptIn)]
    public class Ship {
        /// <summary>
        /// Unique ship ID
        /// </summary>
        [JsonProperty(PropertyName = "ship")]
        private int ID;

        /// <summary>
        /// Player's name
        /// </summary>
        [JsonProperty]
        private string name;

        /// <summary>
        /// Vector2D representing the ship's location
        /// </summary>
        [JsonProperty]
        private Vector2D loc;

        /// <summary>
        /// Vector2D representing the ship's orientation
        /// </summary>
        [JsonProperty]
        private Vector2D dir;

        /// <summary>
        /// bool representing whether or not the ship was firing engines on that frame. 
        /// This can be used by the client to draw a different representation of the ship, e.g., showing engine exhaust
        /// </summary>
        [JsonProperty]
        private bool thrust;

        /// <summary>
        /// Int representing the hit points of the ship. 
        /// This value ranges from 0 - 5. If it is 0, then this ship is temporarily destroyed, and waiting to respawn. 
        /// If the player controlling this ship disconnects, the server will discontinue sending this ship
        /// </summary>
        [JsonProperty]
        private int hp;

        /// <summary>
        /// Int representing the ship's score
        /// </summary>
        [JsonProperty]
        private int score;

        /// <summary>
        /// Used to store this Ship's velocity
        /// </summary>
        private Vector2D velocity;

        public Ship() : this(-1, "") { }

        public Ship(int ID, string name) {
            this.ID = ID;
            this.name = name;
            this.loc = new Vector2D();
            this.dir = new Vector2D(0, 1);
            this.thrust = false;
            this.hp = 5;
            this.score = 0;
            this.Velocity = new Vector2D(0, 0);
        }

        public int id { get => ID; }
        public string Name { get => name; }
        public Vector2D Loc { get => loc; set => loc = value; }
        public Vector2D Dir { get => dir; }
        public bool Thrust { get => thrust; }
        public int HP { get => hp; set => hp = value; }
        public int Score { get => score; }
        public Vector2D Velocity { get => velocity; set => velocity = value; }
    }
}
