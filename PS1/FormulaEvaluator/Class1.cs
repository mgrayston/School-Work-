using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                    // throw exception for invalid variable name
                    if (Regex.IsMatch(tokens[token], "\\d+[a-zA-Z]+")) {    // add for ONLY a letter (a-zA-Z anchored at beginning and end
                        throw new ArgumentException();
                    }

                    // '+' and '-' operators
                    else if (Regex.IsMatch(tokens[token], "\\+|\\-")) {
                        // TODO
                    }

                    // '*' and '/' operators
                    else if (Regex.IsMatch(tokens[token], "\\/|\\*")) {
                        // TODO
                    }

                    // variables
                    else if (Regex.IsMatch(tokens[token], "[a-zA-Z]+\\d+")) {   // add rule for there not being anything before or after
                        // TODO
                    }

                    // digits
                    else if (Regex.IsMatch(tokens[token], "\\d+")) {    // add rule for nothing before or after
                        // TODO
                    }
                }
            }

            return result;
        }
    }
}
