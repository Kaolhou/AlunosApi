using AlunosApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace AlunosApi.Context {
    public class AppDbContext: IdentityDbContext<IdentityUser> {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Aluno> Alunos { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Aluno>().HasData(
                new Aluno() {
                    Id = 1,
                    Nome = "André",
                    Email = "craftmaster788@gmail.com",
                    Idade = 20
                },
                new Aluno() {
                    Id = 2,
                    Nome = "Pedro",
                    Idade = 18,
                    Email = "pedromrocha@gmail.com"
                }
            );
        }*/

    }
}
