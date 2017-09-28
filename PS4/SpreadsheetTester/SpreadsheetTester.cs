using System;
using System.Collections.Generic;
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
            s.SetCellContents("a1", 3);
            Assert.AreEqual((double)3, s.GetCellContents("a1"));
        }

        [TestMethod]
        public void StringSetTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("a1", "hello");
            Assert.AreEqual("hello", s.GetCellContents("a1"));
        }

        [TestMethod]
        public void BasicFormulaSetTest() {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("b1 + c2 + 3/4");
            s.SetCellContents("a1", f);
            Assert.AreEqual(f, s.GetCellContents("a1"));
        }

        [TestMethod]
        public void ValueTest() {
            Spreadsheet s = new Spreadsheet();
            PrivateObject saccess = new PrivateObject(s);

            s.SetCellContents("A1", 3);
            s.SetCellContents("B1", "hello");
            s.SetCellContents("_C39", 13);
            s.SetCellContents("A2", new Formula("a1*_c39", var => var.ToUpper(), var => true));

            Assert.AreEqual((double)3, ((Dictionary<string, object>)saccess.GetField("values"))["A1"]);
            Assert.AreEqual("hello", ((Dictionary<string, object>)saccess.GetField("values"))["B1"]);
            Assert.AreEqual((double)13, ((Dictionary<string, object>)saccess.GetField("values"))["_C39"]);
            Assert.AreEqual((double)39, ((Dictionary<string, object>)saccess.GetField("values"))["A2"]);
        }

        [TestMethod]
        public void FormulaValueMissingVariableTest() {
            Spreadsheet s = new Spreadsheet();
            PrivateObject saccess = new PrivateObject(s);

            s.SetCellContents("a1", new Formula("b1"));
            Assert.IsTrue(((Dictionary<string, object>)saccess.GetField("values"))["a1"] is FormulaError);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularExceptionTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("a1", new Formula("b1"));
            s.SetCellContents("b1", new Formula("a1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellDoubleInvalidNameTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("3", 3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellStringInvalidNameTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("3", "hello");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellFormulaInvalidNameTest() {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("3", new Formula("3 + 7"));
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
            s.SetCellContents("a1", 3);
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
            s.SetCellContents("a1", (string) null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgumentSetFormula() {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("a3");
            f = null;
            s.SetCellContents("a1", f);
        }
        
        // Ensure variables of length one are valid
        [TestMethod]
        public void SingleLetterVariable() {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("a", 3);
        }

        // Ensure variables don't allow symbols
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSymbolVariable() {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("_!", 3);
        }
    }
}
