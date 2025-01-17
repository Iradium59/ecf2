using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ecf2.Models;

namespace ecf2.Data
{
    public class ecf2Context : DbContext
    {
        public ecf2Context (DbContextOptions<ecf2Context> options)
            : base(options)
        {
        }

        public DbSet<ecf2.Models.Evenement> Evenement { get; set; } = default!;
        public DbSet<ecf2.Models.Participant> Participant { get; set; } = default!;
    }
}
