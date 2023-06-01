using ControlePontoAPI.Helpers;
using ControlePontoAPI.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace ControlePontoAPI.UnitTests.Helpers
{
    /**********
     * Testes unitários da classe RelatorioHelper
     */
    public class RelatorioHelperTests
    {
        private MockRepository mockRepository;

        public RelatorioHelperTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
        }

        /**********
         * Cria a instância da classe
         */
        private RelatorioHelper CreateRelatorioHelper()
        {
            return new RelatorioHelper();
        }

        /**********
         * Valida se o Relatório é gerado corretamente se informado 4 registros de ponto
         */
        [Fact]
        public void GeraRelatorioMensal_Informar4Registros_RetornaRelatorio()
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

            var esperado = new Relatorio
            {
                mes = "2023-05",
                horasTrabalhadas = "PT8H0M0S",
                horasExcedentes = "PT0H0M0S",
                horasDevidas = "PT0H0M0S",
                registros = new List<Registro> { registro }
            };

            var relatorioHelper = CreateRelatorioHelper();
            List<Registro> registrosMes = new List<Registro> { registro };

            // Act
            var result = relatorioHelper.GeraRelatorioMensal(registrosMes);

            // Assert
            result.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se o Relatório é gerado corretamente se informado 3 registros de ponto
         */
        [Fact]
        public void GeraRelatorioMensal_Informar3Registros_RetornaRelatorio()
        {
            // Arrange
            var registro = new Registro
            {
                dia = "2023-05-30",
                horarios = new List<string>
                        {
                            "08:00:00",
                            "12:00:00",
                            "13:00:00"
                        }
            };

            var esperado = new Relatorio
            {
                mes = "2023-05",
                horasTrabalhadas = "PT4H0M0S",
                horasExcedentes = "PT0H0M0S",
                horasDevidas = "PT4H0M0S",
                registros = new List<Registro> { registro }
            };

            var relatorioHelper = CreateRelatorioHelper();
            List<Registro> registrosMes = new List<Registro> { registro };

            // Act
            var result = relatorioHelper.GeraRelatorioMensal(registrosMes);

            // Assert
            result.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se o Relatório é gerado corretamente se informado 2 registros de ponto
         */
        [Fact]
        public void GeraRelatorioMensal_Informar2Registros_RetornaRelatorio()
        {
            // Arrange
            var registro = new Registro
            {
                dia = "2023-05-30",
                horarios = new List<string>
                        {
                            "08:00:00",
                            "12:00:00"
                        }
            };

            var esperado = new Relatorio
            {
                mes = "2023-05",
                horasTrabalhadas = "PT4H0M0S",
                horasExcedentes = "PT0H0M0S",
                horasDevidas = "PT4H0M0S",
                registros = new List<Registro> { registro }
            };

            var relatorioHelper = CreateRelatorioHelper();
            List<Registro> registrosMes = new List<Registro> { registro };

            // Act
            var result = relatorioHelper.GeraRelatorioMensal(registrosMes);

            // Assert
            result.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se o Relatório é gerado corretamente se informado 1 registro de ponto
         */
        [Fact]
        public void GeraRelatorioMensal_Informar1Registro_RetornaRelatorio()
        {
            // Arrange
            var registro = new Registro
            {
                dia = "2023-05-30",
                horarios = new List<string>
                        {
                            "08:00:00"
                        }
            };

            var esperado = new Relatorio
            {
                mes = "2023-05",
                horasTrabalhadas = "PT0H0M0S",
                horasExcedentes = "PT0H0M0S",
                horasDevidas = "PT8H0M0S",
                registros = new List<Registro> { registro }
            };

            var relatorioHelper = CreateRelatorioHelper();
            List<Registro> registrosMes = new List<Registro> { registro };

            // Act
            var result = relatorioHelper.GeraRelatorioMensal(registrosMes);

            // Assert
            result.Should().BeEquivalentTo(esperado);
        }
    }
}
