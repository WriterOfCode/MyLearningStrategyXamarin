using System;
namespace MyLearningStrategyMobleXForms.Models
{
    public class UserProfilesDTO : IUserProfilesDTO
    {
        public int UserProfileId { get; set; }
        //[StringLength(450, ErrorMessage = "The ExternalID Name value cannot exceed 450 characters. ")]
        public string ExternalID { get; set; }
        //[StringLength(256, ErrorMessage = "The Display Name value cannot exceed 256 characters. ")]
        public string DisplayName { get; set; }
        //[EmailAddressAttribute]
        public string EmailAddress { get; set; }
        //[StringLength(256, ErrorMessage = "The First Name value cannot exceed 256 characters. ")]
        public string FirstName { get; set; }
        //[StringLength(256, ErrorMessage = "The Last Name value cannot exceed 256 characters. ")]
        public string LastName { get; set; }
        //[StringLength(10, ErrorMessage = "The Postal Code value cannot exceed 10 characters. ")]
        public string PostalCode { get; set; }
        //[StringLength(2083, ErrorMessage = "The Identity Provider value cannot exceed 2083 characters. ")]
        public string IdentityProvider { get; set; }
        public Guid Originator { get; set; }
        public string ImageDevice { get; set; }
        public string ImageCloud { get; set; }
        public int ImageHash { get; set; }
        public bool HasLoggedIn { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset LastModifiedOffset { get; set; }
    }
}
