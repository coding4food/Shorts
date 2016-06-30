using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shorts.Domain
{
    public class ShortsContext: DbContext
    {
        public virtual DbSet<ShortUrl> ShortUrl { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShortUrl>()
                .Property(_ => _.Url).IsRequired();
        }
    }
}
