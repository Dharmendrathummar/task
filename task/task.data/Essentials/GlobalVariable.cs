namespace task.data.Essentials
{
    public class GlobalVariable
    {

        public static bool IsDevelopment { get; set; } = true;
        public static int TokenExpiry { get; set; } = 30;
        public static string ConnectionString = "Server=(local)\\SQLEXPRESS;Database=task;User ID=sa;Password=1234;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true;Pooling=True;Encrypt=False;";
    
        public readonly static string JwtSecret = "Q1S7DCe94s5zT3xcv";
        public readonly static string JwtIssuer = "https://localhost:7013/";

        public readonly static string CookieDomain = "https://localhost:7013/";

        public static void SetVariable()
        {
            IsDevelopment = true;
        }
    }
}