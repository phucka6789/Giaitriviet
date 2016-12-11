namespace Project.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CatNew")]
    public partial class CatNew
    {
       
        public long ID { get; set; }

        public string Name { get; set; }

        public string MetaTitle { get; set; }

        public long? ParentID { get; set; }

        public int? DisplayOrder { get; set; }

        public string SeoTitle { get; set; }

        public int CatDepth { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescriptions { get; set; }

        public bool Status { get; set; }

        public bool? DeleteCateVideo { get; set; }

        public virtual Administrator Administrator { get; set; }
        public virtual Administrator Administrator1 { get; set; }      
        public virtual ICollection<ContenNew> ContenNews { get; set; }
    }
}
