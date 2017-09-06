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

            Stack<String> operands = new Stack<String>();
            Stack<String> operators = new Stack<String>();

            // push all tokens onto stacks
            foreach (int token in Enumerable.Range(0, tokens.Length)) {
                // eliminate extra whitespace
                tokens[token] = Regex.Replace(tokens[token], "\\s+", "");

                // empty token
                if (tokens[token].Equals("")) {
                    continue;
                }

                // digits
                if (Regex.IsMatch(tokens[token], "^\\d+$")) {

                    // '*' operator is next
                    if (operators.IsOnTop("\\*")) {
                        // no more operands to apply operator to
                        if (operands.Count == 0) {
                            throw new ArgumentException();
                        }

                        operators.Pop();
                        int val = int.Parse(operands.Pop());
                        operands.Push((val * int.Parse(tokens[token])).ToString());
                    }

                    // '/' operator is next
                    else if (operators.IsOnTop("/")) {
                        // no more operands to apply operator to
                        if (operands.Count == 0) {
                            throw new ArgumentException();
                        }

                        // division by zero
                        if (operands.IsOnTop("^0+$")) {
                            throw new ArgumentException();
                        }

                        operators.Pop();
                        int val = int.Parse(operands.Pop());
                        operands.Push((val / int.Parse(tokens[token])).ToString());
                    }

                    else {
                        operands.Push(tokens[token]);
                    }
                }

                // variables
                else if (Regex.IsMatch(tokens[token], "[a-zA-Z]+\\d+")) {   // double check on what valid variables are/if this code should catch it
                                                                            // throw exception for invalid variable name
                    if (Regex.IsMatch(tokens[token], "[a-zA-Z]?\\d+[a-zA-Z]+\\d?")) {
                        throw new ArgumentException();
                    }

                    // lookup value of variable
                    int t = variableEvaluator(tokens[token]);

                    // '*' operator is next
                    if (operators.IsOnTop("\\*")) {
                        // no more operands to apply operator to
                        if (operands.Count == 0) {
                            throw new ArgumentException();
                        }

                        operators.Pop();
                        int val = int.Parse(operands.Pop());
                        operands.Push((val * t).ToString());
                    }

                    // '/' operator is next
                    else if (operators.IsOnTop("/")) {
                        // no more operands to apply operator to
                        if (operands.Count == 0) {
                            throw new ArgumentException();
                        }

                        // division by zero
                        if (operands.IsOnTop("^0+$")) {
                            throw new ArgumentException();
                        }

                        operators.Pop();
                        int val = int.Parse(operands.Pop());
                        operands.Push((val / t).ToString());
                    }

                    else {
                        operands.Push(t.ToString());
                    }
                }

                // '+' and '-' operators
                else if (Regex.IsMatch(tokens[token], "\\+|-")) {
                    // token is at beginning or end, or is preceded or followed by another operator
                    if (token == 0 || token == tokens.Length - 1 || Regex.IsMatch(tokens[token - 1], "\\+|-|\\*|/") || Regex.IsMatch(tokens[token + 1], "\\+|-|\\*|/")) {
                        throw new ArgumentException();
                    }

                    if (operators.IsOnTop("\\+")) {
                        // not enough operands
                        if (operands.Count < 2) {
                            throw new ArgumentException();
                        }

                        operators.Pop();

                        int val2 = int.Parse(operands.Pop());
                        int val1 = int.Parse(operands.Pop());
                        operands.Push((val1 + val2).ToString());
                    }

                    else if (operators.IsOnTop("-")) {
                        // not enough operands
                        if (operands.Count < 2) {
                            throw new ArgumentException();
                        }

                        operators.Pop();

                        int val2 = int.Parse(operands.Pop());
                        int val1 = int.Parse(operands.Pop());
                        operands.Push((val1 - val2).ToString());
                    }

                    operators.Push(tokens[token]);
                }

                // '*' and '/' operators
                else if (Regex.IsMatch(tokens[token], "/|\\*")) {
                    // token is at beginning or end, or is preceded or followed by another operator
                    if (token == 0 || token == tokens.Length - 1 || Regex.IsMatch(tokens[token - 1], "\\+|-|\\*|/") || Regex.IsMatch(tokens[token + 1], "\\+|-|\\*|/")) {
                        throw new ArgumentException();
                    }

                    operators.Push(tokens[token]);
                }

                // left parenthesis
                else if (Regex.IsMatch(tokens[token], "\\(")) {
                    operators.Push(tokens[token]);
                }

                // right parenthesis
                else if (Regex.IsMatch(tokens[token], "\\)")) {
                    // next operator is '+'
                    if (operators.IsOnTop("\\+")) {
                        // too few operands
                        if (operands.Count < 2) {
                            throw new ArgumentException();
                        }

                        int val2 = int.Parse(operands.Pop());
                        int val1 = int.Parse(operands.Pop());
                        operators.Pop();
                        operands.Push((val1 + val2).ToString());
                    }

                    // next operator is '-'
                    else if (operators.IsOnTop("-")) {
                        // too few operands
                        if (operands.Count < 2) {
                            throw new ArgumentException();
                        }

                        int val2 = int.Parse(operands.Pop());
                        int val1 = int.Parse(operands.Pop());
                        operators.Pop();
                        operands.Push((val1 - val2).ToString());
                    }

                    // next operator SHOULD be '('
                    if (!operators.IsOnTop("\\(")) {
                        throw new ArgumentException();
                    }
                    else {
                        operators.Pop();
                    }

                    // '*' operator is next
                    if (operators.IsOnTop("\\*")) {
                        // no more operands to apply operator to
                        if (operands.Count < 2) {
                            throw new ArgumentException();
                        }

                        operators.Pop();
                        int val2 = int.Parse(operands.Pop());
                        int val1 = int.Parse(operands.Pop());
                        operands.Push((val1 * val2).ToString());
                    }

                    // '/' operator is next
                    else if (operators.IsOnTop("/")) {
                        // no more operands to apply operator to
                        if (operands.Count < 2) {
                            throw new ArgumentException();
                        }

                        // division by zero
                        if (operands.IsOnTop("^0+$")) {
                            throw new ArgumentException();
                        }

                        operators.Pop();
                        int val2 = int.Parse(operands.Pop());
                        int val1 = int.Parse(operands.Pop());
                        operands.Push((val1 / val2).ToString());
                    }
                }
            }

            return result;
        }
    }

    static class PS1StackExtensions {
        public static bool IsOnTop(this Stack<String> stack, String pattern) {
            return stack.Count > 0 && Regex.IsMatch(stack.Peek(), pattern);
        }
    }
}
