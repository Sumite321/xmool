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
            Console.WriteLine("2 - View current domain information");
            Console.WriteLine("3 - Set environment");

            ConsoleKey key;

            try
            {
                while ((key = Console.ReadKey().Key )!= ConsoleKey.D0)
                {
                    switch (key)
                    {

                        case ConsoleKey.D1:
                            checkIssues = new inconsisCheck();
                            checkIssues.beginTask();
                            break;
                        case ConsoleKey.D2:
                            ViewDomainInformation();
                            break;
                        case ConsoleKey.D3:
                            Console.Write("Enter your new environmet: ");
                            CONFIG_STRING.ENV = Console.ReadLine();

                            Console.WriteLine("Your new env: " + CONFIG_STRING.ENV); //33
                            Console.ReadKey();
                            break;
                        default:
                            print("Make a choice");
                            break;

                    }
                }
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }

        }

        private static void ViewDomainInformation()
        {
            print(" ");
            print("Env is" + CONFIG_STRING.ENV);
            print("Domains are:" + CONFIG_STRING.DOMAIN + "and " + CONFIG_STRING.DOMAIN_DIRECT);
            print("APND are: " + CONFIG_STRING.APDN + " and " + CONFIG_STRING.APDN1);
            print("Cookie domain is: " + CONFIG_STRING.COOKIE_DOMAIN);
            print("Top level domain is: " + CONFIG_STRING.TOP_LEVELDOMAIN);
        }

        private static void print(string s)
        {
            Console.WriteLine(s);
        }
    }
}