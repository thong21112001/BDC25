namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("COMMUNITY")]
    public partial class COMMUNITY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMMUNITY()
        {
            Image = "~/DataImg/images/SubImg/Add.png";
        }

        [Key]
        public int IDCommunity { get; set; }

        [StringLength(100)]
        public string TieuDe { get; set; }

        public string UrlTitle { get; set; }

        public string TomTat { get; set; }

        public string GioiThieu { get; set; }

        public string Image { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgaySua { get; set; }

        [Column(TypeName = "ntext")]
        public string NoiDung { get; set; }

        public long? DanhGia { get; set; }

        public int IDTagsCommunity { get; set; }

        public int IDUser { get; set; }

        public bool AnHien { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENT> COMMENTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMLIKE> COMLIKEs { get; set; }

        public virtual TAGS_COMMUNITY TAGS_COMMUNITY { get; set; }

        public virtual USERWEB USERWEB { get; set; }
    }
}
