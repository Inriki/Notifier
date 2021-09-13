
namespace Notifier.Hubs;
public static class UserHandler
{
    public static HashSet<string> ConnectedIds { get; set; } = new();
}
