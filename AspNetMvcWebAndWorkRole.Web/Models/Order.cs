using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspNetMvcWebAndWorkRole.Web.Models
{
    public class Order
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>=
        /// 主題
        /// </summary>
        [DisplayName("主題")]
        [StringLength(50)]
        public string Name { get; set; }
    }
}