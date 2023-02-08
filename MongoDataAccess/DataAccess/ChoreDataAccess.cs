using MongoDataAccess.Models;
using MongoDB.Driver;
using System.Collections;

namespace MongoDataAccess.DataAccess
{
    public class ChoreDataAccess
    {
        private readonly string connectionString = Environment.GetEnvironmentVariable("MongoDBConnection");
        private readonly string databaseName = "choredb";
        private readonly string Chorecollection = "chore_chart";
        private readonly string Usercollection = "users";
        private readonly string ChoreHistorycollection = "chore_history";

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);
            return db.GetCollection<T>(collection);
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            var usersCollection = ConnectToMongo<UserModel>(Usercollection);
            var results = await usersCollection.FindAsync(_=> true);
            return results.ToList();
        }

        public async Task<List<ChoreModel>> GetAllChores()
        {
            var choresCollection = ConnectToMongo<ChoreModel>(Chorecollection);
            var results = await choresCollection.FindAsync(_ => true);
            return results.ToList();
        }

        public async Task<List<ChoreHistoryModel>> GetAllChoreHistory()
        {
            var choreshistCollection = ConnectToMongo<ChoreHistoryModel>(ChoreHistorycollection);
            var results = await choreshistCollection.FindAsync(_ => true);
            return results.ToList();
        }

        public async Task<List<ChoreModel>> GetAllChoresForUser(UserModel user)
        {
            var choresCollection = ConnectToMongo<ChoreModel>(Chorecollection);
            var results = await choresCollection.FindAsync(x => x.AssignedTo.Id == user.Id);
            return results.ToList();
        }

        public Task CreateUser(UserModel user)
        {
            var usersCollection = ConnectToMongo<UserModel>(Usercollection);
            return usersCollection.InsertOneAsync(user);
        }

        public Task CreateChore(ChoreModel chore)
        {
            var choresCollection = ConnectToMongo<ChoreModel>(Chorecollection);
            return choresCollection.InsertOneAsync(chore);
        }

        public Task UpdateChore(ChoreModel chore)
        {
            var choresCollection = ConnectToMongo<ChoreModel>(Chorecollection);
            var filter = Builders<ChoreModel>.Filter.Eq("Id",chore.Id);
            return choresCollection.ReplaceOneAsync(filter,chore, new ReplaceOptions { IsUpsert= true });
        }

        public Task DeleteChore(ChoreModel chore)
        {
            var choresCollection = ConnectToMongo<ChoreModel>(Chorecollection);
            return choresCollection.DeleteOneAsync(c => c.Id == chore.Id);
        }

        public async Task CompleteChore(ChoreModel chore)
        {
            //var choresCollection = ConnectToMongo<ChoreModel>(Chorecollection);
            //var filter = Builders<ChoreModel>.Filter.Eq("Id", chore.Id);
            //await choresCollection.ReplaceOneAsync(filter, chore);

            //var choreshistCollection = ConnectToMongo<ChoreHistoryModel>(ChoreHistorycollection);
            //await choreshistCollection.InsertOneAsync(new ChoreHistoryModel(chore));
            var client = new MongoClient(connectionString);
            using var session = await client.StartSessionAsync();

            session.StartTransaction();
            try
            {
                var db = client.GetDatabase(databaseName);
                var choreCollection = db.GetCollection<ChoreModel>(Chorecollection);
                var filter = Builders<ChoreModel>.Filter.Eq("Id", chore.Id);
                await choreCollection.ReplaceOneAsync(filter, chore);

            }
            catch (Exception ex)
            {
                
            }

        }
    }
}