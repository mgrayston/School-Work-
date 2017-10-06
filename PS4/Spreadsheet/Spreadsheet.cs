using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using SpreadsheetUtilities;

// Written by Christopher Nielson for CS 3500, October 4, 2017

namespace SS {
    public class Spreadsheet : AbstractSpreadsheet {
        private Dictionary<string, object> contents;    // Dictionary that acts as the "spreadsheet", storing all cells and their contents
        private Dictionary<string, object> values;      // Used to store values of cells
        private DependencyGraph cellGraph;              // Used to track dependencies of all cells

        public override bool Changed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public Spreadsheet() :
            this(s => true, s => s, "default") { }

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version) {
            contents = new Dictionary<string, object>();
            values = new Dictionary<string, object>();
            cellGraph = new DependencyGraph();
        }

        public Spreadsheet(string file, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version) {
            // TODO deal with file
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
            if (isValidName(Normalize(name))) {
                name = Normalize(name);
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
        public override ISet<String> SetContentsOfCell(String name, String content) {
            if (content == null) {
                throw new ArgumentNullException("No content passed");
            }

            if (isValidName(Normalize(name))) {
                name = Normalize(name);
                double tmp;
                if (double.TryParse(content, out tmp)) {
                    return SetCellContents(name, tmp);
                }

                else if (content.StartsWith("=")) {
                    return SetCellContents(name, new Formula(content.Substring(1), Normalize, isValidName));
                }

                else {
                    return SetCellContents(name, content);
                }
            }
            else {
                throw new InvalidNameException();
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
            name = Normalize(name);
            contents[name] = number;
            values[name] = number;
            cellGraph.ReplaceDependees(name, null);
            return new HashSet<string>(GetCellsToRecalculate(name));
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
            name = Normalize(name);
            contents[name] = text;
            values[name] = text;
            cellGraph.ReplaceDependees(name, null);
            return new HashSet<string>(GetCellsToRecalculate(name));
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
            name = Normalize(name);
            // Get variables in formula
            IEnumerable<string> vars = formula.GetVariables();
            // Get all direct/indirect dependents of the formula
            // TODO
            // HashSet<string> changed = new HashSet<string>(GetCellsToRecalculate(new HashSet<string>(vars)));
            // Ensure the formula is not circular
            contents[name] = formula;
            values[name] = formula.Evaluate(GetValue);
            cellGraph.ReplaceDependees(name, vars);
            // CircularException will be thrown by GetCellsToRecalculate if needed
            return new HashSet<string>(GetCellsToRecalculate(name));
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

            if (isValidName(Normalize(name))) {
                return cellGraph.GetDependents(Normalize(name));
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
                if (defaultValid(name) && IsValid(name)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <exception cref="SpreadsheetReadWriteException">Error occured while retrieving version information in " + filename + ":\n" + e.Message</exception>
        public override string GetSavedVersion(string filename) {
            try {
                using (XmlReader reader = XmlReader.Create(filename)) {
                    reader.ReadToFollowing("spreadsheet");
                    return reader.GetAttribute("version");
                }
            }
            catch (Exception e) {
                throw new SpreadsheetReadWriteException("Error occured while retrieving version information in " + filename + ":\n" + e.Message);
            }
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// <spreadsheet version="version information goes here"><cell><name>
        /// cell name goes here
        /// </name><contents>
        /// cell contents goes here
        /// </contents></cell></spreadsheet>
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, it should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="SpreadsheetReadWriteException">Error occured while saving:\n" + e.Message</exception>
        public override void Save(string filename) {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";

            try {
                using (XmlWriter writer = XmlWriter.Create(filename, settings)) {
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);
                    foreach (String cell in GetNamesOfAllNonemptyCells()) {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", cell);
                        writer.WriteStartElement("contents");
                        object cellContents = GetCellContents(cell);

                        // Write depending on type of contents
                        switch (cellContents.GetType().Name) {
                            case "String":
                                writer.WriteString((string)cellContents);
                                break;
                            case "Double":
                                writer.WriteString(((double)cellContents).ToString());
                                break;
                            case "Formula":
                                writer.WriteString("=" + ((Formula)cellContents).ToString());
                                break;
                        }
                        writer.WriteEndElement();   // End of contents
                        writer.WriteEndElement();   // End of cell
                    }
                    writer.WriteEndElement();       // End of spreadsheet
                }
            }
            catch (Exception e) {
                throw new SpreadsheetReadWriteException("Error occured while saving:\n" + e.Message);
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="FormatException">Variable " + name + " not found</exception>
        public override object GetCellValue(string name) {
            if (isValidName(Normalize(name))) {
                name = Normalize(name);

                if (values.ContainsKey(name)) {
                    return values[name];
                }

                return "";
            }

            throw new InvalidNameException();
        }

        /// <summary>
        /// Convenience method for GetCellValue, to be used as a lookup delegate.        
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private double GetValue(string name) {
            object val = GetCellValue(name);

            if (val is double) {
                return (double)val;
            }

            throw new ArgumentException();
        }

        // Private function to test base validity of a variable, as described
        // in the specification.
        private Func<string, bool> defaultValid = s => Regex.IsMatch(s, "^[a-zA-Z]+(\\d+)$");
    }
}
