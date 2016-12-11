namespace Project.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Business")]
    public partial class Business
    {



        [Key]
        [MaxLength(64)]
        [Display(Name = "Mã nghiệp vụ")]
        [Column(TypeName = "varchar")]
        public string BusinessId { get; set; }

        [Required(ErrorMessage = "Hãy nhập tên nghiệp vụ")]
        [Display(Name = "Tên nghiệp vụ")]
        [MaxLength(256)]
        public string BusinessName { get; set; }       
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
