namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COMMENT")]
    public partial class COMMENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMMENT()
        {
            COMMENT_REPLY = new HashSet<COMMENT_REPLY>();
        }

        [Key]
        public int IDComment { get; set; }

        public string NoiDung { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgaySua { get; set; }

        public int? IDCommunity { get; set; }

        public int? IDUser { get; set; }

        public bool AnHien { get; set; }

        public bool IsReport { get; set; }

        public virtual COMMUNITY COMMUNITY { get; set; }

        public virtual USERWEB USERWEB { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENT_REPLY> COMMENT_REPLY { get; set; }
    }
}
