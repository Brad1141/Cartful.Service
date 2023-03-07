namespace Cartful.Service.Entities
{
    public class ItemList
    {
        public Guid listID { get; set; }
        public Guid userID { get; set; }
        public string title { get; set; }
        public List<Item>? items { get; set; }
    }
}