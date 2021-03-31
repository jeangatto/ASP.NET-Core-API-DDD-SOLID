using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetByIbgeRequest : BaseRequest
    {
        public GetByIbgeRequest(string ibge)
        {
            Ibge = ibge;
        }

        public string Ibge { get; }

        public override void Validate()
        {
            AddNotifications(new GetByIbgeRequestValidator().Validate(this));
        }
    }
}
