using System.ComponentModel.DataAnnotations;

namespace OtpImplementation.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [RegularExpression("^[a-zA-Z]+(?: [a-zA-Z]+)*$")]
        public string Name { get; set; }

        [Required(ErrorMessage = "EmailId cannot be empty")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid Email address")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,16}$", ErrorMessage = "Invalid Password")]
        public string Password { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string PhoneNumber { get; set; }
        public int OrganisationId { get; set; }
        public int RoleId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public string ModifiedBy { get; set; } //Not entered by User
        public DateTime ModifiedDate { get; set; }
        public OrganisationModel Organisation { get; set; }
        public RoleModel Role { get; set; }
    }
}
