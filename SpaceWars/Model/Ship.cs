using System.Diagnostics;
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

        private bool connected;

        private Stopwatch canFire;

        /// <summary>
        /// Field representing whether this ship is trying to turn or not; 1 signifies right turn, 0 no turn, and -1 left turn.
        /// </summary>
        private int toTurn;

        /// <summary>
        /// Tracks the number of times the ship hits an opponent with a projectile
        /// </summary>
        private int hits;

        /// <summary>
        /// Tracks number of shots fired
        /// </summary>
        private int shotsFired;

        /// <summary>
        /// Tracks the accuracy of the ship's shooting
        /// </summary>
        private int accuracy;


        public Ship() : this(-1, "", 0, 0) { }

        public Ship(int ID, string name, int x, int y) {
            this.ID = ID;
            this.name = name;
            this.loc = new Vector2D(x, y);
            this.dir = new Vector2D(0, -1);
            this.thrust = false;
            this.hp = 5;
            this.score = 0;
            this.Velocity = new Vector2D(0, 0);
            ToTurn = 0;
            connected = true;
            canFire = new Stopwatch();
            canFire.Start();
        }

        public void Respawn() {
            dir = new Vector2D(0, -1);
            thrust = false;
            velocity = new Vector2D(0, 0);
            HP = 5;
        }

        public bool Fire(int shotDelay) {
            if (canFire.ElapsedMilliseconds > shotDelay) {
                canFire.Restart();
                return true;
            }
            return false;
        }

        public int id { get => ID; }
        public string Name { get => name; }
        public Vector2D Loc { get => loc; set => loc = value; }
        public Vector2D Dir { get => dir; }
        public bool Thrust { get => thrust; set => thrust = value; }
        public int HP { get => hp; set => hp = value; }
        public int Score { get => score; set => score = value; }
        public Vector2D Velocity { get => velocity; set => velocity = value; }
        public bool Connected { get => connected; set => connected = value; }
        public int ToTurn { get => toTurn; set => toTurn = value; }

        //For database tracking
        public int Hits { get => hits; set => hits = value; }
        public int ShotsFired { get => shotsFired; set => shotsFired = value; }
        public int Accuracy { get => accuracy; set => accuracy = value; }

    }
}
