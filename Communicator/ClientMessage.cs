

namespace Communicator
{
    public class ClientMessage
    {
        public ClientMessage(string message, ActionType type = ActionType.MESSAGE)
        {
            Message = message;
            Type = type;
        }

        public string Message { get; set; }
        public ActionType Type { get; set; }
    }
}
