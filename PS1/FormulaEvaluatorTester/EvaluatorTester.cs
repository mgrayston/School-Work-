using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;

namespace EvaluatorTests {
    /// <summary>
    /// This class provides code for testing a FormulaEvaluator
    /// </summary>
    class EvaluatorTester {

        /// <summary>
        /// A simple method matching the signature for a Lookup delegate
        /// This delegate hard-codes the value of a single variable, B4.
        /// </summary>
        /// <param name="s">The name of the variable to lookup</param>
        /// <returns>The value of the variable if known, throws otherwise</returns>
        public static int simpleLookup(string s) {
            if (s == "B4")
                return 17;

            throw new ArgumentException("Unknown variable");
        }

        /// <summary>
        /// A testing method that assumes the evaluation of the expression to
        /// happen successfully with an expected output.
        /// </summary>
        /// <param name="expr">The expression to evaluate</param>
        /// <param name="L">The lookup delegate</param>
        /// <param name="expected">The expected result after evaluation</param>
        /// <returns>True if the expression successfully evaluates to the expected value, 
        /// false otherwise</returns>
        public static bool ValidTest(string expr, Evaluator.Lookup L, int expected) {
            try {
                return Evaluator.Evaluate(expr, L) == expected;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// A testing method that assumes the evaluation of the expression to
        /// throw an ArgumentException.
        /// </summary>
        /// <param name="exp">The expression to evaluate</param>
        /// <param name="L">The lookup delegate</param>
        /// <returns>True if an ArgumentException is thrown, false otherwise</returns>
        public static bool InvalidTest(string exp, Evaluator.Lookup L) {
            try {
                Evaluator.Evaluate(exp, L);
                return false;
            }
            catch (ArgumentException) {
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Simple helper method for displaying whether a test
        /// passed or failed.
        /// </summary>
        /// <param name="testDescription">Description of test being performed</param>
        /// <param name="testResult"></param>
        public static void Test(string testDescription, bool testResult) {
            Console.Write(testDescription + ": ");
            if (testResult) {
                Console.WriteLine("Passed");
            }
            else {
                Console.WriteLine("***Failed***");
            }
        }

        static void Main(string[] args) {
            // Or we can make an object to hold variable values
            // See the definition of FakeSpreadsheet below
            FakeSpreadsheet sheet = new FakeSpreadsheet();

            // This fake spreadsheet has values for three variables, A1, A2, and a4
            sheet.SetCell("A1", 12);
            sheet.SetCell("A2", 100);
            sheet.SetCell("a4", 3);

            // Valid tests
            Test("Order of operations test", ValidTest("2 + 6 * a4", sheet.GetCell, 20));
            Test("Order of operations test 2", ValidTest("2 / 20* 80 - (12/6) +2", simpleLookup, 0));
            Test("Order of operations test 3", ValidTest("(1+2)*2/3", null, 2));
            Test("Order of operations zero test", ValidTest("0*((1+2*2/(2+1))*100000000 + 9999)", null, 0));
            Test("Order of operations with parenthesis test", ValidTest("(2+6)*a4", sheet.GetCell, 24));
            Test("Several operations inside parenthesis test", ValidTest("2+(3+5*9)", sheet.GetCell, 50));
            Test("Consecutive operators test", ValidTest("1+(3/2)", simpleLookup, 2));
            Test("Valid variable name test", ValidTest("B4", simpleLookup, 17));
            Test("Nested parentheses test", ValidTest("(((((((((((((B4)))))))))))))", simpleLookup, 17));
            Test("Valid sheet lookup", ValidTest("a4", sheet.GetCell, 3));

            // Invalid tests
            Test("Consecutive operators test 1", InvalidTest("1//2", simpleLookup));
            Test("Consecutive operators test 2", InvalidTest("1+/2", simpleLookup));
            Test("Consecutive operators test 3", InvalidTest("1+(2/3*)", simpleLookup));
            Test("Division by 0 test 1", InvalidTest("5/0", sheet.GetCell));
            Test("Division by 0 test 2", InvalidTest("5 * B4 + (7-4)/((4/2)-2)", sheet.GetCell));
            Test("Misplaced operator test", InvalidTest("2+5+", sheet.GetCell));
            Test("Invalid variable name test 1", InvalidTest("2+AB", simpleLookup));
            Test("Invalid variable name test 2", InvalidTest("2+AB123ac", simpleLookup));
            Test("Invalid variable name test 3", InvalidTest("2+123AB", simpleLookup));
            Test("Invalid variable name test 4", InvalidTest("2+123AB123", simpleLookup));
            Test("Invalid variable name test 5", InvalidTest("b3", simpleLookup));
            Test("Misplaced parenthesis test 1", InvalidTest("1+2*((3)", null));
            Test("Misplaced parenthesis test 2", InvalidTest("1+2*(3))", null));
            Test("Invalid sheet lookup", InvalidTest("A4", sheet.GetCell));

            // Pause the console
            Console.Read();
        }
    }



    /// <summary>
    /// A class for providing functionality similar to a basic spreadsheet.
    /// This class simply provides a mapping from variable names to their values.
    /// Use this class to provide a variable Lookup delegate for writing tests
    /// for your Evaluate method.
    /// </summary>
    class FakeSpreadsheet {
        // The dictionary holding the variables and their values
        // These are the fake spreadsheet "cells"
        private Dictionary<string, int> cells;

        /// <summary>
        /// A simple constructor. Just initialize the dictionary.
        /// </summary>
        public FakeSpreadsheet() {
            cells = new Dictionary<string, int>();
        }

        /// <summary>
        /// Sets the value of a certain variable
        /// </summary>
        /// <param name="cellName">The name of the variable (or "cell")</param>
        /// <param name="val">The value of the variable</param>
        public void SetCell(string cellName, int val) {
            cells[cellName] = val;
        }

        /// <summary>
        /// Gets the value of a certain variable
        /// </summary>
        /// <param name="cellName">The name of the variable (or "cell")</param>
        /// <returns>The value of the specified variable.</returns>
        public int GetCell(string cellName) {
            if (!cells.ContainsKey(cellName))
                throw new ArgumentException("unknown variable");

            return cells[cellName];
        }
    }
}