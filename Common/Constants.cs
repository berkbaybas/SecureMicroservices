namespace Common
{
    public static class Constants
    {
        public const string MoviesApiScope = "MoviesApiScope";

        public const string MoviesClient = "movies_client"; // postman
        public const string MoviesMcvClient = "movies_mvc_client"; // mcv
        public const string MoviesCustomClient = "movies_custom_client"; // custom jwt - Postman
        
        public const string SecretKey = "your_generated_base64_key_here_123";
        public const string MoviesApiUrl = "https://localhost:5001";
        public const string MoviesMcvClientUrl = "https://localhost:5002";
        public const string CustomJwtServerUrl = "https://localhost:5003";
        public const string IdentityServerUrl = "https://localhost:5005";
    }
}
