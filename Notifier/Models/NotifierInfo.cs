
namespace Notifier.Models;
public class NotifierInfo
{
    public int ReadersCount { get; set; }
    public List<string> Readers { get; internal set; } = null!;
}
