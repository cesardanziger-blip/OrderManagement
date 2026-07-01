using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Configurations
{
    public class OrderHistoryConfiguration : IEntityTypeConfiguration<OrderHistory>
    {
        public void Configure(EntityTypeBuilder<OrderHistory> builder)
        {
            builder.ToTable("OrderHistories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderId)
                .IsRequired();

            builder.Property(x => x.PreviousStatus)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.NewStatus)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.ModificationDate)
                .IsRequired();

            builder.Property(x => x.Reason)
                .HasMaxLength(500);

            builder.HasIndex(x => x.OrderId);
        }
    }
}
