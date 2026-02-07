using OpencartTests.Helpers;

namespace OpencartTests.Tests.DatabaseTests;

// Base class — creates fresh in-memory database before each test
public class BaseDatabaseTest
{
    protected DbHelper Db = null!;

    private const string CreatePoliciesTable = @"
        CREATE TABLE Policies (
            PolicyId       INTEGER PRIMARY KEY AUTOINCREMENT,
            PolicyNumber   TEXT NOT NULL,
            CustomerName   TEXT NOT NULL,
            Premium        DECIMAL NOT NULL,
            EffectiveDate  TEXT NOT NULL
        )";

    private const string CreateClaimsTable = @"
        CREATE TABLE Claims (
            ClaimId      INTEGER PRIMARY KEY AUTOINCREMENT,
            PolicyId     INTEGER NOT NULL,
            ClaimAmount  DECIMAL NOT NULL,
            ClaimDate    TEXT NOT NULL,
            Status       TEXT NOT NULL,
            FOREIGN KEY (PolicyId) REFERENCES Policies(PolicyId) ON DELETE CASCADE
        )";

    [SetUp]
    public void BaseSetUp()
    {
        Db = new DbHelper();
        Db.OpenConnection("Data Source=:memory:");

        // Enable foreign key enforcement (SQLite has it off by default)
        Db.ExecuteNonQuery("PRAGMA foreign_keys = ON");

        // Create tables fresh for each test
        Db.ExecuteNonQuery(CreatePoliciesTable);
        Db.ExecuteNonQuery(CreateClaimsTable);
    }

    [TearDown]
    public void BaseTearDown()
    {
        // Closing in-memory connection destroys the database
        Db.CloseConnection();
        Db.Dispose();
    }
}
