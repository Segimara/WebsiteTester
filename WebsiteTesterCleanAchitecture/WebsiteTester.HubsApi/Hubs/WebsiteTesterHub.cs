using Microsoft.AspNetCore.SignalR;

namespace WebsiteTester.HubsApi.Hubs
{
    public class WebsiteTesterHub : Hub
    {
        private readonly bool isTesterRunning = false;

        // method that returns if tester is running 
        public async Task IsTesterRunning()
        {
            await Clients.Caller.SendAsync("SetWebsiteTesterState", isTesterRunning);
        }

        // method for run test for url and execute `blocktester`
        public async Task RunTest(string url)
        {
            isTesterRunning = true;
        }
    }
}
