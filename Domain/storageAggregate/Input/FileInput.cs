namespace Domain.storageAggregate.Input
{
    public class FileInput
    {
        public string Name { get;  set; }
        public string Extention { get;  set; }
        public string Caption { get;  set; }
        public string FileSize { get;  set; }
        public string Alt { get;  set; }
        public string? Dimension { get;  set; }
        public string CreatedById { get; private set; }
        public int? FolderId { get; private set; }
        public int BlogId { get; private set; }
    }
}
