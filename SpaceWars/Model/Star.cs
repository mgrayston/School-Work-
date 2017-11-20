using Newtonsoft.Json;
using SpaceWars;

namespace Model {
    [JsonObject(MemberSerialization.OptIn)]
    public class Star {
        /// <summary>
        /// Unique ID
        /// </summary>
        [JsonProperty(PropertyName = "star")]
        private int ID;

        /// <summary>
        /// Vector2D representing the location
        /// </summary>
        [JsonProperty]
        private Vector2D loc;

        /// <summary>
        /// a double representing the star's mass. Note that the sample client does not use this information, 
        /// but you may choose to display stars differently based on their mass.
        /// </summary>
        [JsonProperty]
        private double mass;

        public Star(int ID, double mass) {
            this.ID = ID;
            this.loc = new Vector2D();
            this.mass = mass;
        }

        public int id { get => ID; }
        public Vector2D Loc { get => loc; }
        public double Mass { get => mass; }
    }
}
