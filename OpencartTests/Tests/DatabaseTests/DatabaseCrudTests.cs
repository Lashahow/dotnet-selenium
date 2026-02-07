using Allure.NUnit;
using Allure.NUnit.Attributes;

namespace OpencartTests.Tests.DatabaseTests;

// Database CRUD Tests — INSERT, UPDATE, DELETE, JOIN with assertions
[TestFixture]
[Category("Database")]
[AllureNUnit]
[AllureSuite("Database Tests")]
public class DatabaseCrudTests : BaseDatabaseTest
{
    [Test, Order(0)]
    public void Test00_VerifyTablesExist_SchemaIsCorrect()
    {
        // Check both tables exist in sqlite_master
        var count = Convert.ToInt32(
            Db.ExecuteScalar("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name IN ('Policies','Claims')"));

        Assert.That(count, Is.EqualTo(2), "Both Policies and Claims tables should exist");

        TestContext.Out.WriteLine("✓ Verified: Policies and Claims tables created successfully");
    }

    [Test, Order(1)]
    public void Test01_InsertPolicy_VerifyWithQuery()
    {
        // Arrange — insert a policy
        Db.ExecuteNonQuery(@"
            INSERT INTO Policies (PolicyNumber, CustomerName, Premium, EffectiveDate)
            VALUES ('POL-001', 'John Smith', 1500.00, '2025-01-15')");

        // Act — read it back
        using var reader = Db.ExecuteQuery("SELECT * FROM Policies WHERE PolicyNumber = 'POL-001'");

        // Assert
        Assert.That(reader.Read(), Is.True, "Should find the inserted policy");

        var customerName = reader.GetString(reader.GetOrdinal("CustomerName"));
        var premium = reader.GetDecimal(reader.GetOrdinal("Premium"));

        Assert.That(customerName, Is.EqualTo("John Smith"));
        Assert.That(premium, Is.EqualTo(1500.00m));

        TestContext.Out.WriteLine($"✓ Inserted and verified policy: POL-001, Customer: {customerName}, Premium: {premium}");
    }

    [Test, Order(2)]
    public void Test02_UpdatePolicyPremium_VerifyChange()
    {
        // Arrange
        Db.ExecuteNonQuery(@"
            INSERT INTO Policies (PolicyNumber, CustomerName, Premium, EffectiveDate)
            VALUES ('POL-002', 'Jane Doe', 2000.00, '2025-03-01')");

        // Act — update premium
        var rowsAffected = Db.ExecuteNonQuery(@"
            UPDATE Policies SET Premium = 2500.00 WHERE PolicyNumber = 'POL-002'");

        // Assert
        Assert.That(rowsAffected, Is.EqualTo(1), "UPDATE should affect exactly 1 row");

        var newPremium = Convert.ToDecimal(
            Db.ExecuteScalar("SELECT Premium FROM Policies WHERE PolicyNumber = 'POL-002'"));

        Assert.That(newPremium, Is.EqualTo(2500.00m), "Premium should be updated to 2500.00");

        TestContext.Out.WriteLine($"✓ Updated premium from 2000.00 → {newPremium}");
    }

    [Test, Order(3)]
    public void Test03_DeletePolicy_VerifyCascadeDelete()
    {
        // Arrange — insert policy with two claims
        Db.ExecuteNonQuery(@"
            INSERT INTO Policies (PolicyId, PolicyNumber, CustomerName, Premium, EffectiveDate)
            VALUES (100, 'POL-003', 'Bob Wilson', 3000.00, '2025-06-01')");

        Db.ExecuteNonQuery(@"
            INSERT INTO Claims (PolicyId, ClaimAmount, ClaimDate, Status)
            VALUES (100, 500.00, '2025-07-01', 'Open')");

        Db.ExecuteNonQuery(@"
            INSERT INTO Claims (PolicyId, ClaimAmount, ClaimDate, Status)
            VALUES (100, 750.00, '2025-08-15', 'Approved')");

        // Verify claims exist before delete
        var claimsBefore = Convert.ToInt32(
            Db.ExecuteScalar("SELECT COUNT(*) FROM Claims WHERE PolicyId = 100"));
        Assert.That(claimsBefore, Is.EqualTo(2), "Should have 2 claims before delete");

        // Act — delete parent policy
        Db.ExecuteNonQuery("DELETE FROM Policies WHERE PolicyId = 100");

        // Assert — cascade should remove child claims too
        var claimsAfter = Convert.ToInt32(
            Db.ExecuteScalar("SELECT COUNT(*) FROM Claims WHERE PolicyId = 100"));
        Assert.That(claimsAfter, Is.EqualTo(0),
            "CASCADE DELETE: all claims should be removed when policy is deleted");

        var policyCount = Convert.ToInt32(
            Db.ExecuteScalar("SELECT COUNT(*) FROM Policies WHERE PolicyId = 100"));
        Assert.That(policyCount, Is.EqualTo(0), "Policy itself should be deleted");

        TestContext.Out.WriteLine("✓ Deleted policy → CASCADE removed 2 child claims");
    }

    [Test, Order(4)]
    public void Test04_InsertPolicyAndClaim_VerifyWithJoinQuery()
    {
        // Arrange — insert policy and a related claim
        Db.ExecuteNonQuery(@"
            INSERT INTO Policies (PolicyId, PolicyNumber, CustomerName, Premium, EffectiveDate)
            VALUES (200, 'POL-004', 'Alice Johnson', 4000.00, '2025-01-01')");

        Db.ExecuteNonQuery(@"
            INSERT INTO Claims (PolicyId, ClaimAmount, ClaimDate, Status)
            VALUES (200, 1200.00, '2025-02-20', 'Under Review')");

        // Act — JOIN to get policy + claim data together
        using var reader = Db.ExecuteQuery(@"
            SELECT p.PolicyNumber, p.CustomerName, c.ClaimAmount, c.Status
            FROM Policies p
            INNER JOIN Claims c ON p.PolicyId = c.PolicyId
            WHERE p.PolicyNumber = 'POL-004'");

        // Assert
        Assert.That(reader.Read(), Is.True, "JOIN should return a result");

        var policyNumber = reader.GetString(reader.GetOrdinal("PolicyNumber"));
        var customerName = reader.GetString(reader.GetOrdinal("CustomerName"));
        var claimAmount = reader.GetDecimal(reader.GetOrdinal("ClaimAmount"));
        var status = reader.GetString(reader.GetOrdinal("Status"));

        Assert.That(policyNumber, Is.EqualTo("POL-004"));
        Assert.That(customerName, Is.EqualTo("Alice Johnson"));
        Assert.That(claimAmount, Is.EqualTo(1200.00m));
        Assert.That(status, Is.EqualTo("Under Review"));

        TestContext.Out.WriteLine($"✓ JOIN verified: Policy {policyNumber} ({customerName}) → Claim: {claimAmount:C}, Status: {status}");
    }
}
