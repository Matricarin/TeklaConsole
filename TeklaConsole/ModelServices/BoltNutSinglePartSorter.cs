using System;
using System.Collections;
using TSM = Tekla.Structures.Model;
using TMUI = Tekla.Structures.Model.UI;
using TSMO = Tekla.Structures.Model.Operations;

namespace TeklaConsole.ModelServices
{
    internal class BoltNutSinglePartSorter : IMacrosStrategy
    {
        public void Run()
        {
            TSM.Model model = new TSM.Model();

            if (model.GetConnectionStatus())
                throw new Exception("I lost connection with Tekla.");

            ArrayList boltsWithOneNutCollection = new ArrayList();

            TMUI.ModelObjectSelector selector = new TMUI.ModelObjectSelector();
            TSM.ModelObjectEnumerator modelObjectsEnumerator = selector.GetSelectedObjects();

            if (modelObjectsEnumerator.GetSize() == 0)
            {
                TSMO.Operation.DisplayPrompt("You don't select any part.");
                return;
            }

            while (modelObjectsEnumerator.MoveNext())
            {
                if (modelObjectsEnumerator.Current is TSM.Part part)
                {
                    TSM.ModelObjectEnumerator boltsEnumerator = part.GetBolts();
                    while (boltsEnumerator.MoveNext())
                    {
                        if (boltsEnumerator.Current is TSM.BoltGroup boltGroup)
                        {
                            if (boltGroup.Nut1 == true && boltGroup.Nut2 == false)
                            {
                                boltsWithOneNutCollection.Add(boltGroup);
                            }
                        }
                    }
                }
            }

            ArrayList allBoltedParts = new ArrayList();

            foreach (object obj in boltsWithOneNutCollection)
            {
                if (obj is TSM.BoltGroup bolt)
                {
                    allBoltedParts.Add(bolt.PartToBoltTo);
                    allBoltedParts.Add(bolt.PartToBeBolted);
                    allBoltedParts.AddRange(bolt.OtherPartsToBolt);
                }
            }

            ArrayList result = new ArrayList();

            foreach (object obj in allBoltedParts)
            {
                if (obj is TSM.Part part)
                {
                    TSM.Assembly assembly = part.GetAssembly();
                    int secondariesPartsCount = assembly.GetSecondaries().Count;
                    if (secondariesPartsCount == 0)
                        result.Add(assembly);
                }
            }

            if (result.Count == 0)
            {
                TSMO.Operation.DisplayPrompt("I don't find single-part assemblies.");
            }
            else
            {
                selector.Select(result);
                TSMO.Operation.DisplayPrompt($"I find {result.Count} single-part assemblies.");
            }
        }
    }
}