using LevelEditorActions;
using Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.ObjectDivider.Harmony
{
    class DivideAction : SimplerAction
    {
        private static string[] divadableObjectNames = new string[] { "CubeGS", "QuadGS", "EventTriggerBox", "ForceZoneBox", "CooldownTriggerNoVisual", "KillGridBox",
        "CylinderGS","CylinderHDGS","TubeGS","CheeseGS","PentagonGS","HexagonGS","PlaneOneSidedGS", "PlaneGS","KillGridCylinder", "ArchGS", "ArchQuarterGS",
        "WedgeGS"};
        private static string[] xDivadableObjectNames = new string[] { "CubeGS", "QuadGS", "EventTriggerBox", "ForceZoneBox", "CooldownTriggerNoVisual", "KillGridBox",
        "PlaneOneSidedGS", "PlaneGS", "ArchQuarterGS","WedgeGS"};
        private static string[] yDivadableObjectNames = new string[] { "CubeGS", "QuadGS", "EventTriggerBox", "ForceZoneBox", "CooldownTriggerNoVisual", "KillGridBox",
        "CylinderGS","CylinderHDGS","TubeGS","CheeseGS","PentagonGS","HexagonGS"};
        private static string[] zDivadableObjectNames = new string[] { "CubeGS", "QuadGS", "EventTriggerBox", "ForceZoneBox", "CooldownTriggerNoVisual", "KillGridBox",
        "PlaneOneSidedGS", "PlaneGS","KillGridCylinder", "ArchGS"};

        private ReferenceMap.Handle<GameObject> originalObjectHandle_;
        private ReferenceMap.Handle<GameObject>[] newObjectHandles_;
        private List<ReferenceMap.Handle<GameObject>> newGroupObjectHandles_ = new List<ReferenceMap.Handle<GameObject>>();
        private byte[] deletedObjectBytes_;
        private GroupAction groupAction_;

        private int xdivides;
        private int ydivides;
        private int zdivides;
        private bool KTL;
        private float xspacing;
        private float yspacing;
        private float zspacing;
        private bool CTP;
        private bool NPO;

        private bool nothingtodivide = false;

        public DivideAction(GameObject[] gameObjects, int xdivides, int ydivides, int zdivides, bool KTL, float xspacing, float yspacing, float zspacing, bool CTP, bool NPO, out int divideableobjectscount)
        {
            this.xdivides = xdivides;
            this.ydivides = ydivides;
            this.zdivides = zdivides;

            this.KTL = KTL;

            this.xspacing = xspacing;
            this.yspacing = yspacing;
            this.zspacing = zspacing;

            this.CTP = CTP;
            this.NPO = NPO;

            ReferenceMap referenceMap = G.Sys.LevelEditor_.ReferenceMap_;

            List<GameObject> originalObjects = new List<GameObject>();

            int newObjectCount = 0;
            

            foreach (GameObject gameObject in gameObjects)
            {
                if((isXDividable(gameObject) && xdivides > 0) || (isYDividable(gameObject) && ydivides > 0) || (isZDividable(gameObject) && zdivides > 0))
                {
                    originalObjects.Add(gameObject);
                    int currentObjectXDivides = 0;
                    int currentObjectYDivides = 0;
                    int currentObjectZDivides = 0;
                    if (isXDividable(gameObject))
                    {
                        currentObjectXDivides = xdivides;
                    }
                    if (isYDividable(gameObject))
                    {
                        currentObjectYDivides = ydivides;
                    }
                    if (isZDividable(gameObject))
                    {
                        currentObjectZDivides = zdivides;
                    }
                    if(gameObject.name.Equals("CubeGS") && CTP && NPO)
                    {
                        if(xspacing == 0)
                        {
                            currentObjectXDivides = 0;
                        }
                        if (yspacing == 0)
                        {
                            currentObjectYDivides = 0;
                        }
                        if (zspacing == 0)
                        {
                            currentObjectZDivides = 0;
                        }
                        newObjectCount += (currentObjectXDivides + 1) * (currentObjectYDivides + 1) * (currentObjectZDivides + 1);
                    }
                    else
                    {
                        newObjectCount += (currentObjectXDivides + 1) * (currentObjectYDivides + 1) * (currentObjectZDivides + 1);
                    }
                    
                }
            }
            if (originalObjects.Count > 0)
            {
                Group.InitData data = new Group.InitData(Vector3.zero, Quaternion.identity, Vector3.one, new Bounds());
                this.groupAction_ = new GroupAction(originalObjects.ToArray(), data);
                Group group = this.groupAction_.GroupObjects();
                this.originalObjectHandle_ = referenceMap.GetHandle<GameObject>(group.gameObject);
                this.deletedObjectBytes_ = BinarySerializer.SaveGameObjectToBytes(group.gameObject, Resource.LoadPrefab("Group"), true);
                this.groupAction_.Undo();
            }
            /*
            else if(originalObjects.Count == 1)
            {
                this.groupAction_ = (GroupAction)null;
                GameObject gameObject = originalObjects.ToArray()[0];
                this.originalObjectHandle_ = referenceMap.GetHandle<GameObject>(gameObject);
                this.deletedObjectBytes_ = BinarySerializer.SaveGameObjectToBytes(gameObject, Serializer.GetPrefabWithName(gameObject.name), true);
            }
            */
            else
            {
                nothingtodivide = true;
            }
            newObjectHandles_ = new ReferenceMap.Handle<GameObject>[newObjectCount];
            divideableobjectscount = originalObjects.Count;
        }

        public static bool isXDividable(GameObject gameObject)
        {
            string name = gameObject.name;
            foreach (string possiblename in xDivadableObjectNames)
            {
                if (name.Equals(possiblename))
                {
                    return true;
                }
            }
            if (gameObject.HasComponent<BoxCollider>())
            {
                if (gameObject.GetComponent<BoxCollider>().center.x == 0 && !gameObject.name.Equals("KillGridCylinder"))
                {
                    return true;
                }
            }
            if(gameObject.HasComponent<Group>())
            {
                return true;
            }
            return false;
        }

        public static bool isYDividable(GameObject gameObject)
        {
            string name = gameObject.name;
            foreach (string possiblename in yDivadableObjectNames)
            {
                if (name.Equals(possiblename))
                {
                    return true;
                }
            }
            if (gameObject.HasComponent<BoxCollider>())
            {
                if (gameObject.GetComponent<BoxCollider>().center.y == 0)
                {
                    return true;
                }
            }
            if (gameObject.HasComponent<Group>())
            {
                return true;
            }
            return false;
        }

        public static bool isZDividable(GameObject gameObject)
        {
            string name = gameObject.name;
            foreach (string possiblename in zDivadableObjectNames)
            {
                if (name.Equals(possiblename))
                {
                    return true;
                }
            }
            if (gameObject.HasComponent<BoxCollider>())
            {
                if (gameObject.GetComponent<BoxCollider>().center.z == 0 && !gameObject.name.Equals("KillGridCylinder"))
                {
                    return true;
                }
            }
            if (gameObject.HasComponent<Group>())
            {
                return true;
            }
            return false;
        }

        public static bool isDividableObj(GameObject gameObject, int xdividenum, int ydividenum, int zdividenum)
        {
            return (isXDividable(gameObject) && xdividenum > 0) || (isYDividable(gameObject) && ydividenum > 0) || (isZDividable(gameObject) && zdividenum > 0);
        }

        public override string Description_ => "Divide Objects";

        public override void Undo() => this.UndivideObjects();

        public override void Redo() => this.DivideObjects(true);

        public List<GameObject> DivideObjects(bool setNewSelection)
        {
            if(!nothingtodivide)
            {
                int newObjectHandleIndex = 0;
                List<GameObject> newObjects = new List<GameObject>();
                groupAction_.GroupObjects();
                GameObject[] originalobjs = groupAction_.UngroupObjects();
                LevelEditor levelEditor = G.Sys.LevelEditor_;
                groupAction_.GroupObjects();
                levelEditor.DeleteGameObject(this.originalObjectHandle_.Get());
                List<LevelLayer> layersOfObjects = new List<LevelLayer>();
                foreach (GameObject origobj in originalobjs)
                {
                    if (origobj.name.Equals("CubeGS") && CTP && NPO && !(xspacing != 0 && yspacing != 0 && zspacing != 0))
                    {
                        if (xspacing == 0 && yspacing == 0 && zspacing == 0)
                        {
                            DivideObjects3PlaneDivide(ref newObjects, origobj, ref newObjectHandleIndex);
                            continue;
                        }
                        else if ((xspacing == 0 && yspacing != 0 && zspacing != 0) || (xspacing != 0 && yspacing == 0 && zspacing != 0) || (xspacing != 0 && yspacing != 0 && zspacing == 0))
                        {
                            DivideObjects1PlaneDivide(ref newObjects, origobj, ref newObjectHandleIndex);
                            continue;
                        }
                        else if ((xspacing == 0 && yspacing == 0 && zspacing != 0) || (xspacing != 0 && yspacing == 0 && zspacing == 0) || (xspacing == 0 && yspacing != 0 && zspacing == 0))
                        {
                            DivideObjects2PlaneDivide(ref newObjects, origobj, ref newObjectHandleIndex);
                            continue;
                        }
                    }
                    else if(origobj.name.Equals("CubeGS") && CTP)
                    {
                        DivideObjectsPlaneRegularDivide(ref newObjects, origobj, ref newObjectHandleIndex);
                        continue;
                    }
                    LevelLayer origobjlayer= levelEditor.WorkingLevel_.GetLayerOfObject(origobj);
                    List<GameObject> newerObjects = new List<GameObject>();
                    Vector3 origobjpos = origobj.transform.localPosition;
                    Quaternion origobjrot = origobj.transform.localRotation;
                    Vector3 origobjscl = origobj.transform.localScale;
                    GameObject firstNewObject = DuplicateObjectsAction.Duplicate(origobj);
                    firstNewObject.GetComponent<Transform>().position = new Vector3(0,0,0);
                    firstNewObject.GetComponent<Transform>().rotation = new Quaternion(0,0,0,1);
                    firstNewObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
                    newerObjects.Add(firstNewObject);
                    Vector3 defaultObjSize = getDefaultObjSize(origobj.name);
                    if (origobj.HasComponent<BoxCollider>())
                    {
                        defaultObjSize = origobj.GetComponent<BoxCollider>().size;
                    }
                    if(origobj.HasComponent<Group>())
                    {
                        defaultObjSize = origobj.GetComponent<Group>().localBounds_.size;
                    }
                    bool isGoldAndAllowed = origobj.HasComponent<GoldenSimples>() && KTL && (origobj.name.Equals("PlaneGS") || origobj.name.Equals("PlaneOneSidedGS"));
                    if (isGoldAndAllowed) isGoldAndAllowed = isGoldAndAllowed && !origobj.GetComponent<GoldenSimples>().worldMapped_;
                   
                    
                    if (defaultObjSize.x != -1)
                    {
                        GameObject[] newererObjects = new GameObject[xdivides + 1];
                        List<GameObject> newerObjectsTemp = new List<GameObject>();
                        foreach (GameObject gameObject in newerObjects)
                        {
                            GameObject newestObject = DuplicateObjectsAction.Duplicate(gameObject);
                            GoldenSimples originalGS = gameObject.GetComponent<GoldenSimples>();
                            Transform newestObjectTrans = newestObject.GetComponent<Transform>();
                            float originalXScale = newestObjectTrans.localScale.x * defaultObjSize.x;
                            float newXScale = (newestObjectTrans.localScale.x * (defaultObjSize.x - xspacing * xdivides)) / (defaultObjSize.x * (xdivides + 1));
                            float newXScalePart = (defaultObjSize.x - xspacing * xdivides) / (defaultObjSize.x * (xdivides + 1));
                            float translationalSpacingMultiplier = 1 + (xspacing / defaultObjSize.x);
                            newestObjectTrans.localPosition = new Vector3((float)((newestObjectTrans.localPosition.x - ((originalXScale / (1.0 + xdivides)) * .5 * xdivides))* translationalSpacingMultiplier), newestObjectTrans.localPosition.y, newestObjectTrans.localPosition.z);
                            newestObjectTrans.localScale = new Vector3(newXScale, newestObjectTrans.localScale.y, newestObjectTrans.localScale.z);
                            if (isGoldAndAllowed)
                            {
                                GoldenSimples objgold = newestObject.GetComponent<GoldenSimples>();
                                objgold.textureScale_ = new Vector3((float)(originalGS.textureScale_.x * newXScalePart), objgold.textureScale_.y, objgold.textureScale_.z);
                                objgold.textureOffset_ = new Vector3((float)(objgold.textureOffset_.x + originalGS.textureScale_.x * ((gameObject.GetComponent<Transform>().localPosition.x - newestObjectTrans.localPosition.x - ((newXScale * defaultObjSize.x) / 2) + originalXScale/2) / originalXScale)), objgold.textureOffset_.y, objgold.textureOffset_.z);
                            }
                            newererObjects[0] = newestObject;
                            for (int i = 1; i < xdivides + 1; i++)
                            {
                                GameObject newestestObject = DuplicateObjectsAction.Duplicate(newererObjects[i - 1]);
                                Transform newestestObjectTrans = newestestObject.GetComponent<Transform>();
                                newestestObjectTrans.localPosition = new Vector3((float)(newestestObjectTrans.localPosition.x + ( originalXScale / ((1.0 + xdivides)))* translationalSpacingMultiplier), newestestObjectTrans.localPosition.y, newestestObjectTrans.localPosition.z);
                                if (isGoldAndAllowed)
                                {
                                    GoldenSimples objgold = newestestObject.GetComponent<GoldenSimples>();
                                    objgold.textureScale_ = new Vector3((float)(originalGS.textureScale_.x * newXScalePart), objgold.textureScale_.y, objgold.textureScale_.z);
                                    objgold.textureOffset_ = new Vector3((float)(originalGS.textureOffset_.x + originalGS.textureScale_.x * ((gameObject.GetComponent<Transform>().localPosition.x - newestestObjectTrans.localPosition.x - ((newXScale * defaultObjSize.x) / 2) + originalXScale / 2) / originalXScale)), objgold.textureOffset_.y, objgold.textureOffset_.z);
                                }
                                newererObjects[i] = newestestObject;
                            }
                            newerObjectsTemp.AddRange(newererObjects);
                        }
                        newerObjects.Clear();
                        newerObjects.AddRange(newerObjectsTemp);
                    }
                    
                    if (defaultObjSize.y != -1)
                    {
                        GameObject[] newererObjects = new GameObject[ydivides + 1];
                        List<GameObject> newerObjectsTemp = new List<GameObject>();
                        foreach (GameObject gameObject in newerObjects)
                        {
                            GameObject newestObject = DuplicateObjectsAction.Duplicate(gameObject);
                            GoldenSimples originalGS = gameObject.GetComponent<GoldenSimples>();
                            Transform newestObjectTrans = newestObject.GetComponent<Transform>();
                            float originalYScale = newestObjectTrans.localScale.y * defaultObjSize.y;
                            float newYScale = (newestObjectTrans.localScale.y * (defaultObjSize.y - yspacing * ydivides)) / (defaultObjSize.y * (ydivides + 1));
                            float newYScalePart = (defaultObjSize.y - yspacing * ydivides) / (defaultObjSize.y * (ydivides + 1));
                            float translationalSpacingMultiplier = 1 + (yspacing / defaultObjSize.y);
                            newestObjectTrans.localPosition = new Vector3(newestObjectTrans.localPosition.x, (float)((newestObjectTrans.localPosition.y - ((originalYScale / (1.0 + ydivides)) * .5 * ydivides))* translationalSpacingMultiplier), newestObjectTrans.localPosition.z);
                            newestObjectTrans.localScale = new Vector3(newestObjectTrans.localScale.x, newYScale, newestObjectTrans.localScale.z);
                            if (isGoldAndAllowed)
                            {
                                GoldenSimples objgold = newestObject.GetComponent<GoldenSimples>();
                                objgold.textureScale_ = new Vector3(objgold.textureScale_.x, (float)(originalGS.textureScale_.y * newYScalePart), objgold.textureScale_.z);
                                objgold.textureOffset_ = new Vector3(objgold.textureOffset_.x, (float)(originalGS.textureOffset_.y + originalGS.textureScale_.y * ((gameObject.GetComponent<Transform>().localPosition.y - newestObjectTrans.localPosition.y - ((newYScale * defaultObjSize.y) / 2) + originalYScale / 2) / originalYScale)), objgold.textureOffset_.z);
                            }
                            newererObjects[0] = newestObject;
                            for (int i = 1; i < ydivides + 1; i++)
                            {
                                GameObject newestestObject = DuplicateObjectsAction.Duplicate(newererObjects[i - 1]);
                                Transform newestestObjectTrans = newestestObject.GetComponent<Transform>();
                                newestestObjectTrans.localPosition = new Vector3(newestestObjectTrans.localPosition.x , (float)(newestestObjectTrans.localPosition.y + ( originalYScale / ((1.0 + ydivides)))* translationalSpacingMultiplier), newestestObjectTrans.localPosition.z);
                                if (isGoldAndAllowed)
                                {
                                    GoldenSimples objgold = newestestObject.GetComponent<GoldenSimples>();
                                    objgold.textureScale_ = new Vector3(objgold.textureScale_.x, (float)(originalGS.textureScale_.y * newYScalePart), objgold.textureScale_.z);
                                    objgold.textureOffset_ = new Vector3(objgold.textureOffset_.x, (float)(originalGS.textureOffset_.y + originalGS.textureScale_.y * ((gameObject.GetComponent<Transform>().localPosition.y - newestObjectTrans.localPosition.y - ((newYScale * defaultObjSize.y) / 2) + originalYScale / 2) / originalYScale)), objgold.textureOffset_.z);
                                }
                                newererObjects[i] = newestestObject;
                            }
                            newerObjectsTemp.AddRange(newererObjects);
                        }
                        newerObjects.Clear();
                        newerObjects.AddRange(newerObjectsTemp);
                    }
                    if (defaultObjSize.z != -1)
                    {
                        GameObject[] newererObjects = new GameObject[zdivides + 1];
                        List<GameObject> newerObjectsTemp = new List<GameObject>();
                        foreach (GameObject gameObject in newerObjects)
                        {
                            GameObject newestObject = DuplicateObjectsAction.Duplicate(gameObject);
                            GoldenSimples originalGS = gameObject.GetComponent<GoldenSimples>();
                            Transform newestObjectTrans = newestObject.GetComponent<Transform>();
                            float originalZScale = newestObjectTrans.localScale.z * defaultObjSize.z;
                            float newZScale = (newestObjectTrans.localScale.z * (defaultObjSize.z - zspacing * zdivides)) / (defaultObjSize.z * (zdivides + 1));
                            float newZScalePart = (defaultObjSize.z - zspacing * zdivides) / (defaultObjSize.z * (zdivides + 1));
                            float translationalSpacingMultiplier = 1 + (zspacing / defaultObjSize.z);
                            newestObjectTrans.localPosition = new Vector3(newestObjectTrans.localPosition.x, newestObjectTrans.localPosition.y , (float)((newestObjectTrans.localPosition.z - ((originalZScale / (1.0 + zdivides)) * .5 * zdivides))* translationalSpacingMultiplier));
                            newestObjectTrans.localScale = new Vector3(newestObjectTrans.localScale.x, newestObjectTrans.localScale.y, newZScale);
                            /*if (isGoldAndAllowed)
                            {
                                GoldenSimples objgold = newestObject.GetComponent<GoldenSimples>();
                                objgold.textureScale_ = new Vector3(objgold.textureScale_.x, objgold.textureScale_.y, (float)(originalGS.textureScale_.z / (zdivides + 1.0)));
                                objgold.textureOffset_ = new Vector3(objgold.textureOffset_.x, objgold.textureOffset_.y, (float)(originalGS.textureOffset_.z + originalGS.textureScale_.z * (zdivides / (zdivides + 1.0))));
                            }*/
                            if (isGoldAndAllowed)
                            {
                                GoldenSimples objgold = newestObject.GetComponent<GoldenSimples>();
                                objgold.textureScale_ = new Vector3(objgold.textureScale_.x, (float)(originalGS.textureScale_.y * newZScalePart), objgold.textureScale_.z);
                                objgold.textureOffset_ = new Vector3(objgold.textureOffset_.x, (float)(originalGS.textureOffset_.y + originalGS.textureScale_.y * ((gameObject.GetComponent<Transform>().localPosition.z - newestObjectTrans.localPosition.z - ((newZScale * defaultObjSize.z) / 2) + originalZScale / 2) / originalZScale)), objgold.textureOffset_.z);
                            }
                            newererObjects[0] = newestObject;
                            for (int i = 1; i < zdivides + 1; i++)
                            {
                                GameObject newestestObject = DuplicateObjectsAction.Duplicate(newererObjects[i - 1]);
                                Transform newestestObjectTrans = newestestObject.GetComponent<Transform>();
                                newestestObjectTrans.localPosition = new Vector3(newestestObjectTrans.localPosition.x, newestestObjectTrans.localPosition.y , (float)(newestestObjectTrans.localPosition.z + (originalZScale / ((1.0 + zdivides)))* translationalSpacingMultiplier));
                                /*if (isGoldAndAllowed)
                                {
                                    GoldenSimples objgold = newestestObject.GetComponent<GoldenSimples>();
                                    objgold.textureScale_ = new Vector3(objgold.textureScale_.x, objgold.textureScale_.y, (float)(originalGS.textureScale_.z / (zdivides + 1.0)));
                                    objgold.textureOffset_ = new Vector3(objgold.textureOffset_.x, objgold.textureOffset_.y, (float)(originalGS.textureOffset_.z + originalGS.textureScale_.z * ((zdivides - i) / (zdivides + 1.0))));
                                }*/
                                if (isGoldAndAllowed)
                                {
                                    GoldenSimples objgold = newestestObject.GetComponent<GoldenSimples>();
                                    objgold.textureScale_ = new Vector3(objgold.textureScale_.x, (float)(originalGS.textureScale_.y * newZScalePart), objgold.textureScale_.z);
                                    objgold.textureOffset_ = new Vector3(objgold.textureOffset_.x, (float)(originalGS.textureOffset_.y + originalGS.textureScale_.y * ((gameObject.GetComponent<Transform>().localPosition.z - newestestObjectTrans.localPosition.z - ((newZScale * defaultObjSize.z) / 2) + originalZScale / 2) / originalZScale)), objgold.textureOffset_.z);
                                }
                                newererObjects[i] = newestestObject;
                            }
                            newerObjectsTemp.AddRange(newererObjects);
                        }
                        newerObjects.Clear();
                        newerObjects.AddRange(newerObjectsTemp);
                    }
                    
                    
                    /*
                    ReferenceMap.Handle<GameObject>[] tempObjHandles = new ReferenceMap.Handle<GameObject>[newerObjects.Count];
                    LevelLayer[] tempLevelLayers = new LevelLayer[newerObjects.Count];
                    for (int i = 0; i< newerObjects.Count; i++)
                    {
                        tempObjHandles[i] = newObjectHandles_[newObjectHandleIndex];
                        tempLevelLayers[i] = origobjlayer;
                        newObjectHandleIndex += 1;
                    }*/
                    for(int i = 0; i<newerObjects.Count; i++)
                    {
                        levelEditor.AddGameObject(ref newObjectHandles_[newObjectHandleIndex], newerObjects.ToArray()[i], origobjlayer);
                        newObjectHandleIndex += 1;
                    }
                    //levelEditor.AddGameObjects(tempObjHandles, newerObjects.ToArray(), tempLevelLayers.ToArray());
                    Group.InitData data = new Group.InitData(Vector3.zero, Quaternion.identity, Vector3.one, new Bounds());
                    GroupAction groupAction = new GroupAction(newerObjects.ToArray(), data);
                    GameObject newerObjectsGroup = groupAction.GroupObjects().gameObject;
                    newerObjectsGroup.GetComponent<Transform>().localPosition = origobjpos;
                    newerObjectsGroup.GetComponent<Transform>().localRotation = origobjrot;
                    newerObjectsGroup.GetComponent<Transform>().localScale = origobjscl;
                    GameObject[] finalObjectsBeforeAdd = groupAction.UngroupObjects();
                    newObjects.AddRange(finalObjectsBeforeAdd);
                    //levelEditor.DeleteGameObjects(tempObjHandles);
                    for (int i = 0; i< finalObjectsBeforeAdd.Length; i++)
                    {
                        layersOfObjects.Add(origobjlayer);
                    }
                    origobj.GetComponent<Transform>().localPosition = origobjpos;
                    origobj.GetComponent<Transform>().localRotation = origobjrot;
                    origobj.GetComponent<Transform>().localScale = origobjscl;
                }

                //Mod.Logger.Info("FinalOBjsAmnt: " + newObjects.Count);
                //Mod.Logger.Info("OBj HandlesAmnt: " + newObjectHandles_.Length);
                //levelEditor.AddGameObjects(this.newObjectHandles_, newObjects.ToArray(), layersOfObjects.ToArray());
                
                if (setNewSelection) levelEditor.SetSelection((IEnumerable<GameObject>)newObjects.ToArray());
                return newObjects;
            }
            return null;
        }

        public void DivideObjects1PlaneDivide(ref List<GameObject> newObjects, GameObject origobj, ref int newObjectHandleIndex)
        {
            LevelEditor levelEditor = G.Sys.LevelEditor_;
            LevelLayer origobjlayer = levelEditor.WorkingLevel_.GetLayerOfObject(origobj);
            List<GameObject> newerObjects = new List<GameObject>();
            Vector3 origobjpos = origobj.transform.localPosition;
            Quaternion origobjrot = origobj.transform.localRotation;
            Vector3 origobjscl = origobj.transform.localScale;
            GameObject firstNewObject = DuplicateObjectsAction.Duplicate(origobj);
            firstNewObject.GetComponent<Transform>().position = new Vector3(0, 0, 0);
            firstNewObject.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 1);
            //firstNewObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            bool isGoldAndAllowed = KTL && !origobj.GetComponent<GoldenSimples>().worldMapped_;
            float originalZTexoff = firstNewObject.GetComponent<GoldenSimples>().textureOffset_.z;
            float originalZTexScale = firstNewObject.GetComponent<GoldenSimples>().textureScale_.z;
            if (isGoldAndAllowed)
            {
                firstNewObject.GetComponent<GoldenSimples>().textureOffset_.z = 0;
                firstNewObject.GetComponent<GoldenSimples>().textureScale_.z = 1;
            }

            newerObjects.Add(firstNewObject);
            Vector3 defaultObjSize = getDefaultObjSize(origobj.name);
            
            float w = 0;
            float wX = 0;
            float wY = 0;
            if (xspacing != 0 && yspacing != 0 && zspacing == 0)
            {
                xBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref w, true);
                yBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref w, false);
                singleAxisBasedPlazer(defaultObjSize, ref newerObjects, isGoldAndAllowed, origobj, origobjlayer, ref newObjectHandleIndex, w, wX, wY, 1F, originalZTexoff, originalZTexScale, 2);
                //zBasedPlazer(defaultObjSize, ref newerObjects, isGoldAndAllowed, origobj, origobjlayer, ref newObjectHandleIndex, w, originalZTexoff, originalZTexScale);
            }
            if (xspacing != 0 && yspacing == 0 && zspacing != 0)
            {
                xBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref w, true);
                zBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref wX, ref wY, true);
                singleAxisBasedPlazer(defaultObjSize, ref newerObjects, isGoldAndAllowed, origobj, origobjlayer, ref newObjectHandleIndex, w, wX, wY, 1F, originalZTexoff, originalZTexScale, 1);
            }
            if (xspacing == 0 && yspacing != 0 && zspacing != 0)
            {
                yBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref w, false);
                zBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref wX, ref wY, true);
                singleAxisBasedPlazer(defaultObjSize, ref newerObjects, isGoldAndAllowed, origobj, origobjlayer, ref newObjectHandleIndex, w, wX, wY, -1F, originalZTexoff, originalZTexScale, 0);
            }
            /*
            for (int i = 0; i < newerObjects.Count; i++)
            {
                levelEditor.AddGameObject(ref newObjectHandles_[newObjectHandleIndex], newerObjects.ToArray()[i], origobjlayer);
                newObjectHandleIndex += 1;
            }*/
            //levelEditor.AddGameObjects(tempObjHandles, newerObjects.ToArray(), tempLevelLayers.ToArray());
            Group.InitData data = new Group.InitData(Vector3.zero, Quaternion.identity, Vector3.one, new Bounds());
            GroupAction groupAction = new GroupAction(newerObjects.ToArray(), data);
            GameObject newerObjectsGroup = groupAction.GroupObjects().gameObject;
            newerObjectsGroup.GetComponent<Transform>().localPosition = origobjpos;
            newerObjectsGroup.GetComponent<Transform>().localRotation = origobjrot;
            //newerObjectsGroup.GetComponent<Transform>().localScale = origobjscl;
            GameObject[] finalObjectsBeforeAdd = groupAction.UngroupObjects();
            newObjects.AddRange(finalObjectsBeforeAdd);
            //newObjects.AddRange(newerObjects);
            //levelEditor.DeleteGameObjects(tempObjHandles);
            origobj.GetComponent<Transform>().localPosition = origobjpos;
            origobj.GetComponent<Transform>().localRotation = origobjrot;
            origobj.GetComponent<Transform>().localScale = origobjscl;
        }

        public void DivideObjects2PlaneDivide(ref List<GameObject> newObjects, GameObject origobj, ref int newObjectHandleIndex)
        {
            LevelEditor levelEditor = G.Sys.LevelEditor_;
            LevelLayer origobjlayer = levelEditor.WorkingLevel_.GetLayerOfObject(origobj);
            List<GameObject> newerObjects = new List<GameObject>();
            Vector3 origobjpos = origobj.transform.localPosition;
            Quaternion origobjrot = origobj.transform.localRotation;
            Vector3 origobjscl = origobj.transform.localScale;
            GameObject firstNewObject = DuplicateObjectsAction.Duplicate(origobj);
            firstNewObject.GetComponent<Transform>().position = new Vector3(0, 0, 0);
            firstNewObject.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 1);
            //firstNewObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            bool isGoldAndAllowed = KTL && !origobj.GetComponent<GoldenSimples>().worldMapped_;
            float originalZTexoff = firstNewObject.GetComponent<GoldenSimples>().textureOffset_.z;
            float originalZTexScale = firstNewObject.GetComponent<GoldenSimples>().textureScale_.z;
            if (isGoldAndAllowed)
            {
                firstNewObject.GetComponent<GoldenSimples>().textureOffset_.z = 0;
                firstNewObject.GetComponent<GoldenSimples>().textureScale_.z = 1;
            }

            newerObjects.Add(firstNewObject);
            Vector3 defaultObjSize = getDefaultObjSize(origobj.name);

            float w = 0;
            float wX = 0;
            float wY = 0;

            if (xspacing != 0 && yspacing == 0 && zspacing == 0)
            {
                xBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref w, true);
                doubleAxisBasedPlazer(defaultObjSize, ref newerObjects, isGoldAndAllowed, origobj, origobjlayer, ref newObjectHandleIndex, w, wX, wY, 1F, originalZTexoff, originalZTexScale, 0);
                
            }
            if (xspacing == 0 && yspacing != 0 && zspacing == 0)
            {
                yBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref w, false);
                doubleAxisBasedPlazer(defaultObjSize, ref newerObjects, isGoldAndAllowed, origobj, origobjlayer, ref newObjectHandleIndex, w, wX, wY, -1F, originalZTexoff, originalZTexScale, 1);
            }
            if (xspacing == 0 && yspacing == 0 && zspacing != 0)
            {
                zBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref wX, ref wY, true);
                doubleAxisBasedPlazer(defaultObjSize, ref newerObjects, isGoldAndAllowed, origobj, origobjlayer, ref newObjectHandleIndex, w, wX, wY, -1F, originalZTexoff, originalZTexScale, 2);
            }

            Group.InitData data = new Group.InitData(Vector3.zero, Quaternion.identity, Vector3.one, new Bounds());
            GroupAction groupAction = new GroupAction(newerObjects.ToArray(), data);
            GameObject newerObjectsGroup = groupAction.GroupObjects().gameObject;
            newerObjectsGroup.GetComponent<Transform>().localPosition = origobjpos;
            newerObjectsGroup.GetComponent<Transform>().localRotation = origobjrot;
            //newerObjectsGroup.GetComponent<Transform>().localScale = origobjscl;
            GameObject[] finalObjectsBeforeAdd = groupAction.UngroupObjects();
            newObjects.AddRange(finalObjectsBeforeAdd);
            origobj.GetComponent<Transform>().localPosition = origobjpos;
            origobj.GetComponent<Transform>().localRotation = origobjrot;
            origobj.GetComponent<Transform>().localScale = origobjscl;
        }


        public void DivideObjects3PlaneDivide(ref List<GameObject> newObjects, GameObject origobj, ref int newObjectHandleIndex)
        {
            LevelEditor levelEditor = G.Sys.LevelEditor_;
            LevelLayer origobjlayer = levelEditor.WorkingLevel_.GetLayerOfObject(origobj);
            List<GameObject> newerObjects = new List<GameObject>();
            Vector3 origobjpos = origobj.transform.localPosition;
            Quaternion origobjrot = origobj.transform.localRotation;
            Vector3 origobjscl = origobj.transform.localScale;
            GameObject firstNewObject = DuplicateObjectsAction.Duplicate(origobj);
            firstNewObject.GetComponent<Transform>().position = new Vector3(0, 0, 0);
            firstNewObject.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 1);
            //firstNewObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            bool isGoldAndAllowed = KTL && !origobj.GetComponent<GoldenSimples>().worldMapped_;
            float originalZTexoff = firstNewObject.GetComponent<GoldenSimples>().textureOffset_.z;
            float originalZTexScale = firstNewObject.GetComponent<GoldenSimples>().textureScale_.z;
            if (isGoldAndAllowed)
            {
                firstNewObject.GetComponent<GoldenSimples>().textureOffset_.z = 0;
                firstNewObject.GetComponent<GoldenSimples>().textureScale_.z = 1;
            }

            newerObjects.Add(firstNewObject);
            Vector3 defaultObjSize = getDefaultObjSize(origobj.name);

            float w = 0;
            float wX = 0;
            float wY = 0;


            tripleAxisBasedPlazer(defaultObjSize, ref newerObjects, isGoldAndAllowed, origobj, origobjlayer, ref newObjectHandleIndex, w, wX, wY, -1F, originalZTexoff, originalZTexScale);


            Group.InitData data = new Group.InitData(Vector3.zero, Quaternion.identity, Vector3.one, new Bounds());
            GroupAction groupAction = new GroupAction(newerObjects.ToArray(), data);
            GameObject newerObjectsGroup = groupAction.GroupObjects().gameObject;
            newerObjectsGroup.GetComponent<Transform>().localPosition = origobjpos;
            newerObjectsGroup.GetComponent<Transform>().localRotation = origobjrot;
            //newerObjectsGroup.GetComponent<Transform>().localScale = origobjscl;
            GameObject[] finalObjectsBeforeAdd = groupAction.UngroupObjects();
            newObjects.AddRange(finalObjectsBeforeAdd);
            origobj.GetComponent<Transform>().localPosition = origobjpos;
            origobj.GetComponent<Transform>().localRotation = origobjrot;
            origobj.GetComponent<Transform>().localScale = origobjscl;
        }

        public void DivideObjectsPlaneRegularDivide(ref List<GameObject> newObjects, GameObject origobj, ref int newObjectHandleIndex)
        {
            LevelEditor levelEditor = G.Sys.LevelEditor_;
            LevelLayer origobjlayer = levelEditor.WorkingLevel_.GetLayerOfObject(origobj);
            List<GameObject> newerObjects = new List<GameObject>();
            Vector3 origobjpos = origobj.transform.localPosition;
            Quaternion origobjrot = origobj.transform.localRotation;
            Vector3 origobjscl = origobj.transform.localScale;
            GameObject firstNewObject = DuplicateObjectsAction.Duplicate(origobj);
            firstNewObject.GetComponent<Transform>().position = new Vector3(0, 0, 0);
            firstNewObject.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 1);
            //firstNewObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            bool isGoldAndAllowed = KTL && !origobj.GetComponent<GoldenSimples>().worldMapped_;
            float originalZTexoff = firstNewObject.GetComponent<GoldenSimples>().textureOffset_.z;
            float originalZTexScale = firstNewObject.GetComponent<GoldenSimples>().textureScale_.z;
            if (isGoldAndAllowed)
            {
                firstNewObject.GetComponent<GoldenSimples>().textureOffset_.z = 0;
                firstNewObject.GetComponent<GoldenSimples>().textureScale_.z = 1;
            }

            newerObjects.Add(firstNewObject);
            Vector3 defaultObjSize = getDefaultObjSize(origobj.name);

            float w = 0;
            float wX = 0;
            float wY = 0;



            xBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref w, true);
            yBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref w, false);
            zBasedDivider(defaultObjSize, ref newerObjects, isGoldAndAllowed, ref wX, ref wY, true);



            List<GameObject> newererObjects = new List<GameObject>();
            foreach (GameObject gameObject in newerObjects)
            {
                List<GameObject> newerereerObjects = new List<GameObject>();
                GameObject[] planeObjects = CubeToPlaneAction.TurnCubeIntoPlanesForDivider(gameObject, origobj, w, wX, wY, 1F, originalZTexoff, originalZTexScale);

                GroupAction planecubeGroupAction = Group.CreateGroupAction(planeObjects, planeObjects[0]);
                GameObject newerObjectsGroupC = planecubeGroupAction.GroupObjects().gameObject;
                if (newerObjectsGroupC.HasComponent<CustomName>())
                {
                    newerObjectsGroupC.GetComponent<CustomName>().CustomName_ = "PlaneCubeGS";
                }
                ObjHelper.copyAddedComponentsOverWithTexture(ref newerObjectsGroupC, origobj, true);
                ReferenceMap.Handle<GameObject> newerObjectsGroupReplacementHandle = ObjHelper.ReplaceWithDuplicate(ref newerObjectsGroupC, ref newObjectHandles_[newObjectHandleIndex]);
                newObjectHandleIndex += 1;
                newererObjects.Add(newerObjectsGroupC);
            }
            newerObjects.Clear();
            newerObjects.AddRange(newererObjects);


            Group.InitData data = new Group.InitData(Vector3.zero, Quaternion.identity, Vector3.one, new Bounds());
            GroupAction groupAction = new GroupAction(newerObjects.ToArray(), data);
            GameObject newerObjectsGroup = groupAction.GroupObjects().gameObject;
            newerObjectsGroup.GetComponent<Transform>().localPosition = origobjpos;
            newerObjectsGroup.GetComponent<Transform>().localRotation = origobjrot;
            //newerObjectsGroup.GetComponent<Transform>().localScale = origobjscl;
            GameObject[] finalObjectsBeforeAdd = groupAction.UngroupObjects();
            newObjects.AddRange(finalObjectsBeforeAdd);
            origobj.GetComponent<Transform>().localPosition = origobjpos;
            origobj.GetComponent<Transform>().localRotation = origobjrot;
            origobj.GetComponent<Transform>().localScale = origobjscl;
        }

        public void xBasedDivider(Vector3 defaultObjSize, ref List<GameObject> newerObjects, bool isGoldAndAllowed, ref float w, bool useW)
        {
            if (defaultObjSize.x != -1)
            {
                GameObject[] newererObjects = new GameObject[xdivides + 1];
                List<GameObject> newerObjectsTemp = new List<GameObject>();
                foreach (GameObject gameObject in newerObjects)
                {
                    GameObject newestObject = DuplicateObjectsAction.Duplicate(gameObject);
                    GoldenSimples originalGS = gameObject.GetComponent<GoldenSimples>();


                    Transform newestObjectTrans = newestObject.GetComponent<Transform>();
                    float originalXScale = newestObjectTrans.localScale.x * defaultObjSize.x;
                    float newXScale = (newestObjectTrans.localScale.x * (defaultObjSize.x - xspacing * xdivides)) / (defaultObjSize.x * (xdivides + 1));
                    float newXScalePart = (defaultObjSize.x - xspacing * xdivides) / (defaultObjSize.x * (xdivides + 1));
                    float translationalSpacingMultiplier = 1 + (xspacing / defaultObjSize.x);
                    newestObjectTrans.localPosition = new Vector3((float)((newestObjectTrans.localPosition.x - ((originalXScale / (1.0 + xdivides)) * .5 * xdivides)) * translationalSpacingMultiplier), newestObjectTrans.localPosition.y, newestObjectTrans.localPosition.z);
                    newestObjectTrans.localScale = new Vector3(newXScale, newestObjectTrans.localScale.y, newestObjectTrans.localScale.z);
                    if (isGoldAndAllowed)
                    {
                        GoldenSimples objgold = newestObject.GetComponent<GoldenSimples>();
                        objgold.textureScale_ = new Vector3((float)(originalGS.textureScale_.x * newXScalePart), objgold.textureScale_.y, objgold.textureScale_.z);
                        objgold.textureOffset_ = new Vector3((float)(objgold.textureOffset_.x + originalGS.textureScale_.x * ((gameObject.GetComponent<Transform>().localPosition.x - newestObjectTrans.localPosition.x - ((newXScale * defaultObjSize.x) / 2) + originalXScale / 2) / originalXScale)), objgold.textureOffset_.y, objgold.textureOffset_.z);
                    }
                    if (isGoldAndAllowed && w == 0 && useW)
                    {
                        w = newestObject.GetComponent<GoldenSimples>().textureOffset_.x + 1F * originalGS.textureOffset_.x;
                    }
                    newererObjects[0] = newestObject;
                    for (int i = 1; i < xdivides + 1; i++)
                    {
                        GameObject newestestObject = DuplicateObjectsAction.Duplicate(newererObjects[i - 1]);
                        Transform newestestObjectTrans = newestestObject.GetComponent<Transform>();
                        newestestObjectTrans.localPosition = new Vector3((float)(newestestObjectTrans.localPosition.x + (originalXScale / ((1.0 + xdivides))) * translationalSpacingMultiplier), newestestObjectTrans.localPosition.y, newestestObjectTrans.localPosition.z);
                        if (isGoldAndAllowed)
                        {
                            GoldenSimples objgold = newestestObject.GetComponent<GoldenSimples>();
                            objgold.textureScale_ = new Vector3((float)(originalGS.textureScale_.x * newXScalePart), objgold.textureScale_.y, objgold.textureScale_.z);
                            objgold.textureOffset_ = new Vector3((float)(originalGS.textureOffset_.x + originalGS.textureScale_.x * ((gameObject.GetComponent<Transform>().localPosition.x - newestestObjectTrans.localPosition.x - ((newXScale * defaultObjSize.x) / 2) + originalXScale / 2) / originalXScale)), objgold.textureOffset_.y, objgold.textureOffset_.z);
                        }
                        newererObjects[i] = newestestObject;
                    }
                    newerObjectsTemp.AddRange(newererObjects);
                }
                newerObjects.Clear();
                newerObjects.AddRange(newerObjectsTemp);
            }
        }

        public void yBasedDivider(Vector3 defaultObjSize, ref List<GameObject> newerObjects, bool isGoldAndAllowed, ref float w, bool useW)
        {
            if (defaultObjSize.y != -1)
            {
                GameObject[] newererObjects = new GameObject[ydivides + 1];
                List<GameObject> newerObjectsTemp = new List<GameObject>();
                foreach (GameObject gameObject in newerObjects)
                {
                    GameObject newestObject = DuplicateObjectsAction.Duplicate(gameObject);
                    GoldenSimples originalGS = gameObject.GetComponent<GoldenSimples>();
                    Transform newestObjectTrans = newestObject.GetComponent<Transform>();
                    float originalYScale = newestObjectTrans.localScale.y * defaultObjSize.y;
                    float newYScale = (newestObjectTrans.localScale.y * (defaultObjSize.y - yspacing * ydivides)) / (defaultObjSize.y * (ydivides + 1));
                    float newYScalePart = (defaultObjSize.y - yspacing * ydivides) / (defaultObjSize.y * (ydivides + 1));
                    float translationalSpacingMultiplier = 1 + (yspacing / defaultObjSize.y);
                    newestObjectTrans.localPosition = new Vector3(newestObjectTrans.localPosition.x, (float)((newestObjectTrans.localPosition.y - ((originalYScale / (1.0 + ydivides)) * .5 * ydivides)) * translationalSpacingMultiplier), newestObjectTrans.localPosition.z);
                    newestObjectTrans.localScale = new Vector3(newestObjectTrans.localScale.x, newYScale, newestObjectTrans.localScale.z);
                    if (isGoldAndAllowed)
                    {
                        GoldenSimples objgold = newestObject.GetComponent<GoldenSimples>();
                        objgold.textureScale_ = new Vector3(objgold.textureScale_.x, (float)(originalGS.textureScale_.y * newYScalePart), objgold.textureScale_.z);
                        objgold.textureOffset_ = new Vector3(objgold.textureOffset_.x, (float)(originalGS.textureOffset_.y + originalGS.textureScale_.y * ((-gameObject.GetComponent<Transform>().localPosition.y + newestObjectTrans.localPosition.y - ((newYScale * defaultObjSize.y) / 2) + originalYScale / 2) / originalYScale)), objgold.textureOffset_.z);
                    }
                    if (isGoldAndAllowed && w == 0 && useW)
                    {
                        w = newestObject.GetComponent<GoldenSimples>().textureOffset_.y + 1F * originalGS.textureOffset_.y;
                    }
                    newererObjects[0] = newestObject;
                    for (int i = 1; i < ydivides + 1; i++)
                    {
                        GameObject newestestObject = DuplicateObjectsAction.Duplicate(newererObjects[i - 1]);
                        Transform newestestObjectTrans = newestestObject.GetComponent<Transform>();
                        newestestObjectTrans.localPosition = new Vector3(newestestObjectTrans.localPosition.x, (float)(newestestObjectTrans.localPosition.y + (originalYScale / ((1.0 + ydivides))) * translationalSpacingMultiplier), newestestObjectTrans.localPosition.z);
                        if (isGoldAndAllowed)
                        {
                            GoldenSimples objgold = newestestObject.GetComponent<GoldenSimples>();
                            objgold.textureScale_ = new Vector3(objgold.textureScale_.x, (float)(originalGS.textureScale_.y * newYScalePart), objgold.textureScale_.z);
                            objgold.textureOffset_ = new Vector3(objgold.textureOffset_.x, (float)(originalGS.textureOffset_.y + originalGS.textureScale_.y * ((-gameObject.GetComponent<Transform>().localPosition.y + newestestObjectTrans.localPosition.y - ((newYScale * defaultObjSize.y) / 2) + originalYScale / 2) / originalYScale)), objgold.textureOffset_.z);
                        }
                        newererObjects[i] = newestestObject;
                    }
                    newerObjectsTemp.AddRange(newererObjects);
                }
                newerObjects.Clear();
                newerObjects.AddRange(newerObjectsTemp);
            }
        }

        public void zBasedDivider(Vector3 defaultObjSize, ref List<GameObject> newerObjects, bool isGoldAndAllowed, ref float wX, ref float wY, bool useW)
        {
            if (defaultObjSize.z != -1)
            {
                GameObject[] newererObjects = new GameObject[zdivides + 1];
                List<GameObject> newerObjectsTemp = new List<GameObject>();
                foreach (GameObject gameObject in newerObjects)
                {
                    GameObject newestObject = DuplicateObjectsAction.Duplicate(gameObject);
                    GoldenSimples originalGS = gameObject.GetComponent<GoldenSimples>();
                    Transform newestObjectTrans = newestObject.GetComponent<Transform>();
                    float originalZScale = newestObjectTrans.localScale.z * defaultObjSize.z;
                    float newZScale = (newestObjectTrans.localScale.z * (defaultObjSize.z - zspacing * zdivides)) / (defaultObjSize.z * (zdivides + 1));
                    float newZScalePart = (defaultObjSize.z - zspacing * zdivides) / (defaultObjSize.z * (zdivides + 1));
                    float translationalSpacingMultiplier = 1 + (zspacing / defaultObjSize.z);
                    newestObjectTrans.localPosition = new Vector3(newestObjectTrans.localPosition.x, newestObjectTrans.localPosition.y , (float)((newestObjectTrans.localPosition.z - ((originalZScale / (1.0 + zdivides)) * .5 * zdivides)) * translationalSpacingMultiplier));
                    newestObjectTrans.localScale = new Vector3(newestObjectTrans.localScale.x, newestObjectTrans.localScale.y, newZScale);
                    if (isGoldAndAllowed)
                    {
                        GoldenSimples objgold = newestObject.GetComponent<GoldenSimples>();
                        objgold.textureScale_ = new Vector3(objgold.textureScale_.x, objgold.textureScale_.y, (float)(newZScalePart));
                        objgold.textureOffset_ = new Vector3(objgold.textureOffset_.x, objgold.textureOffset_.y, (float)(((gameObject.GetComponent<Transform>().localPosition.z - newestObjectTrans.localPosition.z - ((newZScale * defaultObjSize.z) / 2) + originalZScale / 2) / originalZScale)));
                    }
                    if (isGoldAndAllowed && wX == 0 && useW)
                    {
                        wX = newestObject.GetComponent<GoldenSimples>().textureOffset_.z;
                    }
                    if(isGoldAndAllowed && wY == 0 && useW)
                    {
                        wY = newestObject.GetComponent<GoldenSimples>().textureOffset_.z;
                    }
                    newererObjects[0] = newestObject;
                    for (int i = 1; i < zdivides + 1; i++)
                    {
                        GameObject newestestObject = DuplicateObjectsAction.Duplicate(newererObjects[i - 1]);
                        Transform newestestObjectTrans = newestestObject.GetComponent<Transform>();
                        newestestObjectTrans.localPosition = new Vector3(newestestObjectTrans.localPosition.x, newestestObjectTrans.localPosition.y, (float)(newestestObjectTrans.localPosition.z + (originalZScale / ((1.0 + zdivides))) * translationalSpacingMultiplier));
                        if (isGoldAndAllowed)
                        {
                            GoldenSimples objgold = newestestObject.GetComponent<GoldenSimples>();
                            objgold.textureScale_ = new Vector3(objgold.textureScale_.x, objgold.textureScale_.y, (float)(originalGS.textureScale_.z * newZScalePart));
                            objgold.textureOffset_ = new Vector3(objgold.textureOffset_.x, objgold.textureOffset_.y, (float)(originalGS.textureOffset_.z + originalGS.textureScale_.z * ((gameObject.GetComponent<Transform>().localPosition.z - newestestObjectTrans.localPosition.z - ((newZScale * defaultObjSize.z) / 2) + originalZScale / 2) / originalZScale)));
                        }
                        newererObjects[i] = newestestObject;
                    }
                    newerObjectsTemp.AddRange(newererObjects);
                }
                newerObjects.Clear();
                newerObjects.AddRange(newerObjectsTemp);
            }
        }

        public void zBasedPlazer(Vector3 defaultObjSize, ref List<GameObject> newerObjects, bool isGoldAndAllowed, GameObject origobj, LevelLayer origobjlayer, ref int newObjectHandleIndex, float w,float wX,float wY, float wM, float originalZTexoff, float originalZTexScale)
        {
            List<GameObject> newererObjects = new List<GameObject>();
            List<GameObject> newerObjectsTemp = new List<GameObject>();
            foreach (GameObject gameObject in newerObjects)
            {
                List<GameObject> newerereerObjects = new List<GameObject>();
                GameObject[] planeObjects = CubeToPlaneAction.TurnCubeIntoPlanesForDivider(gameObject, origobj, w, wX, wY, wM, originalZTexoff, originalZTexScale);
                //newerereerObjects.AddRange(planeObjects);
                int positiveZAxisPlaneIndex = 2;
                int negativeZAxisPlaneIndex = 1;
                int positiveYAxisPlaneIndex = 0;
                int negativeYAxisPlaneIndex = 5;
                for (int i = 0; i < 6; i++)
                {
                    if (planeObjects[i].GetComponent<Transform>().localPosition.z < planeObjects[negativeZAxisPlaneIndex].GetComponent<Transform>().localPosition.z)
                    {
                        positiveZAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.z > planeObjects[positiveZAxisPlaneIndex].GetComponent<Transform>().localPosition.z)
                    {
                        negativeZAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.y < planeObjects[negativeYAxisPlaneIndex].GetComponent<Transform>().localPosition.y)
                    {
                        positiveYAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.y > planeObjects[positiveYAxisPlaneIndex].GetComponent<Transform>().localPosition.y)
                    {
                        negativeYAxisPlaneIndex = i;
                    }
                }
                newerereerObjects.Add(planeObjects[positiveZAxisPlaneIndex]);
                newerereerObjects.Add(planeObjects[negativeZAxisPlaneIndex]);
                for (int i = 0; i < 6; i++)
                {
                    if (i != positiveZAxisPlaneIndex && i != negativeZAxisPlaneIndex)
                    {
                        if (i == positiveYAxisPlaneIndex || i == negativeYAxisPlaneIndex)
                        {
                            int dividableObjectsCount;
                            DivideAction action = new DivideAction(new GameObject[] { planeObjects[i] }, 0, 0, zdivides, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            newerereerObjects.AddRange(action.DivideObjects(false));
                        }
                        else
                        {
                            int dividableObjectsCount;
                            DivideAction action = new DivideAction(new GameObject[] { planeObjects[i] }, zdivides, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            newerereerObjects.AddRange(action.DivideObjects(false));
                        }
                    }
                }

                float originalZScale = origobj.GetComponent<Transform>().localScale.z * defaultObjSize.z;
                GameObject[] newererestObjects = new GameObject[zdivides];
                ReferenceMap.Handle<GameObject>[] newererestObjectHandles_ = new ReferenceMap.Handle<GameObject>[zdivides];
                for (int i = 0; i < zdivides; i++)
                {
                    GameObject newestestObject;
                    if (i != 0)
                    {
                        newestestObject = DuplicateObjectsAction.Duplicate(newererestObjects[i - 1]);
                    }
                    else
                    {
                        newestestObject = DuplicateObjectsAction.Duplicate(planeObjects[negativeZAxisPlaneIndex]);
                    }
                    Transform newestestObjectTrans = newestestObject.GetComponent<Transform>();
                    newestestObjectTrans.localPosition = new Vector3(newestestObjectTrans.localPosition.x, newestestObjectTrans.localPosition.y, (float)(newestestObjectTrans.localPosition.z + (originalZScale / ((1.0 + zdivides))) * 1));
                    G.Sys.LevelEditor_.AddGameObject(ref newererestObjectHandles_[i], newestestObject, origobjlayer);
                    newererestObjects[i] = newestestObject;
                }
                newerereerObjects.AddRange(newererestObjects);
                GroupAction planecubeGroupAction = Group.CreateGroupAction(newerereerObjects.ToArray(), newerereerObjects.ToArray()[0]);
                GameObject newerObjectsGroupC = planecubeGroupAction.GroupObjects().gameObject;
                if (newerObjectsGroupC.HasComponent<CustomName>())
                {
                    newerObjectsGroupC.GetComponent<CustomName>().CustomName_ = "DividedPlaneCubeGS";
                }
                ObjHelper.copyAddedComponentsOverWithTexture(ref newerObjectsGroupC, origobj, true);
                ReferenceMap.Handle<GameObject> newerObjectsGroupReplacementHandle = ObjHelper.ReplaceWithDuplicate(ref newerObjectsGroupC, ref newObjectHandles_[newObjectHandleIndex]);
                newObjectHandleIndex += 1;
                newererObjects.Add(newerObjectsGroupC);
            }
            newerObjects.Clear();
            newerObjects.AddRange(newererObjects);
        }

        public void singleAxisBasedPlazer(Vector3 defaultObjSize, ref List<GameObject> newerObjects, bool isGoldAndAllowed, GameObject origobj, LevelLayer origobjlayer, ref int newObjectHandleIndex, float w, float wX, float wY, float wM, float originalZTexoff, float originalZTexScale, int axis)
        {
            List<GameObject> newererObjects = new List<GameObject>();
            foreach (GameObject gameObject in newerObjects)
            {
                List<GameObject> newerereerObjects = new List<GameObject>();
                GameObject[] planeObjects = CubeToPlaneAction.TurnCubeIntoPlanesForDivider(gameObject, origobj, w, wX, wY,wM, originalZTexoff, originalZTexScale);
                int positiveZAxisPlaneIndex = 2;
                int negativeZAxisPlaneIndex = 1;
                int positiveYAxisPlaneIndex = 0;
                int negativeYAxisPlaneIndex = 5;
                int positiveXAxisPlaneIndex = 3;
                int negativeXAxisPlaneIndex = 4;
                for (int i = 0; i < 6; i++)
                {
                    if (planeObjects[i].GetComponent<Transform>().localPosition.z < planeObjects[negativeZAxisPlaneIndex].GetComponent<Transform>().localPosition.z)
                    {
                        positiveZAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.z > planeObjects[positiveZAxisPlaneIndex].GetComponent<Transform>().localPosition.z)
                    {
                        negativeZAxisPlaneIndex = i;
                    }
                    if (planeObjects[i].GetComponent<Transform>().localPosition.y < planeObjects[negativeYAxisPlaneIndex].GetComponent<Transform>().localPosition.y)
                    {
                        positiveYAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.y > planeObjects[positiveYAxisPlaneIndex].GetComponent<Transform>().localPosition.y)
                    {
                        negativeYAxisPlaneIndex = i;
                    }
                    if (planeObjects[i].GetComponent<Transform>().localPosition.x < planeObjects[negativeXAxisPlaneIndex].GetComponent<Transform>().localPosition.x)
                    {
                        positiveXAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.x > planeObjects[positiveXAxisPlaneIndex].GetComponent<Transform>().localPosition.x)
                    {
                        negativeXAxisPlaneIndex = i;
                    }
                }
                int avoidindex1 = 1;
                int avoidindex2 = 2;
                int divideamount = zdivides;
                int negativePlaneIndex = 1;
                if (axis == 0)
                {
                    newerereerObjects.Add(planeObjects[positiveXAxisPlaneIndex]);
                    newerereerObjects.Add(planeObjects[negativeXAxisPlaneIndex]);
                    avoidindex1 = positiveXAxisPlaneIndex;
                    avoidindex2 = negativeXAxisPlaneIndex;
                    divideamount = xdivides;
                    negativePlaneIndex = negativeXAxisPlaneIndex;
                }
                else if (axis == 1)
                {
                    newerereerObjects.Add(planeObjects[positiveYAxisPlaneIndex]);
                    newerereerObjects.Add(planeObjects[negativeYAxisPlaneIndex]);
                    avoidindex1 = positiveYAxisPlaneIndex;
                    avoidindex2 = negativeYAxisPlaneIndex;
                    divideamount = ydivides;
                    negativePlaneIndex = negativeYAxisPlaneIndex;
                }
                else if(axis == 2)
                {
                    newerereerObjects.Add(planeObjects[positiveZAxisPlaneIndex]);
                    newerereerObjects.Add(planeObjects[negativeZAxisPlaneIndex]);
                    avoidindex1 = positiveZAxisPlaneIndex;
                    avoidindex2 = negativeZAxisPlaneIndex;
                    divideamount = zdivides;
                    negativePlaneIndex = negativeZAxisPlaneIndex;
                }
                //newerereerObjects.AddRange(planeObjects);
                
                for (int i = 0; i < 6; i++)
                {
                    if (i != avoidindex1 && i != avoidindex2)
                    {
                        int dividableObjectsCount;
                        DivideAction action;
                        if (i == positiveXAxisPlaneIndex || i == negativeXAxisPlaneIndex)
                        {
                            if(axis == 2) action = new DivideAction(new GameObject[] { planeObjects[i] }, divideamount, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            else action = new DivideAction(new GameObject[] { planeObjects[i] }, 0, 0, divideamount, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            newerereerObjects.AddRange(action.DivideObjects(false));
                        }
                        else if(i == positiveYAxisPlaneIndex || i == negativeYAxisPlaneIndex)
                        {
                            if (axis == 2) action = new DivideAction(new GameObject[] { planeObjects[i] }, 0, 0, divideamount, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            else action = new DivideAction(new GameObject[] { planeObjects[i] }, divideamount, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            newerereerObjects.AddRange(action.DivideObjects(false));
                        }
                        else if (i == positiveZAxisPlaneIndex || i == negativeZAxisPlaneIndex)
                        {
                            if (axis == 0) action = new DivideAction(new GameObject[] { planeObjects[i] }, divideamount, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            else action = new DivideAction(new GameObject[] { planeObjects[i] }, 0, 0, divideamount, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            newerereerObjects.AddRange(action.DivideObjects(false));
                        }
                    }
                }
                
                float originalAxisScale = origobj.GetComponent<Transform>().localScale.z * defaultObjSize.z;
                if(axis == 0) originalAxisScale = origobj.GetComponent<Transform>().localScale.x * defaultObjSize.x;
                else if (axis == 1) originalAxisScale = origobj.GetComponent<Transform>().localScale.y * defaultObjSize.y;

                GameObject[] newererestObjects = new GameObject[divideamount];

                ReferenceMap.Handle<GameObject>[] newererestObjectHandles_ = new ReferenceMap.Handle<GameObject>[divideamount];

                for (int i = 0; i < divideamount; i++)
                {
                    GameObject newestestObject;
                    if (i != 0)
                    {
                        newestestObject = DuplicateObjectsAction.Duplicate(newererestObjects[i - 1]);
                    }
                    else
                    {
                        newestestObject = DuplicateObjectsAction.Duplicate(planeObjects[negativePlaneIndex]);
                    }
                    Transform newestestObjectTrans = newestestObject.GetComponent<Transform>();
                    if (axis == 0) newestestObjectTrans.localPosition = new Vector3((float)(newestestObjectTrans.localPosition.x + (originalAxisScale / ((1.0 + divideamount))) * 1), newestestObjectTrans.localPosition.y, newestestObjectTrans.localPosition.z);
                    else if (axis == 1) newestestObjectTrans.localPosition = new Vector3(newestestObjectTrans.localPosition.x, (float)(newestestObjectTrans.localPosition.y + (originalAxisScale / ((1.0 + divideamount))) * 1), newestestObjectTrans.localPosition.z);
                    else if (axis == 2) newestestObjectTrans.localPosition = new Vector3(newestestObjectTrans.localPosition.x, newestestObjectTrans.localPosition.y, (float)(newestestObjectTrans.localPosition.z + (originalAxisScale / ((1.0 + divideamount))) * 1));
                    G.Sys.LevelEditor_.AddGameObject(ref newererestObjectHandles_[i], newestestObject, origobjlayer);
                    newererestObjects[i] = newestestObject;
                }
                newerereerObjects.AddRange(newererestObjects);
                GroupAction planecubeGroupAction = Group.CreateGroupAction(newerereerObjects.ToArray(), newerereerObjects.ToArray()[0]);
                GameObject newerObjectsGroupC = planecubeGroupAction.GroupObjects().gameObject;
                if (newerObjectsGroupC.HasComponent<CustomName>())
                {
                    newerObjectsGroupC.GetComponent<CustomName>().CustomName_ = "DividedPlaneCubeGS";
                }
                ObjHelper.copyAddedComponentsOverWithTexture(ref newerObjectsGroupC, origobj, true);
                ReferenceMap.Handle<GameObject> newerObjectsGroupReplacementHandle = ObjHelper.ReplaceWithDuplicate(ref newerObjectsGroupC, ref newObjectHandles_[newObjectHandleIndex]);
                newObjectHandleIndex += 1;
                newererObjects.Add(newerObjectsGroupC);
            }
            newerObjects.Clear();
            newerObjects.AddRange(newererObjects);
        }

        public void doubleAxisBasedPlazer(Vector3 defaultObjSize, ref List<GameObject> newerObjects, bool isGoldAndAllowed, GameObject origobj, LevelLayer origobjlayer, ref int newObjectHandleIndex, float w, float wX, float wY, float wM, float originalZTexoff, float originalZTexScale, int axis)
        {
            List<GameObject> newererObjects = new List<GameObject>();
            foreach (GameObject gameObject in newerObjects)
            {
                List<GameObject> newerereerObjects = new List<GameObject>();
                List<GameObject> otheraxisrepetaplanes1 = new List<GameObject>();
                List<GameObject> otheraxisrepetaplanes2 = new List<GameObject>();
                GameObject[] planeObjects = CubeToPlaneAction.TurnCubeIntoPlanesForDivider(gameObject, origobj, w, wX, wY, wM, originalZTexoff, originalZTexScale);
                int positiveZAxisPlaneIndex = 2;
                int negativeZAxisPlaneIndex = 1;
                int positiveYAxisPlaneIndex = 0;
                int negativeYAxisPlaneIndex = 5;
                int positiveXAxisPlaneIndex = 3;
                int negativeXAxisPlaneIndex = 4;
                for (int i = 0; i < 6; i++)
                {
                    if (planeObjects[i].GetComponent<Transform>().localPosition.z < planeObjects[negativeZAxisPlaneIndex].GetComponent<Transform>().localPosition.z)
                    {
                        positiveZAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.z > planeObjects[positiveZAxisPlaneIndex].GetComponent<Transform>().localPosition.z)
                    {
                        negativeZAxisPlaneIndex = i;
                    }
                    if (planeObjects[i].GetComponent<Transform>().localPosition.y < planeObjects[negativeYAxisPlaneIndex].GetComponent<Transform>().localPosition.y)
                    {
                        positiveYAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.y > planeObjects[positiveYAxisPlaneIndex].GetComponent<Transform>().localPosition.y)
                    {
                        negativeYAxisPlaneIndex = i;
                    }
                    if (planeObjects[i].GetComponent<Transform>().localPosition.x < planeObjects[negativeXAxisPlaneIndex].GetComponent<Transform>().localPosition.x)
                    {
                        positiveXAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.x > planeObjects[positiveXAxisPlaneIndex].GetComponent<Transform>().localPosition.x)
                    {
                        negativeXAxisPlaneIndex = i;
                    }
                }
                int avoidindex1axis1 = 1;
                int avoidindex2axis1 = 1;
                int avoidindex1axis2 = 2;
                int avoidindex2axis2 = 2;
                int divideamount1 = ydivides;
                int divideamount2 = zdivides;
                int otheraxis1 = 1;
                int otheraxis2 = 2;
                int negativePlaneIndex1 = 1;
                int negativePlaneIndex2 = 2;
                float originalOtherAxis1Scale = origobj.GetComponent<Transform>().localScale.y * defaultObjSize.y;
                float originalOtherAxis2Scale = origobj.GetComponent<Transform>().localScale.z * defaultObjSize.z;
                if (axis == 0)
                {
                    avoidindex1axis1 = positiveYAxisPlaneIndex;
                    avoidindex2axis1 = negativeYAxisPlaneIndex;
                    avoidindex1axis2 = positiveZAxisPlaneIndex;
                    avoidindex2axis2 = negativeZAxisPlaneIndex;
                    divideamount1 = ydivides;
                    divideamount2 = zdivides;
                    negativePlaneIndex1 = negativeYAxisPlaneIndex;
                    negativePlaneIndex2 = negativeZAxisPlaneIndex;
                    otheraxis1 = 1;
                    otheraxis2 = 2;
                    originalOtherAxis1Scale = origobj.GetComponent<Transform>().localScale.y * defaultObjSize.y;
                    originalOtherAxis2Scale = origobj.GetComponent<Transform>().localScale.z * defaultObjSize.z;
                }
                else if (axis == 1)
                {
                    avoidindex1axis1 = positiveXAxisPlaneIndex;
                    avoidindex2axis1 = negativeXAxisPlaneIndex;
                    avoidindex1axis2 = positiveZAxisPlaneIndex;
                    avoidindex2axis2 = negativeZAxisPlaneIndex;
                    divideamount1 = xdivides;
                    divideamount2 = zdivides;
                    negativePlaneIndex1 = negativeXAxisPlaneIndex;
                    negativePlaneIndex2 = negativeZAxisPlaneIndex;
                    otheraxis1 = 0;
                    otheraxis2 = 2;
                    originalOtherAxis1Scale = origobj.GetComponent<Transform>().localScale.x * defaultObjSize.x;
                    originalOtherAxis2Scale = origobj.GetComponent<Transform>().localScale.z * defaultObjSize.z;
                }
                else if (axis == 2)
                {
                    avoidindex1axis1 = positiveXAxisPlaneIndex;
                    avoidindex2axis1 = negativeXAxisPlaneIndex;
                    avoidindex1axis2 = positiveYAxisPlaneIndex;
                    avoidindex2axis2 = negativeYAxisPlaneIndex;
                    divideamount1 = xdivides;
                    divideamount2 = ydivides;
                    negativePlaneIndex1 = negativeXAxisPlaneIndex;
                    negativePlaneIndex2 = negativeYAxisPlaneIndex;
                    otheraxis1 = 0;
                    otheraxis2 = 1;
                    originalOtherAxis1Scale = origobj.GetComponent<Transform>().localScale.x * defaultObjSize.x;
                    originalOtherAxis2Scale = origobj.GetComponent<Transform>().localScale.y * defaultObjSize.y;
                }
                //newerereerObjects.AddRange(planeObjects);

                GameObject[] newOtherAxis1Planes = new GameObject[divideamount1];
                GameObject[] newOtherAxis2Planes = new GameObject[divideamount2];

                ReferenceMap.Handle<GameObject>[] newOtherAxis1PlaneHandles_ = new ReferenceMap.Handle<GameObject>[divideamount1];
                ReferenceMap.Handle<GameObject>[] newOtherAxis2PlaneHandles_ = new ReferenceMap.Handle<GameObject>[divideamount2];

                for (int i = 0; i < divideamount1; i++)
                {
                    GameObject newOtherAxis1Plane;
                    if (i != 0)
                    {
                        newOtherAxis1Plane = DuplicateObjectsAction.Duplicate(newOtherAxis1Planes[i - 1]);
                    }
                    else
                    {
                        newOtherAxis1Plane = DuplicateObjectsAction.Duplicate(planeObjects[negativePlaneIndex1]);
                    }
                    Transform newOtherAxis1PlaneTrans = newOtherAxis1Plane.GetComponent<Transform>();
                    if (otheraxis1 == 0) newOtherAxis1PlaneTrans.localPosition = new Vector3((float)(newOtherAxis1PlaneTrans.localPosition.x + (originalOtherAxis1Scale / ((1.0 + divideamount1))) * 1), newOtherAxis1PlaneTrans.localPosition.y, newOtherAxis1PlaneTrans.localPosition.z);
                    else if (otheraxis1 == 1) newOtherAxis1PlaneTrans.localPosition = new Vector3(newOtherAxis1PlaneTrans.localPosition.x, (float)(newOtherAxis1PlaneTrans.localPosition.y + (originalOtherAxis1Scale / ((1.0 + divideamount1))) * 1), newOtherAxis1PlaneTrans.localPosition.z);
                    else if (otheraxis1 == 2) newOtherAxis1PlaneTrans.localPosition = new Vector3(newOtherAxis1PlaneTrans.localPosition.x, newOtherAxis1PlaneTrans.localPosition.y, (float)(newOtherAxis1PlaneTrans.localPosition.z + (originalOtherAxis1Scale / ((1.0 + divideamount1))) * 1));
                    G.Sys.LevelEditor_.AddGameObject(ref newOtherAxis1PlaneHandles_[i], newOtherAxis1Plane, origobjlayer);
                    newOtherAxis1Planes[i] = newOtherAxis1Plane;
                }

                for (int i = 0; i < divideamount2; i++)
                {
                    GameObject newOtherAxis2Plane;
                    if (i != 0)
                    {
                        newOtherAxis2Plane = DuplicateObjectsAction.Duplicate(newOtherAxis2Planes[i - 1]);
                    }
                    else
                    {
                        newOtherAxis2Plane = DuplicateObjectsAction.Duplicate(planeObjects[negativePlaneIndex2]);
                    }
                    Transform newOtherAxis2PlaneTrans = newOtherAxis2Plane.GetComponent<Transform>();
                    if (otheraxis2 == 0) newOtherAxis2PlaneTrans.localPosition = new Vector3((float)(newOtherAxis2PlaneTrans.localPosition.x + (originalOtherAxis2Scale / ((1.0 + divideamount2))) * 1), newOtherAxis2PlaneTrans.localPosition.y, newOtherAxis2PlaneTrans.localPosition.z);
                    else if (otheraxis2 == 1) newOtherAxis2PlaneTrans.localPosition = new Vector3(newOtherAxis2PlaneTrans.localPosition.x, (float)(newOtherAxis2PlaneTrans.localPosition.y + (originalOtherAxis2Scale / ((1.0 + divideamount2))) * 1), newOtherAxis2PlaneTrans.localPosition.z);
                    else if (otheraxis2 == 2) newOtherAxis2PlaneTrans.localPosition = new Vector3(newOtherAxis2PlaneTrans.localPosition.x, newOtherAxis2PlaneTrans.localPosition.y, (float)(newOtherAxis2PlaneTrans.localPosition.z + (originalOtherAxis2Scale / ((1.0 + divideamount2))) * 1));
                    G.Sys.LevelEditor_.AddGameObject(ref newOtherAxis2PlaneHandles_[i], newOtherAxis2Plane, origobjlayer);
                    newOtherAxis2Planes[i] = newOtherAxis2Plane;
                }

                
                for (int i = 0; i < 6; i++)
                {
                    if (i != avoidindex1axis1 && i != avoidindex2axis1 && i != avoidindex1axis2 && i != avoidindex2axis2)
                    {
                        int dividableObjectsCount;
                        DivideAction action;
                        if (i == positiveXAxisPlaneIndex || i == negativeXAxisPlaneIndex)
                        {
                            action = new DivideAction(new GameObject[] { planeObjects[i] }, divideamount2, 0, divideamount1, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            newerereerObjects.AddRange(action.DivideObjects(false));
                        }
                        else if (i == positiveYAxisPlaneIndex || i == negativeYAxisPlaneIndex)
                        {
                            action = new DivideAction(new GameObject[] { planeObjects[i] }, divideamount1, 0, divideamount2, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            newerereerObjects.AddRange(action.DivideObjects(false));
                        }
                        else if (i == positiveZAxisPlaneIndex || i == negativeZAxisPlaneIndex)
                        {
                            action = new DivideAction(new GameObject[] { planeObjects[i] }, divideamount1, 0, divideamount2, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            newerereerObjects.AddRange(action.DivideObjects(false));
                        }
                    }
                }
                
                for (int i = 0; i < 6; i++)
                {
                    if (!(i != avoidindex1axis1 && i != avoidindex2axis1 && i != avoidindex1axis2 && i != avoidindex2axis2))
                    {
                        int dividableObjectsCount;
                        DivideAction action;
                        if (i == positiveXAxisPlaneIndex || i == negativeXAxisPlaneIndex)
                        {
                            if (axis == 2) action = new DivideAction(new GameObject[] { planeObjects[i] }, 0, 0, divideamount1, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            else action = new DivideAction(new GameObject[] { planeObjects[i] }, divideamount2, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            newerereerObjects.AddRange(action.DivideObjects(false));
                        }
                        else if (i == positiveYAxisPlaneIndex || i == negativeYAxisPlaneIndex)
                        {
                            if (axis == 0) action = new DivideAction(new GameObject[] { planeObjects[i] }, 0, 0, divideamount2, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            else action = new DivideAction(new GameObject[] { planeObjects[i] }, divideamount1, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            newerereerObjects.AddRange(action.DivideObjects(false));
                        }
                        else if (i == positiveZAxisPlaneIndex || i == negativeZAxisPlaneIndex)
                        {
                            if (axis == 0) action = new DivideAction(new GameObject[] { planeObjects[i] }, 0, 0, divideamount1, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            else action = new DivideAction(new GameObject[] { planeObjects[i] }, divideamount1, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                            newerereerObjects.AddRange(action.DivideObjects(false));
                        }
                    }
                }
                
                for (int i = 0; i < divideamount1; i++)
                {
                    int dividableObjectsCount;
                    DivideAction action;
                    if (otheraxis1 == 0)
                    {
                        if (axis == 2) action = new DivideAction(new GameObject[] { newOtherAxis1Planes[i] }, 0, 0, divideamount1, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        else action = new DivideAction(new GameObject[] { newOtherAxis1Planes[i] }, divideamount2, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        newerereerObjects.AddRange(action.DivideObjects(false));
                    }
                    else if (otheraxis1 == 1)
                    {
                        if (axis == 0) action = new DivideAction(new GameObject[] { newOtherAxis1Planes[i] }, 0, 0, divideamount2, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        else action = new DivideAction(new GameObject[] { newOtherAxis1Planes[i] }, divideamount1, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        newerereerObjects.AddRange(action.DivideObjects(false));
                    }
                    else if (otheraxis1 == 2)
                    {
                        if (axis == 0) action = new DivideAction(new GameObject[] { newOtherAxis1Planes[i] }, 0, 0, divideamount1, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        else action = new DivideAction(new GameObject[] { newOtherAxis1Planes[i] }, divideamount1, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        newerereerObjects.AddRange(action.DivideObjects(false));
                    }
                }
                
                for (int i = 0; i < divideamount2; i++)
                {
                    int dividableObjectsCount;
                    DivideAction action;
                    if (otheraxis2 == 0)
                    {
                        if (axis == 2) action = new DivideAction(new GameObject[] { newOtherAxis2Planes[i] }, 0, 0, divideamount1, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        else action = new DivideAction(new GameObject[] { newOtherAxis2Planes[i] }, divideamount2, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        newerereerObjects.AddRange(action.DivideObjects(false));
                    }
                    else if (otheraxis2 == 1)
                    {
                        if (axis == 0) action = new DivideAction(new GameObject[] { newOtherAxis2Planes[i] }, 0, 0, divideamount2, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        else action = new DivideAction(new GameObject[] { newOtherAxis2Planes[i] }, divideamount1, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        newerereerObjects.AddRange(action.DivideObjects(false));
                    }
                    else if (otheraxis2 == 2)
                    {
                        if (axis == 0) action = new DivideAction(new GameObject[] { newOtherAxis2Planes[i] }, 0, 0, divideamount1, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        else action = new DivideAction(new GameObject[] { newOtherAxis2Planes[i] }, divideamount1, 0, 0, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        newerereerObjects.AddRange(action.DivideObjects(false));
                    }
                }
                
                //newerereerObjects.AddRange(newererestObjects);
                GroupAction planecubeGroupAction = Group.CreateGroupAction(newerereerObjects.ToArray(), newerereerObjects.ToArray()[0]);
                GameObject newerObjectsGroupC = planecubeGroupAction.GroupObjects().gameObject;
                if (newerObjectsGroupC.HasComponent<CustomName>())
                {
                    newerObjectsGroupC.GetComponent<CustomName>().CustomName_ = "DividedPlaneCubeGS";
                }
                ObjHelper.copyAddedComponentsOverWithTexture(ref newerObjectsGroupC, origobj, true);
                ReferenceMap.Handle<GameObject> newerObjectsGroupReplacementHandle = ObjHelper.ReplaceWithDuplicate(ref newerObjectsGroupC, ref newObjectHandles_[newObjectHandleIndex]);
                newObjectHandleIndex += 1;
                newererObjects.Add(newerObjectsGroupC);
            }
            newerObjects.Clear();
            newerObjects.AddRange(newererObjects);
        }

        public void tripleAxisBasedPlazer(Vector3 defaultObjSize, ref List<GameObject> newerObjects, bool isGoldAndAllowed, GameObject origobj, LevelLayer origobjlayer, ref int newObjectHandleIndex, float w, float wX, float wY, float wM, float originalZTexoff, float originalZTexScale)
        {
            List<GameObject> newererObjects = new List<GameObject>();
            foreach (GameObject gameObject in newerObjects)
            {
                List<GameObject> newerereerObjects = new List<GameObject>();
                List<GameObject> otheraxisrepetaplanes1 = new List<GameObject>();
                List<GameObject> otheraxisrepetaplanes2 = new List<GameObject>();
                GameObject[] planeObjects = CubeToPlaneAction.TurnCubeIntoPlanesForDivider(gameObject, origobj, w, wX, wY, wM, originalZTexoff, originalZTexScale);
                int positiveZAxisPlaneIndex = 2;
                int negativeZAxisPlaneIndex = 1;
                int positiveYAxisPlaneIndex = 0;
                int negativeYAxisPlaneIndex = 5;
                int positiveXAxisPlaneIndex = 3;
                int negativeXAxisPlaneIndex = 4;
                for (int i = 0; i < 6; i++)
                {
                    if (planeObjects[i].GetComponent<Transform>().localPosition.z < planeObjects[negativeZAxisPlaneIndex].GetComponent<Transform>().localPosition.z)
                    {
                        positiveZAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.z > planeObjects[positiveZAxisPlaneIndex].GetComponent<Transform>().localPosition.z)
                    {
                        negativeZAxisPlaneIndex = i;
                    }
                    if (planeObjects[i].GetComponent<Transform>().localPosition.y < planeObjects[negativeYAxisPlaneIndex].GetComponent<Transform>().localPosition.y)
                    {
                        positiveYAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.y > planeObjects[positiveYAxisPlaneIndex].GetComponent<Transform>().localPosition.y)
                    {
                        negativeYAxisPlaneIndex = i;
                    }
                    if (planeObjects[i].GetComponent<Transform>().localPosition.x < planeObjects[negativeXAxisPlaneIndex].GetComponent<Transform>().localPosition.x)
                    {
                        positiveXAxisPlaneIndex = i;
                    }
                    else if (planeObjects[i].GetComponent<Transform>().localPosition.x > planeObjects[positiveXAxisPlaneIndex].GetComponent<Transform>().localPosition.x)
                    {
                        negativeXAxisPlaneIndex = i;
                    }
                }
                float originalOtherAxisXScale = origobj.GetComponent<Transform>().localScale.x * defaultObjSize.x;
                float originalOtherAxisYScale = origobj.GetComponent<Transform>().localScale.y * defaultObjSize.y;
                float originalOtherAxisZScale = origobj.GetComponent<Transform>().localScale.z * defaultObjSize.z;

                
                //newerereerObjects.AddRange(planeObjects);

                GameObject[] newOtherAxisXPlanes = new GameObject[xdivides];
                GameObject[] newOtherAxisYPlanes = new GameObject[ydivides];
                GameObject[] newOtherAxisZPlanes = new GameObject[zdivides];

                ReferenceMap.Handle<GameObject>[] newOtherAxisXPlaneHandles_ = new ReferenceMap.Handle<GameObject>[xdivides];
                ReferenceMap.Handle<GameObject>[] newOtherAxisYPlaneHandles_ = new ReferenceMap.Handle<GameObject>[ydivides];
                ReferenceMap.Handle<GameObject>[] newOtherAxisZPlaneHandles_ = new ReferenceMap.Handle<GameObject>[zdivides];

                for (int i = 0; i < xdivides; i++)
                {
                    GameObject newOtherAxisXPlane;
                    if (i != 0)
                    {
                        newOtherAxisXPlane = DuplicateObjectsAction.Duplicate(newOtherAxisXPlanes[i - 1]);
                    }
                    else
                    {
                        newOtherAxisXPlane = DuplicateObjectsAction.Duplicate(planeObjects[negativeXAxisPlaneIndex]);
                    }
                    Transform newOtherAxisXPlaneTrans = newOtherAxisXPlane.GetComponent<Transform>();
                    newOtherAxisXPlaneTrans.localPosition = new Vector3((float)(newOtherAxisXPlaneTrans.localPosition.x + (originalOtherAxisXScale / ((1.0 + xdivides))) * 1), newOtherAxisXPlaneTrans.localPosition.y, newOtherAxisXPlaneTrans.localPosition.z);
                    G.Sys.LevelEditor_.AddGameObject(ref newOtherAxisXPlaneHandles_[i], newOtherAxisXPlane, origobjlayer);
                    newOtherAxisXPlanes[i] = newOtherAxisXPlane;
                }

                for (int i = 0; i < ydivides; i++)
                {
                    GameObject newOtherAxisYPlane;
                    if (i != 0)
                    {
                        newOtherAxisYPlane = DuplicateObjectsAction.Duplicate(newOtherAxisYPlanes[i - 1]);
                    }
                    else
                    {
                        newOtherAxisYPlane = DuplicateObjectsAction.Duplicate(planeObjects[negativeYAxisPlaneIndex]);
                    }
                    Transform newOtherAxisYPlaneTrans = newOtherAxisYPlane.GetComponent<Transform>();
                    newOtherAxisYPlaneTrans.localPosition = new Vector3(newOtherAxisYPlaneTrans.localPosition.x, (float)(newOtherAxisYPlaneTrans.localPosition.y + (originalOtherAxisYScale / ((1.0 + ydivides))) * 1), newOtherAxisYPlaneTrans.localPosition.z);
                    G.Sys.LevelEditor_.AddGameObject(ref newOtherAxisYPlaneHandles_[i], newOtherAxisYPlane, origobjlayer);
                    newOtherAxisYPlanes[i] = newOtherAxisYPlane;
                }

                for (int i = 0; i < zdivides; i++)
                {
                    GameObject newOtherAxisZPlane;
                    if (i != 0)
                    {
                        newOtherAxisZPlane = DuplicateObjectsAction.Duplicate(newOtherAxisZPlanes[i - 1]);
                    }
                    else
                    {
                        newOtherAxisZPlane = DuplicateObjectsAction.Duplicate(planeObjects[negativeZAxisPlaneIndex]);
                    }
                    Transform newOtherAxisZPlaneTrans = newOtherAxisZPlane.GetComponent<Transform>();
                    newOtherAxisZPlaneTrans.localPosition = new Vector3(newOtherAxisZPlaneTrans.localPosition.x, newOtherAxisZPlaneTrans.localPosition.y, (float)(newOtherAxisZPlaneTrans.localPosition.z + (originalOtherAxisZScale / ((1.0 + zdivides))) * 1));
                    G.Sys.LevelEditor_.AddGameObject(ref newOtherAxisZPlaneHandles_[i], newOtherAxisZPlane, origobjlayer);
                    newOtherAxisZPlanes[i] = newOtherAxisZPlane;
                }

                for (int i = 0; i < 6; i++)
                {
                    int dividableObjectsCount;
                    DivideAction action;
                    if (i == positiveXAxisPlaneIndex || i == negativeXAxisPlaneIndex)
                    {
                        action = new DivideAction(new GameObject[] { planeObjects[i] }, zdivides, 0, ydivides, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        newerereerObjects.AddRange(action.DivideObjects(false));
                    }
                    else if (i == positiveYAxisPlaneIndex || i == negativeYAxisPlaneIndex)
                    {
                        action = new DivideAction(new GameObject[] { planeObjects[i] }, xdivides, 0, zdivides, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        newerereerObjects.AddRange(action.DivideObjects(false));
                    }
                    else if (i == positiveZAxisPlaneIndex || i == negativeZAxisPlaneIndex)
                    {
                        action = new DivideAction(new GameObject[] { planeObjects[i] }, ydivides, 0, xdivides, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                        newerereerObjects.AddRange(action.DivideObjects(false));
                    }
                }

                for (int i = 0; i < xdivides; i++)
                {
                    int dividableObjectsCount;
                    DivideAction action;
                    action = new DivideAction(new GameObject[] { newOtherAxisXPlanes[i] }, zdivides, 0, ydivides, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                    newerereerObjects.AddRange(action.DivideObjects(false));
                }

                for (int i = 0; i < ydivides; i++)
                {
                    int dividableObjectsCount;
                    DivideAction action;
                    action = new DivideAction(new GameObject[] { newOtherAxisYPlanes[i] }, xdivides, 0, zdivides, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                    newerereerObjects.AddRange(action.DivideObjects(false));
                }

                for (int i = 0; i < zdivides; i++)
                {
                    int dividableObjectsCount;
                    DivideAction action;
                    action = new DivideAction(new GameObject[] { newOtherAxisZPlanes[i] }, ydivides, 0, xdivides, isGoldAndAllowed, 0, 0, 0, false, false, out dividableObjectsCount);
                    newerereerObjects.AddRange(action.DivideObjects(false));
                }

                //newerereerObjects.AddRange(newererestObjects);
                GroupAction planecubeGroupAction = Group.CreateGroupAction(newerereerObjects.ToArray(), newerereerObjects.ToArray()[0]);
                GameObject newerObjectsGroupC = planecubeGroupAction.GroupObjects().gameObject;
                if (newerObjectsGroupC.HasComponent<CustomName>())
                {
                    newerObjectsGroupC.GetComponent<CustomName>().CustomName_ = "DividedPlaneCubeGS";
                }
                ObjHelper.copyAddedComponentsOverWithTexture(ref newerObjectsGroupC, origobj, true);
                ReferenceMap.Handle<GameObject> newerObjectsGroupReplacementHandle = ObjHelper.ReplaceWithDuplicate(ref newerObjectsGroupC, ref newObjectHandles_[newObjectHandleIndex]);
                newObjectHandleIndex += 1;
                newererObjects.Add(newerObjectsGroupC);
            }
            newerObjects.Clear();
            newerObjects.AddRange(newererObjects);
        }

        public void UndivideObjects()
        {
            if (!nothingtodivide)
            {
                G.Sys.LevelEditor_.DeleteGameObjects(this.newObjectHandles_);

                LevelEditor levelEditor = G.Sys.LevelEditor_;

                GameObject newObj = Deserializer.LoadGameObjectFromBytes<BinaryDeserializer>(this.deletedObjectBytes_, levelEditor.ReferenceMap_.GetIDToObjectMap());
                if ((UnityEngine.Object)newObj == (UnityEngine.Object)null)
                {
                    Debug.LogError((object)"Error loading deleted object");
                }
                else
                {
                    levelEditor.AddGameObject(ref this.originalObjectHandle_, newObj, (LevelLayer)null);
                    G.Sys.LevelEditor_.SelectObject(newObj);
                    if (this.groupAction_ == null)
                        return;
                    this.groupAction_.groupObjectHandle_ = this.originalObjectHandle_;
                    this.groupAction_.UngroupObjects();
                }
            }
        }

        private Vector3 getDefaultObjSize(string objName)
        {
            switch(objName)
            {
                case ("CubeGS"):
                    return new Vector3(64, 64, 64);
                    break;
                case ("PlaneGS"):
                    return new Vector3(64, -1, 64);
                    break;
                case ("PlaneOneSidedGS"):
                    return new Vector3(64, -1, 64);
                    break;
                case ("CylinderGS"):
                    return new Vector3(-1, 64, -1);
                    break;
                case ("CylinderHDGS"):
                    return new Vector3(-1, 64, -1);
                    break;
                case ("TubeGS"):
                    return new Vector3(-1, 64, -1);
                    break;
                case ("QuadGS"):
                    return new Vector3(64, 10, 64);
                    break;
                case ("KillGridBox"):
                    return new Vector3(50, 50, 50);
                    break;
                case ("KillGridCylinder"):
                    return new Vector3(-1, -1, 1);
                    break;
                case ("ArchGS"):
                    return new Vector3(-1, -1, 64);
                    break;
                case ("ArchQuarterGS"):
                    return new Vector3(53.333333F, -1, -1);
                    break;
                case ("WedgeGS"):
                    return new Vector3(64, -1, -1);
                    break;
                case ("PentagonGS"):
                    return new Vector3(-1, 10, -1);
                    break;
                case ("HexagonGS"):
                    return new Vector3(-1, 10, -1);
                    break;
                case ("CheeseGS"):
                    return new Vector3(-1, 25.6F, -1);
                    break;
                case ("TrapezoidGS"):
                    return new Vector3(64, -1, -1);
                    break;
                default:
                    break;
            }
            return new Vector3(-1,-1,-1);
        }
    }
}
