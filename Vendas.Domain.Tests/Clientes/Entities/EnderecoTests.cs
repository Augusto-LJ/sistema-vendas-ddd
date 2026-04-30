using FluentAssertions;
using Vendas.Domain.Clientes.Entities;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Tests.Clientes.Entities;
public class EnderecoTests
{
    private static Endereco CriarEnderecoValido()
    {
        return new Endereco("12345678", "Rua Exemplo", "123", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "Pais Exemplo");
    }

    #region CriarEndereco
    [Fact]
    public void CriarEndereco_DeveCriarEnderecoValido_QuandoDadosForemValidos()
    {
        // Arrange & Act        var endereco = CriarEnderecoValido();
        var endereco = CriarEnderecoValido();

        // Assert
        endereco.Cep.Should().Be("12345678");
        endereco.Logradouro.Should().Be("Rua Exemplo");
        endereco.Numero.Should().Be("123");
        endereco.Bairro.Should().Be("Bairro Exemplo");
        endereco.Cidade.Should().Be("Cidade Exemplo");
        endereco.Estado.Should().Be("Estado Exemplo");
        endereco.Pais.Should().Be("Pais Exemplo");
        endereco.Complemento.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CriarEndereco_NaoDeveCriarEndereco_ComCepInvalido(string cep)
    {
        // Arrange, Act
        Action act = () => new Endereco(cep, "Rua Exemplo", "123", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "Pais Exemplo");
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O CEP é obrigatório");
    }

    [Fact]
    public void CriarEndereco_DeveLancarDomainException_QuandoCepNaoTiverOitoDigitos()
    {
        // Arrange
        var cepInvalido = "12345";

        // Act
        Action act = () => new Endereco(cepInvalido, "Rua Exemplo", "123", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "Pais Exemplo");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("CEP inválido");
    }

    [Theory]
    [InlineData(null, "Rua Exemplo", "123", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "Pais Exemplo", "O CEP é obrigatório")]
    [InlineData("12345678", null, "123", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "Pais Exemplo", "O logradouro é obrigatório")]
    [InlineData("12345678", "Rua Exemplo", null, "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "Pais Exemplo", "O número é obrigatório")]
    [InlineData("12345678", "Rua Exemplo", "123", null, "Cidade Exemplo", "Estado Exemplo", "Pais Exemplo", "O bairro é obrigatório")]
    [InlineData("12345678", "Rua Exemplo", "123", "Bairro Exemplo", null, "Estado Exemplo", "Pais Exemplo", "A cidade é obrigatória")]
    [InlineData("12345678", "Rua Exemplo", "123", "Bairro Exemplo", "Cidade Exemplo", null, "Pais Exemplo", "O estado é obrigatório")]
    [InlineData("12345678", "Rua Exemplo", "123", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", null, "O país é obrigatório")]
    public void CriarEndereco_NaoDeveCriarEndereco_ComDadosInvalidos(string cep, string logradouro, string numero, string bairro, string cidade, string estado, string pais, string mensagemEsperada)
    {
        // Arrange & Act
        Action act = () => new Endereco(cep, logradouro, numero, bairro, cidade, estado, pais);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage(mensagemEsperada);
    }
    #endregion

    #region Atualizar
    [Fact]
    public void Atualizar_DeveAtualizarEndereco_QuandoDadosForemValidos()
    {
        // Arrange
        var endereco = CriarEnderecoValido();

        // Act
        endereco.Atualizar("87654321", "Avenida Exemplo", "321", "Bairro Exemplo 2", "Cidade Exemplo 2", "Estado Exemplo 2", "Pais Exemplo 2", "Complemento Exemplo");

        // Assert
        endereco.Cep.Should().Be("87654321");
        endereco.Logradouro.Should().Be("Avenida Exemplo");
        endereco.Numero.Should().Be("321");
        endereco.Bairro.Should().Be("Bairro Exemplo 2");
        endereco.Cidade.Should().Be("Cidade Exemplo 2");
        endereco.Estado.Should().Be("Estado Exemplo 2");
        endereco.Pais.Should().Be("Pais Exemplo 2");
        endereco.Complemento.Should().Be("Complemento Exemplo");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Atualizar_NaoDeveAtualizarEndereco_ComCepInvalido(string cep)
    {
        // Arrange
        var endereco = CriarEnderecoValido();

        // Act
        Action act = () => endereco.Atualizar(cep, "Avenida Exemplo", "321", "Bairro Exemplo 2", "Cidade Exemplo 2", "Estado Exemplo 2", "Pais Exemplo 2");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O CEP é obrigatório");
    }

    [Theory]
    [InlineData(null, "Rua Exemplo", "123", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "Pais Exemplo", "O CEP é obrigatório")]
    [InlineData("12345678", null, "123", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "Pais Exemplo", "O logradouro é obrigatório")]
    [InlineData("12345678", "Rua Exemplo", null, "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "Pais Exemplo", "O número é obrigatório")]
    [InlineData("12345678", "Rua Exemplo", "123", null, "Cidade Exemplo", "Estado Exemplo", "Pais Exemplo", "O bairro é obrigatório")]
    [InlineData("12345678", "Rua Exemplo", "123", "Bairro Exemplo", null, "Estado Exemplo", "Pais Exemplo", "A cidade é obrigatória")]
    [InlineData("12345678", "Rua Exemplo", "123", "Bairro Exemplo", "Cidade Exemplo", null, "Pais Exemplo", "O estado é obrigatório")]
    [InlineData("12345678", "Rua Exemplo", "123", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", null, "O país é obrigatório")]
    public void AtualizarEndereco_NaoDeveAtualizarEndereco_ComDadosInvalidos(string cep, string logradouro, string numero, string bairro, string cidade, string estado, string pais, string mensagemEsperada)
    {
        // Arrange
        var endereco = CriarEnderecoValido();

        // Act
        Action act = () => endereco.Atualizar(cep, logradouro, numero, bairro, cidade, estado, pais);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage(mensagemEsperada);
    }
    #endregion
}