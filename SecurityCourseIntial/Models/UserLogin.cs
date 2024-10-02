namespace SecurityCourseIntial.Models
{
    public class UserLogin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string EPassword { get; set; }

        public string Salt { get; set; }
    }
}
