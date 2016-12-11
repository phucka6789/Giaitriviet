namespace Project.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Permission")]
    public partial class Permission
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }

        [Required(ErrorMessage = "Hãy nhập tên quyền hạn")]
        [Display(Name = "Tền quyền")]
        [Column(TypeName = "varchar")]
        [MaxLength(250)]
        public string PermissionName { get; set; }

        [Required(ErrorMessage = "Hãy nhập mô tả quyền hạn")]
        [Display(Name = "Mô tả")]
        [MaxLength(256)]
        public string Description { get; set; }

        [Required()]
        [MaxLength(64)]
        [Display(Name = "Mã nghiệp vụ")]
        //[ForeignKey("BlogBusiness")]
        [Column(TypeName = "varchar")]
        public string BusinessId { get; set; }

        public virtual Business Business { get; set; }
      
        public virtual ICollection<GrantPermission> GrantPermissions { get; set; }
    }
}
