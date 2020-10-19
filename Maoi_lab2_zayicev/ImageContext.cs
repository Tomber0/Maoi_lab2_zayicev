using Maoi_lab2_zayicev.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maoi_lab2_zayicev
{
    class ImageContext:DbContext
    {
        public ImageContext()  
        {
            Database.EnsureCreated();
        }
        public DbSet<StandartModel> Standart { get; set; }
        public DbSet<ImageLetterModel> Image { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-C97C4J73\\SQLEXPRESS;Database=Lab2;Trusted_Connection=True;");//LAPTOP-C97C4J73\\SQLEXPRESS
        }

    }
}
