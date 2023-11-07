using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace SignalrServer.Hubs
{
    [HubName("SignatureHub")]
    public class MyHub : Hub
    {
        [Microsoft.AspNetCore.SignalR.HubMethodName("BroadcastMessage")]
        public async Task SendMessageToClients(string file)
        {
            Debug.Print("");
            Debug.Print($"---\tFile path received: {file}");
            Debug.Print("");
            await Clients.All.SendAsync("ReceiveMessage", file);
        }
    }
}
