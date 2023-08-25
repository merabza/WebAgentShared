using System.Threading.Tasks;

namespace LibWebAgentMessages;

public interface IMessagesDataManager
{
    Task SendMessage(string? userName, string message, params string[] parameters);
    void UserConnected(string connectionId, string userName);
    void UserDisconnected(string connectionId, string userName);
}