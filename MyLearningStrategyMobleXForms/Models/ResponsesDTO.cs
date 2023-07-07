
namespace MyLearningStrategyMobleXForms.Models
{
    //[ResponseId] INT IDENTITY(1, 1) NOT NULL,
    //[QuestionId] INT NOT NULL, 
    //[OrderBy] INT NOT NULL DEFAULT 1, 
    //[IsCorrect] BIT NULL,
    //[Response] NVARCHAR(2083) NULL,
    //[Image_1_Device] NVARCHAR(256) NULL, 
    //[Image_1_Cloud] NVARCHAR(2083) NULL, 
    //[Image_1_Hash] INT NULL,
    //[Image_2_Device] NVARCHAR(256) NULL, 
    //[Image_2_Cloud] NVARCHAR(2083) NULL, 
    //[Image_2_Hash] INT NULL,
    //[Image_3_Device] NVARCHAR(256) NULL, 
    //[Image_3_Cloud] NVARCHAR(2083) NULL, 
    //[Image_3_Hash] INT NULL,
    //[Hyperlink_1] VARCHAR(2083) NULL, 
    //[Hyperlink_2] VARCHAR(2083) NULL, 
    //[Hyperlink_3] VARCHAR(2083) NULL, 
    //[Mnemonic] VARCHAR(300) NULL,
    public class ResponsesDTO : BaseDTO
    {
        //[KeyAttribute]
        public int ResponseId { get; set; }
        //[KeyAttribute]
        public int QuestionId { get; set; }
        public int OrderBy { get; set; }
        //[StringLength(2083, ErrorMessage = "The Response cannot exceed 2083 characters. ")]
        public string Response { get; set; }
        public bool IsCorrect { get; set; }
        //[StringLength(256, ErrorMessage = "The Image name cannot exceed 2083 characters. ")]
        public string Image_1_Device { get; set; }
        //[UrlAttribute, StringLength(2083, ErrorMessage = "The Image Url value cannot exceed 2083 characters. ")]
        public string Image_1_Cloud { get; set; }
        public int Image_1_Hash { get; set; }
        //[ StringLength(256, ErrorMessage = "The Image name cannot exceed 2083 characters. ")]
        public string Image_2_Device { get; set; }
        //[UrlAttribute, StringLength(2083, ErrorMessage = "The Image Url value cannot exceed 2083 characters. ")]
        public string Image_2_Cloud { get; set; }
        public int Image_2_Hash { get; set; }
        //[StringLength(256, ErrorMessage = "The Image name cannot exceed 2083 characters. ")]
        public string Image_3_Device { get; set; }
        //[UrlAttribute, StringLength(2083, ErrorMessage = "The Image Url value cannot exceed 2083 characters. ")]
        public string Image_3_Cloud { get; set; }
        public int Image_3_Hash { get; set; }
        //[UrlAttribute, StringLength(2083, ErrorMessage = "The Hyperlink 1 value cannot exceed 2083 characters. ")]
        public string Hyperlink_1 { get; set; }
        //[UrlAttribute, StringLength(2083, ErrorMessage = "The Hyperlink 2 value cannot exceed 2083 characters. ")]
        public string Hyperlink_2 { get; set; }
        //[UrlAttribute, StringLength(2083, ErrorMessage = "The Hyperlink 3 value cannot exceed 2083 characters. ")]
        public string Hyperlink_3 { get; set; }
        //[StringLength(300, ErrorMessage = "The Hyperlink 3 value cannot exceed 300 characters. ")]
        public string Mnemonic { get; set; }
    }
}
