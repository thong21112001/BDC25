namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class COMMENT_REPLY
    {
        [Key]
        public int IDCommentReply { get; set; }

        public string NoiDung { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgaySua { get; set; }

        public int? IDUser { get; set; }

        public int? IDComment { get; set; }

        public bool AnHien { get; set; }

        public bool IsReport { get; set; }

        public virtual COMMENT COMMENT { get; set; }

        public virtual USERWEB USERWEB { get; set; }
    }
}
