using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


class inconsisCheck
{

    public XmlDocument docNonLive;

    /*
    public string ENV = ".auth03.test"; // the environment

    public string PATH_ENV = "M:\\firstCompare\\DomainAuth.xml";
    public string PATH_LIVE = "M:\\firstCompare\\DomainLive.xml";

    public string DOMAIN_DIRECT = "nextdirect.com.auth03.test"; // **
    public string TOP_LEVELDOMAIN = ".com.auth03.test"; // **
    public string APDN = "account.nextdirect.auth03.test"; // **
    public string COOKIE_DOMAIN = "nextdirect.com.auth03.test"; // **

    public string DOMAIN_MOBILE = "mobile.nextdirect.com.auth03.test"; // **
    public string APDN_M = "m-account.nextdirect.com.auth03.test"; // **
    */

    public void beginTask()
    {
        //Console.ReadKey();
        docNonLive = new XmlDocument();
        docNonLive.Load(CONFIG_STRING.PATH_ENV); // ** change string to top

        XmlNodeList fileNodeList = docNonLive.GetElementsByTagName("domain");

        for (int iNode = fileNodeList.Count - 1; iNode >= 0; iNode--)
        {
            XmlNode node = fileNodeList[iNode];
            if (node.NodeType != XmlNodeType.Comment)
            {
                checkOccurance(node["channelid"].InnerText, node["domainname"].InnerText, node);
            }

        }
    }

    /* Method takes in the channelId from the NonLive config file
     * loops through the live config file
     * performs a check if the channelid matches
     * performs the relevant changes
     * */
    void checkOccurance(string channelId, string authDomain, XmlNode nonLive)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(CONFIG_STRING.PATH_LIVE); // ** change string to top

        foreach (XmlNode node in doc.DocumentElement.SelectSingleNode("/domainconfig/domains"))
        {
            if (node.NodeType != XmlNodeType.Comment)
            {

                if (node["channelid"].InnerText.Equals(channelId) && node["channelid"].InnerText.Contains("NEXT")) // finds the channelID for "NEXT_XY" only
                {

                    if (compareDomainsDiff(node["domainname"].InnerText, authDomain, node["channelid"].InnerText) == 2)
                    {
                        performChangeinDesktop(node["channelid"].InnerText, node["domainname"].InnerText, authDomain, nonLive);
                    }
                    break;
                }

                // finds for mobile domains that start with "m"
                if (node["channelid"].InnerText.Equals(channelId) && node["channelid"].InnerText.Contains("MOBI") && node["domainname"].InnerText.StartsWith("m") && authDomain.StartsWith("m"))
                {
                    if (compareDomainsDiff(node["domainname"].InnerText, authDomain, node["channelid"].InnerText) == 1)
                    {
                        performChangeinMobile(node["channelid"].InnerText, node["domainname"].InnerText, authDomain, nonLive);
                    }
                }

                //ADAPTER CHANGE
                if (node["channelid"].InnerText.Equals(channelId) && node["channelid"].InnerText.Contains("MOBI") && !node["domainname"].InnerText.StartsWith("m") && !authDomain.StartsWith("m"))
                {

                    if (compareDomainsDiff(node["domainname"].InnerText, authDomain, node["channelid"].InnerText) == 1)
                    {
                        performChangeinDesktop(node["channelid"].InnerText, node["domainname"].InnerText, authDomain, nonLive);
                    }


                }


            }
        }
    }


    /* 
        Compare Live and Auth Domain with existence of .com,
        if either have .com, it will be shown
    */

    int compareDomains(string liveDomain, string authDomain)
    {

        if ((liveDomain.Contains(".com") && !authDomain.Contains(".com")))
        {
            return 1;
        }
        else if ((!liveDomain.Contains(".com") && authDomain.Contains(".com")))
        {

            return 2;
        }
        return 0;

    }

    /*
     * www. of the liveDomain is taken off
     * auth03.test is taken off from auth domains
     * they are then compared 
     * 
     * RETURNS:
     * 1 - MOBI domain
     * 2 - NEXT domain
     * 0 - None
     */
    int compareDomainsDiff(string liveDomain, string authDomain, string cid)
    {
        // to remove auth03.test(12 indexes removed)
        authDomain = authDomain.Substring(0, authDomain.Length - 12); // ** changed to allow other envs

        /* this condition removes "www."*/


        if (!liveDomain.StartsWith("m")) // case for m.
        {
            liveDomain = liveDomain.Substring(4);
        }

        if (!authDomain.Equals(liveDomain))
        {
            Console.WriteLine(" ");
            Console.WriteLine("**********************************");
            Console.WriteLine(cid);
            Console.WriteLine("In Auth " + authDomain);
            Console.WriteLine("In Live " + liveDomain);
            Console.WriteLine("****************CHANGES*******************");
            if (cid.Contains("MOBI"))
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        return 0;

    }


    /* updates the XML file with the correct domain configs */
    void performChangeinMobile(string cid, string liveDomain, string authDomain, XmlNode nonLive)
    {


        // check if "mobile" exists in live
        if (liveDomain.Contains("mobile.") && !authDomain.Contains("mobile."))
        {

            CONFIG_STRING.COUNTRY = cid.Substring(5).ToLower();

            //apply change to another environment with:

            // domain name goes from  m. to mobile.
            Console.WriteLine("New domain will be: " + CONFIG_STRING.DOMAIN_MOBILE);
            // add m-account.nextdirect.com.local.uat10.test
            Console.WriteLine("New Account Portal will be: " + CONFIG_STRING.APDN_M);
            Console.WriteLine(" ");
            Console.WriteLine("Confirm with Y if you want to make the change");

            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                // the element value cannot be directly changed, therefore a new node would need to be made and then replaced with the previous

                // node of domains selected
                XmlNode newNode = docNonLive.SelectSingleNode("/domainconfig/domains");
                // new element made
                XmlNode newestNode = docNonLive.CreateElement("domain");

                XmlAttribute idAttr = docNonLive.CreateAttribute("id");


                idAttr.Value = nonLive.Attributes[0].InnerText;

                newestNode.Attributes.Append(idAttr);


                newestNode.InnerXml = nonLive.InnerXml; // clone the current node


                // update the configs
                newestNode["domainname"].InnerText = CONFIG_STRING.DOMAIN_MOBILE;
                //newestNode["topleveldomain"].InnerText = CONFIG_STRING.TOP_LEVELDOMAIN; // 
                newestNode["accountportaldomainname"].InnerText = CONFIG_STRING.APDN_M;
                newestNode["parsestrategy"].InnerText = "3";

                newestNode["country"].RemoveAllAttributes();
                newestNode["country"].InnerText = CONFIG_STRING.COUNTRY;

                // replacement
                nonLive.ParentNode.ReplaceChild(newestNode, nonLive);

                //save as modified 
                docNonLive.Save("modified.xml");

                Console.WriteLine("Change made");
            }
            else
            {
                Console.WriteLine("Change not made");
            }
        }

    }

    void performChangeinDesktop(string id, string liveDomain, string authDomain, XmlNode nonLive)
    {

        CONFIG_STRING.COUNTRY = id.Substring(5).ToLower();

        if (compareDomains(liveDomain, authDomain) == 1)
        {
            Console.WriteLine("auth03 needs to be updated with nextdirect.com ");
            /*
            Console.WriteLine("New domain will be: " + CONFIG_STRING.DOMAIN_DIRECT);
            Console.WriteLine("New top leve domain will be: " + CONFIG_STRING.TOP_LEVELDOMAIN);
            Console.WriteLine("New apdn will be: " + CONFIG_STRING.APDN);
            Console.WriteLine("New cookie domain will be: " + CONFIG_STRING.COOKIE_DOMAIN);
            Console.WriteLine("New parse strategy will be: " + "3");
            Console.WriteLine("Country will be: " + CONFIG_STRING.COUNTRY);
            */

            displayChanges(CONFIG_STRING.DOMAIN_DIRECT, CONFIG_STRING.APDN, "3", id.Substring(5));

            writeChanges(nonLive, CONFIG_STRING.DOMAIN_DIRECT, CONFIG_STRING.TOP_LEVELDOMAIN, CONFIG_STRING.APDN, CONFIG_STRING.COOKIE_DOMAIN, "3", CONFIG_STRING.COUNTRY, 1, nonLive.Attributes[0].InnerText);

        }


        if (compareDomains(liveDomain, authDomain) == 2)
        {
            Console.WriteLine(".com needs to be removed from auth03");

            CONFIG_STRING.setUpConfigs();

            displayChanges(CONFIG_STRING.DOMAIN, CONFIG_STRING.APDN1, "2", CONFIG_STRING.COUNTRY);

            writeChanges(nonLive, CONFIG_STRING.DOMAIN, CONFIG_STRING.ENV, CONFIG_STRING.APDN1, CONFIG_STRING.DOMAIN, "2", CONFIG_STRING.COUNTRY, 2, nonLive.Attributes[0].InnerText);

        }
    }

    void displayChanges(string domain, string apdn, string strategy, string country)
    {

        Console.WriteLine("New domain will be: " + domain);
        Console.WriteLine("New top leve domain will be: " + CONFIG_STRING.ENV);
        Console.WriteLine("New apdn will be: " + apdn);
        Console.WriteLine("New cookie domain will be: " + domain);
        Console.WriteLine("New parse strategy will be: " + strategy);
        Console.WriteLine("Country will be: " + country);


    }


    void writeChanges(XmlNode nonLive, string domain, string top_level, string apdn, string cookie, string strategy, string country, int type, string id)
    {
        Console.WriteLine(" ");
        Console.WriteLine("Confirm with Y if you want to make the change");

        if (Console.ReadKey().Key == ConsoleKey.Y)
        {

            XmlNode newNode = docNonLive.SelectSingleNode("/domainconfig/domains");

            XmlNode newestNode = docNonLive.CreateElement("domain");
            XmlAttribute idAttr = docNonLive.CreateAttribute("id");


            idAttr.Value = id;

            newestNode.Attributes.Append(idAttr);

            newestNode.InnerXml = nonLive.InnerXml; // clone the current node

            newestNode["domainname"].InnerText = domain; //

            newestNode["accountportaldomainname"].InnerText = apdn;
            newestNode["cookiedomainname"].InnerText = domain; //
            newestNode["parsestrategy"].InnerText = strategy;

            if (type == 1)
            {
                newestNode["country"].RemoveAllAttributes();
                newestNode["country"].InnerText = country;
                newestNode["topleveldomain"].InnerText = ".com" + CONFIG_STRING.ENV;
            }

            if (type == 2)
            {

                newestNode["country"].RemoveAllAttributes();
                newestNode["country"].SetAttribute("null", "true");
                newestNode["topleveldomain"].InnerText = "." + CONFIG_STRING.COUNTRY + CONFIG_STRING.ENV;
                //newestNode["country"].InnerText = id.Substring(5).ToLower();
            }

            nonLive.ParentNode.ReplaceChild(newestNode, nonLive);



            docNonLive.Save("modified.xml");
            Console.WriteLine("Change made");
        }

        else
        {
            Console.WriteLine("Change not made");
        }
    }
}
