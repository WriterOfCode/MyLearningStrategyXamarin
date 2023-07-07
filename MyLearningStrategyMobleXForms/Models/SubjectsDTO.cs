//using System.ComponentModel.DataAnnotations;

namespace MyLearningStrategyMobleXForms.Models
{
    public class SubjectsDTO : BaseDTO
    {
        //[KeyAttribute]
        public int BodyOfKnowledgeId { get; set; }
        public int UserProfileId { get; set; }
        //[StringLength(150, ErrorMessage = "The Name cannot exceed 150 characters. ")]
        public string Name { get; set; }
        //[StringLength(50, ErrorMessage = "The Acronym cannot exceed 50 characters. ")]
        public string Description { get; set; }
        //[StringLength(100, ErrorMessage = "The Key words cannot exceed 100 characters. ")]
        public string Keywords { get; set; }
        //[StringLength(256, ErrorMessage = "The local image name cannot exceed 100 characters. ")]
        string imageDevice = string.Empty;
        public string ImageDevice
        {
            get { return imageDevice; }
            set { SetProperty(ref imageDevice, value); }
        }
        //public string ImageDevice { get; set; }
        //[StringLength(2083, ErrorMessage = "The cloud image url cannot exceed 2083 characters. ")]
        string imageCloud = string.Empty;
        public string ImageCloud
        {
            get { return imageCloud; }
            set { SetProperty(ref imageCloud, value); }
        }
        public int ImageHash { get; set; }
        public bool IsShared { get; set; }
        public bool HasBeenShared { get; set; }

        public override string ToString()
        {
            return "Subject, " + this.Name + ", Description, " + this.Description + ",  Key words " + Keywords;
        }
    }
}
