using System.Collections.Generic;
using System.Linq;

namespace SGP.PublicApi.Models
{
    public class ApiResponse<T>
    {
        public int Status { get; private set; }
        public bool Success { get; private set; }
        public T Result { get; private set; }
        public IEnumerable<string> Errors { get; private set; } = Enumerable.Empty<string>();
    }
}
