using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using TZCBRF.Models;

namespace TZCBRF.Services
{
    public class TaskDatabaseService
    {
        readonly SQLite.SQLiteAsyncConnection _database;

        public TaskDatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ParticipantInfo>().Wait();
            _database.CreateTableAsync<RstrList>().Wait();
            _database.CreateTableAsync<Accounts>().Wait();
            _database.CreateTableAsync<BICDirectoryEntry>().Wait();
        }

        public Task<List<BICDirectoryEntry>> GetBICDirectoryEntryAsync()
        {
            return _database.Table<BICDirectoryEntry>().ToListAsync();
        }

        public Task<List<ParticipantInfo>> GetParticipantInfoAsync()
        {
            return _database.Table<ParticipantInfo>().ToListAsync();
        }

        public Task<int> ParticipantInfoSaveTaskAsync(ParticipantInfo participantInfo)
        {
            return _database.InsertAsync(participantInfo);
        }
        public Task<int> RstrListSaveTaskAsync(RstrList rstrList)
        {
            return _database.InsertAsync(rstrList);
        }
        public Task<int> AccountsSaveTaskAsync(Accounts accounts)
        {
            return _database.InsertAsync(accounts);
        }
        public Task<int> BICDirectoryEntrySaveTaskAsync(BICDirectoryEntry BICDirectoryEntrys)
        {
            return _database.InsertAsync(BICDirectoryEntrys);
        }
    }
}
