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
     * Testes unitários da classe BatidasController
     */
    public class BatidasControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IRegistroPontoRepository> mockRegistroPontoRepository;

        public BatidasControllerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);

            mockRegistroPontoRepository = mockRepository.Create<IRegistroPontoRepository>();
        }

        /**********
         * Cria a instância da controller
         */
        private BatidasController CreateBatidasController()
        {
            return new BatidasController(
                mockRegistroPontoRepository.Object);
        }

        /**********
         * Valida se o erro 400 é lançado ao não informar o Momento
         */
        [Fact]
        public void InsereBatida_MomentoNull_RetornaErroCampoObrigatorio()
        {
            // Arrange
            var esperado = new Mensagem { mensagem = "Campo obrigatório não informado" };
            var batidasController = CreateBatidasController();
            Momento momento = null;

            // Act
            var result = batidasController.InsereBatida(momento);

            // Assert
            ((ObjectResult)result).StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            ((ObjectResult)result).Value.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se o erro 400 é lançado ao não informar o valor Momento.dataHora
         */
        [Fact]
        public void InsereBatida_DataHoraNull_RetornaErroCampoObrigatorio()
        {
            // Arrange
            var esperado = new Mensagem { mensagem = "Campo obrigatório não informado" };
            var batidasController = CreateBatidasController();
            Momento momento = new Momento { dataHora = null };

            // Act
            var result = batidasController.InsereBatida(momento);

            // Assert
            ((ObjectResult)result).StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            ((ObjectResult)result).Value.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se o erro 400 é lançado ao não informar o valor Momento.dataHora corretamente
         */
        [Fact]
        public void InsereBatida_DataHoraErrada_RetornaErroFormatoInvalido()
        {
            // Arrange
            var esperado = new Mensagem { mensagem = "Data e hora em formato inválido" };
            var batidasController = CreateBatidasController();
            Momento momento = new Momento { dataHora = "2023-05-28T08" };

            // Act
            var result = batidasController.InsereBatida(momento);

            // Assert
            ((ObjectResult)result).StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            ((ObjectResult)result).Value.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se o erro 403 é lançado quando o valor Momento.dataHora cair num Final De Semana
         */
        [Fact]
        public void InsereBatida_DataHoraNoFDS_RetornaErroFormatoInvalido()
        {
            // Arrange
            var esperado = new Mensagem { mensagem = "Sábado e domingo não são permitidos como dia de trabalho" };
            var batidasController = CreateBatidasController();
            Momento momento = new Momento { dataHora = "2023-05-28T08:00:00" };

            // Act
            var result = batidasController.InsereBatida(momento);

            // Assert
            ((ObjectResult)result).StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            ((ObjectResult)result).Value.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se o erro 403 é lançado quando for registrado mais de 4 horários em um mesmo dia
         */
        [Fact]
        public void InsereBatida_HorariosExcedeLimite_RetornaErroLimiteExcedido()
        {
            // Arrange
            var esperado = new Mensagem { mensagem = "Apenas 4 horários podem ser registrados por dia" };
            mockRegistroPontoRepository.Setup(x => x.ObterRegistros()).Returns(
                new List<Registro>
                {
                    new Registro {
                        dia = "2023-05-30",
                        horarios = new List<string> {
                            "08:00:00",
                            "12:00:00",
                            "13:00:00",
                            "17:00:00"
                        }
                    }
                });
            var batidasController = CreateBatidasController();
            Momento momento = new Momento { dataHora = "2023-05-30T18:00:00" };

            // Act
            var result = batidasController.InsereBatida(momento);

            // Assert
            ((ObjectResult)result).StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            ((ObjectResult)result).Value.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se o erro 403 é lançado quando o registro do almoço for inferior a 1 hora
         */
        [Fact]
        public void InsereBatida_HorariosAlmocoCurto_RetornaErroHorarioMinimo()
        {
            // Arrange
            var esperado = new Mensagem { mensagem = "Deve haver no mínimo 1 hora de almoço" };
            mockRegistroPontoRepository.Setup(x => x.ObterRegistros()).Returns(
                new List<Registro>
                {
                    new Registro {
                        dia = "2023-05-30",
                        horarios = new List<string> {
                            "08:00:00",
                            "12:00:00"
                        }
                    }
                });
            var batidasController = CreateBatidasController();
            Momento momento = new Momento { dataHora = "2023-05-30T12:30:00" };

            // Act
            var result = batidasController.InsereBatida(momento);

            // Assert
            ((ObjectResult)result).StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            ((ObjectResult)result).Value.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se o erro 409 é lançado quando já houver um registro igual ao registro informado
         */
        [Fact]
        public void InsereBatida_HorariosRepete_RetornaErroHorarioJaRegistrado()
        {
            // Arrange
            var esperado = new Mensagem { mensagem = "Horário já registrado" };
            mockRegistroPontoRepository.Setup(x => x.ObterRegistros()).Returns(
                new List<Registro>
                {
                    new Registro {
                        dia = "2023-05-30",
                        horarios = new List<string> {
                            "08:00:00",
                            "12:00:00",
                            "13:00:00"
                        }
                    }
                });
            var batidasController = CreateBatidasController();
            Momento momento = new Momento { dataHora = "2023-05-30T13:00:00" };

            // Act
            var result = batidasController.InsereBatida(momento);

            // Assert
            ((ObjectResult)result).StatusCode.Should().Be(StatusCodes.Status409Conflict);
            ((ObjectResult)result).Value.Should().BeEquivalentTo(esperado);
        }

        /**********
         * Valida se um novo Registro é inserido corretamente, retornando o Registro atualizado
         */
        [Fact]
        public void InsereBatida_InsereNovoRegistro_RetornaNovoRegistroInserido()
        {
            // Arrange
            var esperado = new Registro {
                dia = "2023-05-30",
                horarios = new List<string> { "08:00:00" }
            };
            var batidasController = CreateBatidasController();
            Momento momento = new Momento { dataHora = "2023-05-30T08:00:00" };

            // Act
            var result = batidasController.InsereBatida(momento);

            // Assert
            ((ObjectResult)result).StatusCode.Should().Be(StatusCodes.Status201Created);
            ((ObjectResult)result).Value.Should().BeEquivalentTo(esperado);
        }
    }
}
