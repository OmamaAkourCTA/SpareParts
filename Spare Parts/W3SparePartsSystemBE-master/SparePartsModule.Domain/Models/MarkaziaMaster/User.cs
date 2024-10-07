using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MarkaziaMaster.Domain.Models.MarkaziaMaster
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int? UserNo { get; set; }
        public string? FullName { get; set; }
        public string? FullNameAR { get; set; }
        public string? UserName1 { get; set; }
        public string? UserName4 { get; set; }
        public string? UserMobile { get; set; }
        public string? UserEmail { get; set; }
        public int? UserGender { get; set; }
        public DateTime? UserDOB { get; set; }
        public int? UserNationality { get; set; }
        public string? UserLogin { get; set; }
        public string? Password { get; set; }
        public int? UserType { get; set; }
        public int? MainBusinessAreaID { get; set; }
        public string? ProfileImage { get; set; }
        public string? Token { get; set; }
        public string? DeviceToken { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public TimeSpan? LastLogintime { get; set; }
        public string? Remarks { get; set; }
        public int? Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int? EnterUser { get; set; }
        public DateTime? EnterDate { get; set; }
        public TimeSpan? EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }



    }
}
