using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using todolistv1.Models;

namespace todolistv1.Data
{
    public class todolistv1Context : DbContext
    {
        public todolistv1Context()
        {
        }
        public todolistv1Context (DbContextOptions<todolistv1Context> options)
            : base(options)
        {
        }

        public DbSet<todolistv1.Models.User> User { get; set; }

        public DbSet<todolistv1.Models.Todo> Todo { get; set; }
    }
}
