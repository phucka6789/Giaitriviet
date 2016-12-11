using Project.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.BUS
{
    public class ArticleCatManager
    {
        private Entitys db = new Entitys();
        public int? GetMaxOrder(int id)
        {
            int? po;
            if (id != 0)
                po = (from c in db.CatNews where c.ParentID == id && c.DeleteCateVideo == false select c.DisplayOrder).Max();
            else
                po = (from c in db.CatNews where c.ParentID == null && c.DeleteCateVideo == false select c.DisplayOrder).Max();

            if (po != null)
                return po;
            else
                return 0;
        }
        public int? GetMaxOrderVideo(int id)
        {
            int? po;
            if (id != 0)
                po = (from c in db.CatVideos where c.ParentID == id && c.DeleteCateVideo == false select c.DisplayOrder).Max();
            else
                po = (from c in db.CatVideos where c.ParentID == null && c.DeleteCateVideo == false select c.DisplayOrder).Max();

            if (po != null)
                return po;
            else
                return 0;
        }

    }
}