using ControlePontoAPI.Interfaces;
using ControlePontoAPI.Models;

namespace ControlePontoAPI.Helpers
{
    /**********
     * Classe RelatorioHelper
     * Implementa os métodos da interface IRelatorioHelper
     * Responsável pela geração do Relatório Mensal
     */
    public class RelatorioHelper : IRelatorioHelper
    {
        private const int SESSENTA = 60;

        /**********
         * Efetua os cálculos referentes aos registros de ponto informados e
         * gera um Relatorio contendo os resultados no formato ISO 8601
         */
        public Relatorio GeraRelatorioMensal(List<Registro> registrosMes)
        {
            // Cálculo de horas trabalhadas, horas excedentes e horas devidas
            var horasTrabalhadas = TimeSpan.Zero;
            var horasExcedentes = TimeSpan.Zero;
            var horasDevidas = TimeSpan.Zero;

            foreach (Registro registro in registrosMes)
            {
                var totalHorasDia = TimeSpan.Zero;

                if (registro.horarios.Count >= 2) // Considerando pelo menos 2 registros de ponto por dia
                {                                 // Caso haja 2 ou 3 registros, consideramos apenas os 2 primeiros
                    var entrada = TimeSpan.Parse(registro.horarios[0]);
                    var saidaAlmoco = TimeSpan.Parse(registro.horarios[1]);
                    totalHorasDia += saidaAlmoco - entrada; // Horas até a saída para o almoço
                }

                if (registro.horarios.Count == 4) // Considerando 4 registros de ponto por dia
                {
                    var retornoAlmoco = TimeSpan.Parse(registro.horarios[2]);
                    var saida = TimeSpan.Parse(registro.horarios[3]);
                    totalHorasDia += saida - retornoAlmoco; // Horas após o retorno do almoço
                }

                horasTrabalhadas += totalHorasDia;
                horasExcedentes += totalHorasDia - TimeSpan.FromHours(8); // Considerando 8 horas diárias de trabalho
                horasDevidas += TimeSpan.FromHours(8) - totalHorasDia;    // Considerando 8 horas diárias de trabalho
            }

            // Cria objeto Relatorio com os dados calculados
            var relatorio = new Relatorio
            {
                mes = registrosMes[0].dia.Substring(0, 7),
                horasTrabalhadas = $"PT{((int)horasTrabalhadas.TotalHours > 0 ? (int)horasTrabalhadas.TotalHours : 0)}H{((int)horasTrabalhadas.TotalMinutes > 0 ? (int)horasTrabalhadas.TotalMinutes : 0) % SESSENTA}M{((int)horasTrabalhadas.TotalSeconds > 0 ? (int)horasTrabalhadas.TotalSeconds : 0) % SESSENTA}S",
                horasExcedentes = $"PT{((int)horasExcedentes.TotalHours > 0 ? (int)horasExcedentes.TotalHours : 0)}H{((int)horasExcedentes.TotalMinutes > 0 ? (int)horasExcedentes.TotalMinutes : 0) % SESSENTA}M{((int)horasExcedentes.TotalSeconds > 0 ? (int)horasExcedentes.TotalSeconds : 0) % SESSENTA}S",
                horasDevidas = $"PT{((int)horasDevidas.TotalHours > 0 ? (int)horasDevidas.TotalHours : 0)}H{((int)horasDevidas.TotalMinutes > 0 ? (int)horasDevidas.TotalMinutes : 0) % SESSENTA}M{((int)horasDevidas.TotalSeconds > 0 ? (int)horasDevidas.TotalSeconds : 0) % SESSENTA}S",
                registros = registrosMes
            };

            return relatorio;
        }
    }
}
