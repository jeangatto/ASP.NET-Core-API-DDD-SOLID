using SGP.Application.Responses.Common;

namespace SGP.Application.Responses
{
    public class CityResponse : BaseResponse
    {
        public string Ibge { get; set; }
        public string StateAbbr { get; set; }
        public string Name { get; set; }
    }
}