using FluentAssertions;
using Vendas.Domain.Clientes.Entities;
using Vendas.Domain.Clientes.Enums;
using Vendas.Domain.Clientes.Events;
using Vendas.Domain.Clientes.ValueObjects;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Tests.Clientes.Entities;
public class ClienteTests
{
    #region Métodos auxiliares
    private static NomeCompleto CriarNomeCompletoValido(string nome = "Augusto Lima")
    {
        return new NomeCompleto(nome);
    }

    private static Cpf CriarCpfValido(string cpf = "12345678909")
    {
        return new Cpf(cpf);
    }

    private static Email CriarEmailValido(string email = "augusto.lima@example.com.br")
    {
        return new Email(email);
    }

    private static Telefone CriarTelefoneValido(string telefone = "11987654321")
    {
        return new Telefone(telefone);
    }

    private static Endereco CriarEnderecoValido(string cep = "12345678", string logradouro = "Rua Exemplo", string numero = "123", string bairro = "Bairro Exemplo", string cidade = "Cidade Exemplo", string estado = "Estado Exemplo", string pais = "País Exemplo", string complemento = "Casa")
    {
        return new Endereco(cep, logradouro, numero, bairro, cidade, estado, pais, complemento);
    }

    private static Cliente CriarClienteValido()
    {
        return new Cliente(
            CriarNomeCompletoValido(),
            CriarCpfValido(),
            CriarEmailValido(),
            CriarTelefoneValido(),
            CriarEnderecoValido(),
            Sexo.Masculino,
            EstadoCivil.Solteiro
        );
    }
    #endregion

    #region CriarCliente
    [Fact]
    public void CriarCliente_DeveCriarCliente_QuandoDadosForemValidos()
    {
        // Arrange & Act
        var cliente = CriarClienteValido();

        // Assert
        cliente.Status.Should().Be(StatusCliente.Ativo);
        cliente.Nome.NomeCompletoFormatado.Should().Be("Augusto Lima");
        cliente.Enderecos.Should().ContainSingle();
        cliente.EnderecoPrincipalId.Should().Be(cliente.Enderecos.First().Id);
    }

    [Fact]
    public void CriarCliente_DeveGerarEventoClienteCadastrado_QuandoDadosForemValidos()
    {
        // Arrange & Act
        var cliente = CriarClienteValido();

        // Assert
        cliente.DomainEvents.Should().ContainSingle(e => e is ClienteCadastradoEvent)
            .Which.Should().BeOfType<ClienteCadastradoEvent>();
    }

    [Theory]
    [InlineData("Nome")]
    [InlineData("Cpf")]
    [InlineData("Email")]
    [InlineData("Telefone")]
    [InlineData("Endereco")]
    public void CriarCliente_DeveLancarDomainException_QuandoDadosForemInvalidos(string campo)
    {
        // Assert
        NomeCompleto? nome = campo == "Nome" ? null : CriarNomeCompletoValido();
        Cpf? cpf = campo == "Cpf" ? null : CriarCpfValido();
        Email? email = campo == "Email" ? null : CriarEmailValido();
        Telefone? telefone = campo == "Telefone" ? null : CriarTelefoneValido();
        Endereco? endereco = campo == "Endereco" ? null : CriarEnderecoValido();

        // Act
        Action act = () => new Cliente(nome!, cpf!, email!, telefone!, endereco!, Sexo.Masculino, EstadoCivil.Solteiro);

        // Assert
        act.Should().Throw<DomainException>();
    }
    #endregion

    #region AdicionarEndereco
    [Fact]
    public void AdicionarEndereco_DeveAdicionarEndereco_QuandoEnderecoForValido()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var novoEndereco = CriarEnderecoValido("11223344");

        // Act
        cliente.AdicionarEndereco(novoEndereco);

        // Assert
        cliente.Enderecos.Should().HaveCount(2);
    }

    [Fact]
    public void AdicionarEndereco_DeveLancarDomainException_QuandoEnderecoForNulo()
    {
        // Arrange
        var cliente = CriarClienteValido();

        // Act
        Action act = () => cliente.AdicionarEndereco(null!);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Endereço inválido");
    }

    [Fact]
    public void AdicionarEndereco_DeveAtualizarDataModificacao_QuandoEnderecoForAdicionado()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var dataCriacao = cliente.DataCriacao;
        var novoEndereco = CriarEnderecoValido();

        // Act
        cliente.AdicionarEndereco(novoEndereco);

        // Assert
        cliente.DataAtualizacao.Should().BeAfter(dataCriacao);
    }
    #endregion

    #region RemoverEndereco
    [Fact]
    public void RemoverEndereco_DeveRemoverEndereco_QuandoExistirMaisDeUmEndereco()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var novoEndereco = CriarEnderecoValido("11223344");
        cliente.AdicionarEndereco(novoEndereco);

        // Act
        cliente.RemoverEndereco(novoEndereco.Id);

        // Assert
        cliente.Enderecos.Should().HaveCount(1);
    }

    [Fact]
    public void RemoverEndereco_DeveLancarDomainException_QuandoTentarRemoverUltimoEndereco()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var enderecoId = cliente.Enderecos.First().Id;

        // Act
        Action act = () => cliente.RemoverEndereco(enderecoId);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("O cliente deve possuir ao menos um endereço");
    }

    [Fact]
    public void RemoverEndereco_DeveLancarDomainException_QuandoEnderecoNaoExistir()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var enderecoIdInexistente = Guid.NewGuid();

        // Act
        Action act = () => cliente.RemoverEndereco(enderecoIdInexistente);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Endereço não encontrado");
    }

    [Fact]
    public void RemoverEndereco_DeveAtualizarEnderecoPrincipal_QuandoEnderecoPrincipalForRemovido()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var enderecoPrincipalId = cliente.EnderecoPrincipalId;
        var novoEndereco = CriarEnderecoValido("11223344");
        cliente.AdicionarEndereco(novoEndereco);

        // Act
        cliente.RemoverEndereco(enderecoPrincipalId);

        // Assert
        cliente.EnderecoPrincipalId.Should().Be(novoEndereco.Id);
    }

    [Fact]
    public void RemoverEndereco_DeveGerarEventoEnderecoPrincipalAlterado_QuandoEnderecoPrincipalForRemovido()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var enderecoPrincipalId = cliente.EnderecoPrincipalId;
        var novoEndereco = CriarEnderecoValido("11223344");
        cliente.AdicionarEndereco(novoEndereco);

        // Act
        cliente.RemoverEndereco(enderecoPrincipalId);

        // Assert
        cliente.DomainEvents.Should().ContainSingle(e => e is EnderecoPrincipalAlterado);
    }
    #endregion

    #region AlterarEndereco
    [Fact]
    public void AlterarEndereco_DeveAlterarEndereco_QuandoDadosForemValidos()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var enderecoIdPrincipal = cliente.Enderecos.First().Id;

        // Act
        cliente.AlterarEndereco(enderecoIdPrincipal, "87654321", "Avenida Exemplo", "321", "Bairro Exemplo 2", "Cidade Exemplo 2", "Estado Exemplo 2", "Pais Exemplo 2", "Complemento Exemplo");

        // Assert
        var enderecoAlterado = cliente.Enderecos.First(e => e.Id == enderecoIdPrincipal);
        enderecoAlterado.Cep.Should().Be("87654321");
        enderecoAlterado.Logradouro.Should().Be("Avenida Exemplo");
        enderecoAlterado.Numero.Should().Be("321");
        enderecoAlterado.Bairro.Should().Be("Bairro Exemplo 2");
        enderecoAlterado.Cidade.Should().Be("Cidade Exemplo 2");
    }

    [Fact]
    public void AlterarEndereco_DeveLancarDomainException_QuandoEnderecoNaoExistir()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var enderecoIdInexistente = Guid.NewGuid();

        // Act
        Action act = () => cliente.AlterarEndereco(enderecoIdInexistente, "87654321", "Avenida Exemplo", "321", "Bairro Exemplo 2", "Cidade Exemplo 2", "Estado Exemplo 2", "Pais Exemplo 2", "Complemento Exemplo");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Endereço não encontrado");
    }
    #endregion

    #region DefinirEnderecoPrincipal
    [Fact]
    public void DefinirEnderecoPrincipal_DeveAlterarEnderecoPrincipal_QuandoEnderecoExistir()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var novoEndereco = CriarEnderecoValido("11223344");
        cliente.AdicionarEndereco(novoEndereco);

        // Act
        cliente.DefinirEnderecoPrincipal(novoEndereco.Id);

        // Assert
        cliente.EnderecoPrincipalId.Should().Be(novoEndereco.Id);
    }

    [Fact]
    public void DefinirEnderecoPrincipal_DeveLancarDomainException_QuandoEnderecoNaoExistir()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var enderecoIdInexistente = Guid.NewGuid();

        // Act
        Action act = () => cliente.DefinirEnderecoPrincipal(enderecoIdInexistente);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Endereço não encontrado");
    }

    [Fact]
    public void DefinirEnderecoPrincipal_DeveGerarEventoEnderecoPrincipalAlterado_QuandoEnderecoPrincipalForAlterado()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var novoEndereco = CriarEnderecoValido("11223344");
        cliente.AdicionarEndereco(novoEndereco);

        // Act
        cliente.DefinirEnderecoPrincipal(novoEndereco.Id);

        // Assert
        cliente.DomainEvents.Should().ContainSingle(e => e is EnderecoPrincipalAlterado);
    }
    #endregion

    #region ObterEnderecoPrincipal
    [Fact]
    public void ObterEnderecoPrincipal_DeveRetornarEnderecoPrincipal_QuandoEnderecoPrincipalExistir()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var enderecoPrincipal = cliente.Enderecos.First();

        // Act
        var enderecoObtido = cliente.ObterEnderecoPrincipal();

        // Assert
        enderecoObtido!.Id.Should().Be(enderecoPrincipal.Id);
    }
    #endregion

    #region AtualizarPerfil
    [Fact]
    public void AtualizarPerfil_DeveAtualizarPerfil_QuandoDadosForemValidos()
    {
        // Arrange
        var cliente = CriarClienteValido();
        var nome = CriarNomeCompletoValido("Marcela Cury");

        // Act
        cliente.AtualizarPerfil(nome, CriarEmailValido("teste@marcela.com"), CriarTelefoneValido("1112345678"), Sexo.Feminino, EstadoCivil.Casado);

        // Assert
        cliente.Nome.Should().Be(nome);
        cliente.Sexo.Should().Be(Sexo.Feminino);
        cliente.EstadoCivil.Should().Be(EstadoCivil.Casado);
    }

    [Theory]
    [InlineData("Nome")]
    [InlineData("Email")]
    [InlineData("Telefone")]
    public void AtualizarPerfil_DeveLancarDomainException_QuandoDadosForemInvalidos(string campo)
    {
        // Arrange
        var cliente = CriarClienteValido();

        NomeCompleto? nome = campo == "Nome" ? null : CriarNomeCompletoValido();
        Email? email = campo == "Email" ? null : CriarEmailValido();
        Telefone? telefone = campo == "Telefone" ? null : CriarTelefoneValido();

        // Act
        Action act = () => cliente.AtualizarPerfil(nome!, email!, telefone!, Sexo.Feminino, EstadoCivil.Casado);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AtualizarPerfil_NaoDeveAtualizarPerfil_QuandoClienteEstiverBloqueado()
    {
        // Arrange
        var cliente = CriarClienteValido();
        cliente.Bloquear();

        // Act
        Action act = () => cliente.AtualizarPerfil(CriarNomeCompletoValido(), CriarEmailValido(), CriarTelefoneValido(), Sexo.Feminino, EstadoCivil.Casado);

        // Assert
        act.Should().Throw<DomainException>();
    }
    #endregion

    #region Bloquear
    [Fact]
    public void Bloquear_DeveBloquearCliente_QuandoForChamado()
    {
        // Arrange
        var cliente = CriarClienteValido();

        // Act
        cliente.Bloquear();

        // Assert
        cliente.Status.Should().Be(StatusCliente.Bloqueado);
    }
    #endregion
}