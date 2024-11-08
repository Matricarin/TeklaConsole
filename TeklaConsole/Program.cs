namespace TeklaConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var launcher = new MacrosLauncher(MacrosEnum.BoltNutSinglePartSorter);
            launcher.Perform();
        }
    }
}