namespace Project.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Video
    {
     
        public long Id { get; set; }

        public string Name { get; set; }

        public string MetaTitle { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public long? CategoryID { get; set; }

        [StringLength(250)]
        public string Keyoutube { get; set; }

        public string Detail { get; set; }

        public int? Warranty { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescriptions { get; set; }

        public int? ViewCount { get; set; }

        public bool TopHot { get; set; }

        public bool Status { get; set; }

        public string Tags { get; set; }

        public bool? DeleteVideo { get; set; }

        public virtual Administrator Administrator { get; set; }

        public virtual Administrator Administrator1 { get; set; }

        public virtual CatVideo CatVideo { get; set; }
    }
}
