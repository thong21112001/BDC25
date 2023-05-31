namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CATEGORY_NEWS")]
    public partial class CATEGORY_NEWS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CATEGORY_NEWS()
        {
            NEWS = new HashSet<NEWS>();
        }

        [Key]
        [Display(Name = "ID danh mục")]
        public int IDCateNews { get; set; }

        [Display(Name = "Tên danh mục"), Required(ErrorMessage = "Nhập tên danh mục"), StringLength(200)]
        public string Ten { get; set; }

        public string UrlTitle { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgaySua { get; set; }

        [Display(Name = "Trạng thái")]
        public bool AnHien { get; set; }

        public bool StatusDel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NEWS> NEWS { get; set; }
    }
}
