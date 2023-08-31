using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Repositories
{
    public interface IContentRep
    {
        public List<Content> GetAllContents();
        public Content GetContentById(int contentId);
        public void AddContent(Content content);
        public void EditContent(Content content);
        public void RemoveContent(Content content);
        public void RemoveContent(int contentId);
        public bool ExistContent(int contentId);
    }
}
