using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CONFIG_STRING
{
    public static string ENV = ".auth03.test"; // the environment

    public static string PATH_ENV = "M:\\firstCompare\\forReview\\DomainConfig.AUTH03.xml";
    public static string PATH_LIVE = "M:\\firstCompare\\forReview\\DomainLive.xml";

    /* DIRECT */
    public static string DOMAIN_DIRECT = "nextdirect.com.auth03.test"; // **
    public static string TOP_LEVELDOMAIN = ".com.auth03.test"; // **
    public static string APDN = "account.nextdirect.auth03.test"; // **
    public static string COOKIE_DOMAIN = "nextdirect.com.auth03.test"; // **

    /* MOBILE */
    public static string DOMAIN_MOBILE = "mobile.nextdirect.com.auth03.test"; // **
    public static string APDN_M = "m-account.nextdirect.com.auth03.test"; // **

    /* NON-DIRECT */
    public static string DOMAIN = "next.";
    public static string APDN1 = "account.next.";

    public static string COUNTRY = " ";


    public static void setUpConfigs()
    {
        DOMAIN = "next." + COUNTRY + ENV;
        APDN1 = "account.next." + COUNTRY + ENV;
    }

    public static void setUpConfigsInDirect()
    {
        DOMAIN_DIRECT += ENV;
        TOP_LEVELDOMAIN += ENV;
        APDN += ENV;
        COOKIE_DOMAIN += ENV;
    }

    public static void setUpConfigsInMobile()
    {

        DOMAIN_MOBILE += ENV;
        APDN_M += ENV;
    }
}
