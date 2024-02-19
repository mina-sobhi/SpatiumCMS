
namespace Domain.storageAggregate.Input
{
    public class AddFolderInput
    {
        public int StorageId { get; set; }
        public int BlogId { get; set; }
        public string CreatedById { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }    
    }
}
