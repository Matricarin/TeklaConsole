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

            if (!model.GetConnectionStatus())
                throw new Exception("I lost connection with Tekla.");

            ArrayList boltsWithOneNutCollection = new ArrayList();

            TMUI.ModelObjectSelector selector = new TMUI.ModelObjectSelector();
            TSM.ModelObjectEnumerator modelObjectsEnumerator = selector.GetSelectedObjects();

            if (modelObjectsEnumerator.GetSize() == 0)
            {
                TSMO.Operation.DisplayPrompt("You don't select any part.");
                return;
            }

            TSMO.Operation.DisplayPrompt("I am collecting bolts from parts.");

            while (modelObjectsEnumerator.MoveNext())
            {
                TSM.Part part = modelObjectsEnumerator.Current as TSM.Part;
                if (part != null)
                {
                    TSM.ModelObjectEnumerator boltsEnumerator = part.GetBolts();
                    while (boltsEnumerator.MoveNext())
                    {
                        TSM.BoltGroup boltGroup = boltsEnumerator.Current as TSM.BoltGroup;
                        if (boltGroup != null)
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

            TSMO.Operation.DisplayPrompt("I am looking for bolts with one nut.");

            foreach (object obj in boltsWithOneNutCollection)
            {
                TSM.BoltGroup bolt = obj as TSM.BoltGroup;
                if (bolt != null)
                {
                    allBoltedParts.Add(bolt.PartToBoltTo);
                    allBoltedParts.Add(bolt.PartToBeBolted);
                    allBoltedParts.AddRange(bolt.OtherPartsToBolt);
                }
            }

            ArrayList result = new ArrayList();


            TSMO.Operation.DisplayPrompt("I am looking for single-part assemblies.");

            foreach (object obj in allBoltedParts)
            {
                TSM.Part part = obj as TSM.Part;
                if (part != null)
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
                TSMO.Operation.DisplayPrompt("I find " + result.Count + " single-part assemblies.");
            }
        }
    }
}