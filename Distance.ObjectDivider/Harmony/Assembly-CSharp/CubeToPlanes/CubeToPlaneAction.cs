using LevelEditorActions;
using Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.ObjectDivider.Harmony
{
    class CubeToPlaneAction : SimplerAction
    {
        private ReferenceMap.Handle<GameObject> originalObjectHandle_;
        private ReferenceMap.Handle<GameObject>[] newObjectHandles_;
        private byte[] deletedObjectBytes_;
        private GroupAction groupAction_;
        private List<GroupAction> groupActions_2 = new List<GroupAction>();
        private List<ReferenceMap.Handle<GameObject>> newgrouphandles = new List<ReferenceMap.Handle<GameObject>>();

        private bool nothingtodo = false;

        public CubeToPlaneAction(GameObject[] gameObjects)
        {
            ReferenceMap referenceMap = G.Sys.LevelEditor_.ReferenceMap_;

            List<GameObject> cubeObjects = new List<GameObject>();

            int newObjectCount = 0;

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.name.Equals("CubeGS"))
                {
                    newObjectCount+=6;
                    cubeObjects.Add(gameObject);
                }
            }
            if (cubeObjects.Count > 0)
            {
                Group.InitData data = new Group.InitData(Vector3.zero, Quaternion.identity, Vector3.one, new Bounds());
                this.groupAction_ = new GroupAction(cubeObjects.ToArray(), data);
                Group group = this.groupAction_.GroupObjects();
                this.originalObjectHandle_ = referenceMap.GetHandle<GameObject>(group.gameObject);
                this.deletedObjectBytes_ = BinarySerializer.SaveGameObjectToBytes(group.gameObject, Resource.LoadPrefab("Group"), true);
                this.groupAction_.Undo();
            }
            else
            {
                nothingtodo = true;
            }

            newObjectHandles_ = new ReferenceMap.Handle<GameObject>[newObjectCount];

        }
        public override string Description_ => "Turns cubes into planes.";

        public override void Undo() => this.UnturnCubeFromPlanes();

        public override void Redo() => this.TurnCubeIntoPlanes();

        public void TurnCubeIntoPlanes()
        {
            if (!nothingtodo)
            {
                int newObjectHandleIndex = 0;
                List<GameObject> newObjects = new List<GameObject>();
                groupAction_.GroupObjects();
                GameObject[] originalobjs = groupAction_.UngroupObjects();
                LevelEditor levelEditor = G.Sys.LevelEditor_;
                ResourceManager resourceManager = G.Sys.ResourceManager_;
                groupAction_.GroupObjects();
                levelEditor.DeleteGameObject(this.originalObjectHandle_.Get());
                List<LevelLayer> layersOfObjects = new List<LevelLayer>();
                foreach (GameObject origobj in originalobjs)
                {
                    LevelLayer origobjlayer = levelEditor.WorkingLevel_.GetLayerOfObject(origobj);
                    GoldenSimples originalGS = origobj.GetComponent<GoldenSimples>();
                    List<GameObject> newerObjects = new List<GameObject>();
                    Vector3 origobjpos = origobj.transform.localPosition;
                    Quaternion origobjrot = origobj.transform.localRotation;
                    Vector3 origobjscl = origobj.transform.localScale;
                    GameObject dummyCube = DuplicateObjectsAction.Duplicate(origobj);
                    dummyCube.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 1);

                    GameObject topplane = Resource.LoadPrefabInstance("PlaneGS");
                    topplane.GetComponent<Transform>().localPosition = new Vector3(origobjpos.x, origobjpos.y+32* origobjscl.y, origobjpos.z);
                    topplane.GetComponent<Transform>().localScale = new Vector3(origobjscl.x, 1, origobjscl.z);

                    GameObject sidePlane1 = Resource.LoadPrefabInstance("PlaneGS");
                    sidePlane1.GetComponent<Transform>().localPosition = new Vector3(origobjpos.x, origobjpos.y, origobjpos.z - 32 * origobjscl.z);
                    sidePlane1.GetComponent<Transform>().localScale = new Vector3(origobjscl.x, 1, origobjscl.y);
                    sidePlane1.GetComponent<Transform>().localRotation = new Quaternion(0, .7F, -.7F, 0);
                    
                    GameObject sidePlane2 = Resource.LoadPrefabInstance("PlaneGS");
                    sidePlane2.GetComponent<Transform>().localPosition = new Vector3(origobjpos.x, origobjpos.y, origobjpos.z + 32 * origobjscl.z);
                    sidePlane2.GetComponent<Transform>().localScale = new Vector3(origobjscl.x, 1, origobjscl.y);
                    sidePlane2.GetComponent<Transform>().localRotation = new Quaternion(-.7F, 0, 0, -.7F);
                    
                    GameObject sidePlane3 = Resource.LoadPrefabInstance("PlaneGS");
                    sidePlane3.GetComponent<Transform>().localPosition = new Vector3(origobjpos.x + 32 * origobjscl.x, origobjpos.y, origobjpos.z);
                    sidePlane3.GetComponent<Transform>().localScale = new Vector3(origobjscl.z, 1, origobjscl.y);
                    sidePlane3.GetComponent<Transform>().localRotation = new Quaternion(.5F, .5F, -.5F, .5F);
                    
                    GameObject sidePlane4 = Resource.LoadPrefabInstance("PlaneGS");
                    sidePlane4.GetComponent<Transform>().localPosition = new Vector3(origobjpos.x - 32 * origobjscl.x, origobjpos.y, origobjpos.z);
                    sidePlane4.GetComponent<Transform>().localScale = new Vector3(origobjscl.z, 1, origobjscl.y);
                    sidePlane4.GetComponent<Transform>().localRotation = new Quaternion(-.5F, .5F, -.5F, -.5F);
                    
                    GameObject bottomPlane = Resource.LoadPrefabInstance("PlaneGS");
                    bottomPlane.GetComponent<Transform>().localPosition = new Vector3(origobjpos.x, origobjpos.y - 32 * origobjscl.y, origobjpos.z);
                    bottomPlane.GetComponent<Transform>().localScale = new Vector3(origobjscl.x, 1, origobjscl.z);
                    bottomPlane.GetComponent<Transform>().localRotation = new Quaternion(1, 0, 0, 0);
                    

                    GameObject[] planes = new GameObject[] { topplane, sidePlane1, sidePlane2, sidePlane3, sidePlane4, bottomPlane };
                    for (int i = 0; i < planes.Length; i++)
                    {
                        levelEditor.AddGameObject(ref newObjectHandles_[newObjectHandleIndex], planes[i], origobjlayer);
                        newObjectHandleIndex += 1;
                    }
                    ObjHelper.copyGoldenSimpleOverWithColor(ref topplane, originalGS);
                    ObjHelper.copyGoldenSimpleOverWithColor(ref sidePlane1, originalGS);
                    ObjHelper.copyGoldenSimpleOverWithColor(ref sidePlane2, originalGS);
                    ObjHelper.copyGoldenSimpleOverWithColor(ref sidePlane3, originalGS);
                    ObjHelper.copyGoldenSimpleOverWithColor(ref sidePlane4, originalGS);
                    ObjHelper.copyGoldenSimpleOverWithColor(ref bottomPlane, originalGS);
                    Group.InitData data = new Group.InitData(origobjpos, Quaternion.identity, Vector3.one, new Bounds(new Vector3(0,0,0),new Vector3(origobjscl.x*64, origobjscl.y * 64, origobjscl.z * 64)));
                    GroupAction groupAction = new GroupAction(planes, data);
                    GameObject newerObjectsGroup = groupAction.GroupObjects().gameObject;
                    newerObjectsGroup.GetComponent<Transform>().localRotation = origobjrot;
                    if(newerObjectsGroup.HasComponent<CustomName>())
                    {
                        newerObjectsGroup.GetComponent<CustomName>().CustomName_ = "PlaneCubeGS";
                    }
                    ObjHelper.copyAddedComponentsOverWithTexture(ref newerObjectsGroup, origobj, true);
                    ReferenceMap.Handle<GameObject> newerObjectsGroupReplacementHandle = ObjHelper.ReplaceWithDuplicate(ref newerObjectsGroup);
                    newObjects.Add(newerObjectsGroup);
                    newgrouphandles.Add(newerObjectsGroupReplacementHandle);
                    groupActions_2.Add(groupAction);
                    layersOfObjects.Add(origobjlayer);
                    //levelEditor.DeleteGameObjects(tempObjHandles);
                    origobj.GetComponent<Transform>().localPosition = origobjpos;
                    origobj.GetComponent<Transform>().localRotation = origobjrot;
                    origobj.GetComponent<Transform>().localScale = origobjscl;
                }

                levelEditor.SetSelection((IEnumerable<GameObject>)newObjects.ToArray());
            }
        }

        public void UnturnCubeFromPlanes()
        {
            if (!nothingtodo)
            {
                /*
                for(int i = 0; i< groupActions_2.Count; i++)
                {
                    groupActions_2.ToArray()[i].UngroupObjects();
                }
                groupActions_2.Clear();
                */
                G.Sys.LevelEditor_.DeleteGameObjects(newgrouphandles.ToArray());
                newgrouphandles.Clear();

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

        public static GameObject[] TurnCubeIntoPlanesForDivider(GameObject cubeObj, GameObject origcube, float w, float wX, float wY, float wM, float originalZTexoff, float originalZTexScale)
        {
            
            int newObjectHandleIndex = 0;
            List<GameObject> newObjects = new List<GameObject>();
           // groupAction_.GroupObjects();
            //GameObject[] originalobjs = groupAction_.UngroupObjects();
            LevelEditor levelEditor = G.Sys.LevelEditor_;
            ResourceManager resourceManager = G.Sys.ResourceManager_;
            //groupAction_.GroupObjects();
            //levelEditor.DeleteGameObject(cubeObj);
            List<LevelLayer> layersOfObjects = new List<LevelLayer>();
            ReferenceMap.Handle<GameObject>[] newObjectHandles_ = new ReferenceMap.Handle<GameObject>[6];

            LevelLayer cubeObjlayer = levelEditor.WorkingLevel_.GetLayerOfObject(cubeObj);
            GoldenSimples originalGS = cubeObj.GetComponent<GoldenSimples>();
            List<GameObject> newerObjects = new List<GameObject>();
            Vector3 cubeObjpos = cubeObj.transform.localPosition;
            Quaternion cubeObjrot = cubeObj.transform.localRotation;
            Vector3 cubeObjscl = cubeObj.transform.localScale;
            GameObject dummyCube = DuplicateObjectsAction.Duplicate(cubeObj);
            dummyCube.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 1);
            

            GameObject topplane = Resource.LoadPrefabInstance("PlaneGS");
            topplane.GetComponent<Transform>().localPosition = new Vector3(cubeObjpos.x, cubeObjpos.y + 32 * cubeObjscl.y, cubeObjpos.z);
            topplane.GetComponent<Transform>().localScale = new Vector3(cubeObjscl.x, 1, cubeObjscl.z);

            GameObject sidePlane1 = Resource.LoadPrefabInstance("PlaneGS");
            sidePlane1.GetComponent<Transform>().localPosition = new Vector3(cubeObjpos.x, cubeObjpos.y, cubeObjpos.z - 32 * cubeObjscl.z);
            sidePlane1.GetComponent<Transform>().localScale = new Vector3(cubeObjscl.x, 1, cubeObjscl.y);
            sidePlane1.GetComponent<Transform>().localRotation = new Quaternion(0, .7F, -.7F, 0);

            GameObject sidePlane2 = Resource.LoadPrefabInstance("PlaneGS");
            sidePlane2.GetComponent<Transform>().localPosition = new Vector3(cubeObjpos.x, cubeObjpos.y, cubeObjpos.z + 32 * cubeObjscl.z);
            sidePlane2.GetComponent<Transform>().localScale = new Vector3(cubeObjscl.x, 1, cubeObjscl.y);
            sidePlane2.GetComponent<Transform>().localRotation = new Quaternion(-.7F, 0, 0, -.7F);

            GameObject sidePlane3 = Resource.LoadPrefabInstance("PlaneGS");
            sidePlane3.GetComponent<Transform>().localPosition = new Vector3(cubeObjpos.x + 32 * cubeObjscl.x, cubeObjpos.y, cubeObjpos.z);
            sidePlane3.GetComponent<Transform>().localScale = new Vector3(cubeObjscl.z, 1, cubeObjscl.y);
            sidePlane3.GetComponent<Transform>().localRotation = new Quaternion(.5F, .5F, -.5F, .5F);

            GameObject sidePlane4 = Resource.LoadPrefabInstance("PlaneGS");
            sidePlane4.GetComponent<Transform>().localPosition = new Vector3(cubeObjpos.x - 32 * cubeObjscl.x, cubeObjpos.y, cubeObjpos.z);
            sidePlane4.GetComponent<Transform>().localScale = new Vector3(cubeObjscl.z, 1, cubeObjscl.y);
            sidePlane4.GetComponent<Transform>().localRotation = new Quaternion(-.5F, .5F, -.5F, -.5F);

            GameObject bottomPlane = Resource.LoadPrefabInstance("PlaneGS");
            bottomPlane.GetComponent<Transform>().localPosition = new Vector3(cubeObjpos.x, cubeObjpos.y - 32 * cubeObjscl.y, cubeObjpos.z);
            bottomPlane.GetComponent<Transform>().localScale = new Vector3(cubeObjscl.x, 1, cubeObjscl.z);
            bottomPlane.GetComponent<Transform>().localRotation = new Quaternion(1, 0, 0, 0);


            GameObject[] planes = new GameObject[] { topplane, sidePlane1, sidePlane2, sidePlane3, sidePlane4, bottomPlane };
            for (int i = 0; i < planes.Length; i++)
            {
                levelEditor.AddGameObject(ref newObjectHandles_[newObjectHandleIndex], planes[i], cubeObjlayer);
                newObjectHandleIndex += 1;
            }
            ObjHelper.copyGoldenSimpleOverWithColor(ref topplane, originalGS);
            ObjHelper.copyGoldenSimpleOverWithColor(ref sidePlane1, originalGS);
            ObjHelper.copyGoldenSimpleOverWithColor(ref sidePlane2, originalGS);
            ObjHelper.copyGoldenSimpleOverWithColor(ref sidePlane3, originalGS);
            ObjHelper.copyGoldenSimpleOverWithColor(ref sidePlane4, originalGS);
            ObjHelper.copyGoldenSimpleOverWithColor(ref bottomPlane, originalGS);

            GoldenSimples orginalorigninalGS = origcube.GetComponent<GoldenSimples>();
            topplane.GetComponent<GoldenSimples>().textureOffset_ = new Vector3(originalGS.textureOffset_.x, orginalorigninalGS.textureOffset_.y + orginalorigninalGS.textureScale_.y * originalGS.textureOffset_.z, originalZTexoff);
            topplane.GetComponent<GoldenSimples>().textureScale_ = new Vector3(originalGS.textureScale_.x, orginalorigninalGS.textureScale_.y * originalGS.textureScale_.z, originalZTexScale);
            
            bottomPlane.GetComponent<GoldenSimples>().textureOffset_ = new Vector3(originalGS.textureOffset_.x, orginalorigninalGS.textureOffset_.y + orginalorigninalGS.textureScale_.y * wY - (orginalorigninalGS.textureScale_.y * originalGS.textureOffset_.z), originalZTexoff);
            bottomPlane.GetComponent<GoldenSimples>().textureScale_ = new Vector3(originalGS.textureScale_.x, orginalorigninalGS.textureScale_.y * originalGS.textureScale_.z, originalZTexScale);
            
            sidePlane1.GetComponent<GoldenSimples>().textureOffset_ = new Vector3(w- wM*originalGS.textureOffset_.x, originalGS.textureOffset_.y, originalZTexoff);
            sidePlane1.GetComponent<GoldenSimples>().textureScale_ = new Vector3(originalGS.textureScale_.x, originalGS.textureScale_.y, originalZTexScale);
            
            sidePlane2.GetComponent<GoldenSimples>().textureOffset_ = new Vector3(originalGS.textureOffset_.x, originalGS.textureOffset_.y, originalZTexoff);
            sidePlane2.GetComponent<GoldenSimples>().textureScale_ = new Vector3(originalGS.textureScale_.x, originalGS.textureScale_.y, originalZTexScale);
            
            sidePlane3.GetComponent<GoldenSimples>().textureOffset_ = new Vector3(orginalorigninalGS.textureOffset_.x+orginalorigninalGS.textureScale_.x * wX - (orginalorigninalGS.textureScale_.x * originalGS.textureOffset_.z), originalGS.textureOffset_.y, originalZTexoff);
            sidePlane3.GetComponent<GoldenSimples>().textureScale_ = new Vector3(orginalorigninalGS.textureScale_.x * originalGS.textureScale_.z, originalGS.textureScale_.y, originalZTexScale);
            
            sidePlane4.GetComponent<GoldenSimples>().textureOffset_ = new Vector3(orginalorigninalGS.textureOffset_.x + orginalorigninalGS.textureScale_.x * originalGS.textureOffset_.z, originalGS.textureOffset_.y, originalZTexoff);
            sidePlane4.GetComponent<GoldenSimples>().textureScale_ = new Vector3(orginalorigninalGS.textureScale_.x * originalGS.textureScale_.z, originalGS.textureScale_.y, originalZTexScale);

            Group.InitData data = new Group.InitData(cubeObjpos, Quaternion.identity, Vector3.one, new Bounds(new Vector3(0, 0, 0), new Vector3(cubeObjscl.x * 64, cubeObjscl.y * 64, cubeObjscl.z * 64)));
            GroupAction groupAction = new GroupAction(planes, data);
            GameObject newerObjectsGroup = groupAction.GroupObjects().gameObject;
            newerObjectsGroup.GetComponent<Transform>().localRotation = cubeObjrot;
            GameObject[] finalObjectsBeforeAdd = groupAction.UngroupObjects();
            //ObjHelper.copyAddedComponentsOverWithTexture(ref newerObjectsGroup, cubeObj, true);
            //ReferenceMap.Handle<GameObject> newerObjectsGroupReplacementHandle = ObjHelper.ReplaceWithDuplicate(ref newerObjectsGroup);
            //newObjects.Add(newerObjectsGroup);
            //newgrouphandles.Add(newerObjectsGroupReplacementHandle);
            //groupActions_2.Add(groupAction);
            //layersOfObjects.Add(cubeObjlayer);
            //levelEditor.DeleteGameObjects(tempObjHandles);
            cubeObj.GetComponent<Transform>().localPosition = cubeObjpos;
            cubeObj.GetComponent<Transform>().localRotation = cubeObjrot;
            cubeObj.GetComponent<Transform>().localScale = cubeObjscl;


            //levelEditor.SetSelection((IEnumerable<GameObject>)newObjects.ToArray());
            return finalObjectsBeforeAdd;
        }

    }
}
