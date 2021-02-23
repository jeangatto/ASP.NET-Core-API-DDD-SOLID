using SGP.Shared.Entities;
using SGP.Shared.Interfaces;

namespace SGP.Domain.Entities
{
    public class Cliente : BaseEntity, IAggregateRoot
    {
        //public Cliente(string nome, CadastroPessoaFisica cpf, Sexo sexo, DataNascimento dataNascimento)
        //{
        //    Nome = nome;
        //    CPF = cpf;
        //    Sexo = sexo;
        //    DataNascimento = dataNascimento;
        //    DataCadastro = DataCadastro.Agora();
        //}

        //private Cliente()
        //{
        //}

        //public string Nome { get; private set; }
        //public string CPF { get; private set; }
        //public Sexo Sexo { get; private set; }
        //public DateTime DataNascimento { get; private set; }
    }
}