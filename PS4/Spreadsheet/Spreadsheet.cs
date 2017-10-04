// TODO replace all names with a normalized version

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;

// Written by Christopher Nielson for CS 3500, September 29, 2017

namespace SS {
    public class Spreadsheet : AbstractSpreadsheet {
        private Dictionary<string, object> contents;    // Dictionary that acts as the "spreadsheet", storing all cells and their contents
        private Dictionary<string, object> values;      // Used to store values of cells
        private DependencyGraph cellGraph;              // Used to track dependencies of all cells
        private Func<string, bool> isValid;             // Used to test the validity of variables
        private Func<string, string> normalize;         // Used to normalize variables
        private string version;                         // Used to track version information of a Spreadsheet

        public override bool Changed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version) {
            this.isValid = isValid;
            this.normalize = normalize;
            this.version = version;
            contents = new Dictionary<string, object>();
            values = new Dictionary<string, object>();
            cellGraph = new DependencyGraph();
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells() {
            return new List<string>(contents.Keys);
        }

        /// <summary>
        /// Gets the cell contents.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        public override object GetCellContents(string name) {
            if (isValidName(normalize(name))) {
                name = normalize(name);
                if (contents.ContainsKey(name) && contents[name] != null) {
                    return contents[name];
                }
                else {
                    return "";
                }
            }

            throw new InvalidNameException();
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public abstract ISet<String> SetContentsOfCell(String name, String content) {
            if (content == null) {
                throw new ArgumentNullException("No content passed");
            }

            if (isValidName(normalize(name))) {
                name = normalize(name);

                // TODO
            }
            else {
                throw new InvalidNameException(name + " is not a valid name");
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        protected override ISet<string> SetCellContents(string name, double number) {
            if (isValidName(normalize(name))) {
                name = normalize(name);
                contents[name] = number;
                values[name] = number;
                cellGraph.ReplaceDependees(name, null);
                return new HashSet<string>(GetCellsToRecalculate(name));
            }

            throw new InvalidNameException();
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidNameException"></exception>
        protected override ISet<string> SetCellContents(string name, string text) {
            if (text == null) {
                throw new ArgumentNullException(text);
            }

            if (isValidName(normalize(name))) {
                name = normalize(name);
                contents[name] = text;
                values[name] = text;
                cellGraph.ReplaceDependees(name, null);
                return new HashSet<string>(GetCellsToRecalculate(name));
            }

            throw new InvalidNameException();
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The provided formula was null</exception>
        /// <exception cref="InvalidNameException"></exception>
        protected override ISet<string> SetCellContents(string name, Formula formula) {
            if (formula == null) {
                throw new ArgumentNullException("The provided formula was null");
            }

            if (isValidName(normalize(name))) {
                name = normalize(name);
                // Get variables in formula
                IEnumerable<string> vars = formula.GetVariables();
                // Get all direct/indirect dependents of the formula
                HashSet<string> changed = new HashSet<string>(GetCellsToRecalculate(new HashSet<string>(vars)));
                // Ensure the formula is not circular
                contents[name] = formula;
                values[name] = formula.Evaluate(GetCellValue);
                cellGraph.ReplaceDependees(name, vars);
                // CircularException will be thrown by GetCelsToRecalculate if needed
                return new HashSet<string>(GetCellsToRecalculate(name));
            }

            throw new InvalidNameException();
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidNameException"></exception>
        protected override IEnumerable<string> GetDirectDependents(string name) {
            if (name == null) {
                throw new ArgumentNullException(name);
            }

            if (isValidName(normalize(name))) {
                name = normalize(name);
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
                    if (name.Length == 1 || !Regex.IsMatch(name.Substring(1), "[^\\d^\\w^_]")) {
                        if (isValid(name)) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /*
        /// <summary>
        /// Gets the value of the given variable.
        /// Used as a delegate for Formula.evaluate to calculate a cell's value.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        /// <exception cref="FormatException">Variable " + var + " not found</exception>
        private double getValue(string var) {
            if (values[var] is double) {
                return (double)values[var];
            }
            else {
                throw new FormatException("Variable " + var + " not found");
            }
        }
        // TODO deprecated
        */

        public override string GetSavedVersion(string filename) {
            throw new NotImplementedException();
        }

        public override void Save(string filename) {
            throw new NotImplementedException();
        }

        public override object GetCellValue(string name) {
            if (isValidName(normalize(name))) {
                name = normalize(name);

                if (values[name] is double) {
                    return (double)values[name];
                }
            }

            throw new FormatException("Variable " + name + " not found");
        }

        public override ISet<string> SetContentsOfCell(string name, string content) {
            throw new NotImplementedException();
        }
    }
}
