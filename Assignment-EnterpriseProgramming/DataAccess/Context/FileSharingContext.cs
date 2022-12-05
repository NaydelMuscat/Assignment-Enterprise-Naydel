using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Context
{
    public class FileSharingContext: IdentityDbContext
    {
        public FileSharingContext(DbContextOptions<FileSharingContext> options)
                : base(options)
        {
        }

        public DbSet<Acl> Acls { get; set; }
        public DbSet<TextFile> TextFiles { get; set; }
    }
}
