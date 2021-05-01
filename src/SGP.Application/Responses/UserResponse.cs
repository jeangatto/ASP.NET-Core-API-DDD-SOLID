using SGP.Application.Responses.Common;
using System;

namespace SGP.Application.Responses
{
    public class UserResponse : BaseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}