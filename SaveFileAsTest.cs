using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using PlotXAxisTicksLabel.Common;
using System;
using System.Collections.Generic;
using System.IO;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ACADCommands
{
    public class Commands
    {
        /// <summary>
        /// Command to Open Dialog box and prompt user for a filename.
        /// </summary>
        [CommandMethod("SaveFileAsTest")]
        public static void SaveFileAsTest()
        {
            string output = SelectFolder("File name to save as:");

            AcAp.ShowAlertDialog($"Filename to save as is: {output}");
        }    
    
        internal static string SelectFolder(string promptText)
        {
            //ask for a file using the open file dialog
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSaveFileOptions op = new PromptSaveFileOptions(promptText);            
            op.InitialDirectory = "C:\\";
            op.Filter = "CSV Files (*.csv)|*.csv";
            op.FilterIndex = 2;
            PromptFileNameResult fn = ed.GetFileNameForSave(op);
            
            return fn.StringResult;
        }
    }
}
