namespace Domain.StorageAggregate.Input
{
    public class FileInput
    {
        public string Name { get;  set; }
        public string Extention { get;  set; }
        public string Caption { get;  set; }
        public string FileSize { get;  set; }
        public string Alt { get;  set; }
        public string Dimension { get;  set; }
        public string CreatedById { get;  set; }
        public int? FolderId { get;  set; }
        public int BlogId { get;  set; }
        public string UrlPath { get;  set; }


    }
}
