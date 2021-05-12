using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.CityRequests
{
    public class GetAllByStateRequest : BaseRequest
    {
        public GetAllByStateRequest(string stateAbbr)
        {
            StateAbbr = stateAbbr;
        }

        public string StateAbbr { get; }
    }
}