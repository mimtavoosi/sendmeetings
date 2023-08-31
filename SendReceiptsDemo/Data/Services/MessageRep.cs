using Microsoft.EntityFrameworkCore;
using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Services
{
    public class MessageRep:IMessageRep
    {
        private SendReceiptContext _context;
        public MessageRep(SendReceiptContext context)
        {
            _context = context;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }
        public void EditMessage(Message message)
        {
            _context.Messages.Update(message);
            _context.SaveChanges();
        }

        public bool ExistMessage(int messageId)
        {
            return _context.Messages.Any(a => a.MessageId == messageId);
        }

        public Message GetMessageById(int messageId)
        {
            return _context.Messages.Include(m => m.Customer).SingleOrDefault(m=> m.MessageId == messageId);
        }

        public List<Message> GetAllMessages()
        {
            return _context.Messages.OrderByDescending(m => m.MessageId).Include(m=> m.Customer).ToList();
        }

        public List<Message> GetMessagesForPages(int skip)
        {
            return _context.Messages.Include(m => m.Customer).OrderByDescending(a => a.MessageId).Skip(skip).Take(20).ToList();
        }

        public void RemoveMessage(Message message)
        {
            _context.Messages.Remove(message);
            _context.SaveChanges();
        }

        public void RemoveMessage(int messageId)
        {
           Message message = GetMessageById(messageId);
           RemoveMessage(message);
        }
    }
}
