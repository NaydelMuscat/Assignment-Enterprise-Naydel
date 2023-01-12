using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.context
{
    public class FileSharingContext : IdentityDbContext<CustomUsers>
    {
        public FileSharingContext(DbContextOptions<FileSharingContext> options)
            : base(options)
        {
        }
        public DbSet<TextFile> TextFiles { get; set; }
        public DbSet<Acl> Acls { get; set; }
        public DbSet<Log> Logs { get; set; }

        
    }
}