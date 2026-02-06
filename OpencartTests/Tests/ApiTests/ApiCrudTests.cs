using Allure.NUnit;
using Allure.NUnit.Attributes;
using Bogus;
using RestSharp;
using System.Net;

namespace OpencartTests.Tests.ApiTests;

/// <summary>
/// API CRUD Tests - Tests run sequentially (POST -> GET -> PUT -> GET)
/// Uses RestSharp for HTTP requests and Bogus for test data generation
/// </summary>
[TestFixture]
[Category("API")]
[AllureNUnit]
[AllureSuite("API Tests")]
public class ApiCrudTests
{
    private RestClient _client = null!;
    private string _createdObjectId = string.Empty;
    private string _expectedName = string.Empty;
    private string _expectedEmail = string.Empty;
    private int _expectedAge;
    
    private const string BaseUrl = "https://api.restful-api.dev";
    
    [SetUp]
    public void Setup()
    {
        _client = new RestClient(BaseUrl);
    }
    
    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
    }
    
    [Test, Order(1)]
    public async Task Test01_POST_CreateUserObject_ReturnsSuccess()
    {
        // Generate random user data using Bogus
        var faker = new Faker();
        _expectedName = faker.Name.FullName();
        _expectedEmail = faker.Internet.Email();
        _expectedAge = faker.Random.Int(18, 65);
        
        var requestBody = new
        {
            name = _expectedName,
            data = new
            {
                email = _expectedEmail,
                age = _expectedAge,
                occupation = faker.Name.JobTitle()
            }
        };
        
        var request = new RestRequest("/objects", Method.Post);
        request.AddJsonBody(requestBody);
        
        var response = await _client.ExecuteAsync(request);
        
        // Verify status code
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            $"Expected 200 OK but got {(int)response.StatusCode} {response.StatusCode}");
        
        Assert.That(response.Content, Is.Not.Null.And.Not.Empty);
        
        // Extract object ID from response
        var responseData = System.Text.Json.JsonDocument.Parse(response.Content!);
        _createdObjectId = responseData.RootElement.GetProperty("id").GetString()!;
        
        Assert.That(_createdObjectId, Is.Not.Null.And.Not.Empty,
            "Object ID should be returned in the response");
        
        TestContext.Out.WriteLine($"✓ Created object with ID: {_createdObjectId}");
    }
    
    [Test, Order(2)]
    public async Task Test02_GET_RetrieveCreatedObject_ReturnsCorrectData()
    {
        Assert.That(_createdObjectId, Is.Not.Null.And.Not.Empty,
            "Object ID must be set by the POST test");
        
        var request = new RestRequest($"/objects/{_createdObjectId}", Method.Get);
        var response = await _client.ExecuteAsync(request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content, Is.Not.Null.And.Not.Empty);
        
        // Validate returned data matches what I created
        var responseData = System.Text.Json.JsonDocument.Parse(response.Content!);
        var root = responseData.RootElement;
        
        var returnedId = root.GetProperty("id").GetString();
        Assert.That(returnedId, Is.EqualTo(_createdObjectId));
        
        var returnedName = root.GetProperty("name").GetString();
        Assert.That(returnedName, Is.EqualTo(_expectedName));
        
        if (root.TryGetProperty("data", out var dataElement))
        {
            var returnedEmail = dataElement.GetProperty("email").GetString();
            var returnedAge = dataElement.GetProperty("age").GetInt32();
            
            Assert.That(returnedEmail, Is.EqualTo(_expectedEmail));
            Assert.That(returnedAge, Is.EqualTo(_expectedAge));
            
            TestContext.Out.WriteLine($"✓ Retrieved object and validated data");
        }
        else
        {
            Assert.Fail("Response should contain 'data' property");
        }
    }
    
    [Test, Order(3)]
    public async Task Test03_PUT_UpdateObjectWithAdditionalFields_ReturnsSuccess()
    {
        Assert.That(_createdObjectId, Is.Not.Null.And.Not.Empty);
        
        // Generate additional data for update
        var faker = new Faker();
        var updatedPhone = faker.Phone.PhoneNumber();
        var updatedCity = faker.Address.City();
        
        // Include existing fields plus new fields (PUT replaces entire resource)
        var updateBody = new
        {
            name = _expectedName,
            data = new
            {
                email = _expectedEmail,
                age = _expectedAge,
                phone = updatedPhone,
                city = updatedCity,
                accountStatus = "Premium",
                lastUpdated = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            }
        };
        
        var request = new RestRequest($"/objects/{_createdObjectId}", Method.Put);
        request.AddJsonBody(updateBody);
        
        var response = await _client.ExecuteAsync(request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content, Is.Not.Null.And.Not.Empty);
        
        var responseData = System.Text.Json.JsonDocument.Parse(response.Content!);
        var root = responseData.RootElement;
        
        var returnedId = root.GetProperty("id").GetString();
        Assert.That(returnedId, Is.EqualTo(_createdObjectId));
        
        Assert.That(root.TryGetProperty("updatedAt", out _), Is.True);
        
        TestContext.Out.WriteLine($"✓ Updated object {_createdObjectId}");
    }
    
    /// <summary>
    /// Reuse GET test to verify updated data persists (DRY principle)
    /// Tests actual persistence, not just PUT response
    /// </summary>
    [Test, Order(4)]
    public async Task Test04_GET_VerifyUpdatedDataPersists_ReturnsUpdatedObject()
    {
        Assert.That(_createdObjectId, Is.Not.Null.And.Not.Empty);
        
        var request = new RestRequest($"/objects/{_createdObjectId}", Method.Get);
        var response = await _client.ExecuteAsync(request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content, Is.Not.Null.And.Not.Empty);
        
        var responseData = System.Text.Json.JsonDocument.Parse(response.Content!);
        var root = responseData.RootElement;
        
        var returnedId = root.GetProperty("id").GetString();
        var returnedName = root.GetProperty("name").GetString();
        
        Assert.That(returnedId, Is.EqualTo(_createdObjectId));
        Assert.That(returnedName, Is.EqualTo(_expectedName));
        
        if (root.TryGetProperty("data", out var dataElement))
        {
            var returnedEmail = dataElement.GetProperty("email").GetString();
            var returnedAge = dataElement.GetProperty("age").GetInt32();
            
            Assert.That(returnedEmail, Is.EqualTo(_expectedEmail));
            Assert.That(returnedAge, Is.EqualTo(_expectedAge));
            
            // Verify updated fields persisted
            var hasPhone = dataElement.TryGetProperty("phone", out _);
            var hasCity = dataElement.TryGetProperty("city", out _);
            
            Assert.That(hasPhone || hasCity, Is.True,
                "Updated fields should be persisted");
            
            TestContext.Out.WriteLine($"✓ Verified data persistence after update");
            TestContext.Out.WriteLine($"  CRUD cycle complete: POST → GET → PUT → GET");
        }
        else
        {
            Assert.Fail("Response should contain 'data' property");
        }
    }
}
