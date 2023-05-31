namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("NEWS")]
    public partial class NEWS
    {
        public NEWS() 
        {
            Image = "~/DataImg/images/SubImg/Add.png";
        }   
        [Key]
        public int IDNews { get; set; }

        [StringLength(1000),Display(Name = "Tiêu đề bài viết"), Required(ErrorMessage = "Nhập tiêu đề")]
        public string TieuDe { get; set; }
        public string UrlTitle { get; set; }


        [StringLength(1000), Display(Name = "Tóm tắt bài viết")]
        public string TomTat { get; set; }

        [StringLength(3000), Display(Name = "Giới thiệu bài viết")]
        public string GioiThieu { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Column(TypeName = "ntext"), Display(Name = "Nội dung bài viết"), Required(ErrorMessage = "Nhập nội dung")]
        public string NoiDung { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgaySua { get; set; }

        [Display(Name = "Trạng thái")]
        public bool AnHien { get; set; }

        [Display(Name = "Danh mục")]
        public int IDCateNews { get; set; }

        public virtual CATEGORY_NEWS CATEGORY_NEWS { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}
