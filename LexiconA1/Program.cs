using System;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace LexiconA1
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;

            while (true)
            {
                Console.WriteLine("Hello, gimme something to compute! x closes the app!");
                input = Console.ReadLine();
                if (input == "x") return;
                Console.WriteLine(input + "=" + ParseMath(input)); //manuellt löst, med mer felhanterning
                Console.WriteLine("XPath-evaled: " + input + "=" + XPathEval(input)); //min favorit för text->matte, löser allt med ordning,
                                                                                      //parenteser osv utan extra libraries, lite långsamt dock
            }
        }

        /// <summary>
        /// takes an input string, splits it and calculates the result
        /// </summary>
        /// <param name="inp"></param>
        /// <returns></returns>
        public static double ParseMath(string inp)
        {
            double num1, num2, result =double.NaN;
            string[] mathstring = inp.Split('+','-','/','*','^');
            if (!(mathstring.Length == 2)) //stod inget om att kunna hantera flera operatorer, så gör det enkelt för mig själv här
            {
                Console.WriteLine("That isn't a valid input, please try again!");
                return double.NaN;
            }
            num1 = AskForDouble(mathstring[0]);
            num2 = AskForDouble(mathstring[1]); //säkerställa att vi har två st double
            if (inp.Contains('+')) result = CalcPlus(num1, num2);
            if (inp.Contains('-')) result = CalcMinus(num1, num2);
            if (inp.Contains('*')) result = CalcTimes(num1, num2);
            if (inp.Contains('/')) result = CalcDivide(num1, num2);
            if (inp.Contains('^')) result = CalcExp(num1, num2);
            return result;
        }

        /// <summary>
        /// Calculates the sum of x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double CalcMinus(double x, double y)
        {
            return x - y;
        }
        /// <summary>
        /// Calculates the difference of x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double CalcPlus(double x, double y)
        {
            return x + y;
        }
        /// <summary>
        /// Calculates the fraction of x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y">This should not be 0, but the function handles it by returning NaN</param>
        /// <returns></returns>
        public static double CalcDivide(double x, double y)
        {
            if (y==0)
            {
                Console.WriteLine("OMG. You tried to divide by 0! That's almost as bad as googling google!");
                return double.NaN;
            }
            else
            {
                return x / y;
            }
            
        }
        /// <summary>
        /// Calculates the product of x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double CalcTimes(double x, double y)
        {
            return x * y;
        }
        /// <summary>
        /// Calculates the power of x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double CalcExp(double x, double y)
        {
            return Math.Pow(x,y);
        }

        /// <summary>
        /// Uses XPathDocument Evaluate function to do the math stuff
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Calculated value, NaN if it can't calculate it</returns>
        public static double XPathEval(string expression)
        {
            try
            {   //hmm. /0 verkar ju ge lite underliga resultat (om den inte menar 8 = infinity). Skulle kunna fixas med en regex-matchning.
                var xsltExpression =
                    string.Format("number({0})",
                        new Regex(@"([\+\-\*])").Replace(expression, " ${1} ")
                                                .Replace("/", " div ")
                                                .Replace("%", " mod "));

                return (double)new XPathDocument
                    (new System.IO.StringReader("<r/>"))
                        .CreateNavigator()
                        .Evaluate(xsltExpression);
            }
            catch (Exception ex)
            {
                Console.WriteLine("XPath can't evaluate that string!");
                return double.NaN;
            }
        }


        /// <summary>
        /// Tests a string, and loops a prompt until the user inputs a valid double
        /// </summary>
        /// <returns></returns>
        public static double AskForDouble(string inp)
        {
            if (double.TryParse(inp, out double value))
            {
                return value;
            }
            else
            {
                Console.WriteLine(inp + " isn't a valid input, please try again!");
                return AskForDouble();
            }

        }
        
        /// <summary>
        /// Loops a prompt until the user inputs a valid int
        /// </summary>
        /// <returns></returns>
        public static double AskForDouble()
        {
        double value;
        while (!double.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("That isn't a valid input, please try again!");
            }
            return value;
        }
    }
}
