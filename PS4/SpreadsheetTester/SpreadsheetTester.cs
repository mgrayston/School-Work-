using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;

namespace SpreadsheetTester {
    [TestClass]
    public class SpreadsheetTester {

        [TestMethod]
        public void ConstructorTest() {
            Spreadsheet s = new Spreadsheet();
        }

        [TestMethod]
        public void DoubleSetTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "3");
            Assert.AreEqual((double)3, s.GetCellContents("a1"));
        }

        [TestMethod]
        public void StringSetTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "hello");
            Assert.AreEqual("hello", s.GetCellContents("a1"));
        }

        [TestMethod]
        public void BasicFormulaSetTest() {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("b1 + c2 + 3/4");
            s.SetContentsOfCell("a1", "=" + f);
            Assert.AreEqual(f, s.GetCellContents("a1"));
        }

        [TestMethod]
        public void ValueTest() {
            Spreadsheet s = new Spreadsheet(var => true, var => var.ToUpper(), "default");

            s.SetContentsOfCell("A1", "3");
            s.SetContentsOfCell("B1", "hello");
            s.SetContentsOfCell("cC39", "13");
            s.SetContentsOfCell("A2", "=a1*cc39");

            Assert.AreEqual((double)3, s.GetCellValue("a1"));
            Assert.AreEqual("hello", s.GetCellValue("b1"));
            Assert.AreEqual((double)13, s.GetCellValue("cc39"));
            Assert.AreEqual((double)39, s.GetCellValue("a2"));
        }

        [TestMethod]
        public void FormulaValueMissingVariableTest() {
            Spreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("a1", "=b1");
            Assert.IsTrue(s.GetCellValue("a1") is FormulaError);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularExceptionTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=b1");
            s.SetContentsOfCell("b1", "=a1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellDoubleInvalidNameTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("3", "3");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellStringInvalidNameTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("3", "hello");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellFormulaInvalidNameTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("3", "=3 + 7");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsInvalidNameTest() {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("3");
        }

        [TestMethod]
        public void GetNamesEmpty() {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual(0, System.Linq.Enumerable.Count(s.GetNamesOfAllNonemptyCells()));
            s.SetContentsOfCell("a1", "3");
            Assert.AreEqual(1, System.Linq.Enumerable.Count(s.GetNamesOfAllNonemptyCells()));
            Assert.IsTrue(System.Linq.Enumerable.Contains(s.GetNamesOfAllNonemptyCells(), "a1"));
        }

        [TestMethod]
        public void NullNameGetDependents() {
            Spreadsheet s = new Spreadsheet();
            PrivateObject acc = new PrivateObject(s);

            try {
                acc.Invoke("GetDirectDependents", new string[] { null });
            }
            catch (Exception e) {
                Assert.IsInstanceOfType(e.InnerException, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void InvalidNameGetDependents() {
            Spreadsheet s = new Spreadsheet();
            PrivateObject acc = new PrivateObject(s);

            try {
                acc.Invoke("GetDirectDependents", new string[] { "3" });
            }
            catch (Exception e) {
                Assert.IsInstanceOfType(e.InnerException, typeof(InvalidNameException));
            }
        }

        [TestMethod]
        public void GetEmptyCellContents() {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("a1"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgumentSetText() {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", (string) null);
        }

        // Deprecated test; cannot pass Formulas, only Strings
        /*
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgumentSetFormula() {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("a3");
            f = null;
            s.SetContentsOfCell("a1", f);
        }
        */
        
        // Deprecated test; new specification in PS5
        /*
        // Ensure variables of length one are valid
        [TestMethod]
        public void SingleLetterVariable() {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a", "3");
        }
        */

        // Ensure variables don't allow symbols
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSymbolVariable() {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("_!", "3");
        }

        [TestMethod]
        public void Constructor3Argument() {
            Spreadsheet s = new Spreadsheet(v => true, v => v, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestValidityInvalid() {
            Spreadsheet s = new Spreadsheet(f => !f.StartsWith("_"), f => f, "default");
            s.SetContentsOfCell("_a1", "3");
        }

        // This tests three things. It tests the capability of spreadsheet to return
        // the correct cell contents, value, and the normalization function.
        // Since _a1 is passed, it should be stored as a1, and have the contents AND
        // value of 3.
        [TestMethod]
        public void TestValidityValidAfterNormalization() {
            Spreadsheet s = new Spreadsheet(f => !f.StartsWith("_"), f => f.Replace("_", ""), "default");
            s.SetContentsOfCell("_a1", "3");
            Assert.AreEqual((double)3, s.GetCellContents("a1"));
            Assert.AreEqual((double)3, s.GetCellValue("a1"));
        }

        [TestMethod]
        public void FormulaValueAndContentsTest() {
            Spreadsheet s = new Spreadsheet(f => !f.StartsWith("_"), f => f.ToUpper(), "default");
            Formula a = new Formula("3 + 4");
            s.SetContentsOfCell("a1", "=3+4");
            Assert.AreEqual(a, s.GetCellContents("A1"));
            Assert.AreEqual((double)7, s.GetCellValue("a1"));
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidVariable() {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=!a");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidVariableFromPassedValidator() {
            Spreadsheet s = new Spreadsheet(f => !Regex.IsMatch(f, "[a-z]"), f => f, "default");
            s.SetContentsOfCell("a1", "3");
        }

        [TestMethod]
        public void ValidVariableFromPassedValidator() {
            Spreadsheet s = new Spreadsheet(f => !Regex.IsMatch(f, "[a-z]"), f => f, "default");
            s.SetContentsOfCell("A1", "3");
        }

        [TestMethod]
        public void SaveTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "3");
            s.SetContentsOfCell("a2", "=a1*2");
            s.Save("test.xml");
        }
    }
}
