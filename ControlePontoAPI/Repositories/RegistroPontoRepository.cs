using ControlePontoAPI.Interfaces;
using ControlePontoAPI.Models;

namespace ControlePontoAPI.Repositories
{
    /**********
     * Classe RegistroPontoRepository
     * Implementa os métodos da interface IRegistroPontoRepository
     * Responsável pela persistência dos dados
     */
    public class RegistroPontoRepository : IRegistroPontoRepository
    {
        // Lista in-memory para persistência dos dados
        private static List<Registro> _registros = new List<Registro>();

        /**********
         * Adiciona um registro de ponto à lista in-memory
         */
        public void AdicionarRegistro(Registro novoRegistro)
        {
            _registros.Add(novoRegistro);
        }

        /**********
         * Recupera a lista in-memory
         */
        public List<Registro> ObterRegistros()
        {
            return _registros;
        }

        /**********
         * Recupera a lista in-memory, filtrando por mês
         */
        public List<Registro> ObterRegistrosPorMes(string mes)
        {
            return _registros.Where(r => r.dia.StartsWith(mes)).ToList();
        }

        /**********
         * Limpa a lista in-memory. Necessário nos testes unitários
         */
        public void LimparRegistros()
        {
            _registros.Clear();
        }
    }
}
