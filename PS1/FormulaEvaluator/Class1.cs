using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator {
    public static class Evaluator {

        public delegate int Lookup(String v);

        public static int Evaluate(String exp, Lookup variableEvaluator) {
            string[] tokens = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            String regexOps = "\\+|\\-|\\*|\\/|\\(|\\)";
            String regexParens = "\\(|\\)";

            Stack<String> values = new Stack<String>();
            Stack<String> operators = new Stack<String>();

            foreach (int token in Enumerable.Range(0,tokens.Length)) {
                // continue to next token if it is empty
                if (tokens[token] != null) {
                    // eliminate extra whitespace
                    Regex.Replace(tokens[token], "\\s+", "");
                    // continue to next token if nothing remains
                    if (token == null) {
                        continue;
                    }

                    // token is a value
                    if (!Regex.IsMatch(tokens[token], regexOps)) {
                        // invalid variable
                        if (Regex.IsMatch(tokens[token], "\\d+[a-zA-Z]+")) {
                            throw new ArgumentException();
                        }

                        // if last token was not an operator, throw exception
                        if (token > 0 && !Regex.IsMatch(tokens[token - 1], regexOps)) {
                            throw new ArgumentException();
                        }

                        // push valid value
                        values.Push(tokens[token]);
                    } else {
                        // token is an operator

                        // current token is not a parenthesis and last token is an operator other than a parenthesis
                        if (!Regex.IsMatch(tokens[token], regexParens) && Regex.IsMatch(tokens[token - 1], regexOps) 
                            && !Regex.IsMatch(tokens[token - 1], regexParens)) {
                            throw new ArgumentException();
                        }

                        // push valid operator
                        operators.Push(tokens[token]);
                    }
                }
            }

            return -1;
        }
    }
}
