namespace Cau2_QuanCaPhe;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        var form = new FormMain();

        if (args.Contains("--test"))
        {
            form.Shown += async (s, e) =>
            {
                await Task.Delay(1500);
                await form.RunAutomatedTestAsync();
                Environment.Exit(0);
            };
        }

        Application.Run(form);
    }    
}