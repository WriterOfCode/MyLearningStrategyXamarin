namespace MyLearningStrategyMobleXForms.Models
{
    public class QuestionsDTO: BaseDTO
    {
        //[KeyAttribute]
        public int QuestionId { get; set; }
        public int BodyOfKnowledgeId { get; set; }
        public int OrderBy { get; set; }
        public string Question { get; set; }
        //[StringLength(256, ErrorMessage = "The Image name cannot exceed 2083 characters. ")]
        public string Image_1_Device { get; set; }

        //[UrlAttribute, StringLength(2083, ErrorMessage = "The Image Url value cannot exceed 2083 characters. ")]
        public string Image_1_Cloud { get; set; }
        public int Image_1_Hash { get; set; }
        //[StringLength(256, ErrorMessage = "The Image name cannot exceed 2083 characters. ")]
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
        //[StringLength(300,ErrorMessage = "The value cannot exceed 300 characters. ")]
        public string Mnemonic { get; set; }
        public string Image
        {
            get
            {
                string image = Image_1_Device ?? Image_1_Cloud ?? string.Empty;
                return image.Length == 0 ? string.Empty : image;
            }
            set
            {
                Image_1_Device = value;
                OnPropertyChanged("Image_1_Device");
                OnPropertyChanged("Image");
            }
        }
    }
}
