namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("COMLIKE")]
    public partial class COMLIKE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMLIKE()
        {
            USERWEBs = new HashSet<USERWEB>();
            COMMUNITies = new HashSet<COMMUNITY>();
        }

        [Key]
        public int IDLike { get; set; }
        public int IDUser { get; set; }
        public int IDCommunity { get; set; }
        public bool IsLike { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USERWEB> USERWEBs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMUNITY> COMMUNITies { get; set; }
    }
}
