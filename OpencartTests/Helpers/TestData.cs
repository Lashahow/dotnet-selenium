namespace OpencartTests.Helpers
{
    /// <summary>
    /// Simple static test data - no JSON, no configuration loading!
    /// Just plain C# constants that are easy to read and use
    /// </summary>
    public static class TestData
    {
        /// <summary>
        /// Test user credentials for login tests
        /// </summary>
        public static class Users
        {
            public static class Invalid
            {
                public const string Email = "invalid@test.com";
                public const string Password = "wrongpassword";
            }

            public static class Valid
            {
                public const string Email = "testuser@opencart.com";
                public const string Password = "ValidPassword123";
            }
        }

        /// <summary>
        /// Product names for search tests
        /// </summary>
        public static class Products
        {
            public const string MacBook = "MacBook";
            public const string iPhone = "iPhone";
            public const string Canon = "Canon EOS 5D";
        }

        /// <summary>
        /// Expected error messages
        /// </summary>
        public static class ErrorMessages
        {
            public const string InvalidLogin = "Warning";
            public const string RequiredField = "This field is required";
        }
    }
}
