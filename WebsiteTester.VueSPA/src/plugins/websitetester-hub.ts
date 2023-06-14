import * as signalR from '@aspnet/signalr';
import { useWebsiteTesterStore } from '@/stores/WebsiteTesterStore';

const SignalRPlugin = {
    install(Vue: any) {
        // make your signalR connection
        const connection = new signalR.HubConnectionBuilder()
            .configureLogging(signalR.LogLevel.Information)
            .withUrl(import.meta.env.VITE_APP_BASEURL + "/websitetesterhub")
            .build();
        // expose a global function to connect/start the signalR connection

        connection.on("SetWebsiteTesterState", (IsTesterRunning) => {
          useWebsiteTesterStore().isUrlBeingTested = IsTesterRunning;
          if (!IsTesterRunning) {
            useWebsiteTesterStore().fetchLinks();
          }
      });
    }
}

export default SignalRPlugin;