using System.Net;

namespace BigDataReader.Middlewares
{
    public class IpFilter
    {
        private readonly RequestDelegate _next;
        private readonly List<IPAddress> _allowedIps = new List<IPAddress>() { IPAddress.Parse("::1"), IPAddress.Parse("0.0.0.0") };

        public IpFilter(RequestDelegate next, string[] allowedIps)
        {
            _next = next;
            foreach (var allowedIp in allowedIps)
            {
                _allowedIps.Add(IPAddress.Parse(allowedIp));
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;
            if (remoteIp == null || !_allowedIps.Contains(remoteIp))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await _next.Invoke(context);
        }
    }
}
