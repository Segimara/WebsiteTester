using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebsiteTester.Domain.Models;

namespace WebsiteTester.Persistenсe.Configurations
{
    public class TestedLinkConfiguration : IEntityTypeConfiguration<TestedLink>
    {
        public void Configure(EntityTypeBuilder<TestedLink> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Url)
                .IsRequired();

            builder.HasMany(x => x.Links)
                .WithOne(x => x.TestedLink)
                .HasForeignKey(x => x.TestedLinkId);
        }
    }
}
