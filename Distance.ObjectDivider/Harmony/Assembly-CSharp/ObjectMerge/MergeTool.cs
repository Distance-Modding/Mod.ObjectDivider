
using Centrifuge.Distance.EditorTools.Attributes;
using LevelEditorActions;
using LevelEditorTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.ObjectDivider.Harmony
{
    [EditorTool, KeyboardShortcut("SHIFT+K")]
    public class MergeTool : InstantTool
    {
        internal static ToolInfo info_ => new ToolInfo("Merge", "Merges objects. More accurately, it scales, rotates, and positions the first object you select to the same size, rotation, and position of the group bounds that would exist if you grouped the selected objects.", ToolCategory.Edit, ToolButtonState.Button, true, 1428);
        public override ToolInfo Info_ => info_;

        // Required by distance itself
        public static void Register()
        {
            if (!G.Sys.LevelEditor_.registeredToolsNamesToTypes_.ContainsKey(info_.Name_))
                G.Sys.LevelEditor_.RegisterTool(info_);
        }

        public override bool Run()
        {

            List<GameObject> trackNodeObjects = G.Sys.LevelEditor_.SelectedNonTrackNodeObjects_;

            if (trackNodeObjects.Count > 0)
            {
                //CubeToPlaneAction action = new CubeToPlaneAction(cubeObjects.ToArray());
                //Mod.Logger.Info("QUAT: "+ cubeObjects.ToArray()[0].GetComponent<Transform>().localRotation);
                MergeAction action = new MergeAction(trackNodeObjects.ToArray());
                action.MergeObjects();
                action.FinishAndAddToLevelEditorActions();
                LevelEditorTool.PrintFormattedCountMessage("{0} object{1} were Merged.", trackNodeObjects.Count);
            }
            else
            {
                LevelEditorTool.PrintErrorMessage("No Objects Were Merged");
            }




            return true;
        }
    }
}
