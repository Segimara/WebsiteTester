using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebsiteTester.Domain.Models;

namespace WebsiteTester.Persistance.Configurations
{
    public class LinkConfiguration : IEntityTypeConfiguration<Link>
    {
        public void Configure(EntityTypeBuilder<Link> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Url)
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasMany(x => x.LinkTestResults)
                .WithOne(x => x.Link)
                .HasForeignKey(x => x.LinkId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
