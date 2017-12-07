using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ModelTesting
{
    [TestClass]
    public class WorldTesting
    {
        [TestMethod]
        public void TestWorldConstructor()
        {
            World w = new World(750);
            Assert.AreEqual(0, w.Id);
            Assert.AreEqual(750, w.WorldSize);

            ConcurrentDictionary<int,Ship> ships = w.Ships;
            ConcurrentDictionary<int, Star> stars = w.Stars;
            ConcurrentDictionary<int, Projectile> projectiles = w.Projectiles;
            Assert.AreEqual(0, ships.Count);
            Assert.AreEqual(0, stars.Count);
            Assert.AreEqual(0, projectiles.Count);

            World w2 = new World(750, 1);
            Assert.AreEqual(1, w2.Id);
        }

        [TestMethod]
        public void TestID()
        {
            World w = new World(750);
            Assert.AreEqual(0, w.Id);
            w.Id = 1;
            Assert.AreEqual(1, w.Id);
        }

        [TestMethod]
        public void TestAddShip()
        {
            World w = new World(750);
            ConcurrentDictionary<int, Ship> ships = w.Ships;
            Assert.AreEqual(0, ships.Count);
            Ship s = new Ship();
            w.AddShip(s);
            Assert.AreEqual(1, ships.Count);
        }

        [TestMethod]
        public void TestAddStar()
        {
            World w = new World(750);
            ConcurrentDictionary<int, Star> stars = w.Stars;
            Assert.AreEqual(0, stars.Count);
            Star s = new Star();
            w.AddStar(s);
            Assert.AreEqual(1, stars.Count);
        }

        [TestMethod]
        public void TestRemoveShip()
        {
            World w = new World(750);
            ConcurrentDictionary<int, Ship> ships = w.Ships;
            Ship s = new Ship();
            w.AddShip(s);
            Assert.AreEqual(1, ships.Count);
            w.RemoveShip(-1);
            Assert.AreEqual(0, ships.Count);
            w.RemoveShip(-1);
        }

        [TestMethod]
        public void TestRemoveProjectile()
        {
            World w = new World(750);
            w.RemoveProjectile(-1);
        }

        [TestMethod]
        public void TestGetName()
        {
            World w = new World(750);
            ConcurrentDictionary<int, Ship> ships = w.Ships;
            Ship s = new Ship();
            w.AddShip(s);
            Assert.AreEqual("", w.GetName(-1));
            Ship s2 = new Ship(1, "We da best", 0, 0);
            w.AddShip(s2);
            Assert.AreEqual("We da best", w.GetName(1));
        }

        [TestMethod]
        public void TestAddPoint()
        {
            World w = new World(750);
            Ship s = new Ship();
            w.AddShip(s);
            Assert.AreEqual(0, s.Score);
            w.AddPoint(-1);
            Assert.AreEqual(1, s.Score);

        }
    }
}
