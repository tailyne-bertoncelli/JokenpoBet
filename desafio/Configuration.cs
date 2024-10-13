namespace desafio
{
    public class Configuration
    {
        public static string JwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        public static string ApiKeyName = Environment.GetEnvironmentVariable("API_KEY");
        public static string ApiKey = Environment.GetEnvironmentVariable("API_KEY_NAME");
    }
}
