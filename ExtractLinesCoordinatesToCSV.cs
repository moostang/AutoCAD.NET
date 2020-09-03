        // Extract x,y,z coordinates of line(s) to CSV file
        
        [CommandMethod("ExtractLinesCoordinatesToCSV")]
        public static void ExtractLinesCoordinatesToCSV()
        {
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            Editor acEd = acDoc.Editor;

            // Prepare empty list to store line coordinates
            List<Point3d> lineCoords = new List<Point3d>();

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Request for objects to be selected in the drawing area
                PromptSelectionResult acSSPrompt = acDoc.Editor.GetSelection();

                // If the prompt status is OK, objects were selected
                if (acSSPrompt.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet = acSSPrompt.Value;

                    // End point of last line in selection
                    Point3d lastPoint = new Point3d();

                    // Step through the objects in the selection set
                    foreach (SelectedObject acSSObj in acSSet)
                    {
                        // Check to make sure a valid SelectedObject object was returned
                        if (acSSObj != null)
                        {
                            // Read object as a Line 
                            Line line = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForRead) as Line;
                            if (line != null)
                            {
                                // Add coordinates of line to list
                                lineCoords.Add(line.StartPoint);
                                lastPoint = line.EndPoint;
                            }
                        }
                    }

                    // Add last point of last line in selection
                    lineCoords.Add(lastPoint);

                    // Save the new object to the database
                    acTrans.Commit();
                }

                // Dispose of the transaction
            }

            // Open CSV file to write 
            using (StreamWriter file = new StreamWriter(@"c:\PROJECTS\output.csv"))
            {
                foreach(var row in lineCoords)
                {
                    file.WriteLine($"{row.X}, {row.Y}, {row.Z}");
                }
            }
            
        }
