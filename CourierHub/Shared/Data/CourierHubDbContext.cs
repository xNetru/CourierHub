using CourierHub.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CourierHub.Shared.Data;

public partial class CourierHubDbContext : DbContext {
    public CourierHubDbContext() { }

    public CourierHubDbContext(DbContextOptions<CourierHubDbContext> options) : base(options) { }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<ClientData> ClientDatum { get; set; }

    public virtual DbSet<Evaluation> Evaluations { get; set; }

    public virtual DbSet<Inquire> Inquires { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Parcel> Parcels { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<Scaler> Scalers { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        string? connection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DefaultConnection"];
        optionsBuilder.UseSqlServer(connection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Address>(entity => {
            entity.ToTable("Address");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Flat)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Number)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PostalCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Postal_code");
            entity.Property(e => e.Street).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
        });

        modelBuilder.Entity<ClientData>(entity => {
            entity.HasKey(e => e.Id).HasName("PK_Client_data");

            entity.ToTable("Client_data");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.AddressId).HasColumnName("Address_Id");
            entity.Property(e => e.Company).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Photo).HasColumnType("image");
            entity.Property(e => e.SourceAddressId).HasColumnName("Source_address_Id");
            entity.Property(e => e.ClientId).HasColumnName("Client_Id");

            entity.HasOne(d => d.Address).WithMany(p => p.ClientDatumAddresses)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_data_Address1");

            entity.HasOne(d => d.SourceAddress).WithMany(p => p.ClientDatumSourceAddresses)
                .HasForeignKey(d => d.SourceAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_data_Address2");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientData)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Client_data_User");
        });

        modelBuilder.Entity<Evaluation>(entity => {
            entity.ToTable("Evaluation");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Datetime).HasColumnType("datetime");
            entity.Property(e => e.RejectionReason)
                .HasColumnType("ntext")
                .HasColumnName("Rejection_reason");
            entity.Property(e => e.OfficeWorkerId).HasColumnName("Worker_Id");

            entity.HasOne(d => d.OfficeWorker).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.OfficeWorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evaluation_User");
        });

        modelBuilder.Entity<Inquire>(entity => {
            entity.ToTable("Inquire");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Datetime).HasColumnType("datetime");
            entity.Property(e => e.DestinationDate)
                .HasColumnType("date")
                .HasColumnName("Destination_date");
            entity.Property(e => e.DestinationId).HasColumnName("Destination_Id");
            entity.Property(e => e.SourceDate)
                .HasColumnType("date")
                .HasColumnName("Source_date");
            entity.Property(e => e.SourceId).HasColumnName("Source_Id");
            entity.Property(e => e.ClientId).HasColumnName("Client_Id");
            entity.Property(e => e.Code).HasMaxLength(50);

            entity.HasOne(d => d.Destination).WithMany(p => p.InquireDestinations)
                .HasForeignKey(d => d.DestinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inquire_Address1");

            entity.HasOne(d => d.Source).WithMany(p => p.InquireSources)
                .HasForeignKey(d => d.SourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inquire_Address");

            entity.HasOne(d => d.Client).WithMany(p => p.Inquires)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Inquire_User");
        });

        modelBuilder.Entity<Order>(entity => {
            entity.ToTable("Order");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ClientAddressId).HasColumnName("Client_Address_Id");
            entity.Property(e => e.ClientCompany)
                .HasMaxLength(50)
                .HasColumnName("Client_Company");
            entity.Property(e => e.ClientEmail)
                .HasMaxLength(50)
                .HasColumnName("Client_Email");
            entity.Property(e => e.ClientName)
                .HasMaxLength(50)
                .HasColumnName("Client_Name");
            entity.Property(e => e.ClientPhone)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Client_Phone");
            entity.Property(e => e.ClientSurname)
                .HasMaxLength(50)
                .HasColumnName("Client_Surname");
            entity.Property(e => e.EvaluationId).HasColumnName("Evaluation_Id");
            entity.Property(e => e.InquireId).HasColumnName("Inquire_Id");
            entity.Property(e => e.ParcelId).HasColumnName("Parcel_Id");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ReviewId).HasColumnName("Review_Id");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.StatusId).HasColumnName("Status_Id");

            entity.HasOne(d => d.ClientAddress).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Address");

            entity.HasOne(d => d.Evaluation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EvaluationId)
                .HasConstraintName("FK_Order_Evaluation");

            entity.HasOne(d => d.Inquire).WithMany(p => p.Orders)
                .HasForeignKey(d => d.InquireId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Inquire");

            entity.HasOne(d => d.Parcel).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ParcelId)
                .HasConstraintName("FK_Order_Parcel");

            entity.HasOne(d => d.Review).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ReviewId)
                .HasConstraintName("FK_Order_Review");

            entity.HasOne(d => d.Service).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Service");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Status");
        });

        modelBuilder.Entity<Parcel>(entity => {
            entity.ToTable("Parcel");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.DeliveryDatetime)
                .HasColumnType("datetime")
                .HasColumnName("Delivery_datetime");
            entity.Property(e => e.PickupDatetime)
                .HasColumnType("datetime")
                .HasColumnName("Pickup_datetime");
            entity.Property(e => e.UndeliveredReason)
                .HasColumnType("ntext")
                .HasColumnName("Undelivered_reason");
            entity.Property(e => e.CourierId).HasColumnName("Courier_Id");

            entity.HasOne(d => d.Courier).WithMany(p => p.Parcels)
                .HasForeignKey(d => d.CourierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parcel_User");
        });

        modelBuilder.Entity<Review>(entity => {
            entity.ToTable("Review");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Datetime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
        });

        modelBuilder.Entity<Rule>(entity => {
            entity.ToTable("Rule");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DepthMax).HasColumnName("Depth_max");
            entity.Property(e => e.LengthMax).HasColumnName("Length_max");
            entity.Property(e => e.MassMax).HasColumnName("Mass_max");
            entity.Property(e => e.VelocityMax).HasColumnName("Velocity_max");
            entity.Property(e => e.WidthMax).HasColumnName("Width_max");
        });

        modelBuilder.Entity<Scaler>(entity => {
            entity.ToTable("Scaler");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Company).HasColumnType("money");
            entity.Property(e => e.Depth).HasColumnType("money");
            entity.Property(e => e.Distance).HasColumnType("money");
            entity.Property(e => e.Fee).HasColumnType("money");
            entity.Property(e => e.Length).HasColumnType("money");
            entity.Property(e => e.Mass).HasColumnType("money");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Priority).HasColumnType("money");
            entity.Property(e => e.Time).HasColumnType("money");
            entity.Property(e => e.Weekend).HasColumnType("money");
            entity.Property(e => e.Width).HasColumnType("money");
        });

        modelBuilder.Entity<Service>(entity => {
            entity.ToTable("Service");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ApiKey)
                .HasMaxLength(100)
                .HasColumnName("Api_key");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Statute).HasMaxLength(100);
        });

        modelBuilder.Entity<Status>(entity => {
            entity.ToTable("Status");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity => {
            entity.HasKey(e => e.Id).HasName("PK_User");

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(50);

        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
