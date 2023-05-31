namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("USERWEB")]
    public partial class USERWEB
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USERWEB()
        {
            COMMENTs = new HashSet<COMMENT>();
            COMMENT_REPLY = new HashSet<COMMENT_REPLY>();
            COMMUNITies = new HashSet<COMMUNITY>();
            COMLIKEs= new HashSet<COMLIKE>();
            Image = "~/DataImg/images/SubImg/Add.png";
        }

        [Key]
        public int IDUser { get; set; }


        [StringLength(50),Display(Name = "Tài khoản")]
        public string TaiKhoan { get; set; }

        [StringLength(50)]
        public string MatKhau { get; set; }

        [StringLength(200), Display(Name = "Họ và Tên")]
        public string HoTen { get; set; }

        [StringLength(50), Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100), Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [StringLength(10), Display(Name = "Số điện thoại")]
        public string DienThoai { get; set; }

        public string Image { get; set; }

        [Display(Name = "Ngày tạo tài khoản")]
        public DateTime? NgayTao { get; set; }

        [Display(Name = "Ngày sửa tài khoản")]
        public DateTime? NgaySua { get; set; }

        [Display(Name = "Trạng thái")]
        public bool AnHien { get; set; }

        [Display(Name = "Ngày khóa")]
        public DateTime? NgayKhoa { get; set; }

        [Display(Name = "Ngày mở khóa")]
        public DateTime? NgayMoKhoa { get; set; }

        [Display(Name = "Số lần khóa tài khoản")]
        public int SoLanKhoa { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENT> COMMENTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENT_REPLY> COMMENT_REPLY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMUNITY> COMMUNITies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMLIKE> COMLIKEs { get; set; }
    }
}
