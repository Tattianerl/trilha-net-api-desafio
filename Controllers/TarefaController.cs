using Microsoft.AspNetCore.Mvc;
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
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
            {
                return NotFound();
            }
            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefas = _context.Tarefas.ToList();

            if (tarefas == null || tarefas.Count == 0)
            {
                return NotFound("Nenhuma tarefa encontrada");
            }
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefas = _context.Tarefas.Where(x => x.Titulo.Contains(titulo));
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            // Busca todas as tarefas cuja data seja igual à data especificada (apenas a parte da data)
            var tarefas = _context.Tarefas.Where(x => x.Data.Date == data.Date).ToList();

            // Verifica se alguma tarefa foi encontrada
            if (tarefas == null || !tarefas.Any())
                return NotFound(new { Erro = "Nenhuma tarefa encontrada para a data especificada." });

            // Retorna a lista de tarefas
            return Ok(tarefas);
        }


        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas.Where(x => x.Status == status);

            if (tarefa == null || !tarefa.Any())
                return NotFound(new { Erro = "Nenhuma tarefa encontrada." });

            return Ok(tarefa);
        }




        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            // Definir a data e hora atual como o valor do campo Data
            tarefa.Data = DateTime.Now;

            _context.Add(tarefa);
            _context.SaveChanges();

            // Retorna o código 201 Created com a tarefa recém-criada
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }



        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return NoContent();
        }
    }
}



