using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoZebraFlickrAPI;
using NoZebraSite.Models;
using PagedList;

namespace NoZebraSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string orderBy, string txtSearch, int? page)
        {
            #region Initialize search controls
            ViewBag.CurrentSort = orderBy;
            ViewBag.CurrentFilter = txtSearch;

            //Initialize select box containing fields to sort with
            var OrderLst = new List<string>();
            OrderLst.Add(Helpers.SortByTypes.Title.ToString());
            OrderLst.Add(Helpers.SortByTypes.Published.ToString());
            ViewBag.orderBy = new SelectList(OrderLst);
            #endregion

            #region Tags
            //Check if we have received some tags to search for
            string strTags = "";
            if (!string.IsNullOrEmpty(txtSearch))
                strTags = txtSearch;

            //Initialize list with flickritems
            List<FlickrItem> flickrItems = new List<FlickrItem>();

            //Check if we have searched for the same tags before, if we have, then show the results from that search
            if (Helpers.LatestSearchTags == strTags)
                flickrItems = Helpers.LatestSearchItem;
            else
            {
                //We have not searched for these tags in our latest search, so we need to update the session
                Helpers.LatestSearchTags = strTags;
                flickrItems = NoZebraFlickrAPI.FlickrFeed.GetFlickItems(strTags);
                Helpers.LatestSearchItem = flickrItems;
            }
            #endregion

            #region Sorting
            //Check if we have selected a sortorder
            if(!string.IsNullOrEmpty(orderBy))
            {
                if (orderBy == Helpers.SortByTypes.Title.ToString())
                    flickrItems = flickrItems.OrderBy(o => o.Title).ToList();
                else if (orderBy == Helpers.SortByTypes.Published.ToString())
                    flickrItems = flickrItems.OrderBy(o => o.Published).ToList();
            }

            //Initializing model and populate it with our flickr feed
            FlickrModel flickrModel = new FlickrModel();
            flickrModel.flickrFeed = flickrItems;
            #endregion

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(flickrModel.flickrFeed.ToPagedList(pageNumber, pageSize));
        }
    }
}