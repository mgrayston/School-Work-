using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Collections.Generic;
using System.Collections.Concurrent;
using SpaceWars;

namespace ModelTesting
{
    [TestClass]
    public class ModelTesting
    {
        //
        //World Testing
        //

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

        //
        //Ship Testing
        //

        [TestMethod]
        public void TestShipConstructor()
        {
            Ship s = new Ship();
            Assert.AreEqual(-1, s.id);
            Assert.AreEqual("", s.Name);
            Assert.AreEqual(0, s.Score);
            Assert.AreEqual(0, s.ToTurn);
            s.ToTurn = 2;
            Assert.AreEqual(2, s.ToTurn);
            Assert.AreEqual(true, s.Connected);
            s.Connected = false;
            Assert.AreEqual(false, s.Connected);


            Ship s2 = new Ship(1, "testing", 0, 0);
            Assert.AreEqual(1, s2.id);
            Assert.AreEqual("testing", s2.Name);
        }

        [TestMethod]
        public void TestRespawn()
        {
            Ship s = new Ship();
            s.Thrust = true;
            s.HP = 0;
            Assert.IsTrue(s.Thrust);
            Assert.AreEqual(0, s.HP);
            s.Respawn();
            Assert.IsFalse(s.Thrust);
            Assert.AreEqual(5, s.HP);
        }

        [TestMethod]
        public void TestSetLocation()
        {
            Ship s = new Ship();
            s.Loc = new Vector2D(1, 1);
            Assert.AreEqual(1, s.Loc.GetX());
            Assert.AreEqual(1, s.Loc.GetY());
        }

        [TestMethod]
        public void TestGetDir()
        {
            Ship s = new Ship();
            Assert.AreEqual(0, s.Dir.GetX());
            Assert.AreEqual(-1, s.Dir.GetY());
        }

        [TestMethod]
        public void TestGetVelocity()
        {
            Ship s = new Ship();
            Assert.AreEqual(0, s.Velocity.GetX());
            Assert.AreEqual(0, s.Velocity.GetY());
            s.Velocity = new Vector2D(10, -10);
            Assert.AreEqual(10, s.Velocity.GetX());
            Assert.AreEqual(-10, s.Velocity.GetY());
        }

        [TestMethod]
        public void TestGetScore()
        {
            Ship s = new Ship();
            Assert.AreEqual(0, s.Score);
            s.Score = 200;
            Assert.AreEqual(200, s.Score);
        }

        //
        //Projectile Testing
        //

        [TestMethod]
        public void TestProjectileConstructor()
        {
            //test no argument constructor
            Projectile p = new Projectile();
            Assert.AreEqual(-1, p.id);
            Assert.AreEqual(-1, p.Owner);
            Assert.AreEqual(0, p.Loc.GetX());
            Assert.AreEqual(0, p.Loc.GetY());
            Assert.AreEqual(0, p.Dir.GetX());
            Assert.AreEqual(0, p.Dir.GetY());
            Assert.AreEqual(true, p.Alive);

            //test constructor with arguments
            p = new Projectile(1, 2 , 1, -1, 1, -1);
            Assert.AreEqual(1, p.id);
            Assert.AreEqual(2, p.Owner);
            Assert.AreEqual(1, p.Loc.GetX());
            Assert.AreEqual(-1, p.Loc.GetY());
            Assert.AreEqual(1, p.Dir.GetX());
            Assert.AreEqual(-1, p.Dir.GetY());
            Assert.AreEqual(true, p.Alive);
        }

        [TestMethod]
        public void TestLocation()
        {
            Projectile p = new Projectile();
            p.Loc = new Vector2D(5, 6);
            Assert.AreEqual(5, p.Loc.GetX());
            Assert.AreEqual(6, p.Loc.GetY());
        }

        [TestMethod]
        public void TestAlive()
        {
            Projectile p = new Projectile();
            p.Alive = false;
            Assert.IsFalse(p.Alive);
        }

        //
        // Star Testing
        //
        [TestMethod]
        public void TestStarConstructor()
        {
            //test no argument constructor
            Star p = new Star();
            Assert.AreEqual(-1, p.id);
            Assert.AreEqual(0, p.Loc.GetX());
            Assert.AreEqual(0, p.Loc.GetY());
            Assert.AreEqual(0, p.Mass);

            //test constructor with arguments
            p = new Star(1, 2.5, 2 , 3);
            Assert.AreEqual(1, p.id);
            Assert.AreEqual(2, p.Loc.GetX());
            Assert.AreEqual(3, p.Loc.GetY());
            Assert.AreEqual(2.5, p.Mass);
        }
    }
}
