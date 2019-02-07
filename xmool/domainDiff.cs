using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xmool
{

    /* use cases:
     * live has mobile. but other env has m. -> matches what live has
     * live has nextdirect.com but, other env has next.{countrycode}
     * live has next.{countrycode} but, other env has nextdirect.com or next.com.{countrycode}
     * 
     * not supported:
     * live has m. but other env has mobile.
        ***********************************************************************/
    class domainDiff
    {

        private static inconsisCheck checkIssues;

        static void Main(string[] args)
        {

            mainMenu();
        }

        private static void mainMenu()
        {

            Console.WriteLine(" ");
            Console.WriteLine("Main menu");
            Console.WriteLine("1 - Check for inconsistency");
            Console.WriteLine("2 - Update domain information");
            Console.WriteLine("3 - Set environment");

            ConsoleKey key = Console.ReadKey().Key;

            if (key == ConsoleKey.D1)
            {
                checkIssues = new inconsisCheck();
                checkIssues.beginTask();

            }
            else if (key == ConsoleKey.D3)
            {
                //Console.ReadKey();
                Console.Write("Enter your new environmet: ");
                CONFIG_STRING.ENV = Console.ReadLine();

                Console.WriteLine("Your new env: " + CONFIG_STRING.ENV); //33
                Console.ReadKey();
            }
        }
    }
}