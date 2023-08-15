using alwatnia.Helper;
using alwatnia.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace alwatnia.Controllers
{
    public class SearchController : BaseController
    {
        private readonly DataModel _dataModel;

        public SearchController()
        {
            _dataModel = new DataModel();
        }

        [Route("Search")]
        public ActionResult Index(string query, SearchTypes type, int? page = 1)
        {
            try
            {
                var model = new SearchResultViewModel
                {
                    Query = query,
                    Type = type
                };

                var lang = Functions.GetLanguage();

                switch (type)
                {
                    case SearchTypes.News:
                        model.News = _dataModel.News
                            .Where(e => e.Lang == lang &&
                                        e.Title.Contains(query) ||
                                        e.Details.Contains(query))
                            .OrderByDescending(e => e.Id)
                            .Paginate(page);
                        break;
                    case SearchTypes.Products:
                        model.Products = _dataModel.Products
                            .Where(e => e.Lang == lang &&
                                        e.Title.Contains(query) ||
                                        e.Details.Contains(query))
                            .OrderByDescending(e => e.Id)
                            .Paginate(page);
                        break;
                    case SearchTypes.Activities:
                        model.Activities = _dataModel.Activities
                            .Where(e => e.Lang == lang &&
                                        e.Title.Contains(query) ||
                                        e.Details.Contains(query))
                            .OrderByDescending(e => e.Id)
                            .Paginate(page);
                        break;
                    default:
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
