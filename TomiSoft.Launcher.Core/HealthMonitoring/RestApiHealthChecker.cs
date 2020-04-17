using System;
using System.Net;
using System.Threading.Tasks;

namespace TomiSoft.Launcher.Core.HealthMonitoring
{
    public class RestApiHealthMonitor : IHealthMonitor
    {
        private readonly Uri healthUri;

        public RestApiHealthMonitor(Uri healthUri)
        {
            this.healthUri = healthUri;
        }

        public async Task<bool> IsRunningAsync()
        {
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(healthUri);
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
