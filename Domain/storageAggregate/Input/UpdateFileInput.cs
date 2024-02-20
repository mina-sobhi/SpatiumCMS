namespace Domain.StorageAggregate.Input
{
    public class UpdateFileInput
    {
        public string Name { get; set; }
        public string Extention { get; set; }
        public string Caption { get; set; }
        public string FileSize { get; set; }
        public string Alt { get; set; }
        public string? Dimension { get; set; }
    }
}
