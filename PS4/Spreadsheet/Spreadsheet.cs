using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;

// Written by Christopher Nielson for CS 3500, September 29, 2017

namespace SS {
    public class Spreadsheet : AbstractSpreadsheet {
        private Dictionary<string, object> cells;   // Dictionary that acts as the "spreadsheet", storing all cells and their contents
        private DependencyGraph cellGraph;          // Used to track dependencies of all cells

        public Spreadsheet() {
            cells = new Dictionary<string, object>();
            cellGraph = new DependencyGraph();
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells() {
            return new List<string>(cells.Keys);
        }

        public override object GetCellContents(string name) {
            if (isValidName(name)) {
                return cells[name];
            }

            throw new InvalidNameException();
        }

        public override ISet<string> SetCellContents(string name, double number) {
            if (isValidName(name)) {
                cells[name] = number;
                cellGraph.ReplaceDependees(name, null);
                return new HashSet<string> (GetCellsToRecalculate(name));
                /*HashSet<string> dependents = new HashSet<string>();
                dependents.Add(name);
                foreach (string cell in cellGraph.GetDependents(name)) {
                    dependents.Add(cell);
                }
                return dependents;
                */
            }

            throw new InvalidNameException();
        }

        public override ISet<string> SetCellContents(string name, string text) {
            if (text == null) {
                throw new ArgumentNullException(text);
            }

            if (isValidName(name)) {
                cells[name] = text;
                cellGraph.ReplaceDependees(name, null);
                return new HashSet<string> (GetCellsToRecalculate(name));
            }

            throw new InvalidNameException();
        }

        public override ISet<string> SetCellContents(string name, Formula formula) {
            if (formula == null) {
                throw new ArgumentNullException(formula.ToString());
            }

            if (isValidName(name)) {
                // Get variables in formula
                IEnumerable<string> vars = formula.GetVariables();
                // Get all direct/indirect dependents of the formula
                HashSet<string> changed = new HashSet<string>(GetCellsToRecalculate(new HashSet<string>(vars)));
                // Ensure the formula is not circular
                if (changed.Contains(name)) {
                    throw new CircularException();
                }
                else {
                    cells[name] = formula;
                    cellGraph.ReplaceDependees(name, vars);
                    return new HashSet<string>(GetCellsToRecalculate(name));
                }
            }

            throw new InvalidNameException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name) {
            if (name == null) {
                throw new ArgumentNullException(name);
            }

            if (isValidName(name)) {
                return cellGraph.GetDependents(name);
            }

            throw new InvalidNameException();
        }

        /// <summary>
        /// Determines whether name is a valid variable name.
        /// </summary>
        /// <param name="name">The variable name to check.</param>
        /// <returns>
        ///   <c>true</c> if name is a valid variable name; otherwise, <c>false</c>.
        /// </returns>
        private bool isValidName(string name) {
            if (name != null) {
                if (char.IsLetter(name[0]) || name[0] == '_') {
                    if (!Regex.IsMatch(name.Substring(1), "[^\\d^\\w^_]")) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
