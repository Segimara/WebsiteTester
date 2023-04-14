using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebsiteTester.Domain;

namespace WebsiteTester.Persistenсe.Configurations
{
    public class TestedLinkConfiguration : IEntityTypeConfiguration<TestedLink>
    {
        public void Configure(EntityTypeBuilder<TestedLink> builder)
        {
            builder.HasKey(x => x.Url);

            builder.Property(x => x.Url)
                .IsRequired();

            builder.HasMany(x => x.Links)
                .WithOne(x => x.TestedLink)
                .HasForeignKey(x => x.TestedLinkId);
        }
    }
}
