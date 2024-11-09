namespace SnackHub.OrderService.Api.Configuration
{
    public record AuthLambdaSettings
    {
        public string SignUpUrl { get; set; }
        public string SignInUrl { get; set; }
    }
}
