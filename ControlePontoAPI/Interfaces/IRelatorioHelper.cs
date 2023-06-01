using ControlePontoAPI.Models;

namespace ControlePontoAPI.Interfaces
{
    /**********
     * Interface IRelatorioHelper
     */
    public interface IRelatorioHelper
    {
        /**********
         * Efetua os cálculos referentes aos registros de ponto informados e
         * gera um Relatorio contendo os resultados no formato ISO 8601
         */
        Relatorio GeraRelatorioMensal(List<Registro> registrosMes);
    }
}
