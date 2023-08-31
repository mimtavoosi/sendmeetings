using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace SendReceiptsDemo.Data.Services
{
    public class ContentRep: IContentRep
    {
        private SendReceiptContext _context;
        public ContentRep(SendReceiptContext context)
        {
            _context = context;
        }

        public void AddContent(Content content)
        {
            _context.Contents.Add(content);
            _context.SaveChanges();
            _context.Entry(content).State = EntityState.Detached;
        }

        public void EditContent(Content content)
        {
            _context.Contents.Update(content);
            _context.SaveChanges();
            _context.Entry(content).State = EntityState.Detached;
        }

        public bool ExistContent(int contentId)
        {
           return _context.Contents.Any(x => x.ContentId == contentId);
        }

        public List<Content> GetAllContents()
        {
            return _context.Contents.OrderByDescending(c=>c.ContentId).ToList();
        }

        public Content GetContentById(int contentId)
        {
            return _context.Contents.Find(contentId);
        }

      
        public void RemoveContent(Content content)
        {
            _context.Contents.Remove(content);
            _context.SaveChanges();
            _context.Entry(content).State = EntityState.Detached;
        }

        public void RemoveContent(int contentId)
        {
            var content = GetContentById(contentId);
            RemoveContent(content);
        }
    }
}
