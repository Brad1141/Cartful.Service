namespace Cartful.Service.Entities
{
    public class Item
    {
        public Guid itemID { get; set; }
        public Guid listID { get; set; }
        public string itemName { get; set; }
        public bool isChecked { get; set; }
    }
}