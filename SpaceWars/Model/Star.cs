using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Star
    {
        /// <summary>
        /// Unique ID
        /// </summary>
        private int star;

        /// <summary>
        /// Vector2D representing the location
        /// </summary>
        private Vector2D loc;

        /// <summary>
        /// a double representing the star's mass. Note that the sample client does not use this information, 
        /// but you may choose to display stars differently based on their mass.
        /// </summary>
        private double mass;

        public Star(int star, double mass)
        {
            this.star = star;
            this.loc = new Vector2D(); //TODO center of screen
            this.mass = mass;
        }

        public int ID { get => star; }
        public Vector2D Loc { get => loc; }
        public double Mass { get => mass; }
    }
}
