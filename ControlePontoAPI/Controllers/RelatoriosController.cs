using ControlePontoAPI.Interfaces;
using ControlePontoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ControlePontoAPI.Controllers
{
    [ApiController]
    [Route("v1/folhas-de-ponto")]
    [ProducesResponseType(typeof(Relatorio), 200)]
    [ProducesResponseType(typeof(Mensagem), 404)]
    public class RelatoriosController : ControllerBase
    {
        private readonly IRegistroPontoRepository _registroRepository;
        private readonly IRelatorioHelper _relatorioHelper;
        
        public RelatoriosController(IRegistroPontoRepository registroRepository, IRelatorioHelper relatorioHelper)
        {
            _registroRepository = registroRepository;
            _relatorioHelper = relatorioHelper;
        }

        /// <summary>
        /// Relatório mensal
        /// </summary>
        /// <remarks>Geração de relatório mensal de usuário</remarks>
        /// <param name="mes">O campo "mes" deverá ser informado no formato "yyyy-MM"</param>
        /// <returns>Um <see cref="Relatorio"/> contendo os dados referentes ao mês informado</returns>
        [HttpGet("{mes}")]
        public IActionResult GeraRelatorioMensal([Required] string mes)
        {
            // Obtem os registros do mês utilizando o registroRepository
            List<Registro> registrosMes = _registroRepository.ObterRegistrosPorMes(mes);
            
            // Verifica se existem registros para o mês informado
            if (registrosMes.Count == 0)
            {
                return NotFound(new Mensagem { mensagem = "Relatório não encontrado" });
            }
            
            // Cria objeto Relatorio com os dados calculados
            var relatorio = _relatorioHelper.GeraRelatorioMensal(registrosMes);

            return Ok(relatorio);
        }
    }
}
