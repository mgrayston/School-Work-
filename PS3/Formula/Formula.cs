﻿// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

// Implemented by Christopher Nielson for CS 2500, September 2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities {
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and "y"; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula {

        private string formula;
        private Func<string, string> normalize;
        private Func<string, bool> isValid;

        // Patterns for individual tokens
        private static String lpPattern = @"\(";
        private static String rpPattern = @"\)";
        private static String opPattern = @"[\+\-*/]";
        private static String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
        private static String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        private static String spacePattern = @"\s+";

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true) {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid) {
            this.normalize = normalize;
            this.isValid = isValid;

            IEnumerable<string> tokens = GetTokens(formula);
            if (Regex.IsMatch(tokens.First(), String.Format("({0}) | ({1}))", rpPattern, opPattern))) {
                throw new FormulaFormatException("Invalid first token: " + tokens.First());
            }

            if (Regex.IsMatch(tokens.First(), String.Format("({0}) | ({1}))", lpPattern, opPattern))) {
                throw new FormulaFormatException("Invalid last token: " + tokens.Last());
            }

            foreach (string s in GetTokens(formula)) {

                if (Regex.IsMatch(s, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*")) {
                    string tmp = normalize(s);
                    if (isValid(tmp)) {
                        this.formula += tmp;
                    }
                    else {
                        throw new FormulaFormatException("Invalid normalized variable: " + tmp);
                    }
                }

                else {
                    this.formula += s;
                }
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup) {
            IEnumerable<String> tokens = GetTokens(formula);

            Stack<String> operands = new Stack<String>();
            Stack<String> operators = new Stack<String>();

            // Iterate through all tokens and process them accordingly
            foreach (int token in Enumerable.Range(0, tokens.Length)) {
                // Trim extra whitespace
                tokens[token] = Regex.Replace(tokens[token], "^\\s+", "");
                tokens[token] = Regex.Replace(tokens[token], "\\s+$", "");

                // Catch invalid tokens with whitespace in the middle
                if (Regex.IsMatch(tokens[token], "\\s+")) {
                    throw new ArgumentException("Invalid token: " + tokens[token]);
                }

                // Continue on empty token
                if (tokens[token].Equals("")) {
                    continue;
                }

                // Digits
                if (Regex.IsMatch(tokens[token], "^\\d+$")) {
                    // '*' operator is next
                    if (operators.IsOnTop("\\*")) {
                        multiply(tokens[token]);
                    }

                    // '/' operator is next
                    else if (operators.IsOnTop("/")) {
                        divide(tokens[token]);
                    }

                    // No current operators to deal with; pushes token onto the operand stack
                    else {
                        operands.Push(tokens[token]);
                    }
                }

                // Variables
                else if (Regex.IsMatch(tokens[token], "[a-zA-Z]+\\d+")) {
                    // Throw exception for invalid variable name
                    if (Regex.IsMatch(tokens[token], "[a-zA-Z]?\\d+[a-zA-Z]+\\d?")) {
                        throw new ArgumentException("Invalid variable: " + tokens[token]);
                    }

                    // Lookup value of variable
                    int varResult = variableEvaluator(tokens[token]);

                    // '*' operator is next
                    if (operators.IsOnTop("\\*")) {
                        multiply(varResult.ToString());
                    }

                    // '/' operator is next
                    else if (operators.IsOnTop("/")) {
                        divide(varResult.ToString());
                    }

                    // No current operators to deal with; pushes token onto the operand stack
                    else {
                        operands.Push(varResult.ToString());
                    }
                }

                // '+' and '-' operators
                else if (Regex.IsMatch(tokens[token], "\\+|-")) {
                    // Operator token is in an invalid position; throws ArgumentException
                    if (token == 0 || token == tokens.Length - 1 || Regex.IsMatch(tokens[token - 1], "\\+|-|\\*|/") || Regex.IsMatch(tokens[token + 1], "\\+|-|\\*|/")) {
                        throw new ArgumentException();
                    }

                    // '+' operator is next
                    if (operators.IsOnTop("\\+")) {
                        add();
                    }

                    // '-' operator is next
                    else if (operators.IsOnTop("-")) {
                        subtract();
                    }

                    // Push current token onto operator stack
                    operators.Push(tokens[token]);
                }

                // '*' and '/' operators
                else if (Regex.IsMatch(tokens[token], "/|\\*")) {
                    // Operator token is in an invalid position; throws ArgumentException
                    if (token == 0 || token == tokens.Length - 1 || Regex.IsMatch(tokens[token - 1], "\\+|-|\\*|/") || Regex.IsMatch(tokens[token + 1], "\\+|-|\\*|/")) {
                        throw new ArgumentException();
                    }

                    // Push current token onto operator stack
                    operators.Push(tokens[token]);
                }

                // Left parenthesis
                else if (Regex.IsMatch(tokens[token], "\\(")) {
                    operators.Push(tokens[token]);
                }

                // Right parenthesis
                else if (Regex.IsMatch(tokens[token], "\\)")) {
                    // Next operator is '+'
                    if (operators.IsOnTop("\\+")) {
                        add();
                    }

                    // Next operator is '-'
                    else if (operators.IsOnTop("-")) {
                        subtract();
                    }

                    // Next operator SHOULD be '('; pops if it is, else throws ArgumentException
                    if (!operators.IsOnTop("\\(")) {
                        throw new ArgumentException("Missing left parenthesis");
                    }
                    else {
                        operators.Pop();
                    }

                    // '*' operator is next
                    if (operators.IsOnTop("\\*")) {
                        // No more operands to apply operator to; throws ArgumentException
                        if (operands.Count < 2) {
                            throw new ArgumentException();
                        }

                        // Multiplies the top two operands, pushing the result
                        operators.Pop();
                        int val2 = int.Parse(operands.Pop());
                        int val1 = int.Parse(operands.Pop());
                        operands.Push((val1 * val2).ToString());
                    }

                    // '/' operator is next
                    else if (operators.IsOnTop("/")) {
                        // No more operands to apply operator to; throws ArgumentException
                        if (operands.Count < 2) {
                            throw new ArgumentException();
                        }

                        // Division by zero; throws ArgumentException
                        if (operands.IsOnTop("^0+$")) {
                            throw new ArgumentException();
                        }

                        // Divides the top two operands, pushing the result
                        operators.Pop();
                        int val2 = int.Parse(operands.Pop());
                        int val1 = int.Parse(operands.Pop());
                        operands.Push((val1 / val2).ToString());
                    }

                    // Next token is not an operator
                    if (Regex.IsMatch(tokens[token + 1], "[a-zA-Z0-9]")) {
                        throw new ArgumentException(") not followed by an operator! Found " + tokens[token + 1] + " instead!");
                    }
                }
            }

            // Helper method to add the next two operands, pushing the result onto the stack
            void add() {
                // Not enough operands; throws ArgumentException
                if (operands.Count < 2) {
                    throw new ArgumentException();
                }

                operators.Pop();
                int val2 = int.Parse(operands.Pop());
                int val1 = int.Parse(operands.Pop());
                operands.Push((val1 + val2).ToString());
            }

            // Helper method to subtract the next two operands, pushing the result onto the stack
            void subtract() {
                // Not enough operands; throws ArgumentException
                if (operands.Count < 2) {
                    throw new ArgumentException();
                }

                operators.Pop();
                int val2 = int.Parse(operands.Pop());
                int val1 = int.Parse(operands.Pop());
                operands.Push((val1 - val2).ToString());
            }

            // Helper method to multiply the next operand by the given token, pushing the result onto the stack
            void multiply(string token) {
                // Not enough operands; throws ArgumentException
                if (operands.Count == 0) {
                    throw new ArgumentException("Not enough operands");
                }

                operators.Pop();
                int val = int.Parse(operands.Pop());
                operands.Push((val * int.Parse(token)).ToString());
            }

            // Helper method to Divide the next operand by the given token, pushing the result onto the stack
            void divide(string token) {
                // Not enough operands; throws ArgumentException
                if (operands.Count == 0) {
                    throw new ArgumentException("Not enough operands");
                }

                // Division by zero; throws ArgumentException
                if (Regex.IsMatch(token, "^0+$")) {
                    throw new ArgumentException("Division by zero: " + operands.Peek() + "\\" + token);
                }

                operators.Pop();
                int val = int.Parse(operands.Pop());
                operands.Push((val / int.Parse(token)).ToString());
            }

            // Final stack conditions
            if (operators.Count == 0) {     // Empty operator stack
                if (operands.Count != 1) {  // Leftover operands; throws ArgumentException
                    throw new ArgumentException("Too many operands in final stack: " + operands.ToString());
                }

                // Return result
                return int.Parse(operands.Pop());
            }
            else {                          // Non-empty operator stack
                if (operators.Count != 1) { // Leftover operators; throws ArgumentException
                    throw new ArgumentException("Too man operators in final stack: " + operators.ToString());
                }

                // Last operator is '+'
                if (operators.Peek().Equals("+")) {
                    add();
                }
                // Last operator is '-'
                else if (operators.Peek().Equals("-")) {
                    subtract();
                }
                // Last operator is invalid; throws ArgumentException
                else {
                    throw new ArgumentException("Invalid final operator on stack: " + operators.Peek());
                }

                // Return result
                return int.Parse(operands.Pop());
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables() {
            return null;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString() {
            return null;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj) {
            return false;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2) {
            return (ReferenceEquals(f1, null) && ReferenceEquals(f2, null)) || f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2) {
            return !(ReferenceEquals(f1, null) && ReferenceEquals(f2, null)) && !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode() {
            return 0;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula) {
            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace)) {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline)) {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message) {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this() {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }

    /// <summary>
    /// Simple stackextension; adds the functionality of checking if the top operator matches a given pattern, ensuring a non-empty stack in the process.
    /// </summary>
    static class PS3StackExtensions {
        public static bool IsOnTop(this Stack<String> stack, String pattern) {
            return stack.Count > 0 && Regex.IsMatch(stack.Peek(), pattern);
        }
    }
}
