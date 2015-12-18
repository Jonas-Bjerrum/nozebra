using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NoZebraFlickrAPI
{
    public class FlickrFeed
    {
        /// <summary>
        /// Select all flickr items from the feed
        /// </summary>
        /// <returns>List of flickr items</returns>
        public static List<FlickrItem> GetFlickItems()
        {
            return GetFlickItems("");
        }

        /// <summary>
        /// Select flickr items that match some specific tags
        /// </summary>
        /// <param name="strTags">Tags, seperated by comma</param>
        /// <returns>List of flickr items</returns>
        public static List<FlickrItem> GetFlickItems(string strTags)
        {
            //Generate tag part of querystring
            string strQuery = "";
            if (!string.IsNullOrEmpty(strTags))
                strQuery = "&tags=" + strTags;

            //Select matching results from the feed
            List<FlickrItem> flickrItems = ReadFromFeed(strQuery);

            //Return the results
            return flickrItems;
        }

        /// <summary>
        /// Select flickr items that match some specific tags
        /// </summary>
        /// <param name="strQuery">Query with tags</param>
        /// <returns>List of flickr items</returns>
        private static List<FlickrItem> ReadFromFeed(string strQuery)
        {
            //Intitialize list with flickr elements, that will be returned by the method
            List<FlickrItem> lstToReturn = new List<FlickrItem>();

            //Create new xml document
            XmlDocument xmlDoc = new XmlDocument();

            //Load feed from url, which is specified in the web.config
            xmlDoc.Load(ConfigurationSettings.AppSettings["flickrfeed"] + strQuery);

            //Resolve root node item
            XmlNodeList xmlRootNode = xmlDoc.SelectNodes("rss/channel/item");

            //Read each node into object
            foreach (XmlNode node in xmlRootNode)
            {
                string strTitle = "";
                string strImageUrl = "";
                DateTime dtmPublished = new DateTime(1900, 1, 1);

                //Read title
                XmlNode subNode = node.SelectSingleNode("title");
                if (subNode != null)
                    strTitle = subNode.InnerText;

                //Read imageurl
                subNode = node.SelectSingleNode("enclosure");
                if (subNode != null)
                    strImageUrl = subNode.Attributes[0].Value;

                //Read date for publish
                subNode = node.SelectSingleNode("pubDate");
                if (subNode != null)
                    DateTime.TryParse(subNode.InnerText, out dtmPublished);

                //Add photo element to the list
                lstToReturn.Add(new FlickrItem { Title = strTitle, ImageUrl = strImageUrl, Published = dtmPublished });
            }

            //Return list with flickritems
            return lstToReturn;
        }
    }
}
