using Microsoft.AspNetCore.SignalR;
using WebsiteTester.WebApi.Services;

namespace WebsiteTester.WebApi.Hubs
{
    public class WebsiteTesterHub : Hub
    {
        private readonly WebsiteTesterStateService _websiteTesterStateService;

        public WebsiteTesterHub(WebsiteTesterStateService websiteTesterStateService)
        {
            _websiteTesterStateService = websiteTesterStateService;
        }

        public async Task IsTesterRunning()
        {
            await Clients.Caller.SendAsync("SetWebsiteTesterState", _websiteTesterStateService.IsTesterRunning);
        }
    }
}
