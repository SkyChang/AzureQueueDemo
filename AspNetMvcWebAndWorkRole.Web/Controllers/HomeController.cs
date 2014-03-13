using AspNetMvcWebAndWorkRole.Web.Interface;
using AspNetMvcWebAndWorkRole.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AspNetMvcWebAndWorkRole.Web.Controllers
{
    public class HomeController : Controller
    {
        IUnitOfWork _uow;
        QueueHelper _queueHelp;

        public HomeController()
        {
            _uow = new UnitOfWork();
            _queueHelp = new QueueHelper(_uow);
        }

        public HomeController(IUnitOfWork uow)
        {
            _uow = uow;
            _queueHelp = new QueueHelper(_uow);
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> Create([Bind(Include = "Name")]Order order, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                await _queueHelp.SendMessageAsync(order);
                return RedirectToAction("Index");
            }
            return View(order);
        }
	}
}