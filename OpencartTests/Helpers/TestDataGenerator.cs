using Bogus;

namespace OpencartTests.Helpers
{
    /// <summary>
    /// Simple test data generator using Bogus (Faker for C#)
    /// Generates unique data for each test run
    /// </summary>
    public class TestDataGenerator
    {
        private readonly Faker _faker;

        public TestDataGenerator()
        {
            _faker = new Faker();
        }

        /// <summary>
        /// Generate random user data for registration
        /// Each call returns NEW unique data
        /// </summary>
        public UserRegistrationData GenerateRandomUser()
        {
            return new UserRegistrationData
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                Telephone = _faker.Phone.PhoneNumber("###-###-####"),
                Password = "Test@123456" // Simple fixed password for testing
            };
        }
    }

    /// <summary>
    /// Model for user registration data
    /// </summary>
    public class UserRegistrationData
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
