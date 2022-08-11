namespace VrChatBouncerBot.Client
{
    internal class ApiClientOptions
    {
        public string ApiKey { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string BasePath { get; set; } = null!;
    }
}
