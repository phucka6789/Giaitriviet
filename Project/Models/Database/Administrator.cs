namespace Project.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Administrator")]
    public partial class Administrator
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }


        [Required(ErrorMessage = "Hãy nhập tên người dùng")]
        [StringLength(64, ErrorMessage = "Tên đăng nhập có khoảng từ 3-64 ký tự", MinimumLength = 3)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }



        [Required(ErrorMessage = "Hãy nhập mật khẩu")]
        [MaxLength(256)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        [Required(ErrorMessage = "Hãy nhập họ và tên")]
        [Display(Name = "Họ và tên")]
        [MaxLength(64)]  
        public string FullName { get; set; }



        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Column(TypeName = "varchar")]
        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }



        [Display(Name = "Ảnh đại diện")]
        [Column(TypeName = "varchar")]
        [MaxLength(256)]
        public string Avatar { get; set; }

        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }


        [Display(Name = "Là quản trị")]
        public byte? isAdmin { get; set; }

        [Display(Name = "Kích hoạt")]
        public byte? Allowed { get; set; }


   
        public virtual ICollection<CatNew> CatNews { get; set; }     
        public virtual ICollection<CatNew> CatNews1 { get; set; }    
        public virtual ICollection<CatVideo> CatVideos { get; set; }      
        public virtual ICollection<CatVideo> CatVideos1 { get; set; }       
        public virtual ICollection<ContenNew> ContenNews { get; set; }      
        public virtual ICollection<ContenNew> ContenNews1 { get; set; }      
        public virtual ICollection<GrantPermission> GrantPermissions { get; set; }        
        public virtual ICollection<Video> Videos { get; set; }      
        public virtual ICollection<Video> Videos1 { get; set; }
    }
}
