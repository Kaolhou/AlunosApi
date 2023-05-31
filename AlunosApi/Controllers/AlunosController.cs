using AlunosApi.Models;
using AlunosApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlunosApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AlunosController : ControllerBase {

        private IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService) {
            _alunoService = alunoService;
        }

        [HttpGet("aluno")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos() {
            try {
                var alunos = await _alunoService.GetAlunos();
                return Ok(alunos);
            } catch {
                Console.WriteLine("pindamonhangaba");
                return StatusCode(StatusCodes.Status500InternalServerError,"Erro ao obter lista de alunos");
            }
        }

        [HttpGet("aluno/name")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunosByName([FromQuery] string nome) {
            try {
                var alunos = await _alunoService.GetAlunoByName(nome);

                if (alunos == null)
                    return NotFound($"Não existem alunos com o nome {nome}");

                return Ok(alunos);
            } catch {
                return StatusCode(StatusCodes.Status500InternalServerError,"Não foi possível obter a lista de alunos");
            }
        }

        [HttpGet("aluno/{id:int}", Name = "GetAluno")]
        public async Task<ActionResult<Aluno>> GetAluno(int id) {
            try {
                var aluno = await _alunoService.GetAluno(id);

                if (aluno == null)
                    return NotFound($"Não existe aluno com o id {id}");

                return Ok(aluno);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi procurar pelo aluno");
            }
        }

        [HttpPost("aluno")]
        public async Task<ActionResult> Create(Aluno aluno) {
            try {
                await _alunoService.CreateAluno(aluno);
                return CreatedAtRoute(nameof(GetAluno),new {id = aluno.Id}, aluno);

            } catch {
                return BadRequest("Request Inválido");
            }
        }

        [HttpPut("aluno/{id:int}")]
        public async Task<ActionResult> Edit(int id, [FromBody] Aluno aluno) {
            try {
                if(aluno.Id == id) {
                    await _alunoService.UpdateAluno(aluno);
                    return Ok($"Aluno com id ${id} foi atualizado com sucesso");
                }
                return BadRequest("Dados Inconsistentes");
            } catch {
                return BadRequest("Request Inválido");
            }
        }

        [HttpDelete("aluno/{id:int}")]
        public async Task<ActionResult> Delete(int id) {
            try {
                var aluno = await _alunoService.GetAluno(id);
                if (aluno != null) {
                    await _alunoService.DeleteAluno(aluno);
                    return Ok($"Aluno com id ${id} foi deletado com sucesso");
                }
                return NotFound($"Aluno com id ${id} não encontrado");
            }
            catch {
                return BadRequest("Request Inválido");
            }
        }

    }
}
