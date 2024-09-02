namespace ShuanWebAPI.Dtos
{
    public class UploadFileDto
    {
        public Guid UploadFileID { get; set; }

        public string Name { get; set; } = null!;

        public string Src { get; set; } = null!;

        public Guid ID { get; set; }
    }
}
