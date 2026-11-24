namespace ScholarshipManagementSystem_27109
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Database.InitializeDatabase();

            Application.Run(new LoginForm());
        }
    }
}