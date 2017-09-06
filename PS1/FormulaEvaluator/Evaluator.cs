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
            string[] tokens = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            Stack<String> operands = new Stack<String>();
            Stack<String> operators = new Stack<String>();

            // push all tokens onto stacks
            foreach (int token in Enumerable.Range(0, tokens.Length)) {
                // eliminate extra whitespace
                tokens[token] = Regex.Replace(tokens[token], "^\\s+", "");
                tokens[token] = Regex.Replace(tokens[token], "\\s+$", "");

                // catch invalid tokens with whitespace in the middle
                if (Regex.IsMatch(tokens[token], "\\s+")) {
                    throw new ArgumentException("Invalid token: " + tokens[token]);
                }

                // continue on empty token
                if (tokens[token].Equals("")) {
                    continue;
                }

                // digits
                if (Regex.IsMatch(tokens[token], "^\\d+$")) {
                    // '*' operator is next
                    if (operators.IsOnTop("\\*")) {
                        multiply(tokens[token]);
                    }

                    // '/' operator is next
                    else if (operators.IsOnTop("/")) {
                        divide(tokens[token]);
                    }

                    else {
                        operands.Push(tokens[token]);
                    }
                }

                // variables
                else if (Regex.IsMatch(tokens[token], "[a-zA-Z]+\\d+")) {
                    // throw exception for invalid variable name
                    if (Regex.IsMatch(tokens[token], "[a-zA-Z]?\\d+[a-zA-Z]+\\d?")) {
                        throw new ArgumentException("Invalid variable: " + tokens[token]);
                    }

                    // lookup value of variable
                    int varResult = variableEvaluator(tokens[token]);

                    // '*' operator is next
                    if (operators.IsOnTop("\\*")) {
                        multiply(varResult.ToString());
                    }

                    // '/' operator is next
                    else if (operators.IsOnTop("/")) {
                        divide(varResult.ToString());
                    }

                    else {
                        operands.Push(varResult.ToString());
                    }
                }

                // '+' and '-' operators
                else if (Regex.IsMatch(tokens[token], "\\+|-")) {
                    // token is at beginning or end, or is preceded or followed by another operator
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
                        add();
                    }

                    // next operator is '-'
                    else if (operators.IsOnTop("-")) {
                        subtract();
                    }

                    // next operator SHOULD be '('
                    if (!operators.IsOnTop("\\(")) {
                        throw new ArgumentException("Missing left parenthesis");
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

            // adds and pushes the next two operands
            void add() {
                // not enough operands
                if (operands.Count < 2) {
                    throw new ArgumentException();
                }

                operators.Pop();

                int val2 = int.Parse(operands.Pop());
                int val1 = int.Parse(operands.Pop());
                operands.Push((val1 + val2).ToString());
            }

            // subtracts and pushes the next two operands
            void subtract() {
                // not enough operands
                if (operands.Count < 2) {
                    throw new ArgumentException();
                }

                operators.Pop();

                int val2 = int.Parse(operands.Pop());
                int val1 = int.Parse(operands.Pop());
                operands.Push((val1 - val2).ToString());
            }

            // pops and multiplies the first operand by the given token, pushing the result
            void multiply(string token) {
                // no more operands to apply operator to
                if (operands.Count == 0) {
                    throw new ArgumentException("Not enough operands");
                }

                operators.Pop();
                int val = int.Parse(operands.Pop());
                operands.Push((val * int.Parse(token)).ToString());
            }

            // pops and divides the first operand by the given token, pushing the result
            void divide(string token) {
                // no more operands to apply operator to
                if (operands.Count == 0) {
                    throw new ArgumentException("Not enough operands");
                }

                // division by zero
                if (Regex.IsMatch(token, "^0+$")) {
                    throw new ArgumentException("Division by zero: " + operands.Peek() + "\\" + token);
                }

                operators.Pop();
                int val = int.Parse(operands.Pop());
                operands.Push((val / int.Parse(token)).ToString());
            }

            // final stack conditions
            if (operators.Count == 0) {     // empty operator stack
                if (operands.Count != 1) {
                    throw new ArgumentException("Too many operands in final stack: " + operands.ToString());
                }

                return int.Parse(operands.Pop());
            }
            else {                          // non-empty operator stack
                if (operators.Count != 1) {
                    throw new ArgumentException("Too man operators in final stack: " + operators.ToString());
                }

                // last operator is '+'
                if (operators.Peek().Equals("+")) {
                    add();
                }
                // last operator is '-'
                else if (operators.Peek().Equals("-")) {
                    subtract();
                }
                // last operator is invalid
                else {
                    throw new ArgumentException("Invalid final operator on stack: " + operators.Peek());
                }

                return int.Parse(operands.Pop());
            }
        }
    }

    static class PS1StackExtensions {
        public static bool IsOnTop(this Stack<String> stack, String pattern) {
            return stack.Count > 0 && Regex.IsMatch(stack.Peek(), pattern);
        }
    }
}
