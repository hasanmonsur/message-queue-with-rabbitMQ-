using Dapper;
using MessageQueueApp.Contacts;
using MessageQueueApp.Data;
using MessageQueueApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MessageQueueApp.Services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DapperContext _dapperContext;

        public MessageRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task SaveMessageAsync(string content)
        {
            using (var db = _dapperContext.CreateDbConnection())
            {
                var query = "INSERT INTO Messages (Content) VALUES (@Content)";
                await db.ExecuteAsync(query, new { Content = content });
            }
        }

        public async Task<IEnumerable<MessageModel>> GetMessagesAsync()
        {
            using (var db = _dapperContext.CreateDbConnection())
            {
                return await db.QueryAsync<MessageModel>("SELECT * FROM Messages");
            }
        }
    }
}
