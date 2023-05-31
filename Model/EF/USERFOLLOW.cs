namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("USERFOLLOW")]
    public partial class USERFOLLOW
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USERFOLLOW()
        {

        }

        [Key]
        public int IDFollow { get; set; }
        public int IDUser { get; set; }
        public int IDUserFollow { get; set; }
        public bool IsFollow { get; set; }
        public DateTime? NgayFollow { get; set; }
    }
}
