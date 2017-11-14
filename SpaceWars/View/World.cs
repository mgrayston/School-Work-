using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    class World
    {
        // In reality, these should not be public,
        // but for the purposes of this lab, the "World" 
        // class is just a wrapper around these two fields.
        private Dictionary<int, Player> players;
        private Dictionary<int, Powerup> powerups;

        public Dictionary<int, Player> Players { get => players; }
        public Dictionary<int, Powerup> Powerups { get => powerups; }
        

    public World()
        {
            players = new Dictionary<int, Player>();
            powerups = new Dictionary<int, Powerup>();
        }
    }
}
