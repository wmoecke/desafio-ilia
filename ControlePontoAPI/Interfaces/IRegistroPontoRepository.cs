using ControlePontoAPI.Models;

namespace ControlePontoAPI.Interfaces
{
    /**********
     * Interface IRegistroPontoRepository
     */
    public interface IRegistroPontoRepository
    {
        /**********
         * Adiciona um registro de ponto à lista in-memory
         */
        void AdicionarRegistro(Registro novoRegistro);

        /**********
         * Recupera a lista in-memory
         */
        List<Registro> ObterRegistros();

        /**********
         * Recupera a lista in-memory, filtrando por mês
         */
        List<Registro> ObterRegistrosPorMes(string mes);

        /**********
         * Limpa a lista in-memory. Necessário nos testes unitários
         */
        void LimparRegistros();
    }
}
