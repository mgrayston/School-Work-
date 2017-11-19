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
            this.Id = id;
            this.WorldSize = worldSize;
        }
        
        public int Id { get => id; set => id = value; }
        public int WorldSize { get => worldSize; set => worldSize = value; }
        public ConcurrentDictionary<int, Ship> Ships { get => ships; set => ships = value; }
        public ConcurrentDictionary<int, Star> Stars { get => stars; set => stars = value; }
        public ConcurrentDictionary<int, Projectile> Projectiles { get => projectiles; set => projectiles = value; }

        /// <summary>
        /// Used to return an Enumerable list of Ships.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Ship> GetShips() {
            return ships.Values;
        }
        
        /// <summary>
        /// Used to return an Enumerable list of Projectiles.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Projectile> GetProjectiles() {
            return projectiles.Values;
        }

        /// <summary>
        /// Used to return an Enumerable list of Suns.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Star> GetStars() {
            return stars.Values;
        }
    }
}
