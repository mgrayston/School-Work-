using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

namespace Model {
    public class World {
        private int id;
        private int worldSize;

        // Concurrent dictionaries are thread safe
        private ConcurrentDictionary<int, Ship> ships;
        private ConcurrentDictionary<int, Star> stars;
        private ConcurrentDictionary<int, Projectile> projectiles;
        private ConcurrentDictionary<int, Timer> timers;

        public World(int worldSize, int id = 0) {
            ships = new ConcurrentDictionary<int, Ship>();
            stars = new ConcurrentDictionary<int, Star>();
            projectiles = new ConcurrentDictionary<int, Projectile>();
            timers = new ConcurrentDictionary<int, Timer>();
            this.Id = id;
            this.worldSize = worldSize;
        }

        public int Id { get => id; set => id = value; }
        public int WorldSize { get => worldSize; }
        public ConcurrentDictionary<int, Ship> Ships { get => ships; }
        public ConcurrentDictionary<int, Star> Stars { get => stars; }
        public ConcurrentDictionary<int, Projectile> Projectiles { get => projectiles; }
        public ConcurrentDictionary<int, Timer> Timers { get => timers; }

        public void AddShip(Ship ship) {
            Ships.TryAdd(ship.id, ship);
        }

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

        /// <summary>
        /// Removes the ship with the specific ID from the world
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveShip(int ID)
        {
            ships.TryRemove(ID, out Ship s);
        }

        /// <summary>
        /// Removes the projectile with the specific ID from the world
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveProjectile(int ID)
        {
            projectiles.TryRemove(ID, out Projectile proj);
        }

        /// <summary>
        /// Returns a ship object based on an ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Ship GetShip(int ID)
        {
            IEnumerable<Ship> ships = GetShips();
            foreach(Ship s in ships)
            {
                if (s.id == ID)
                    return s;
            }
            return null;
        }
    }
}
