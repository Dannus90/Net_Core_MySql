using System;
using Microsoft.EntityFrameworkCore;
using QuotesApi.Models;

namespace QuotesApi.Data
{
  public class QuotesDbContext : DbContext
  {
    public QuotesDbContext(DbContextOptions<QuotesDbContext>options):base(options)
    {

    }

    //Will create table named as Quotes
    public DbSet<Quote> Quotes { get; set; }
  }
}