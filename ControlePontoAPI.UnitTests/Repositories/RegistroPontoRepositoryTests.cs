using ControlePontoAPI.Models;
using ControlePontoAPI.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ControlePontoAPI.UnitTests.Repositories
{
    /**********
     * Testes unitários da classe RegistroPontoRepository
     */
    public class RegistroPontoRepositoryTests : IDisposable
    {
        private MockRepository _mockRepository;
        private Registro _novoRegistro = new Registro
        {
            dia = "2023-05-30",
            horarios = new List<string>
                        {
                            "08:00:00",
                            "12:00:00",
                            "13:00:00",
                            "17:00:00"
                        }
        };

        public RegistroPontoRepositoryTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
        }

        /**********
         * Limpa os registros para o próximo teste
         */
        public void Dispose()
        {
            var registroPontoRepository = CreateRegistroPontoRepository();
            registroPontoRepository.LimparRegistros();
        }

        /**********
         * Cria a instância da classe
         */
        private RegistroPontoRepository CreateRegistroPontoRepository()
        {
            return new RegistroPontoRepository();
        }

        /**********
         * Valida se um registro adicionado persiste e é retornado
         */
        [Fact]
        public void AdicionarObterRegistros_InsereObtemRegistros_Sucesso()
        {
            // Arrange
            var registroPontoRepository = CreateRegistroPontoRepository();
            var esperado = new List<Registro> { _novoRegistro };
            
            // Act
            registroPontoRepository.AdicionarRegistro(_novoRegistro);
            var result = registroPontoRepository.ObterRegistros();

            // Assert
            result.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se um registro adicionado persiste e é retornado com filtro do mês
         */
        [Fact]
        public void AdicionarObterRegistrosPorMes_InsereObtemRegistrosMes_Sucesso()
        {
            // Arrange
            var registroPontoRepository = CreateRegistroPontoRepository();
            var esperado = new List<Registro> { _novoRegistro };

            var mes = "2023-05";

            // Act
            registroPontoRepository.AdicionarRegistro(_novoRegistro);
            var result = registroPontoRepository.ObterRegistrosPorMes(mes);

            // Assert
            result.Should().BeEquivalentTo(esperado);
        }
    }
}
