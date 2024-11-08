using System;
using TeklaConsole.DrawingServices;
using TeklaConsole.ModelServices;

namespace TeklaConsole
{
    internal class MacrosLauncher
    {
        private readonly IMacrosStrategy _executionAlgorithm;

        public MacrosLauncher(MacrosEnum concreteMacros)
        {
            switch (concreteMacros)
            {
                case MacrosEnum.BoltNutSinglePartSorter:
                    _executionAlgorithm = new BoltNutSinglePartSorter();
                    break;
                case MacrosEnum.DeleteViews:
                    _executionAlgorithm = new DeleteViews();
                    break;
                default:
                    throw new ArgumentException(nameof(concreteMacros), concreteMacros.ToString(), null);
            }
        }

        public void Perform()
        {
            try
            {
                _executionAlgorithm.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}