using Microsoft.Data.SqlClient;
using SecurityCourseIntial.Models;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace SecurityCourseIntial.Repositories
{
    public class UserCommentRepository
    {

        private readonly string _connectionString;

        public UserCommentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<UserComment>> GetAll()
        {
            var comments = new List<UserComment>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT Id, Author, Content, CreatedAt FROM UserComments";
                var command = new SqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        comments.Add(new UserComment
                        {
                            Id = reader.GetInt32(0),
                            Author = reader.GetString(1),
                            Content = reader.GetString(2),
                            CreatedAt = reader.GetDateTime(3)
                        });
                    }
                }
            }
            return comments;
        }
        public async Task Add(UserComment comment)
        {
            comment.CreatedAt = DateTime.UtcNow;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO UserComments (Author, Content, CreatedAt) OUTPUT INSERTED.Id VALUES (@Author, @Content, @CreatedAt)";


                //var query = $"INSERT INTO UserComments (Author, Content) OUTPUT INSERTED.Id VALUES('{comment.Author}','{comment.Content}')";
                
                Console.Write(query);

                var command = new SqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@Author", comment.Author);
                command.Parameters.AddWithValue("@Content", comment.Content);
                command.Parameters.AddWithValue("@CreatedAt", comment.CreatedAt);
                
                await connection.OpenAsync();
                comment.Id = (int)await command.ExecuteScalarAsync(); // Get the new ID of the inserted row
            }
        }
    }
}
