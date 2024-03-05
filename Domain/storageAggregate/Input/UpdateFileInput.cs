namespace Domain.StorageAggregate.Input
{
    public class UpdateFileInput
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int? FolderId { get; set; }
        public string Name { get; set; }
        public string FileSize { get; set; }
        public string Extention { get; set; }
        public string Caption { get; set; }
        public string Url { get; set; }
        public string Createdby { get; set; }
        public string Alt { get; set; }
        public string Dimension { get; set; }
        public DateTime LastUpdate  { get; set; }
    }
}
