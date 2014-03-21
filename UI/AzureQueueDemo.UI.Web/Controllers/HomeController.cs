using AzureQueueDemo.Core.Models;
using AzureQueueDemo.Repository.Data;
using AzureQueueDemo.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzureQueueDemo.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork _uowForRead;

        public HomeController(UnitOfWorkForRead s)
        {
            _uowForRead = s;
        }

        public HomeController()
            : this(new UnitOfWorkForRead())
        {

        }

        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read()
        {
            return View(_uowForRead.Repository<Order>().Get());
        }

	}
}