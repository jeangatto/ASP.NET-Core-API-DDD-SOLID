using SGP.Shared.Messages;

namespace SGP.Application.Requests.UsuarioRequests
{
    public class AddUsuarioRequest : BaseRequestWithValidator<AddUsuarioRequest>
    {
        public AddUsuarioRequest(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
        }

        public AddUsuarioRequest()
        {
        }

        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
