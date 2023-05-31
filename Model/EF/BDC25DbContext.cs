using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Model.EF
{
    public partial class BDC25DbContext : DbContext
    {
        public BDC25DbContext()
            : base("name=BDC25DbContext")
        {
        }

        public virtual DbSet<ADMINWEB> ADMINWEBs { get; set; }
        public virtual DbSet<CATEGORY_NEWS> CATEGORY_NEWS { get; set; }
        public virtual DbSet<COMMENT> COMMENTs { get; set; }
        public virtual DbSet<COMMENT_REPLY> COMMENT_REPLY { get; set; }
        public virtual DbSet<COMMUNITY> COMMUNITies { get; set; }
        public virtual DbSet<NEWS> NEWS { get; set; }
        public virtual DbSet<TAGS_COMMUNITY> TAGS_COMMUNITY { get; set; }
        public virtual DbSet<USERWEB> USERWEBs { get; set; }
        public virtual DbSet<COMLIKE> COMLIKEs { get; set; }
        public virtual DbSet<DONATE> DONATEs { get; set; }
        public virtual DbSet<USERFOLLOW> USERFOLLOWs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
