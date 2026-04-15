using FluentAssertions;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.ValueObjects;

namespace Vendas.Domain.Tests.ValueObjects;
public class EnderecoEntregaTests
{
    [Fact]
    public void Criar_DeveRetornarEnderecoValido_QuandoDadosForemValidos_()
    {
        // Arrange
        var cep = "12345-678";
        var logradouro = "Rua Exemplo";
        var complemento = "Apto 101";
        var bairro = "Centro";
        var cidade = "São Paulo";
        var estado = "SP";
        var pais = "Brasil";

        // Act
        var endereco = EnderecoEntrega.Criar(cep, logradouro, complemento, bairro, cidade, estado, pais);

        // Assert
        endereco.Should().NotBeNull();
        endereco.Cep.Should().Be(cep);
        endereco.Logradouro.Should().Be(logradouro);
        endereco.Complemento.Should().Be(complemento);
        endereco.Bairro.Should().Be(bairro);
        endereco.Cidade.Should().Be(cidade);
        endereco.Estado.Should().Be(estado);
        endereco.Pais.Should().Be(pais);
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("12-345678")]
    [InlineData("ABCDE-678")]
    public void Criar_DeveLancarDomainException_QuandoCepForInvalido(string cepInvalido)
    {
        // Arrange
        var logradouro = "Rua Exemplo";
        var complemento = "Apto 101";
        var bairro = "Centro";
        var cidade = "São Paulo";
        var estado = "SP";
        var pais = "Brasil";

        // Act
        Action act = () => EnderecoEntrega.Criar(cepInvalido, logradouro, complemento, bairro, cidade, estado, pais);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("CEP inválido. Deve estar no formato 00000-000");
    }

    [Fact]
    public void Criar_DeveCriarInstanciasIguais_QuandoInformacoesForemIdenticas()
    {
        // Arrange
        var endereco1 = EnderecoEntrega.Criar("12345-678", "Rua Exemplo", "Apto 101", "Centro", "São Paulo", "SP", "Brasil");
        var endereco2 = EnderecoEntrega.Criar("12345-678", "Rua Exemplo", "Apto 101", "Centro", "São Paulo", "SP", "Brasil");

        // Act
        var enderecosSaoIguais = endereco1 == endereco2;

        // Assert
        endereco1.Should().Be(endereco2);
        enderecosSaoIguais.Should().BeTrue();
    }

    [Fact]
    public void Criar_DeveCriarInstanciasDiferentes_QuandoInformacoesForemDiferentes()
    {
        // Arrange
        var endereco1 = EnderecoEntrega.Criar("12345-678", "Rua Exemplo", "Apto 101", "Centro", "São Paulo", "SP", "Brasil");
        var endereco2 = EnderecoEntrega.Criar("87654-321", "Rua Diferente", "Apto 101", "Centro", "São Paulo", "SP", "Brasil");
        // Act
        var enderecosSaoIguais = endereco1 == endereco2;

        // Assert
        endereco1.Should().NotBe(endereco2);
        enderecosSaoIguais.Should().BeFalse();
    }

    [Fact]
    public void Criar_DeveCriarInstanciaImutavel_AposCriacao()
    {
        // Arrange
        var endereco = EnderecoEntrega.Criar("12345-678", "Rua Exemplo", "Apto 101", "Centro", "São Paulo", "SP", "Brasil");

        // Act não aplicável

        // Assert
        endereco.GetType().GetProperties().All(p => p.SetMethod == null || p.SetMethod.IsPrivate).Should().BeTrue();
    }

    [Theory]
    [InlineData(null, "Rua Exemplo", "Apto 101", "Centro", "São Paulo", "SP", "Brasil")]
    [InlineData("12345-678", null, "Apto 101", "Centro", "São Paulo", "SP", "Brasil")]
    [InlineData("12345-678", "Rua Exemplo", null, "Centro", "São Paulo", "SP", "Brasil")]
    [InlineData("12345-678", "Rua Exemplo", "Apto 101", null, "São Paulo", "SP", "Brasil")]
    [InlineData("12345-678", "Rua Exemplo", "Apto 101", "Centro", null, "SP", "Brasil")]
    [InlineData("12345-678", "Rua Exemplo", "Apto 101", "Centro", "São Paulo", null, "Brasil")]
    [InlineData("12345-678", "Rua Exemplo", "Apto 101", "Centro", "São Paulo", "SP", null)]
    public void Criar_DeveLancarDomainException_QuandoAlgumaInformacaoObrigatoriaForNula(string cep, string logradouro, string complemento, string bairro, string cidade, string estado, string pais)
    {
        // Act
        Action act = () => EnderecoEntrega.Criar(cep, logradouro, complemento, bairro, cidade, estado, pais);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*não pode ser nulo*");
    }
}
