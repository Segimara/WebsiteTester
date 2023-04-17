using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebsiteTester.Domain.Models;

namespace WebsiteTester.Persistenсe.Configurations
{
    public class LinkTestResultConfiguration : IEntityTypeConfiguration<LinkTestResult>
    {
        public void Configure(EntityTypeBuilder<LinkTestResult> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Url)
                .IsRequired();

            builder.Property(x => x.RenderTimeMilliseconds)
                .IsRequired();

            builder.Property(x => x.IsInSitemap)
                .IsRequired();

            builder.Property(x => x.IsInWebsite)
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .IsRequired();

            builder.HasOne(x => x.Link)
                .WithMany(x => x.LinkTestResults)
                .HasForeignKey(x => x.LinkId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
