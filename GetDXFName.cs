        [CommandMethod("GetDXFName")]
        public void GetDXFName()
        {
            // Get Current Document and Database //
            Document doc = AcAp.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            using(Transaction tr = db.TransactionManager.StartTransaction())
            {
                // Request for obect(s) to be selected in the drawing area //
                PromptSelectionResult ssPrompt = ed.GetSelection();

                if (ssPrompt.Status == PromptStatus.OK)
                {
                    SelectionSet ss = ssPrompt.Value;

                    foreach(SelectedObject ssObj in ss)
                    {
                        ed.WriteMessage($"\ndxfName: {ssObj.ObjectId.ObjectClass.DxfName}, ObjectClassName: {ssObj.ObjectId.ObjectClass.Name}");
                    }

                }
            }
        }
