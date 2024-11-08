using Tekla.Structures.Model;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model.Operations;

namespace TeklaConsole.DrawingServices
{
    internal class DeleteViews : IMacrosStrategy
    {
        public void Run()
        {
            var model = new Model();
            var MyDrawingHandler = new DrawingHandler();
            if (MyDrawingHandler.GetConnectionStatus())
            {
                var SelectedDrawings = MyDrawingHandler.GetDrawingSelector().GetSelected();
                while (SelectedDrawings.MoveNext())
                {
                    var views = SelectedDrawings.Current.GetSheet().GetAllViews();
                    while (views.MoveNext())
                        if (views.Current is View)
                            views.Current.Delete();
                }
            }

            Operation.DisplayPrompt("Done.");
            model.CommitChanges();
        }
    }
}