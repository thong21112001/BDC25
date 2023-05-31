namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("TAGS_COMMUNITY")]
    public partial class TAGS_COMMUNITY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TAGS_COMMUNITY()
        {
            COMMUNITies = new HashSet<COMMUNITY>();
        }

        [Key]
        [Display(Name = "ID tag")]
        public int IDTagsCommunity { get; set; }


        [Display(Name = "Tên tag"), Required(ErrorMessage = "Nhập tên tag"), StringLength(100)]
        public string Ten { get; set; }

        public string UrlTitle { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgaySua { get; set; }

        [Display(Name = "Trạng thái")]
        public bool AnHien { get; set; }
        public bool StatusDel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMUNITY> COMMUNITies { get; set; }
    }
}
