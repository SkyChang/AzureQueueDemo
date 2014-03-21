using AzureQueueDemo.Core.Models;
using AzureQueueDemo.Storage.Service;
using AzureQueueDemo.Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace AzureQueueDemo.UI.Web.Controllers.Api
{
    public class OrderController : ApiController
    {
        private IQueueSevice _queueService;

        public OrderController(IQueueSevice s)
        {
            _queueService = s;
        }

        public OrderController()
            :this(new QueueService())
        {

        }

        // GET api/order
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/order/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/order
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order value)
        {
            _queueService.SaveQueue(value);
            return CreatedAtRoute("DefaultApi", new { id = value.Id }, value);
        
        }
        // PUT api/order/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/order/5
        public void Delete(int id)
        {
        }
    }
}
