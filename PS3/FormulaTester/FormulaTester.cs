using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTester {
    [TestClass]
    public class FormulaTester {


        public static double simpleLookup(string s) {
            if (s == "A3") {
                return 59;
            }

            if (s == "_abc123a") {
                return 13;
            }

            throw new ArgumentException("Unknown variable");
        }

        [TestMethod]
        public void validConstructorWithoutValidatorNormalizer() {
            Formula f = new Formula("4+3");
            Assert.AreEqual(7, (double) f.Evaluate(null));
        }

        [TestMethod]
        public void validConstructorWithValidatorNormalizer() {
            Formula f = new Formula("4 + a3", s => s.ToUpper(), s => true);
            Assert.AreEqual(63, (double) f.Evaluate(simpleLookup));
        }

        [TestMethod]
        public void weirdVariableName() {
            Formula f = new Formula("13 + _Abc123A", s => s.ToLower(), s => true);
            Assert.AreEqual(26, (double) f.Evaluate(simpleLookup));
        }

        [TestMethod]
        public void validVariableByPassedValidator() {
            Formula f = new Formula("13 + _Abc123A", s => s.ToLower(), s => Regex.IsMatch(s, "_+"));
            Assert.AreEqual(26, (double) f.Evaluate(simpleLookup));
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidVariableByPassedValidator() {
            Formula f = new Formula("13 + _Abc123A", s => s.ToLower(), s => !Regex.IsMatch(s, "_+"));
        }

        [TestMethod]
        public void equalsNullFail() {
            Formula f = new Formula("6");
            Assert.IsFalse(f.Equals(null));
        }

        [TestMethod]
        public void equalsFail() {
            Formula f1 = new Formula("6");
            Formula f2 = new Formula("3");
            Assert.IsFalse(f1.Equals(f2));
        }

        [TestMethod]
        public void equalsPass() {
            Formula f1 = new Formula("6");
            Formula f2 = new Formula("6");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod]
        public void equalsPassComplicatedVariables() {
            Formula f1 = new Formula("13 + _abCCDJ2ff1/ 9", s => s.ToLower().Replace("_", ""), s => true);
            Formula f2 = new Formula("13+abccdj2ff1/9");
            Assert.IsTrue(f1.Equals(f2));
        }

        // Ensures the equals method is not comparing the evaluated answers
        [TestMethod]
        public void equalsFailNumerical() {
            Formula f1 = new Formula("A3");
            Formula f2 = new Formula("59");
            Assert.IsFalse(f1.Equals(f2));
        }

        [TestMethod]
        public void opEqualsBothNull() {
            Formula f1 = null;
            Formula f2 = null;
            Assert.IsTrue(f1 == f2);
        }

        [TestMethod]
        public void opEqualsOneNull1() {
            Formula f1 = new Formula("13");
            Formula f2 = null;
            Assert.IsFalse(f1 == f2);
        }

        [TestMethod]
        public void opEqualsOneNull2() {
            Formula f1 = new Formula("13");
            Formula f2 = null;
            Assert.IsFalse(f2 == f1);
        }

        [TestMethod]
        public void opEqualsTrue() {
            Formula f1 = new Formula("13 + _abCCDJ2ff1/ 9", s => s.ToLower().Replace("_", ""), s => true);
            Formula f2 = new Formula("13+abccdj2ff1/9");
            Assert.IsTrue(f1 == f2);
        }

        // Should return false if both are null
        [TestMethod]
        public void opInequalsBothNull() {
            Formula f1 = null;
            Formula f2 = null;
            Assert.IsFalse(f1 != f2);
        }

        [TestMethod]
        public void opInequalsOneNull() {
            Formula f1 = null;
            Formula f2 = new Formula("13");
            Assert.IsTrue(f1 != f2);
        }

        [TestMethod]
        public void opInequalsTrue() {
            Formula f1 = new Formula("13");
            Formula f2 = new Formula("6 + 7");
            Assert.IsTrue(f1 != f2);
        }

        [TestMethod]
        public void toString() {
            Formula f1 = new Formula("__bbfccDDfj192", s => s.ToUpper().Replace("_", "").Replace("9", "Q"), s => true);
            String correctToString = "BBFCCDDFJ1Q2";
            Assert.AreEqual(correctToString, f1.ToString());
        }

        [TestMethod]
        public void hashCode() {
            Formula f1 = new Formula("__bbfccDDfj192", s => s.ToUpper().Replace("_", "").Replace("9", "Q"), s => true);
            int correctHashCode = "BBFCCDDFJ1Q2".GetHashCode();
            Assert.AreEqual(correctHashCode, f1.GetHashCode());
        }
        /*
        [TestMethod]
        public void getVariablesSimple() {
            Formula f1 = new Formula("3 + a");
            IEnumerable<string> s = f1.GetVariables();
            s.cou
        }
        */

        [TestMethod]
        public void evaluateTest() {
            Formula f1 = new Formula("(a3 * ((29+23) / 26)/59)+ 11", s => s.ToUpper(), s => true);
            Assert.AreEqual(13, (double) f1.Evaluate(simpleLookup));
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void unevenParenthesesLeft() {
            Formula f1 = new Formula("(((9*3) + 1)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void unevenParenthesesRight() {
            Formula f1 = new Formula("(9*3))");
        }

        [TestMethod]
        public void divisionByZero() {
            Formula f1 = new Formula("A3*13/83910501048391 + 1/0");
            Assert.IsTrue(f1.Evaluate(simpleLookup) is FormulaError);
        }
    }
}
