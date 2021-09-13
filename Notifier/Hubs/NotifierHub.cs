using Microsoft.AspNetCore.SignalR;

namespace Notifier.Hubs;

public static class UserHandler
{
    public static HashSet<string> ConnectedIds = new HashSet<string>();
}
    
public class NotifierInfo
{
    public int ReadersCount { get; set; }
    public List<string> Readers { get; internal set; } = null!;
}
2

public class NotifierHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.Others.SendAsync("ReceiveMessage", message);
    }

    public async Task GetInfo()
    {
        var readers = UserHandler.ConnectedIds.Where(c => c != Context.ConnectionId).ToList();
        NotifierInfo info = new NotifierInfo { ReadersCount = readers.Count, Readers = readers };
        await Clients.Caller.SendAsync("ReceiveInfo", info);
    }

    public override Task OnConnectedAsync()
    {
        UserHandler.ConnectedIds.Add(Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        UserHandler.ConnectedIds.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
