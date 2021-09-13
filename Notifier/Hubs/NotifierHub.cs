using Microsoft.AspNetCore.SignalR;
using Notifier.Models;

namespace Notifier.Hubs;
   
public class NotifierHub : Hub
{
    private readonly ILogger log;
    public NotifierHub(ILoggerFactory loggerFactory)
    {
        log = loggerFactory.CreateLogger(typeof(NotifierHub));
    }
    public async Task SendMessage(string message)
    {
        log.LogInformation("Publisher send: {message}", message);
        await Clients.Others.SendAsync("ReceiveMessage", message);
    }

    public async Task GetInfo()
    {
        log.LogInformation("Publisher request info.");

        var readers = UserHandler.ConnectedIds.Where(c => c != Context.ConnectionId).ToList();
        NotifierInfo info = new() { ReadersCount = readers.Count, Readers = readers };

        log.LogInformation("The are {readers.Count} readers connected.", readers.Count);

        await Clients.Caller.SendAsync("ReceiveInfo", info);
    }

    public override Task OnConnectedAsync()
    {
        UserHandler.ConnectedIds.Add(Context.ConnectionId);
        log.LogInformation("User {Context.ConnectionId} has suscribed.", Context.ConnectionId);

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        UserHandler.ConnectedIds.Remove(Context.ConnectionId);

        if (exception == null) log.LogInformation("User {Context.ConnectionId} has unsuscribed.", Context.ConnectionId);
        else log.LogError("There was an error and user {Context.ConnectionId} has unsuscribed. Error message {exception.Message}", Context.ConnectionId, exception.Message);

        return base.OnDisconnectedAsync(exception);
    }
}
