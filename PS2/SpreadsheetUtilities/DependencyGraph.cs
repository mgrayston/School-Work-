﻿// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)

// Code by Christopher Nielson, September 2017
// V 1.1 Working; any failed tests are fixed.

using System.Collections.Generic;

namespace SpreadsheetUtilities {

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>

    public class DependencyGraph {
        private Dictionary<string, HashSet<string>> dentGraph;
        private Dictionary<string, HashSet<string>> deeGraph;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph() {
            dentGraph = new Dictionary<string, HashSet<string>>();
            deeGraph = new Dictionary<string, HashSet<string>>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size {
            get {
                int count = 0;

                // Sum 
                foreach (string s in dentGraph.Keys) {
                    count += dentGraph[s].Count;
                }

                return count;
            }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s] {
            get {
                return deeGraph.ContainsKey(s) ? deeGraph[s].Count : 0;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s) {
            return dentGraph.ContainsKey(s) && dentGraph[s].Count > 0;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s) {
            return deeGraph.ContainsKey(s) && deeGraph[s].Count > 0;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s) {
            if (dentGraph.ContainsKey(s)) {
                return dentGraph[s];
            }
            return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s) {
            if (deeGraph.ContainsKey(s)) {
                return deeGraph[s];
            }
            return new HashSet<string>();
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t) {
            // If s hasn't been added as dependee before, give it an empty HashSet
            if (!dentGraph.ContainsKey(s)) {
                dentGraph.Add(s, new HashSet<string>());
            }
            // Add t as dependent
            dentGraph[s].Add(t);

            // If t hasn't been added as dependent before, give it an empty HashSet
            if (!deeGraph.ContainsKey(t)) {
                deeGraph.Add(t, new HashSet<string>());
            }
            // Add s as dependee
            deeGraph[t].Add(s);
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t) {
            if (dentGraph.ContainsKey(s) && dentGraph[s].Contains(t)) {
                dentGraph[s].Remove(t);
                deeGraph[t].Remove(s);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents) {
            // Remove s as dependee from all r
            if (dentGraph.ContainsKey(s)) {
                foreach (string r in dentGraph[s]) {
                    deeGraph[r].Remove(s);
                }
            }

            // Clear all r
            dentGraph[s] = new HashSet<string>();

            // Add new t
            if (newDependents != null) {
                foreach (string t in newDependents) {
                    dentGraph[s].Add(t);
                    if (!deeGraph.ContainsKey(t)) {
                        deeGraph.Add(t, new HashSet<string>());
                    }
                    deeGraph[t].Add(s);
                }
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees) {
            // Remove s as dependent from all r
            if (deeGraph.ContainsKey(s)) {
                foreach (string r in deeGraph[s]) {
                    dentGraph[r].Remove(s);
                }
            }

            // Clear all r
            deeGraph[s] = new HashSet<string>();

            // Add new t
            if (newDependees != null) {
                foreach (string t in newDependees) {
                    deeGraph[s].Add(t);
                    if (!dentGraph.ContainsKey(t)) {
                        dentGraph.Add(t, new HashSet<string>());
                    }
                    dentGraph[t].Add(s);
                }
            }
        }
    }
}