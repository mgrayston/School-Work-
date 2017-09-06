using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FormulaEvaluator {
    /// <summary>
    /// Class for evaluating given arithmetic expressions, returning the answer.
    /// </summary>
    public static class Evaluator {

        /// <summary>
        /// Delegate used for looking up variables in the given expression.
        /// </summary>
        /// <param name="v">Variable to lookup</param>
        /// <returns></returns>
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
    }

    /// <summary>
    /// Simple stackextension; adds the functionality of checking if the top operator matches a given pattern, ensuring a non-empty stack in the process.
    /// </summary>
    static class PS1StackExtensions {
        public static bool IsOnTop(this Stack<String> stack, String pattern) {
            return stack.Count > 0 && Regex.IsMatch(stack.Peek(), pattern);
        }
    }
}
