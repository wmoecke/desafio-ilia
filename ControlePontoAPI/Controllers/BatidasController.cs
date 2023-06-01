using ControlePontoAPI.Interfaces;
using ControlePontoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ControlePontoAPI.Controllers
{
    [ApiController]
    [Route("v1/batidas")]
    [ProducesResponseType(typeof(Registro), 201)]
    [ProducesResponseType(typeof(Mensagem), 400)]
    [ProducesResponseType(typeof(Mensagem), 403)]
    [ProducesResponseType(typeof(Mensagem), 409)]
    public class BatidasController : ControllerBase
    {
        private readonly IRegistroPontoRepository _registroRepository;
        private const int UMA_HORA = 60;

        public BatidasController(IRegistroPontoRepository registroRepository)
        {
            _registroRepository = registroRepository;
        }

        /// <summary>
        /// Bater ponto
        /// </summary>
        /// <remarks>Registrar um horário da jornada diária de trabalho</remarks>
        /// <param name="momento">O campo <see cref="Momento.dataHora"/> deverá ser informado no formato "yyyy-MM-ddTHH:mm:ss"</param>
        /// <returns>Um <see cref="Registro"/> atualizado com o registro de ponto informado</returns>
        [HttpPost]
        public IActionResult InsereBatida([FromBody] Momento momento)
        {
            // Verifica se a data e hora foi informada
            if (momento == null || string.IsNullOrEmpty(momento.dataHora))
            {
                return BadRequest(new Mensagem { mensagem = "Campo obrigatório não informado" });
            }

            // Verifica se o formato de data e hora é válido
            if (!DateTime.TryParseExact(momento.dataHora, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataHora))
            {
                return BadRequest(new Mensagem { mensagem = "Data e hora em formato inválido" });
            }

            // Verifica se sábado ou domingo
            if (dataHora.DayOfWeek == DayOfWeek.Saturday || dataHora.DayOfWeek == DayOfWeek.Sunday)
            {
                return StatusCode(403, new Mensagem { mensagem = "Sábado e domingo não são permitidos como dia de trabalho" });
            }

            // Obtem o objeto Registro correspondente ao dia
            var registros = _registroRepository.ObterRegistros();
            var registroDia = registros?.FirstOrDefault(r => r.dia == dataHora.Date.ToString("yyyy-MM-dd"));

            // Verifica se já existem quatro horários registrados para o dia
            if (registroDia != null && registroDia.horarios.Count == 4)
            {
                return StatusCode(403, new Mensagem { mensagem = "Apenas 4 horários podem ser registrados por dia" });
            }

            if (registroDia != null && registroDia.horarios.Count == 2)
            {
                var saidaAlmoco = DateTime.ParseExact(registroDia.horarios[1], "HH:mm:ss", CultureInfo.InvariantCulture);
                if (dataHora.TimeOfDay.Subtract(saidaAlmoco.TimeOfDay).TotalMinutes < UMA_HORA)
                {
                    return StatusCode(403, new Mensagem { mensagem = "Deve haver no mínimo 1 hora de almoço" });
                }
            }

            // Verifica se já existe um horário registrado para o dia
            if (registroDia != null && registroDia.horarios.Any(horario => horario.Contains(dataHora.TimeOfDay.ToString())))
            {
                return StatusCode(409, new Mensagem { mensagem = "Horário já registrado" });
            }

            Registro novoRegistro;

            if (registroDia != null)
            {
                // Atualiza o Registro existente
                registroDia.horarios.Add(dataHora.TimeOfDay.ToString());
                novoRegistro = registroDia;
            }
            else
            {
                // Cria um novo objeto Registro
                novoRegistro = new Registro
                {
                    dia = dataHora.Date.ToString("yyyy-MM-dd"),
                    horarios = new List<string> { dataHora.TimeOfDay.ToString() }
                };

                // Adiciona o registro utilizando o registroRepository
                _registroRepository.AdicionarRegistro(novoRegistro);
            }

            return Created("", novoRegistro);
        }
    }
}
