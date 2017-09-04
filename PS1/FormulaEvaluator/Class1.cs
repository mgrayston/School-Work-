using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FormulaEvaluator {
    public static class Evaluator {

        public delegate int Lookup(String v);

        /// <summary>
        /// Evaluates the result of a given arithmetic expression. Returns the result.
        /// </summary>
        /// <param name="exp">The arithmetic expression to be evaluated.</param>
        /// <param name="variableEvaluator">Lookup used to retrieve stored values of variables in the expression.</param>
        /// <returns>Result of exp.</returns>
        public static int Evaluate(String exp, Lookup variableEvaluator) {
            int result = -999;

            string[] tokens = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            Stack<String> values = new Stack<String>();
            Stack<String> operators = new Stack<String>();

            // push all tokens onto stacks
            foreach (int token in Enumerable.Range(0, tokens.Length)) {
                // eliminate extra whitespace
                Regex.Replace(tokens[token], "\\s+", "");

                if (!tokens[token].Equals("")) {
                    // '+' and '-' operators
                    if (Regex.IsMatch(tokens[token], "\\+|\\-")) {
                        if (token == 0 || token == tokens.Length) {
                            throw new ArgumentException();
                        }
                        // TODO
                    }

                    // '*' and '/' operators
                    else if (Regex.IsMatch(tokens[token], "\\/|\\*")) {
                        if (token == 0 || token == tokens.Length) {
                            throw new ArgumentException();
                        }
                        // TODO
                    }

                    // parentheses
                    else if (Regex.IsMatch(tokens[token], "\\(|\\)")) {
                        // TODO
                    }

                    // variables
                    else if (Regex.IsMatch(tokens[token], "[a-zA-Z]+\\d+")) {   // double check on what valid variables are/if this code should catch it
                        // throw exception for invalid variable name
                        if (Regex.IsMatch(tokens[token], "[a-zA-Z]?\\d+[a-zA-Z]+\\d?")) {
                            throw new ArgumentException();
                        }
                        // TODO
                    }

                    // digits
                    else if (Regex.IsMatch(tokens[token], "\\d+")) {
                        // TODO
                    }
                }
            }

            return result;
        }
    }
}
