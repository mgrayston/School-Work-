using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;

namespace SS {
    public class Spreadsheet : AbstractSpreadsheet {
        private Dictionary<string, object> cells;   // Dictionary that acts as the "spreadsheet", storing all cells and their contents
        private DependencyGraph cellGraph;          // Used to track dependencies of all cells

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet" /> class.
        /// </summary>
        public Spreadsheet() {
            cells = new Dictionary<string, object>();
            cellGraph = new DependencyGraph();
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells() {
            return new List<string>(cells.Keys);
        }

        public override object GetCellContents(string name) {
            if (isValidName(name)) {
                // TODO check for different types (formula string etc.)
                return cells[name];
            }

            throw new InvalidNameException();
        }

        public override ISet<string> SetCellContents(string name, double number) {
            if (isValidName(name)) {
                cells[name] = number;
                HashSet<string> dependents = new HashSet<string>();
                dependents.Add(name);
                foreach (string cell in cellGraph.GetDependents(name)) {
                    dependents.Add(cell);
                }
                return dependents;
            }

            throw new InvalidNameException();
        }

        public override ISet<string> SetCellContents(string name, string text) {
            if (text == null) {
                throw new ArgumentNullException(text);
            }

            if (isValidName(name)) {
                cells[name] = text;
                HashSet<string> dependents = new HashSet<string>();
                dependents.Add(name);
                foreach (string cell in cellGraph.GetDependents(name)) {
                    dependents.Add(cell);
                }
                return dependents;
            }

            throw new InvalidNameException();
        }

        public override ISet<string> SetCellContents(string name, Formula formula) {
            if (formula == null) {
                throw new ArgumentNullException(formula.ToString());
            }

            if (isValidName(name)) {
                // TODO
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
