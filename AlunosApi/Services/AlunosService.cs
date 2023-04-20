using AlunosApi.Context;
using AlunosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlunosApi.Services {
    public class AlunosService : IAlunoService {

        private readonly AppDbContext _context;

        public AlunosService(AppDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Aluno>> GetAlunos() {
            try {
                return await _context.Alunos.ToListAsync();
            }
            catch {
                throw;
            };
        }
        public async Task<IEnumerable<Aluno>> GetAlunoByName(string nome) {
            IEnumerable<Aluno> alunos;
            if (!string.IsNullOrWhiteSpace(nome)) {
                alunos = await _context.Alunos.Where(i => i.Nome.Contains(nome)).ToListAsync();
            }
            else {
                alunos = await GetAlunos();
            }
            return alunos;
        }
        public async Task<Aluno> GetAluno(int id) {
            return await _context.Alunos.FindAsync(id);
        }


        public async Task CreateAluno(Aluno aluno) {
            _context.Add(aluno);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAluno(Aluno aluno) {
            _context.Entry(aluno).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAluno(Aluno aluno) {
            _context.Remove(aluno);
            await _context.SaveChangesAsync();
        }
    }
}
