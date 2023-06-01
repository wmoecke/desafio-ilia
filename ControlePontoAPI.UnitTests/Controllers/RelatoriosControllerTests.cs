using ControlePontoAPI.Controllers;
using ControlePontoAPI.Interfaces;
using ControlePontoAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ControlePontoAPI.UnitTests.Controllers
{
    /**********
     * Testes unitários da classe RelatoriosController
     */
    public class RelatoriosControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IRegistroPontoRepository> mockRegistroPontoRepository;
        private Mock<IRelatorioHelper> mockRelatorioHelper;

        public RelatoriosControllerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);

            mockRegistroPontoRepository = mockRepository.Create<IRegistroPontoRepository>();
            mockRelatorioHelper = mockRepository.Create<IRelatorioHelper>();
        }

        /**********
         * Cria a instância da controller
         */
        private RelatoriosController CreateRelatoriosController()
        {
            return new RelatoriosController(
                mockRegistroPontoRepository.Object,
                mockRelatorioHelper.Object);
        }

        /**********
         * Valida se o erro 404 é lançado ao informar um mês inexistente
         */
        [Fact]
        public void GeraRelatorioMensal_MesNaoExiste_RetornaErroNaoEncontrado()
        {
            // Arrange
            var esperado = new Mensagem { mensagem = "Relatório não encontrado" };
            mockRegistroPontoRepository.Setup(x => x.ObterRegistrosPorMes(It.IsAny<string>())).Returns(new List<Registro>());
            var relatoriosController = this.CreateRelatoriosController();
            string mes = "2023-05";

            // Act
            var result = relatoriosController.GeraRelatorioMensal(mes);

            // Assert
            ((ObjectResult)result).StatusCode.Should().Be(StatusCodes.Status404NotFound);
            ((ObjectResult)result).Value.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se o Relatório Mensal é validado com status 200
         * Não valida a geração do Relatório, somente o status com dados informados
         * A geração do Relatório é validada na suite RelatorioHelperTests
         */
        [Fact]
        public void GeraRelatorioMensal_MesCorreto_RetornaOK()
        {
            // Arrange
            var registro = new Registro
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
            mockRegistroPontoRepository.Setup(x => x.ObterRegistrosPorMes(It.IsAny<string>())).Returns(new List<Registro> { registro });
            var relatoriosController = this.CreateRelatoriosController();
            string mes = "2023-05";

            // Act
            var result = relatoriosController.GeraRelatorioMensal(mes);

            // Assert
            ((ObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
