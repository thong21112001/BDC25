namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("DONATE")]
    public partial class DONATE
    {
        [Key]
        public int IDDonate { get; set; }

        [StringLength(500)]
        public string TieuDe { get; set; }

        [StringLength(1000)]
        public string UrlTitle { get; set; }

        [StringLength(500)]
        public string File { get; set; }

        [StringLength(500)]
        public string Type { get; set; }

        public bool AnHien { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgaySua { get; set; }

        public IEnumerable<HttpPostedFileBase> files { get; set; }
    }
}
