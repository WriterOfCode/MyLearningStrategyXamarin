using System;

namespace MyLearningStrategyMobleXForms.Models
{
    public class UserPermissionsDTO
    {
        //[KeyAttribute]
        public int PermissionsId { get; set; }
        //[KeyAttribute]
        public Guid Originator { get; set; }
        //[KeyAttribute]
        //[StringLength(250, ErrorMessage = "The Claim Type cannot exceed 250 characters. ")]
        public string ClaimType { get; set; }
        //[KeyAttribute]
        //[StringLength(250, ErrorMessage = "The Claim value cannot exceed 250 characters. ")]
        public string ClaimValue { get; set; }
    }
}
