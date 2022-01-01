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
    [EditorTool, KeyboardShortcut("SHIFT+P")]
    public class CubeToPlaneTool : InstantTool
    {
        internal static ToolInfo info_ => new ToolInfo("Cube to planes", "Turns cubes into planes.", ToolCategory.Edit, ToolButtonState.Button, true, 1427);
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
            List<GameObject> cubeObjects = new List<GameObject>();
            bool canPlanerizeAny = false;
            int cubeCount = 0;
            foreach (GameObject gameObject in trackNodeObjects)
            {
                if (gameObject.name.Equals("CubeGS"))
                {
                    canPlanerizeAny = true;
                    cubeCount++;
                    cubeObjects.Add(gameObject);
                }
            }
            if(canPlanerizeAny)
            {
                //CubeToPlaneAction action = new CubeToPlaneAction(cubeObjects.ToArray());
                //Mod.Logger.Info("QUAT: "+ cubeObjects.ToArray()[0].GetComponent<Transform>().localRotation);
                CubeToPlaneAction action = new CubeToPlaneAction(cubeObjects.ToArray());
                action.TurnCubeIntoPlanes();
                action.FinishAndAddToLevelEditorActions();
                LevelEditorTool.PrintFormattedCountMessage("{0} object{1} were Planerized.", cubeCount);
            }
            else
            {
                LevelEditorTool.PrintErrorMessage("No Objects Were Planerized");
            }




            return true;
        }
    }
}
