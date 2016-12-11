namespace Project.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrantPermission")]
    public partial class GrantPermission
    {

        [Key]
        [Column(Order = 1)]
        //[ForeignKey("Permission")]
        [Display(Name = "Mã quyền hạn")]
        [Required]
        public int PermissionId { get; set; }

        [Key]
        [Column(Order = 2)]
        //[ForeignKey("Administrator")]
        [Display(Name = "Mã người dùng")]
        [Required]
        public int UserId { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public virtual Administrator Administrator { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
