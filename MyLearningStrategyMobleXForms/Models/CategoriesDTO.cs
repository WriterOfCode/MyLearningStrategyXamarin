namespace MyLearningStrategyMobleXForms.Models
{
    public class CategoriesDTO : BaseDTO
    {
        public int CategoryId { get; set; }
        public int UserProfileId { get; set; }
        public string CategoryName { get; set; }
       // [StringLength(256, ErrorMessage = "The Image path value cannot exceed 256 characters. ")]
        public string ImageDevice { get; set; }
        //[UrlAttribute, StringLength(2083, ErrorMessage = "The Image Url value cannot exceed 2083 characters. ")]
        public string ImageCloud { get; set; }
        public int ImageHash { get; set; }
    }
}
