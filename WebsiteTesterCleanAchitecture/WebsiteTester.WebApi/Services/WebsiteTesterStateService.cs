namespace WebsiteTester.WebApi.Services
{
    public class WebsiteTesterStateService
    {
        private bool isTesterRunning;
        private readonly object lockObject = new object();

        public WebsiteTesterStateService()
        {
            isTesterRunning = false;
        }

        public bool IsTesterRunning
        {
            get
            {
                lock (lockObject)
                {
                    return isTesterRunning;
                }
            }
            set
            {
                lock (lockObject)
                {
                    isTesterRunning = value;
                }
            }
        }
    }
}
