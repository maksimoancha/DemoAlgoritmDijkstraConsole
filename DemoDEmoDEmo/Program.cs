using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;

namespace DemoDEmoDEmo
{
    class Program
    {
        static void Main(string[] args)
        {
//            string pattern2 = @"([+])|([-])|([*])|([/])|([(])|([)])";
//            
//            string input2 = @"216+27*(23/86+16-89)";

//            var x = Regex.Split(input2, pattern2).Where(o=>o!="").ToList();
//            foreach (string result in x)
//            {
//                Console.WriteLine( "'{0}'   ", result);
//            }

            Console.Write("Enter the Expression: ");
            string str = Console.ReadLine();
            var x = Dijkstra(str); 
            Console.WriteLine(x.ToString());
            Console.Read();
        }

        static int Dijkstra(string expression)
        {
            Stack<string> stackOperations = new Stack<string>(); //stack for operations
            Stack<string> stackDigit = new Stack<string>(); //stack for numbers

            //some correct expression string
            expression.Replace(" ", "").Replace("(-", "(0-").Replace(",-", ",0-");
            if (expression[0] == '-') 
            {
                expression = "0" + expression;
            }

            //split expression string to TOKENS
            List<string> listTokens = Regex.Split(expression, @"([+])|([-])|([*])|([/])|([(])|([)])")
                .Where(o => o != "").ToList();


            while (listTokens.Count > 0)
            {
                //read token
                string token = listTokens.First();
                listTokens.RemoveAt(0);             //take first token and delete it from list of tokens

                if (isIntNumber(token))    //If the token is a number, then add it to stack Digits.
                {
                    stackDigit.Push(token);
                }
                else if (isOperator(token)) //If the token is a operator, then:
                {
                    //While there is a token at the top of the stack and...
                    while (stackOperations.Count != 0 && isOperator(stackOperations.Peek()))
                    {
                        //... and if Prioritet of current token less or equal top token from stack Operations...
                        if (getPrecedence(token) <= getPrecedence(stackOperations.Peek()))
                        {
                            //take out top operator and two top digits from apropriate stacks, calculate and push result to Digits
                            string res = doIntProcedure(stackDigit.Pop(), stackDigit.Pop(), stackOperations.Pop())
                                .ToString();
                            stackDigit.Push(res);

                        }
                        // ...or  push token to stack Operation and break the while cycle.
                        else
                        {
                            stackOperations.Push(token);
                            break;
                        }
                    }

                    //if stack operation still empty or top of this stack if OpenBracket...
                    if (stackOperations.Count == 0 || isOpenBracket(stackOperations.Peek()))
                    {
                        //..than push this token to stack Operations
                        stackOperations.Push(token);
                    }
                }
                
                //if token is a OpemBracket than push it to stack Operations
                else if(isOpenBracket(token))
                {
                    stackOperations.Push(token);
                }

                //if token is a CloseBracket than..
                else if (isCloseBracket(token))
                {
                    //while top item from stack Operations is not a OpenBracket...
                    while (!isOpenBracket(stackOperations.Peek())) 
                    {
                        //take out top operator and two top digits from apropriate stacks, calculate and push result to Digits
                        string res = doIntProcedure(stackDigit.Pop(), stackDigit.Pop(), stackOperations.Pop())
                            .ToString();
                        stackDigit.Push(res);
                    }

                    //just take out top operator, it must be OpenBracket 
                    var x = stackOperations.Pop();
                }

                //if executing last number
                if (listTokens.Count==0)
                {
                    //execute all of the operations is stack
                    while (stackOperations.Count>0)
                    {
                        string res = doIntProcedure(stackDigit.Pop(), stackDigit.Pop(), stackOperations.Pop())
                            .ToString();
                        stackDigit.Push(res);
                    }
                }
            }
            //top and only one token in stack Digits is a result of expression
            return Int32.Parse(stackDigit.Pop());
        }

        static int doIntProcedure(string val2, string val1, string op)
        {
            int res = 0;
            switch (op)
            {
                case "+":
                    res = Int32.Parse(val1) + Int32.Parse(val2);
                    break;
                case "-":
                    res = Int32.Parse(val1) - Int32.Parse(val2);
                    break;
                case "*":
                    res = Int32.Parse(val1) * Int32.Parse(val2);
                    break;
                case "/":
                    res = Int32.Parse(val1) / Int32.Parse(val2);
                    break;
            }

            return res;
        }

        static int getPrecedence(string token)
        {
            if (token.Equals("+") || token.Equals("-"))
            {
                return 1;
            }
            return 2;
        } //return rank of operator: 1 or 2

        static bool isIntNumber(string token)
        {
            bool ka = Int32.TryParse(token, out int digResult);
            return ka;
        }

        static bool isOperator(string token)
        {
            string OPERATORS = "+-*/";
            return OPERATORS.Contains(token);
        }

        static bool isOpenBracket(string token)
        {
            return token.Equals("(");
        }

        static bool isCloseBracket(string token)
        {
            return token.Equals(")");
        }
    }
}
