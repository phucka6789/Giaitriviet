using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Areas.Backends.Models
{
    public class ArticleCatViewModel
    {
        public long ID { get; set; }
        public String Name { get; set; }
        public int? CatDepth { get; set; }
    }
}