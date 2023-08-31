using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Repositories
{
    public interface IMessageRep
    {
        public List<Message> GetAllMessages();
        public List<Message> GetMessagesForPages(int skip);
        public Message GetMessageById(int messageId);
        public void AddMessage(Message message);
        public void EditMessage(Message message);
        public void RemoveMessage(Message message);
        public void RemoveMessage(int messageId);
        public bool ExistMessage(int messageId);
    }
}
