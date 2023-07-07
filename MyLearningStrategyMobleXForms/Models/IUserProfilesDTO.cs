using System;

namespace MyLearningStrategyMobleXForms.Models
{
    public interface IUserProfilesDTO
    {
        string DisplayName { get; set; }
        string EmailAddress { get; set; }
        string ExternalID { get; set; }
        string FirstName { get; set; }
        bool HasLoggedIn { get; set; }
        string IdentityProvider { get; set; }
        string ImageCloud { get; set; }
        string ImageDevice { get; set; }
        int ImageHash { get; set; }
        bool IsDeleted { get; set; }
        bool IsDisabled { get; set; }
        bool IsLocked { get; set; }
        DateTimeOffset LastModifiedOffset { get; set; }
        string LastName { get; set; }
        Guid Originator { get; set; }
        string PostalCode { get; set; }
        int UserProfileId { get; set; }
    }
}