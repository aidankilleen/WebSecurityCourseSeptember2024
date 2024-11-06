using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SecurityCourseIntial.Models;
using System.Security.Cryptography;
using System.Text;

namespace SecurityCourseIntial.Controllers
{
    public class UserLoginController : Controller
    {
        private readonly string _connectionString = "Server=tcp:database.windows.net,1433;Initial Catalog=Northwind;Persist Security Info=False;User ID=;Password=;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public IActionResult Index()
        {
            return View();
        }

        public async Task<bool> AuthenticateLoginWithHash(UserLogin login)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                // read the Salt from the database for this user


                // regenerate the hash using password just entered and the salt from the database


                // compare the hash just generated with the hash in the database









                conn.Open();







                //string query = $"SELECT COUNT(*) FROM UserLogins WHERE username = @Username AND epassword = @EPassword";


                string query = $"SELECT COUNT(*) FROM UserLogins WHERE username = '{login.Username}' AND password = '{login.Password}'";

                //string query = $"SELECT COUNT(*) FROM UserLogins WHERE username = @Username AND epassword = @EPassword";
                Console.WriteLine(query);

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    //cmd.Parameters.AddWithValue("@Username", login.Username);
                    //cmd.Parameters.AddWithValue("@EPassword", login.EPassword);

                    int result = (int)cmd.ExecuteScalar();
                    return result > 0;
                }
            }
        }

        public async Task<bool> AuthenticateLogin(UserLogin login)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = $"SELECT COUNT(*) FROM UserLogins WHERE username = '{login.Username}' AND password = '{login.Password}'";

                //string query = $"SELECT COUNT(*) FROM UserLogins WHERE username = @Username AND password = @Password";
                Console.WriteLine(query);

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", login.Username);
                    cmd.Parameters.AddWithValue("@Password", login.Password);

                    int result = (int)cmd.ExecuteScalar();
                    return result > 0;
                }
            }
        }
        private async Task<UserLogin> GetUserByUsernameAsync(string username)
        {
            UserLogin user = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT username, epassword, salt FROM Users WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new UserLogin
                            {
                                Username = reader.GetString(0),
                                EPassword = reader.GetString(1),
                                Salt = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            return user;
        }
        public IActionResult ProtectedPage()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Index", "Home");
            } else
            {
                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin login)
        {
            // check the username and password from the database
            bool isAuthenticated = await AuthenticateLogin(login);

            if (isAuthenticated)
            {
                HttpContext.Session.SetString("username", login.Username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

            /*
            if (ModelState.IsValid)
            {
                // check if user exists in database
                // if yes, redirect to home page
                return View();
                // if no, return error message
            }
            return View(login);
            */
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register() { 
            return View(); 
        }

        [HttpPost]
        public IActionResult Register(UserLogin login)
        {

            // generate a hash
            var salt = GenerateSalt();
            login.Salt = Convert.ToBase64String(salt);

            login.EPassword = ComputeSha256Hash(login.Password, salt);

            // save the hash to the database
            Console.WriteLine(login.Salt);
            Console.WriteLine(login.EPassword);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                //string query = $"SELECT COUNT(*) FROM UserLogins WHERE username = '{login.Username}' AND password = '{login.Password}'";

                string query = $"INSERT INTO UserLogins (username, password, epassword, salt) VALUES(@Username, @Password, @EPassword, @Salt)";
                Console.WriteLine(query);

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", login.Username);
                    cmd.Parameters.AddWithValue("@Password", login.Password);
                    cmd.Parameters.AddWithValue("@EPassword", login.EPassword);
                    cmd.Parameters.AddWithValue("@Salt", login.Salt);

                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index", "LoginUser");
                }
            }





            return View(login);
        }

        public static byte[] GenerateSalt(int size = 32)
        {
            var salt = new byte[size];
            RandomNumberGenerator.Fill(salt); // This fills the byte array with cryptographically strong random bytes
            return salt;
        }
        public static string ComputeSha256Hash(string input, byte[] salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Combine input string and salt
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] saltedInput = new byte[inputBytes.Length + salt.Length];
                Buffer.BlockCopy(inputBytes, 0, saltedInput, 0, inputBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedInput, inputBytes.Length, salt.Length);

                // Compute hash
                byte[] hashBytes = sha256.ComputeHash(saltedInput);

                // Convert hash bytes to string
                return Convert.ToBase64String(hashBytes);
            }
        }
    }

}
