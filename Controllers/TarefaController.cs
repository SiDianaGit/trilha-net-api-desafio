using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // TODO: Buscar o Id no banco utilizando o EF
            // TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            // caso contrário retornar OK com a tarefa encontrada
                var tarefaBD = _context.Tarefas.Find(id);

                if (tarefaBD == null)
                    return NotFound();

                return Ok(tarefaBD);

        }

        [HttpGet("ObterTodos")]
        public async Task<IActionResult> ObterTodos()
            {
                try
                {
                    var tarefas = await _context.Tarefas
                        //.Where(t => titulo == null || t.Titulo.Contains(titulo))
                        .OrderBy(t => t.Data)
                        .ToListAsync();

                    if (!tarefas.Any())
                    {
                        return NotFound();
                    }

                    return Ok(tarefas);
                }
                catch (Exception ex)
                {
                    // Logar a exceção ou retornar um erro personalizado
                    return StatusCode(500, "Erro ao buscar as tarefas");
                }
            }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            var tarefaBD = _context.Tarefas.Where(x => x.Titulo.Contains(titulo));

                if (tarefaBD == null)
                    return NotFound();

                return Ok(tarefaBD);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefaBD = _context.Tarefas.Where(x => x.Data.Date == data.Date);

            if (tarefaBD == null)
                return NotFound();
            
            return Ok(tarefaBD);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            var tarefaBD = _context.Tarefas.Where(x => x.Status == status);

            if (tarefaBD == null)
                return NotFound();

            return Ok(tarefaBD);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
          
            
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            _context.Add(tarefa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBD = _context.Tarefas.Find(id);

            if (tarefaBD == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            tarefaBD.Titulo = tarefa.Titulo;
            tarefaBD.Descricao = tarefa.Descricao;
            tarefaBD.Status = tarefa.Status;
            tarefaBD.Data = tarefa.Data;

            _context.Tarefas.Update(tarefaBD);
            _context.SaveChanges();

            return Ok(tarefaBD);

        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBD = _context.Tarefas.Find(id);

            if (tarefaBD == null)
                return NotFound();

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            _context.Tarefas.Remove(tarefaBD);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
