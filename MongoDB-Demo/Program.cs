using MongoDataAccess.DataAccess;
using MongoDataAccess.Models;

ChoreDataAccess db = new ChoreDataAccess();
await db.CreateUser(new UserModel() {
    FirstName= "Andre",
    LastName= "Jackson"
});

await db.CreateUser(new UserModel()
{
    FirstName = "Paul",
    LastName = "Walker"
});

await db.CreateUser(new UserModel()
{
    FirstName = "Tyrone",
    LastName = "Samuel"
});

var users = await db.GetAllUsers();

foreach(var user in users)
{
    Console.WriteLine($"{user.Id}: {user.FirstName} {user.LastName}");
}

var chore = new ChoreModel() {
    AssignedTo = users.First(u => u.FirstName == "Paul"),
    FrequencyInDays = 10,
    ChoreText = "Wash the Car"
};

await db.CreateChore(chore);