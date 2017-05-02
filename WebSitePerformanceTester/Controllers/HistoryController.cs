using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebSitePerformanceTester.Services;

namespace WebSitePerformanceTester.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IUrlHIstoryService _service;
        public HistoryController(IUrlHIstoryService service)
        {
            _service = service;
        }
        // GET: History
        public async Task<ActionResult> Index()
        {
            //var viewModel = new HistoryViewModel();
            //var service = new UrlHIstoryService();
            //viewModel.HistoryDtos = service.GetModel();
            //return View(viewModel);
            var viewModel = new ViewModel {List = await _service.GetURls()};
            return View(viewModel);
        }

    }

    public class ViewModel
    {
        public List<string> List { get; set; }
    }
}