using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

namespace Model {
    public class World {
        private int id;
        private int worldSize;
        private int projectileId;

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
            projectileId = 0;
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

        public void AddStar(Star star) {
            Stars.TryAdd(star.id, star);
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
        public void RemoveShip(int ID) {
            ships.TryRemove(ID, out Ship s);
        }

        /// <summary>
        /// Removes the projectile with the specific ID from the world
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveProjectile(int ID) {
            projectiles.TryRemove(ID, out Projectile dispose);
        }

        /// <summary>
        /// Returns a ship object based on an ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Ship GetShip(int ID) {
            if (ships[ID] != null) {
                return ships[ID];
            }
            return null;
        }

        public void AddProjectile(int ownerId, double locX, double locY, double dirX, double dirY) {
            projectiles[projectileId] = new Projectile(projectileId++, ownerId, (int)locX, (int)locY, dirX, dirY);
        }

        public void AddPoint(int shipID) {
            ships[shipID].Score++;
        }

        public string GetName(int id) {
            return GetShip(id).Name;
        }
    }
}
