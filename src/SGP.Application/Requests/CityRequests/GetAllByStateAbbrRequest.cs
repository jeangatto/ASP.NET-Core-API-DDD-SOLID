using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.CityRequests
{
    public class GetAllByStateAbbrRequest : BaseRequest
    {
        public GetAllByStateAbbrRequest(string stateAbbr)
        {
            StateAbbr = stateAbbr;
        }

        public string StateAbbr { get; }
    }
}