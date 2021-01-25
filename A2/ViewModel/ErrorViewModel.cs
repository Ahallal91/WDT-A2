using System.Collections.Generic;

namespace A2.ViewModel
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int ErrorCode { get; set; }
        public Dictionary<int, string> errorDict = new Dictionary<int, string>
        {
            {400, "Bad request" },
            {401, "Unauthorized" },
            {403, "Forbidden" },
            {404, "Page not found" },
            {408, "The server timed out waiting for the request" },
            {502, "Bad Gateway" },
            {504, "Gateway Timeout" },
        };
    }
}
