using AlunosApi.Models;

namespace AlunosApi.Services {
    public interface IAlunoService {

        Task<IEnumerable<Aluno>> GetAlunos();
        Task<IEnumerable<Aluno>> GetAlunoByName(string nome);

        Task<Aluno> GetAluno(int id);

        Task CreateAluno(Aluno aluno);

        Task UpdateAluno(Aluno aluno);
        Task DeleteAluno(Aluno aluno);


    }
}
