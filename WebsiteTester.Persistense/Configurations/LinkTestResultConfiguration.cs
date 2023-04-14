using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebsiteTester.Domain;

namespace WebsiteTester.Persistense.Configurations
{
    public class LinkTestResultConfiguration : IEntityTypeConfiguration<LinkTestResult>
    {
        public void Configure(EntityTypeBuilder<LinkTestResult> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Url).IsRequired();
            builder.Property(x => x.RenderTimeMilliseconds).IsRequired();
            builder.Property(x => x.IsInSitemap).IsRequired();
            builder.Property(x => x.IsInWebsite).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.HasOne(x => x.TestedLink).WithMany(x => x.Links).HasForeignKey(x => x.TestedLinkId);
        }
    }
}
