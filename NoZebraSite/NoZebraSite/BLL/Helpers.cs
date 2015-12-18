using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoZebraFlickrAPI;

namespace NoZebraSite
{
    public class Helpers
    {
        //Enum containing sort types, by doing this we are sure that we always spell the fields correct
        public enum SortByTypes { Title, Published };

        //Method either store or receive tags from users session
        public static string LatestSearchTags
        {
            get
            {
                if (HttpContext.Current.Session["LatestSearchQuery"] != null)
                    return HttpContext.Current.Session["LatestSearchQuery"].ToString();
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["LatestSearchQuery"] = value;
            }
        }

        //Method either store or receive items from users session
        public static List<FlickrItem> LatestSearchItem
        {
            get
            {
                if (HttpContext.Current.Session["LatestSearchItem"] != null)
                    return HttpContext.Current.Session["LatestSearchItem"] as List<FlickrItem>;
                else
                    return new List<FlickrItem>();
            }
            set
            {
                HttpContext.Current.Session["LatestSearchItem"] = value;
            }
        }
    }
}