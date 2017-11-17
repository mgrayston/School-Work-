using System.Collections.Generic;

namespace Model {
    public class World {
        private int id;
        private int worldSize;

        private Dictionary<int, Ship> ships;
        private Dictionary<int, Star> stars;
        private Dictionary<int, Projectile> projectiles;

        public World(int worldSize, int id = 0) {
            ships = new Dictionary<int, Ship>();
            stars = new Dictionary<int, Star>();
            projectiles = new Dictionary<int, Model.Projectile>();
            this.id = id;
            this.worldSize = worldSize;
        }
        
        public int Id { get => id; }
        public int WorldSize { get => worldSize; }
        public Dictionary<int, Ship> Ships { get => ships; set => ships = value; }
        public Dictionary<int, Star> Stars { get => stars; set => stars = value; }
        public Dictionary<int, Projectile> Projectiles { get => projectiles; set => projectiles = value; }

    }
}
