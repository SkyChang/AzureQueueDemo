using AzureQueueDemo.Core.Models;
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
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read()
        {
            //DBContextForRead db = new DBContextForRead();
            //UnitOfWorkForRead _uow = new UnitOfWorkForRead();
            
            //return View(_uow.Repository<Order>().Get());
            return View();
        
        }

	}
}