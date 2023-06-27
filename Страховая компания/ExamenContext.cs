using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Страховая_компания.DataBase;

namespace Страховая_компания;

public partial class ExamenContext : DbContext
{
    public ExamenContext()
    {
        Database.EnsureCreated();
    }

    //public ExamenContext(DbContextOptions<ExamenContext> options)
    //    : base(options)
    //{
    //}

    public virtual DbSet<Clients> client { get; set; }
    public virtual DbSet<Employees> employee { get; set; }
    public virtual DbSet<InsurancePayments> insurancepayment { get; set; }
    public virtual DbSet<ObjectsOfInsurance> objectofinsurance { get; set; }
    public virtual DbSet<Treaty> treaty { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("host=localhost;port=5432;database=examen;username=postgres;password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
