using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Model {
    public class World {
        private int id;
        private int worldSize;

        //concurrent dictionaries are thread safe
        private ConcurrentDictionary<int, Ship> ships;
        private ConcurrentDictionary<int, Star> stars;
        private ConcurrentDictionary<int, Projectile> projectiles;

        public World(int worldSize, int id = 0) {
            ships = new ConcurrentDictionary<int, Ship>();
            stars = new ConcurrentDictionary<int, Star>();
            projectiles = new ConcurrentDictionary<int, Model.Projectile>();
            this.id = id;
            this.worldSize = worldSize;
        }
        
        public int Id { get => id; }
        public int WorldSize { get => worldSize; }
        public ConcurrentDictionary<int, Ship> Ships { get => ships; set => ships = value; }
        public ConcurrentDictionary<int, Star> Stars { get => stars; set => stars = value; }
        public ConcurrentDictionary<int, Projectile> Projectiles { get => projectiles; set => projectiles = value; }

    }
}
