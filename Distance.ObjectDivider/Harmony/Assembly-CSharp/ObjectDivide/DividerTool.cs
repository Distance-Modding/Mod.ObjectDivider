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
    [EditorTool, KeyboardShortcut("CTRL+SHIFT+D")]
    public class DividerTool : InstantTool
    {
        internal static ToolInfo info_ => new ToolInfo("Divider", "Divides objects.", ToolCategory.Edit, ToolButtonState.Button, true, 1122);
        public override ToolInfo Info_ => info_;

        public static GameObject tabSystemUIRootIntInput1P;
        public static GameObject divideXBool1TogP;

        public static GameObject tabSystemUIRootIntInput2P;
        public static GameObject divideYBool2TogP;

        public static GameObject tabSystemUIRootIntInput3P;
        public static GameObject divideZBool3TogP;

        public static GameObject lockdividesTogP;
        public static GameObject keeptexturelookTogP;

        public static GameObject dividerspacingInputXP;
        public static GameObject dividerspacingInputYP;
        public static GameObject dividerspacingInputZP;

        
        public static GameObject cubetoplanesTogP;
        public static GameObject noplaneoverlapTogP;
        public static GameObject noplaneoverlapP;

        public static GameObject tespopwinP;

        public static bool SDNOEA = false;
        public static bool KTL = false;
        public static bool CTP = false;
        public static bool NPO = false;

        public static bool justCreated = false;
        public static bool doesIsPresent = false;

        public static int lastXDivide = 1;
        public static int lastYDivide = 1;
        public static int lastZDivide = 1;

        public static float lastXSpacing = 0;
        public static float lastYSpacing = 0;
        public static float lastZSpacing = 0;

        // Required by distance itself
        public static void Register()
        {
            if (!G.Sys.LevelEditor_.registeredToolsNamesToTypes_.ContainsKey(info_.Name_))
                G.Sys.LevelEditor_.RegisterTool(info_);
        }

        public override bool Run()
        {

            GameObject[] gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

            List<GameObject> validGameObjects = new List<GameObject>();
            //GameObject tespop = new GameObject("Tespop");
            
            //tespop.AddComponent<UIWidget>();

            GameObject tabSystemUIRoot = FOONIC(gameObjects, "TabSystemUIRoot");
            
            if (!IsGameObjNull(tabSystemUIRoot) && !doesIsPresent)
            {
                GameObject globalPopup = FOONIC(FOONIC(tabSystemUIRoot, "Drag Drop Root"), "GlobalPopup");
                GameObject searchBar = FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(tabSystemUIRoot, "TabAutoHideTrayRootLeftSide"), "SlidingTray"), "Content Panel"), "Library Tab(Clone)"), "SearchBar");
                if (IsGameObjNull(searchBar)) Mod.Logger.Info("BLARRR");
                GameObject intInput = FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(tabSystemUIRoot, "TabAutoHideTrayRootLeftSide"), "SlidingTray"), "Content Panel"), "Level Data Tab(Clone)"), "PropertiesAreaParent"),"PropertiesArea(Clone)"),"PropertiesPanel"),"Container"),"IntInspector"),"IntInput");
                GameObject boolInput = FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(FOONIC(tabSystemUIRoot, "TabAutoHideTrayRootLeftSide"), "SlidingTray"), "Content Panel"), "Level Data Tab(Clone)"), "PropertiesAreaParent"), "PropertiesArea(Clone)"), "PropertiesPanel"), "Container"), "BoolInspector"), "Toggle");
                if (IsGameObjNull(intInput)) Mod.Logger.Info("BLARRR");
                //TabAutoHideTrayRootLeftSide, SlidingTray,Content Panel,Library Tab(Clone),SearchBar
                //Level Data Tab(Clone),PropertiesAreaParent,PropertiesArea(Clone),PropertiesPanel,Container,IntInspector,IntInput
                if (!IsGameObjNull(globalPopup))
                {
                    
                    GameObject globalPopupBack = FOONIC(globalPopup, "Background");
                    GameObject globalPopupPop = FOONIC(globalPopup, "Popup");
                    if(!IsGameObjNull(globalPopupBack) && !IsGameObjNull(globalPopupPop))
                    {
                        
                        UISprite gpbs2 = globalPopupBack.GetComponent<UISprite>();
                        UIWidget gpubuiw = globalPopupBack.GetComponent<UIWidget>();
                        UIWidget gppuiw = globalPopupPop.GetComponent<UIWidget>();
                        UIExInput sebr = searchBar.GetComponent<UIExInput>();
                        BoxCollider gpubbx = globalPopupPop.GetComponent<BoxCollider>();
                        UIExIntegerInput intinmputcomp = intInput.GetComponent<UIExIntegerInput>();
                        UIExDisableOnSelect intinputdos = intInput.GetComponent<UIExDisableOnSelect>();
                        UIWidget intinputwig = intInput.GetComponent<UIWidget>();

                        GameObject intinputbg = FOONIC(intInput, "BackgroundSprite");
                        UISprite intinputbgsprite = intinputbg.GetComponent<UISprite>();
                        GameObject intinputFacadeButton = FOONIC(intInput, "FacadeButton");
                        UIWidget intinputfacadeButtonWidget = intinputFacadeButton.GetComponent<UIWidget>();
                        BoxCollider intinputfacadeButtonBoxCollider = intinputFacadeButton.GetComponent<BoxCollider>();
                        UIButton intinputfacadeButtonUIButton = intinputFacadeButton.GetComponent<UIButton>();
                        GameObject intinputValueLabel = FOONIC(intInput, "ValueLabel");
                        UILabel intinputValueLabelUILabel = intinputValueLabel.GetComponent<UILabel>();
                        GameObject leftarrowintin = FOONIC(intInput, "LeftArrow");
                        UIButton leftarrowintinUIBut = leftarrowintin.GetComponent<UIButton>();
                        UISprite leftarrowintinUISprite = FOONIC(leftarrowintin, "Sprite").GetComponent<UISprite>();

                        UIToggle boolInputTog = boolInput.GetComponent<UIToggle>();
                        UIButton bollInpuntUIbut = boolInput.GetComponent<UIButton>();
                        UISprite bollInpuntBackUISprit = FOONIC(boolInput, "BackGround").GetComponent<UISprite>();
                        UISprite bollInpuntCeckUISprit = FOONIC(boolInput, "CheckSprite").GetComponent<UISprite>();

                        //printPropsAndFields(bollInpuntCeckUISprit);


                        //Mod.Logger.Info("mChildren S: " + comp.mChildren.ToArray());
                        //Mod.Logger.Info("mChildren W: " + gpubuiw.mChildren.ToArray());
                        
                        Layers laya = tabSystemUIRoot.gameObject.GetLayer();
                        GameObject tespopwin = tabSystemUIRoot.gameObject.CreateChild("Tespop");
                        justCreated = true;
                        doesIsPresent = true;

                        tabSystemUIRoot.gameObject.GetChildren().Last().SetLayer(laya);
                        tabSystemUIRoot.gameObject.GetChildren().Last().AddComponent<UIWidget>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetComponent<UIWidget>().depth = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetComponent<UIWidget>().autoResizeBoxCollider = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetComponent<UIWidget>().aspectRatio = gpubuiw.aspectRatio;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetComponent<UIWidget>().mainTexture = gpubuiw.mainTexture;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetComponent<UIWidget>().shader = gpubuiw.shader;
                        
                        tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("Background");
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().SetLayer(laya);
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<Transform>().localPosition = new Vector3(0,-25,0);
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UISprite>();
                        //Mod.Logger.Info(tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().depth);
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().depth = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().width = 400;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().height = 200+51;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().atlas = gpbs2.atlas;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().autoResizeBoxCollider = true;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().shader = gpubuiw.shader;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().mainTexture = gpubuiw.mainTexture;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().spriteName = gpbs2.spriteName;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().aspectRatio = gpbs2.aspectRatio;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().type = gpbs2.type;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().color = gpbs2.color;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().isTrigger = false;
                        
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().ResizeCollider();
                        BoxCollider tabSystemUIRootBgBoxcollider1 = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>();
                        tabSystemUIRootBgBoxcollider1.center = new Vector3(tabSystemUIRootBgBoxcollider1.center.x, tabSystemUIRootBgBoxcollider1.center.y,-1);
                        
                        GameObject divideXBool1 = tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("DivideXBool1");
                        divideXBool1.SetLayer(laya);
                        divideXBool1.GetComponent<Transform>().position = new Vector3((float)0.1, (float)0.1,0);
                        GameObject divideXBool1Lab = divideXBool1.CreateChild("NameLabel");
                        divideXBool1Lab.SetLayer(laya);
                        divideXBool1Lab.GetComponent<Transform>().localPosition = new Vector3((float)-55, (float)0.0, 0);
                        UILabel divideXBool1LabUILab = divideXBool1Lab.AddComponent<UILabel>();
                        divideXBool1LabUILab.ambigiousFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue");
                        divideXBool1LabUILab.depth = 10;
                        divideXBool1LabUILab.fontSize = 24;
                        divideXBool1LabUILab.height = 16;
                        divideXBool1LabUILab.maxLineCount = 1;
                        divideXBool1LabUILab.multiLine = false; 
                        divideXBool1LabUILab.overflowMethod = UILabel.Overflow.ResizeFreely;
                        divideXBool1LabUILab.supportEncoding = false;
                        divideXBool1LabUILab.text = "Y-Divide:";
                        divideXBool1LabUILab.trueTypeFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); 
                        //divideXBool1LabUILab.width = intinputValueLabelUILabel.width; 
                        divideXBool1LabUILab.aspectRatio = 7.875F; 

                        GameObject divideXBool1Tog = divideXBool1.CreateChild("Toggle");
                        divideXBool1Tog.SetLayer(laya);
                        UIWidget divideXBool1TogUIWig = divideXBool1Tog.AddComponent<UIWidget>();
                        divideXBool1TogUIWig.depth = 4;
                        divideXBool1TogUIWig.autoResizeBoxCollider = true;
                        divideXBool1TogUIWig.aspectRatio = gpubuiw.aspectRatio;
                        BoxCollider divideXBool1TogBoxCol = divideXBool1Tog.AddComponent<BoxCollider>();
                        divideXBool1TogBoxCol.isTrigger = true;
                        UIToggle divideXBool1TogUITog = divideXBool1Tog.AddComponent<UIToggle>();
                        divideXBool1TogUITog.optionCanBeNone = true;
                        divideXBool1TogUITog.value = true;
                        divideXBool1TogUITog.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnTogChange1)));
                        //divideXBool1TogUITog.checkAnimation = boolInputTog.checkAnimation;

                        divideXBool1TogUITog.functionName = "";
                        UIButton divideXBool1TogUIBut= divideXBool1Tog.AddComponent<UIButton>();
                        divideXBool1TogUIBut.normalSprite = bollInpuntUIbut.normalSprite;
                        divideXBool1TogUIBut.hover = bollInpuntUIbut.hover;
                        divideXBool1TogUIBut.disabledColor = bollInpuntUIbut.disabledColor;
                        divideXBool1TogUIBut.pressed = bollInpuntUIbut.pressed;
                        divideXBool1TogUIBut.mStartingColor = bollInpuntUIbut.mStartingColor;
                        divideXBool1TogUIBut.mDefaultColor = bollInpuntUIbut.mStartingColor;
                        divideXBool1Tog.AddComponent<UIExMouseWheelScrollView>();

                        GameObject divideXBool1Back = divideXBool1Tog.CreateChild("Background");
                        divideXBool1Back.SetLayer(laya);
                        UISprite divideXBool1BackSprite = divideXBool1Back.AddComponent<UISprite>();
                        divideXBool1BackSprite.atlas = bollInpuntBackUISprit.atlas;
                        divideXBool1BackSprite.color = bollInpuntBackUISprit.color;
                        divideXBool1BackSprite.depth = 6;
                        divideXBool1BackSprite.height = 32;
                        divideXBool1BackSprite.width = 32;
                        divideXBool1BackSprite.type = UIBasicSprite.Type.Sliced;
                        divideXBool1BackSprite.autoResizeBoxCollider = true;
                        divideXBool1BackSprite.spriteName = "Highlight";
                        divideXBool1TogUIBut.tweenTarget = divideXBool1Back;
                        divideXBool1TogUIBut.mWidget = divideXBool1BackSprite;
                        

                        GameObject divideXBool1CheckSprite = divideXBool1Tog.CreateChild("CheckSprite");
                        divideXBool1CheckSprite.SetLayer(laya);
                        UISprite divideXBool1CheckSpriteUISprite = divideXBool1CheckSprite.AddComponent<UISprite>();
                        divideXBool1CheckSpriteUISprite.alpha = 1;
                        divideXBool1CheckSpriteUISprite.depth = 7;
                        divideXBool1CheckSpriteUISprite.atlas = bollInpuntCeckUISprit.atlas;
                        divideXBool1CheckSpriteUISprite.color = bollInpuntCeckUISprit.color;
                        divideXBool1CheckSpriteUISprite.height = 20;
                        divideXBool1CheckSpriteUISprite.width = 20;
                        divideXBool1CheckSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        divideXBool1CheckSpriteUISprite.spriteName = "MenuButtonBackgroundWhite";
                        //divideXBool1TogUITog.checkSprite = divideXBool1CheckSpriteUISprite;
                        divideXBool1TogUITog.activeSprite = divideXBool1CheckSpriteUISprite;
                        //divideXBool1TogUITog.onChange.Clear();
                        divideXBool1TogUIWig.ResizeCollider();
                        divideXBool1TogBoxCol.size = new Vector3((float)(divideXBool1TogBoxCol.size.x/3), (float)(divideXBool1TogBoxCol.size.y/ 3), divideXBool1TogBoxCol.size.z);
                        //bollInpuntCeckUISprit
                        //divideXBool1TogUIBut.

                        divideXBool1TogP = divideXBool1Tog;

                        tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("DivideInput1");
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().SetLayer(laya);
                        
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIWidget>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().depth = intinputwig.depth;
                        
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().aspectRatio = intinputwig.aspectRatio;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().height = intinputwig.height;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().width = intinputwig.width;
                        
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAlphaFrameID = 994;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAnchorsCached = true;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = gppuiw.mCam;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateAnchors = false;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateFrame = 944;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mMoved = true;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().isTrigger = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().autoResizeBoxCollider = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().ResizeCollider();
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>().size = new Vector3(intinputwig.width,intinputwig.height,0);



                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIExIntegerInput>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().value = lastYDivide + "";
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().cursorPosition = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().defaultText = lastYDivide + "";
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().enabled = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().selectionStart = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().selectionEnd = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().characterLimit = 256;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().facadeButton_ = intinmputcomp.facadeButton_;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().label = intinmputcomp.label;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mDefaultText = lastYDivide + "";
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mDoInit = false;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mLoadSavedValue = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mPosition = 63;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().caretColor = intinmputcomp.caretColor;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().Min_ = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().min_ = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mPivot = UIWidget.Pivot.Center;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().onChange_.Add(new EventDelegate(new EventDelegate.Callback(this.OnIntInputChange1)));
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().onFinish_.Add(new EventDelegate(new EventDelegate.Callback(this.OnIntInputChange1)));
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIExDisableOnSelect>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { };
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { };
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExDisableOnSelect>().disableOnSelect_ = new Behaviour[] { };
                        
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIExMouseWheelScrollView>();
                        GameObject tabSystemUIRootIntInput1 = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last();
                        tabSystemUIRootIntInput1P = tabSystemUIRootIntInput1;

                        GameObject intin1left = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("LeftArrow");
                        intin1left.SetLayer(laya);
                        intin1left.GetComponent<Transform>().localPosition = new Vector3(-55,0,0);
                        intin1left.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget intin1leftUIWig = intin1left.AddComponent<UIWidget>();
                        intin1leftUIWig.depth = 6;
                        intin1leftUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        intin1leftUIWig.height = 18;
                        intin1leftUIWig.width = 18;
                        intin1leftUIWig.autoResizeBoxCollider = true;
                        BoxCollider intin1leftUIBoxCol = intin1left.AddComponent<BoxCollider>();
                        intin1leftUIBoxCol.isTrigger = true;
                        intin1leftUIWig.ResizeCollider();
                        UIButton intin1leftUIBut = intin1left.AddComponent<UIButton>();
                        intin1leftUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        intin1leftUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        intin1leftUIBut.hover = leftarrowintinUIBut.hover;
                        intin1leftUIBut.pressed = leftarrowintinUIBut.pressed;
                        intin1leftUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        intin1leftUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        intin1left.AddComponent<UIExMouseWheelScrollView>();
                        GameObject intin1leftSprite = intin1left.CreateChild("Sprite");
                        intin1leftSprite.SetLayer(laya);
                        UISprite intin1leftSpriteUISprite = intin1leftSprite.AddComponent<UISprite>();
                        intin1leftSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        intin1leftSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        intin1leftSpriteUISprite.depth = 7;
                        intin1leftSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        intin1leftSpriteUISprite.color = leftarrowintinUISprite.color;
                        intin1leftSpriteUISprite.height = leftarrowintinUISprite.height;
                        intin1leftSpriteUISprite.width = leftarrowintinUISprite.width;
                        intin1leftSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        intin1leftUIBut.tweenTarget = intin1leftSprite;
                        intin1leftUIBut.mWidget = intin1leftSpriteUISprite;
                        intin1leftUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnLeft1Press)));

                        GameObject intin1right = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("RightArrow");
                        intin1right.SetLayer(laya);
                        intin1right.GetComponent<Transform>().localPosition = new Vector3(55, 0, 0);
                        //intin1right.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget intin1rightUIWig = intin1right.AddComponent<UIWidget>();
                        intin1rightUIWig.depth = 6;
                        intin1rightUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        intin1rightUIWig.height = 18;
                        intin1rightUIWig.width = 18;
                        intin1rightUIWig.autoResizeBoxCollider = true;
                        BoxCollider intin1rightUIBoxCol = intin1right.AddComponent<BoxCollider>();
                        intin1rightUIBoxCol.isTrigger = true;
                        intin1rightUIWig.ResizeCollider();
                        UIButton intin1rightUIBut = intin1right.AddComponent<UIButton>();
                        intin1rightUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        intin1rightUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        intin1rightUIBut.hover = leftarrowintinUIBut.hover;
                        intin1rightUIBut.pressed = leftarrowintinUIBut.pressed;
                        intin1rightUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        intin1rightUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        intin1right.AddComponent<UIExMouseWheelScrollView>();
                        GameObject intin1rightSprite = intin1right.CreateChild("Sprite");
                        intin1rightSprite.SetLayer(laya);
                        UISprite intin1rightSpriteUISprite = intin1rightSprite.AddComponent<UISprite>();
                        intin1rightSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        intin1rightSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        intin1rightSpriteUISprite.depth = 7;
                        intin1rightSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        intin1rightSpriteUISprite.color = leftarrowintinUISprite.color;
                        intin1rightSpriteUISprite.height = leftarrowintinUISprite.height;
                        intin1rightSpriteUISprite.width = leftarrowintinUISprite.width;
                        intin1rightSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        intin1rightUIBut.tweenTarget = intin1rightSprite;
                        intin1rightUIBut.mWidget = intin1rightSpriteUISprite;
                        intin1rightUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnRight1Press)));

                        GameObject tabSystemUIRootIntInput1ValLab = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("ValueLabel");
                        tabSystemUIRootIntInput1ValLab.SetLayer(laya);
                        tabSystemUIRootIntInput1ValLab.AddComponent<UILabel>();
                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().alignment = NGUIText.Alignment.Center;
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().ambigiousFont = intinputValueLabelUILabel.ambigiousFont; //->PetitaBold
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().depth = intinputValueLabelUILabel.depth; //0->3
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().fontSize = intinputValueLabelUILabel.fontSize; //16->12
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().height = intinputValueLabelUILabel.height; //100->16
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().maxLineCount = intinputValueLabelUILabel.maxLineCount; //0->1
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().multiLine = intinputValueLabelUILabel.multiLine; //True->false
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().overflowMethod = UILabel.Overflow.ClampContent;//intinputValueLabelUILabel.overflowMethod; //Shrinkcontent->Clampcontent
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().supportEncoding = intinputValueLabelUILabel.supportEncoding; //True->false
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().text = lastYDivide + "";
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().trueTypeFont = intinputValueLabelUILabel.trueTypeFont; //->PetitaBold
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().width = intinputValueLabelUILabel.width; //100->126
                        tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>().aspectRatio = intinputValueLabelUILabel.aspectRatio; //1->7.875

                       //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().mPivot = UIWidget.Pivot.Center;
                       //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().rawPivot = UIWidget.Pivot.Center;
                       //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(0, 0, 0);
                       //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(63, 10, 0);


                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("BackgroundSprite");
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().SetLayer(laya);
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().position = intinputbg.GetComponent<Transform>().position;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().localScale = intinputbg.GetComponent<Transform>().localScale;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UISprite>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().depth = intinputbgsprite.depth; //2
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().atlas = intinputbgsprite.atlas;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().autoResizeBoxCollider = intinputbgsprite.autoResizeBoxCollider;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().spriteName = intinputbgsprite.spriteName;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().aspectRatio = intinputbgsprite.aspectRatio;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().type = intinputbgsprite.type;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().mWidth = intinputbgsprite.mWidth; //126
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().mHeight = intinputbgsprite.mHeight; //20
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().pivot = UIWidget.Pivot.Center;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().rawPivot = UIWidget.Pivot.Center;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().mPivot = UIWidget.Pivot.Center;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().color = intinputbgsprite.color;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().position = new Vector3(0,0,0);
                        GameObject tabSystemUIRootBackground1Sprite = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last();
                        
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("FacadeButton");
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().SetLayer(laya);
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UIWidget>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().depth = 3;//3 //intinputfacadeButtonWidget.depth; //1
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().height = intinputfacadeButtonWidget.height; //20
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().width = intinputfacadeButtonWidget.width; //126
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAlphaFrameID = intinputfacadeButtonWidget.mAlphaFrameID;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAnchorsCached = intinputfacadeButtonWidget.mAnchorsCached;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = intinputfacadeButtonWidget.mCam;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateAnchors = intinputfacadeButtonWidget.mUpdateAnchors;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateFrame = intinputfacadeButtonWidget.mUpdateFrame;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mMoved = intinputfacadeButtonWidget.mMoved;
                        
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().size = intinputfacadeButtonBoxCollider.size; //126w, 20h.
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().center = new Vector3(0,0,0);//intinputfacadeButtonBoxCollider.center; 
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().contactOffset = intinputfacadeButtonBoxCollider.contactOffset;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().isTrigger = intinputfacadeButtonBoxCollider.isTrigger;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UIButton>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().normalSprite = intinputfacadeButtonUIButton.normalSprite;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().disabledColor = intinputfacadeButtonUIButton.disabledColor;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().hover = intinputfacadeButtonUIButton.hover;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().pressed = intinputfacadeButtonUIButton.pressed;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mDefaultColor = intinputfacadeButtonUIButton.mDefaultColor;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mStartingColor = intinputfacadeButtonUIButton.mStartingColor;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mSprite = intinputfacadeButtonUIButton.mSprite;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().tweenTarget = tabSystemUIRootBackground1Sprite;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mSprite = tabSystemUIRootBackground1Sprite.GetComponent<UISprite>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mWidget = tabSystemUIRootBackground1Sprite.GetComponent<UISprite>();
                        
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UIExMouseWheelScrollView>();

                        GameObject tabSystemUIRootFacadeButton1 = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last();

                        tabSystemUIRootIntInput1.GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { tabSystemUIRootFacadeButton1.GetComponent<UIButton>() };
                        tabSystemUIRootIntInput1.GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { tabSystemUIRootFacadeButton1 };

                        tabSystemUIRootIntInput1.GetComponent<UIExIntegerInput>().label = tabSystemUIRootIntInput1ValLab.GetComponent<UILabel>();
                        tabSystemUIRootIntInput1.GetComponent<UIExIntegerInput>().facadeButton_ = tabSystemUIRootFacadeButton1.GetComponent<UIButton>();
                        tabSystemUIRootIntInput1.GetComponent<UIExIntegerInput>().onChange.Clear();
                        
                        tabSystemUIRootIntInput1.GetComponent<UIExIntegerInput>().Awake();

                        GameObject divideinput1 = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last();


                        GameObject divideYBool2 = tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("DivideYBool2");
                        divideYBool2.SetLayer(laya);
                        //divideYBool2.GetComponent<Transform>().position = new Vector3((float)0.1, (float)0.1, 0);
                        divideYBool2.GetComponent<Transform>().localPosition = new Vector3((float)130, (float)35, 0);
                        GameObject divideYBool2Lab = divideYBool2.CreateChild("NameLabel");
                        divideYBool2Lab.SetLayer(laya);
                        divideYBool2Lab.GetComponent<Transform>().localPosition = new Vector3((float)-19, (float)0.0, 0);
                        UILabel divideYBool2LabUILab = divideYBool2Lab.AddComponent<UILabel>();
                        divideYBool2LabUILab.ambigiousFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        divideYBool2LabUILab.depth = 10;
                        divideYBool2LabUILab.fontSize = 24;
                        divideYBool2LabUILab.height = 16;
                        divideYBool2LabUILab.maxLineCount = 1;
                        divideYBool2LabUILab.multiLine = false;
                        divideYBool2LabUILab.overflowMethod = UILabel.Overflow.ResizeFreely;
                        divideYBool2LabUILab.supportEncoding = false;
                        divideYBool2LabUILab.text = "Z-Divide:";
                        divideYBool2LabUILab.trueTypeFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        //divideXBool1LabUILab.width = intinputValueLabelUILabel.width; 
                        divideYBool2LabUILab.aspectRatio = 7.875F;

                        GameObject divideYBool2Tog = divideYBool2.CreateChild("Toggle");
                        divideYBool2Tog.SetLayer(laya);
                        divideYBool2Tog.GetComponent<Transform>().localPosition = new Vector3((float)38, (float)0, 0);
                        UIWidget divideYBool2TogUIWig = divideYBool2Tog.AddComponent<UIWidget>();
                        divideYBool2TogUIWig.depth = 4;
                        divideYBool2TogUIWig.autoResizeBoxCollider = true;
                        divideYBool2TogUIWig.aspectRatio = gpubuiw.aspectRatio;
                        BoxCollider divideYBool2TogBoxCol = divideYBool2Tog.AddComponent<BoxCollider>();
                        divideYBool2TogBoxCol.isTrigger = true;
                        UIToggle divideYBool2TogUITog = divideYBool2Tog.AddComponent<UIToggle>();
                        divideYBool2TogUITog.optionCanBeNone = true;
                        divideYBool2TogUITog.value = true;
                        divideYBool2TogUITog.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnTogChange2)));
                        //divideXBool1TogUITog.checkAnimation = boolInputTog.checkAnimation;

                        divideYBool2TogUITog.functionName = "";
                        UIButton divideYBool2TogUIBut = divideYBool2Tog.AddComponent<UIButton>();
                        divideYBool2TogUIBut.normalSprite = bollInpuntUIbut.normalSprite;
                        divideYBool2TogUIBut.hover = bollInpuntUIbut.hover;
                        divideYBool2TogUIBut.disabledColor = bollInpuntUIbut.disabledColor;
                        divideYBool2TogUIBut.pressed = bollInpuntUIbut.pressed;
                        divideYBool2TogUIBut.mStartingColor = bollInpuntUIbut.mStartingColor;
                        divideYBool2TogUIBut.mDefaultColor = bollInpuntUIbut.mStartingColor;
                        divideYBool2Tog.AddComponent<UIExMouseWheelScrollView>();

                        GameObject divideYBool2Back = divideYBool2Tog.CreateChild("Background");
                        divideYBool2Back.SetLayer(laya);
                        UISprite divideYBool2BackSprite = divideYBool2Back.AddComponent<UISprite>();
                        divideYBool2BackSprite.atlas = bollInpuntBackUISprit.atlas;
                        divideYBool2BackSprite.color = bollInpuntBackUISprit.color;
                        divideYBool2BackSprite.depth = 6;
                        divideYBool2BackSprite.height = 32;
                        divideYBool2BackSprite.width = 32;
                        divideYBool2BackSprite.type = UIBasicSprite.Type.Sliced;
                        divideYBool2BackSprite.autoResizeBoxCollider = true;
                        divideYBool2BackSprite.spriteName = "Highlight";
                        divideYBool2TogUIBut.tweenTarget = divideYBool2Back;
                        divideYBool2TogUIBut.mWidget = divideYBool2BackSprite;


                        GameObject divideYBool2CheckSprite = divideYBool2Tog.CreateChild("CheckSprite");
                        divideYBool2CheckSprite.SetLayer(laya);
                        UISprite divideYBool2CheckSpriteUISprite = divideYBool2CheckSprite.AddComponent<UISprite>();
                        divideYBool2CheckSpriteUISprite.alpha = 1;
                        divideYBool2CheckSpriteUISprite.depth = 7;
                        divideYBool2CheckSpriteUISprite.atlas = bollInpuntCeckUISprit.atlas;
                        divideYBool2CheckSpriteUISprite.color = bollInpuntCeckUISprit.color;
                        divideYBool2CheckSpriteUISprite.height = 20;
                        divideYBool2CheckSpriteUISprite.width = 20;
                        divideYBool2CheckSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        divideYBool2CheckSpriteUISprite.spriteName = "MenuButtonBackgroundWhite";
                        //divideXBool1TogUITog.checkSprite = divideXBool1CheckSpriteUISprite;
                        divideYBool2TogUITog.activeSprite = divideYBool2CheckSpriteUISprite;
                        //divideXBool1TogUITog.onChange.Clear();
                        divideYBool2TogUIWig.ResizeCollider();
                        divideYBool2TogBoxCol.size = new Vector3((float)(divideYBool2TogBoxCol.size.x / 3), (float)(divideYBool2TogBoxCol.size.y / 3), divideYBool2TogBoxCol.size.z);
                        //bollInpuntCeckUISprit
                        //divideXBool1TogUIBut.

                        divideYBool2TogP = divideYBool2Tog;

                        tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("DivideInput2");
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().SetLayer(laya);
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<Transform>().localPosition = new Vector3((float)130, (float)0.0, 0);
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIWidget>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().depth = intinputwig.depth;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().aspectRatio = intinputwig.aspectRatio;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().height = intinputwig.height;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().width = intinputwig.width;

                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAlphaFrameID = 994;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAnchorsCached = true;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = gppuiw.mCam;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateAnchors = false;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateFrame = 944;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mMoved = true;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().isTrigger = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().autoResizeBoxCollider = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().ResizeCollider();
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>().size = new Vector3(intinputwig.width,intinputwig.height,0);



                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIExIntegerInput>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().value = lastZDivide + "";
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().cursorPosition = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().defaultText = lastZDivide + "";
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().enabled = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().selectionStart = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().selectionEnd = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().characterLimit = 256;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().facadeButton_ = intinmputcomp.facadeButton_;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().label = intinmputcomp.label;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mDefaultText = lastZDivide + "";
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mDoInit = false;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mLoadSavedValue = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mPosition = 63;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().caretColor = intinmputcomp.caretColor;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().Min_ = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().min_ = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mPivot = UIWidget.Pivot.Center;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().onChange_.Add(new EventDelegate(new EventDelegate.Callback(this.OnIntInputChange2)));
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().onFinish_.Add(new EventDelegate(new EventDelegate.Callback(this.OnIntInputChange2)));
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIExDisableOnSelect>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { };
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { };
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExDisableOnSelect>().disableOnSelect_ = new Behaviour[] { };

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIExMouseWheelScrollView>();
                        GameObject tabSystemUIRootIntInput2 = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last();
                        tabSystemUIRootIntInput2P = tabSystemUIRootIntInput2;

                        GameObject intin2left = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("LeftArrow");
                        intin2left.SetLayer(laya);
                        intin2left.GetComponent<Transform>().localPosition = new Vector3(-55, 0, 0);
                        intin2left.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget intin2leftUIWig = intin2left.AddComponent<UIWidget>();
                        intin2leftUIWig.depth = 6;
                        intin2leftUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        intin2leftUIWig.height = 18;
                        intin2leftUIWig.width = 18;
                        intin2leftUIWig.autoResizeBoxCollider = true;
                        BoxCollider intin2leftUIBoxCol = intin2left.AddComponent<BoxCollider>();
                        intin2leftUIBoxCol.isTrigger = true;
                        intin2leftUIWig.ResizeCollider();
                        UIButton intin2leftUIBut = intin2left.AddComponent<UIButton>();
                        intin2leftUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        intin2leftUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        intin2leftUIBut.hover = leftarrowintinUIBut.hover;
                        intin2leftUIBut.pressed = leftarrowintinUIBut.pressed;
                        intin2leftUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        intin2leftUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        intin2left.AddComponent<UIExMouseWheelScrollView>();
                        GameObject intin2leftSprite = intin2left.CreateChild("Sprite");
                        intin2leftSprite.SetLayer(laya);
                        UISprite intin2leftSpriteUISprite = intin2leftSprite.AddComponent<UISprite>();
                        intin2leftSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        intin2leftSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        intin2leftSpriteUISprite.depth = 7;
                        intin2leftSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        intin2leftSpriteUISprite.color = leftarrowintinUISprite.color;
                        intin2leftSpriteUISprite.height = leftarrowintinUISprite.height;
                        intin2leftSpriteUISprite.width = leftarrowintinUISprite.width;
                        intin2leftSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        intin2leftUIBut.tweenTarget = intin2leftSprite;
                        intin2leftUIBut.mWidget = intin2leftSpriteUISprite;
                        intin2leftUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnLeft2Press)));

                        GameObject intin2right = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("RightArrow");
                        intin2right.SetLayer(laya);
                        intin2right.GetComponent<Transform>().localPosition = new Vector3(55, 0, 0);
                        //intin1right.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget intin2rightUIWig = intin2right.AddComponent<UIWidget>();
                        intin2rightUIWig.depth = 6;
                        intin2rightUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        intin2rightUIWig.height = 18;
                        intin2rightUIWig.width = 18;
                        intin2rightUIWig.autoResizeBoxCollider = true;
                        BoxCollider intin2rightUIBoxCol = intin2right.AddComponent<BoxCollider>();
                        intin2rightUIBoxCol.isTrigger = true;
                        intin2rightUIWig.ResizeCollider();
                        UIButton intin2rightUIBut = intin2right.AddComponent<UIButton>();
                        intin2rightUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        intin2rightUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        intin2rightUIBut.hover = leftarrowintinUIBut.hover;
                        intin2rightUIBut.pressed = leftarrowintinUIBut.pressed;
                        intin2rightUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        intin2rightUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        intin2right.AddComponent<UIExMouseWheelScrollView>();
                        GameObject intin2rightSprite = intin2right.CreateChild("Sprite");
                        intin2rightSprite.SetLayer(laya);
                        UISprite intin2rightSpriteUISprite = intin2rightSprite.AddComponent<UISprite>();
                        intin2rightSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        intin2rightSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        intin2rightSpriteUISprite.depth = 7;
                        intin2rightSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        intin2rightSpriteUISprite.color = leftarrowintinUISprite.color;
                        intin2rightSpriteUISprite.height = leftarrowintinUISprite.height;
                        intin2rightSpriteUISprite.width = leftarrowintinUISprite.width;
                        intin2rightSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        intin2rightUIBut.tweenTarget = intin2rightSprite;
                        intin2rightUIBut.mWidget = intin2rightSpriteUISprite;
                        intin2rightUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnRight2Press)));

                        GameObject tabSystemUIRootIntInput2ValLab = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("ValueLabel");
                        tabSystemUIRootIntInput2ValLab.SetLayer(laya);
                        tabSystemUIRootIntInput2ValLab.AddComponent<UILabel>();
                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().alignment = NGUIText.Alignment.Center;
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().ambigiousFont = intinputValueLabelUILabel.ambigiousFont; //->PetitaBold
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().depth = intinputValueLabelUILabel.depth; //0->3
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().fontSize = intinputValueLabelUILabel.fontSize; //16->12
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().height = intinputValueLabelUILabel.height; //100->16
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().maxLineCount = intinputValueLabelUILabel.maxLineCount; //0->1
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().multiLine = intinputValueLabelUILabel.multiLine; //True->false
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().overflowMethod = UILabel.Overflow.ClampContent;//intinputValueLabelUILabel.overflowMethod; //Shrinkcontent->Clampcontent
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().supportEncoding = intinputValueLabelUILabel.supportEncoding; //True->false
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().text = lastZDivide + "";
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().trueTypeFont = intinputValueLabelUILabel.trueTypeFont; //->PetitaBold
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().width = intinputValueLabelUILabel.width; //100->126
                        tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>().aspectRatio = intinputValueLabelUILabel.aspectRatio; //1->7.875

                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().mPivot = UIWidget.Pivot.Center;
                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().rawPivot = UIWidget.Pivot.Center;
                        //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(0, 0, 0);
                        //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(63, 10, 0);


                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("BackgroundSprite");
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().SetLayer(laya);
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().position = intinputbg.GetComponent<Transform>().position;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().localScale = intinputbg.GetComponent<Transform>().localScale;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UISprite>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().depth = intinputbgsprite.depth; //2
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().atlas = intinputbgsprite.atlas;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().autoResizeBoxCollider = intinputbgsprite.autoResizeBoxCollider;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().spriteName = intinputbgsprite.spriteName;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().aspectRatio = intinputbgsprite.aspectRatio;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().type = intinputbgsprite.type;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().mWidth = intinputbgsprite.mWidth; //126
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().mHeight = intinputbgsprite.mHeight; //20
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().pivot = UIWidget.Pivot.Center;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().rawPivot = UIWidget.Pivot.Center;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().mPivot = UIWidget.Pivot.Center;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().color = intinputbgsprite.color;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
                        GameObject tabSystemUIRootBackground2Sprite = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last();

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("FacadeButton");
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().SetLayer(laya);
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UIWidget>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().depth = 3;//3 //intinputfacadeButtonWidget.depth; //1
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().height = intinputfacadeButtonWidget.height; //20
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().width = intinputfacadeButtonWidget.width; //126
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAlphaFrameID = intinputfacadeButtonWidget.mAlphaFrameID;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAnchorsCached = intinputfacadeButtonWidget.mAnchorsCached;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = intinputfacadeButtonWidget.mCam;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateAnchors = intinputfacadeButtonWidget.mUpdateAnchors;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateFrame = intinputfacadeButtonWidget.mUpdateFrame;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mMoved = intinputfacadeButtonWidget.mMoved;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().size = intinputfacadeButtonBoxCollider.size; //126w, 20h.
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);//intinputfacadeButtonBoxCollider.center; 
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().contactOffset = intinputfacadeButtonBoxCollider.contactOffset;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().isTrigger = intinputfacadeButtonBoxCollider.isTrigger;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UIButton>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().normalSprite = intinputfacadeButtonUIButton.normalSprite;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().disabledColor = intinputfacadeButtonUIButton.disabledColor;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().hover = intinputfacadeButtonUIButton.hover;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().pressed = intinputfacadeButtonUIButton.pressed;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mDefaultColor = intinputfacadeButtonUIButton.mDefaultColor;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mStartingColor = intinputfacadeButtonUIButton.mStartingColor;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mSprite = intinputfacadeButtonUIButton.mSprite;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().tweenTarget = tabSystemUIRootBackground2Sprite;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mSprite = tabSystemUIRootBackground2Sprite.GetComponent<UISprite>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mWidget = tabSystemUIRootBackground2Sprite.GetComponent<UISprite>();

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UIExMouseWheelScrollView>();

                        GameObject tabSystemUIRootFacadeButton2 = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last();

                        tabSystemUIRootIntInput2.GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { tabSystemUIRootFacadeButton2.GetComponent<UIButton>() };
                        tabSystemUIRootIntInput2.GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { tabSystemUIRootFacadeButton2 };

                        tabSystemUIRootIntInput2.GetComponent<UIExIntegerInput>().label = tabSystemUIRootIntInput2ValLab.GetComponent<UILabel>();
                        tabSystemUIRootIntInput2.GetComponent<UIExIntegerInput>().facadeButton_ = tabSystemUIRootFacadeButton2.GetComponent<UIButton>();
                        tabSystemUIRootIntInput2.GetComponent<UIExIntegerInput>().onChange.Clear();

                        tabSystemUIRootIntInput2.GetComponent<UIExIntegerInput>().Awake();

                        GameObject divideinput2 = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last();


                        
                        GameObject divideZBool3 = tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("DivideZBool3");
                        divideZBool3.SetLayer(laya);
                        //divideYBool2.GetComponent<Transform>().position = new Vector3((float)0.1, (float)0.1, 0);
                        divideZBool3.GetComponent<Transform>().localPosition = new Vector3((float)-130, (float)35, 0);
                        GameObject divideZBool3Lab = divideZBool3.CreateChild("NameLabel");
                        divideZBool3Lab.SetLayer(laya);
                        divideZBool3Lab.GetComponent<Transform>().localPosition = new Vector3((float)-18, (float)0.0, 0);
                        UILabel divideZBool3LabUILab = divideZBool3Lab.AddComponent<UILabel>();
                        divideZBool3LabUILab.ambigiousFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        divideZBool3LabUILab.depth = 10;
                        divideZBool3LabUILab.fontSize = 24;
                        divideZBool3LabUILab.height = 16;
                        divideZBool3LabUILab.maxLineCount = 1;
                        divideZBool3LabUILab.multiLine = false;
                        divideZBool3LabUILab.overflowMethod = UILabel.Overflow.ResizeFreely;
                        divideZBool3LabUILab.supportEncoding = false;
                        divideZBool3LabUILab.text = "X-Divide:";
                        divideZBool3LabUILab.trueTypeFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        //divideXBool1LabUILab.width = intinputValueLabelUILabel.width; 
                        divideYBool2LabUILab.aspectRatio = 7.875F;

                        GameObject divideZBool3Tog = divideZBool3.CreateChild("Toggle");
                        divideZBool3Tog.SetLayer(laya);
                        divideZBool3Tog.GetComponent<Transform>().localPosition = new Vector3((float)38, (float)0, 0);
                        UIWidget divideZBool3TogUIWig = divideZBool3Tog.AddComponent<UIWidget>();
                        divideZBool3TogUIWig.depth = 4;
                        divideZBool3TogUIWig.autoResizeBoxCollider = true;
                        divideZBool3TogUIWig.aspectRatio = gpubuiw.aspectRatio;
                        BoxCollider divideZBool3TogBoxCol = divideZBool3Tog.AddComponent<BoxCollider>();
                        divideZBool3TogBoxCol.isTrigger = true;
                        UIToggle divideZBool3TogUITog = divideZBool3Tog.AddComponent<UIToggle>();
                        divideZBool3TogUITog.optionCanBeNone = true;
                        divideZBool3TogUITog.value = true;
                        divideZBool3TogUITog.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnTogChange3)));
                        //divideXBool1TogUITog.checkAnimation = boolInputTog.checkAnimation;

                        divideZBool3TogUITog.functionName = "";
                        UIButton divideZBool3TogUIBut = divideZBool3Tog.AddComponent<UIButton>();
                        divideZBool3TogUIBut.normalSprite = bollInpuntUIbut.normalSprite;
                        divideZBool3TogUIBut.hover = bollInpuntUIbut.hover;
                        divideZBool3TogUIBut.disabledColor = bollInpuntUIbut.disabledColor;
                        divideZBool3TogUIBut.pressed = bollInpuntUIbut.pressed;
                        divideZBool3TogUIBut.mStartingColor = bollInpuntUIbut.mStartingColor;
                        divideZBool3TogUIBut.mDefaultColor = bollInpuntUIbut.mStartingColor;
                        divideZBool3Tog.AddComponent<UIExMouseWheelScrollView>();

                        GameObject divideZBool3Back = divideZBool3Tog.CreateChild("Background");
                        divideZBool3Back.SetLayer(laya);
                        UISprite divideZBool3BackSprite = divideZBool3Back.AddComponent<UISprite>();
                        divideZBool3BackSprite.atlas = bollInpuntBackUISprit.atlas;
                        divideZBool3BackSprite.color = bollInpuntBackUISprit.color;
                        divideZBool3BackSprite.depth = 6;
                        divideZBool3BackSprite.height = 32;
                        divideZBool3BackSprite.width = 32;
                        divideZBool3BackSprite.type = UIBasicSprite.Type.Sliced;
                        divideZBool3BackSprite.autoResizeBoxCollider = true;
                        divideZBool3BackSprite.spriteName = "Highlight";
                        divideZBool3TogUIBut.tweenTarget = divideZBool3Back;
                        divideZBool3TogUIBut.mWidget = divideZBool3BackSprite;


                        GameObject divideZBool3CheckSprite = divideZBool3Tog.CreateChild("CheckSprite");
                        divideZBool3CheckSprite.SetLayer(laya);
                        UISprite divideZBool3CheckSpriteUISprite = divideZBool3CheckSprite.AddComponent<UISprite>();
                        divideZBool3CheckSpriteUISprite.alpha = 1;
                        divideZBool3CheckSpriteUISprite.depth = 7;
                        divideZBool3CheckSpriteUISprite.atlas = bollInpuntCeckUISprit.atlas;
                        divideZBool3CheckSpriteUISprite.color = bollInpuntCeckUISprit.color;
                        divideZBool3CheckSpriteUISprite.height = 20;
                        divideZBool3CheckSpriteUISprite.width = 20;
                        divideZBool3CheckSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        divideZBool3CheckSpriteUISprite.spriteName = "MenuButtonBackgroundWhite";
                        //divideXBool1TogUITog.checkSprite = divideXBool1CheckSpriteUISprite;
                        divideZBool3TogUITog.activeSprite = divideZBool3CheckSpriteUISprite;
                        //divideXBool1TogUITog.onChange.Clear();
                        divideZBool3TogUIWig.ResizeCollider();
                        divideZBool3TogBoxCol.size = new Vector3((float)(divideZBool3TogBoxCol.size.x / 3), (float)(divideZBool3TogBoxCol.size.y / 3), divideZBool3TogBoxCol.size.z);
                        //bollInpuntCeckUISprit
                        //divideXBool1TogUIBut.

                        divideZBool3TogP = divideZBool3Tog;

                        tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("DivideInput3");
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().SetLayer(laya);
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<Transform>().localPosition = new Vector3((float)-130, (float)0.0, 0);
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIWidget>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().depth = intinputwig.depth;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().aspectRatio = intinputwig.aspectRatio;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().height = intinputwig.height;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().width = intinputwig.width;

                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAlphaFrameID = 994;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAnchorsCached = true;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = gppuiw.mCam;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateAnchors = false;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateFrame = 944;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mMoved = true;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().isTrigger = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().autoResizeBoxCollider = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().ResizeCollider();
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>().size = new Vector3(intinputwig.width,intinputwig.height,0);



                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIExIntegerInput>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().value = lastXDivide + "";
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().cursorPosition = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().defaultText = lastXDivide + "";
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().enabled = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().selectionStart = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().selectionEnd = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().characterLimit = 256;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().facadeButton_ = intinmputcomp.facadeButton_;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().label = intinmputcomp.label;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mDefaultText = lastXDivide + "";
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mDoInit = false;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mLoadSavedValue = true;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mPosition = 63;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().caretColor = intinmputcomp.caretColor;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().Min_ = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().min_ = 0;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().mPivot = UIWidget.Pivot.Center;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().onChange_.Add(new EventDelegate(new EventDelegate.Callback(this.OnIntInputChange3)));
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().onFinish_.Add(new EventDelegate(new EventDelegate.Callback(this.OnIntInputChange3)));
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIExDisableOnSelect>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { };
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { };
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExDisableOnSelect>().disableOnSelect_ = new Behaviour[] { };

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<UIExMouseWheelScrollView>();
                        GameObject tabSystemUIRootIntInput3 = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last();
                        tabSystemUIRootIntInput3P = tabSystemUIRootIntInput3;

                        GameObject intin3left = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("LeftArrow");
                        intin3left.SetLayer(laya);
                        intin3left.GetComponent<Transform>().localPosition = new Vector3(-55, 0, 0);
                        intin3left.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget intin3leftUIWig = intin3left.AddComponent<UIWidget>();
                        intin3leftUIWig.depth = 6;
                        intin3leftUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        intin3leftUIWig.height = 18;
                        intin3leftUIWig.width = 18;
                        intin3leftUIWig.autoResizeBoxCollider = true;
                        BoxCollider intin3leftUIBoxCol = intin3left.AddComponent<BoxCollider>();
                        intin3leftUIBoxCol.isTrigger = true;
                        intin3leftUIWig.ResizeCollider();
                        UIButton intin3leftUIBut = intin3left.AddComponent<UIButton>();
                        intin3leftUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        intin3leftUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        intin3leftUIBut.hover = leftarrowintinUIBut.hover;
                        intin3leftUIBut.pressed = leftarrowintinUIBut.pressed;
                        intin3leftUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        intin3leftUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        intin3left.AddComponent<UIExMouseWheelScrollView>();
                        GameObject intin3leftSprite = intin3left.CreateChild("Sprite");
                        intin3leftSprite.SetLayer(laya);
                        UISprite intin3leftSpriteUISprite = intin3leftSprite.AddComponent<UISprite>();
                        intin3leftSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        intin3leftSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        intin3leftSpriteUISprite.depth = 7;
                        intin3leftSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        intin3leftSpriteUISprite.color = leftarrowintinUISprite.color;
                        intin3leftSpriteUISprite.height = leftarrowintinUISprite.height;
                        intin3leftSpriteUISprite.width = leftarrowintinUISprite.width;
                        intin3leftSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        intin3leftUIBut.tweenTarget = intin3leftSprite;
                        intin3leftUIBut.mWidget = intin3leftSpriteUISprite;
                        intin3leftUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnLeft3Press)));

                        GameObject intin3right = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("RightArrow");
                        intin3right.SetLayer(laya);
                        intin3right.GetComponent<Transform>().localPosition = new Vector3(55, 0, 0);
                        //intin1right.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget intin3rightUIWig = intin3right.AddComponent<UIWidget>();
                        intin3rightUIWig.depth = 6;
                        intin3rightUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        intin3rightUIWig.height = 18;
                        intin3rightUIWig.width = 18;
                        intin3rightUIWig.autoResizeBoxCollider = true;
                        BoxCollider intin3rightUIBoxCol = intin3right.AddComponent<BoxCollider>();
                        intin3rightUIBoxCol.isTrigger = true;
                        intin3rightUIWig.ResizeCollider();
                        UIButton intin3rightUIBut = intin3right.AddComponent<UIButton>();
                        intin3rightUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        intin3rightUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        intin3rightUIBut.hover = leftarrowintinUIBut.hover;
                        intin3rightUIBut.pressed = leftarrowintinUIBut.pressed;
                        intin3rightUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        intin3rightUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        intin3right.AddComponent<UIExMouseWheelScrollView>();
                        GameObject intin3rightSprite = intin3right.CreateChild("Sprite");
                        intin3rightSprite.SetLayer(laya);
                        UISprite intin3rightSpriteUISprite = intin3rightSprite.AddComponent<UISprite>();
                        intin3rightSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        intin3rightSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        intin3rightSpriteUISprite.depth = 7;
                        intin3rightSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        intin3rightSpriteUISprite.color = leftarrowintinUISprite.color;
                        intin3rightSpriteUISprite.height = leftarrowintinUISprite.height;
                        intin3rightSpriteUISprite.width = leftarrowintinUISprite.width;
                        intin3rightSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        intin3rightUIBut.tweenTarget = intin3rightSprite;
                        intin3rightUIBut.mWidget = intin3rightSpriteUISprite;
                        intin3rightUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnRight3Press)));

                        GameObject tabSystemUIRootIntInput3ValLab = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("ValueLabel");
                        tabSystemUIRootIntInput3ValLab.SetLayer(laya);
                        tabSystemUIRootIntInput3ValLab.AddComponent<UILabel>();
                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().alignment = NGUIText.Alignment.Center;
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().ambigiousFont = intinputValueLabelUILabel.ambigiousFont; //->PetitaBold
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().depth = intinputValueLabelUILabel.depth; //0->3
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().fontSize = intinputValueLabelUILabel.fontSize; //16->12
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().height = intinputValueLabelUILabel.height; //100->16
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().maxLineCount = intinputValueLabelUILabel.maxLineCount; //0->1
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().multiLine = intinputValueLabelUILabel.multiLine; //True->false
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().overflowMethod = UILabel.Overflow.ClampContent;//intinputValueLabelUILabel.overflowMethod; //Shrinkcontent->Clampcontent
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().supportEncoding = intinputValueLabelUILabel.supportEncoding; //True->false
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().text = lastXDivide + "";
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().trueTypeFont = intinputValueLabelUILabel.trueTypeFont; //->PetitaBold
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().width = intinputValueLabelUILabel.width; //100->126
                        tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>().aspectRatio = intinputValueLabelUILabel.aspectRatio; //1->7.875

                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().mPivot = UIWidget.Pivot.Center;
                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().rawPivot = UIWidget.Pivot.Center;
                        //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(0, 0, 0);
                        //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(63, 10, 0);


                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("BackgroundSprite");
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().SetLayer(laya);
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().position = intinputbg.GetComponent<Transform>().position;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().localScale = intinputbg.GetComponent<Transform>().localScale;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UISprite>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().depth = intinputbgsprite.depth; //2
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().atlas = intinputbgsprite.atlas;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().autoResizeBoxCollider = intinputbgsprite.autoResizeBoxCollider;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().spriteName = intinputbgsprite.spriteName;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().aspectRatio = intinputbgsprite.aspectRatio;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().type = intinputbgsprite.type;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().mWidth = intinputbgsprite.mWidth; //126
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().mHeight = intinputbgsprite.mHeight; //20
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().pivot = UIWidget.Pivot.Center;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().rawPivot = UIWidget.Pivot.Center;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().mPivot = UIWidget.Pivot.Center;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UISprite>().color = intinputbgsprite.color;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
                        GameObject tabSystemUIRootBackground3Sprite = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last();

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().CreateChild("FacadeButton");
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().SetLayer(laya);
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UIWidget>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().depth = 3;//3 //intinputfacadeButtonWidget.depth; //1
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().height = intinputfacadeButtonWidget.height; //20
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().width = intinputfacadeButtonWidget.width; //126
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAlphaFrameID = intinputfacadeButtonWidget.mAlphaFrameID;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAnchorsCached = intinputfacadeButtonWidget.mAnchorsCached;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = intinputfacadeButtonWidget.mCam;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateAnchors = intinputfacadeButtonWidget.mUpdateAnchors;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateFrame = intinputfacadeButtonWidget.mUpdateFrame;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mMoved = intinputfacadeButtonWidget.mMoved;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().size = intinputfacadeButtonBoxCollider.size; //126w, 20h.
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);//intinputfacadeButtonBoxCollider.center; 
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().contactOffset = intinputfacadeButtonBoxCollider.contactOffset;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<BoxCollider>().isTrigger = intinputfacadeButtonBoxCollider.isTrigger;

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UIButton>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().normalSprite = intinputfacadeButtonUIButton.normalSprite;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().disabledColor = intinputfacadeButtonUIButton.disabledColor;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().hover = intinputfacadeButtonUIButton.hover;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().pressed = intinputfacadeButtonUIButton.pressed;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mDefaultColor = intinputfacadeButtonUIButton.mDefaultColor;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mStartingColor = intinputfacadeButtonUIButton.mStartingColor;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mSprite = intinputfacadeButtonUIButton.mSprite;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().tweenTarget = tabSystemUIRootBackground3Sprite;
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mSprite = tabSystemUIRootBackground3Sprite.GetComponent<UISprite>();
                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mWidget = tabSystemUIRootBackground3Sprite.GetComponent<UISprite>();

                        tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().AddComponent<UIExMouseWheelScrollView>();

                        GameObject tabSystemUIRootFacadeButton3 = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last();

                        tabSystemUIRootIntInput3.GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { tabSystemUIRootFacadeButton3.GetComponent<UIButton>() };
                        tabSystemUIRootIntInput3.GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { tabSystemUIRootFacadeButton3 };

                        tabSystemUIRootIntInput3.GetComponent<UIExIntegerInput>().label = tabSystemUIRootIntInput3ValLab.GetComponent<UILabel>();
                        tabSystemUIRootIntInput3.GetComponent<UIExIntegerInput>().facadeButton_ = tabSystemUIRootFacadeButton3.GetComponent<UIButton>();
                        tabSystemUIRootIntInput3.GetComponent<UIExIntegerInput>().onChange.Clear();

                        tabSystemUIRootIntInput3.GetComponent<UIExIntegerInput>().Awake();

                        GameObject divideinput3 = tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last();

                        GameObject lockdivides = tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("LockDivides");
                        lockdivides.SetLayer(laya);
                        lockdivides.GetComponent<Transform>().localPosition = new Vector3((float)33, (float)(-35.0 + 8), 0);
                        GameObject lockdividesLab = lockdivides.CreateChild("NameLabel");
                        lockdividesLab.SetLayer(laya);
                        lockdividesLab.GetComponent<Transform>().localPosition = new Vector3((float)-136, (float)0.0, 0);
                        UILabel lockdividesLabUILab = lockdividesLab.AddComponent<UILabel>();
                        lockdividesLabUILab.ambigiousFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        lockdividesLabUILab.depth = 10;
                        lockdividesLabUILab.fontSize = 16;
                        lockdividesLabUILab.height = 16;
                        lockdividesLabUILab.maxLineCount = 1;
                        lockdividesLabUILab.multiLine = false;
                        lockdividesLabUILab.overflowMethod = UILabel.Overflow.ResizeFreely;
                        lockdividesLabUILab.supportEncoding = false;
                        lockdividesLabUILab.text = "SAME DIVIDE NUMBER ON EACH AXIS:";
                        lockdividesLabUILab.trueTypeFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        //divideXBool1LabUILab.width = intinputValueLabelUILabel.width; 
                        lockdividesLabUILab.aspectRatio = 7.875F;

                        GameObject lockdividesTog = lockdivides.CreateChild("Toggle");
                        lockdividesTog.SetLayer(laya);
                        lockdividesTog.GetComponent<Transform>().localPosition = new Vector3((float)-38, (float)0, 0);
                        UIWidget lockdividesTogUIWig = lockdividesTog.AddComponent<UIWidget>();
                        lockdividesTogUIWig.depth = 4;
                        lockdividesTogUIWig.autoResizeBoxCollider = true;
                        lockdividesTogUIWig.aspectRatio = gpubuiw.aspectRatio;
                        BoxCollider lockdividesTogBoxCol = lockdividesTog.AddComponent<BoxCollider>();
                        lockdividesTogBoxCol.isTrigger = true;
                        UIToggle lockdividesTogUITog = lockdividesTog.AddComponent<UIToggle>();
                        lockdividesTogUITog.optionCanBeNone = true;
                        lockdividesTogUITog.value = SDNOEA;
                        lockdividesTogUITog.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnTogSDNOEAChange)));
                        //divideXBool1TogUITog.checkAnimation = boolInputTog.checkAnimation;

                        lockdividesTogUITog.functionName = "";
                        UIButton lockdividesTogUIBut = lockdividesTog.AddComponent<UIButton>();
                        lockdividesTogUIBut.normalSprite = bollInpuntUIbut.normalSprite;
                        lockdividesTogUIBut.hover = bollInpuntUIbut.hover;
                        lockdividesTogUIBut.disabledColor = bollInpuntUIbut.disabledColor;
                        lockdividesTogUIBut.pressed = bollInpuntUIbut.pressed;
                        lockdividesTogUIBut.mStartingColor = bollInpuntUIbut.mStartingColor;
                        lockdividesTogUIBut.mDefaultColor = bollInpuntUIbut.mStartingColor;
                        lockdividesTog.AddComponent<UIExMouseWheelScrollView>();

                        GameObject lockdividesBack = lockdividesTog.CreateChild("Background");
                        lockdividesBack.SetLayer(laya);
                        UISprite lockdividesBackSprite = lockdividesBack.AddComponent<UISprite>();
                        lockdividesBackSprite.atlas = bollInpuntBackUISprit.atlas;
                        lockdividesBackSprite.color = bollInpuntBackUISprit.color;
                        lockdividesBackSprite.depth = 6;
                        lockdividesBackSprite.height = 16;
                        lockdividesBackSprite.width = 16;
                        lockdividesBackSprite.type = UIBasicSprite.Type.Sliced;
                        lockdividesBackSprite.autoResizeBoxCollider = true;
                        lockdividesBackSprite.spriteName = "Highlight";
                        lockdividesTogUIBut.tweenTarget = lockdividesBack;
                        lockdividesTogUIBut.mWidget = lockdividesBackSprite;


                        GameObject lockdividesCheckSprite = lockdividesTog.CreateChild("CheckSprite");
                        lockdividesCheckSprite.SetLayer(laya);
                        UISprite lockdividesCheckSpriteUISprite = lockdividesCheckSprite.AddComponent<UISprite>();
                        lockdividesCheckSpriteUISprite.alpha = 1;
                        lockdividesCheckSpriteUISprite.depth = 7;
                        lockdividesCheckSpriteUISprite.atlas = bollInpuntCeckUISprit.atlas;
                        lockdividesCheckSpriteUISprite.color = bollInpuntCeckUISprit.color;
                        lockdividesCheckSpriteUISprite.height = 11;
                        lockdividesCheckSpriteUISprite.width = 11;
                        lockdividesCheckSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        lockdividesCheckSpriteUISprite.spriteName = "MenuButtonBackgroundWhite";
                        //divideXBool1TogUITog.checkSprite = divideXBool1CheckSpriteUISprite;
                        lockdividesTogUITog.activeSprite = lockdividesCheckSpriteUISprite;
                        //divideXBool1TogUITog.onChange.Clear();
                        lockdividesTogUIWig.ResizeCollider();
                        lockdividesTogBoxCol.size = new Vector3((float)(lockdividesTogBoxCol.size.x / 5), (float)(lockdividesTogBoxCol.size.y / 5), lockdividesTogBoxCol.size.z);
                        //bollInpuntCeckUISprit
                        //divideXBool1TogUIBut.

                        lockdividesTogP = lockdividesTog;

                        GameObject keeptexturelook = tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("KeepTexture");
                        keeptexturelook.SetLayer(laya);
                        keeptexturelook.GetComponent<Transform>().localPosition = new Vector3((float)33, (float)(-50.0 + 8), 0);
                        GameObject keeptexturelookLab = keeptexturelook.CreateChild("NameLabel");
                        keeptexturelookLab.SetLayer(laya);
                        keeptexturelookLab.GetComponent<Transform>().localPosition = new Vector3((float)-98, (float)0.0, 0);
                        UILabel keeptexturelookLabUILab = keeptexturelookLab.AddComponent<UILabel>();
                        keeptexturelookLabUILab.ambigiousFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        keeptexturelookLabUILab.depth = 10;
                        keeptexturelookLabUILab.fontSize = 16;
                        keeptexturelookLabUILab.height = 16;
                        keeptexturelookLabUILab.maxLineCount = 1;
                        keeptexturelookLabUILab.multiLine = false;
                        keeptexturelookLabUILab.overflowMethod = UILabel.Overflow.ResizeFreely;
                        keeptexturelookLabUILab.supportEncoding = false;
                        keeptexturelookLabUILab.text = "KEEP TEXTURE LOOK:";
                        keeptexturelookLabUILab.trueTypeFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        //divideXBool1LabUILab.width = intinputValueLabelUILabel.width; 
                        keeptexturelookLabUILab.aspectRatio = 7.875F;

                        GameObject keeptexturelookTog = keeptexturelook.CreateChild("Toggle");
                        keeptexturelookTog.SetLayer(laya);
                        keeptexturelookTog.GetComponent<Transform>().localPosition = new Vector3((float)-38, (float)0, 0);
                        UIWidget keeptexturelookTogUIWig = keeptexturelookTog.AddComponent<UIWidget>();
                        keeptexturelookTogUIWig.depth = 4;
                        keeptexturelookTogUIWig.autoResizeBoxCollider = true;
                        keeptexturelookTogUIWig.aspectRatio = gpubuiw.aspectRatio;
                        BoxCollider keeptexturelookTogBoxCol = keeptexturelookTog.AddComponent<BoxCollider>();
                        keeptexturelookTogBoxCol.isTrigger = true;
                        UIToggle keeptexturelookTogUITog = keeptexturelookTog.AddComponent<UIToggle>();
                        keeptexturelookTogUITog.optionCanBeNone = true;
                        keeptexturelookTogUITog.value = KTL;
                        keeptexturelookTogUITog.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnTogKTLChange)));
                        //divideXBool1TogUITog.checkAnimation = boolInputTog.checkAnimation;

                        keeptexturelookTogUITog.functionName = "";
                        UIButton keeptexturelookTogUIBut = keeptexturelookTog.AddComponent<UIButton>();
                        keeptexturelookTogUIBut.normalSprite = bollInpuntUIbut.normalSprite;
                        keeptexturelookTogUIBut.hover = bollInpuntUIbut.hover;
                        keeptexturelookTogUIBut.disabledColor = bollInpuntUIbut.disabledColor;
                        keeptexturelookTogUIBut.pressed = bollInpuntUIbut.pressed;
                        keeptexturelookTogUIBut.mStartingColor = bollInpuntUIbut.mStartingColor;
                        keeptexturelookTogUIBut.mDefaultColor = bollInpuntUIbut.mStartingColor;
                        keeptexturelookTog.AddComponent<UIExMouseWheelScrollView>();

                        GameObject keeptexturelookBack = keeptexturelookTog.CreateChild("Background");
                        keeptexturelookBack.SetLayer(laya);
                        UISprite keeptexturelookBackSprite = keeptexturelookBack.AddComponent<UISprite>();
                        keeptexturelookBackSprite.atlas = bollInpuntBackUISprit.atlas;
                        keeptexturelookBackSprite.color = bollInpuntBackUISprit.color;
                        keeptexturelookBackSprite.depth = 6;
                        keeptexturelookBackSprite.height = 16;
                        keeptexturelookBackSprite.width = 16;
                        keeptexturelookBackSprite.type = UIBasicSprite.Type.Sliced;
                        keeptexturelookBackSprite.autoResizeBoxCollider = true;
                        keeptexturelookBackSprite.spriteName = "Highlight";
                        keeptexturelookTogUIBut.tweenTarget = keeptexturelookBack;
                        keeptexturelookTogUIBut.mWidget = keeptexturelookBackSprite;


                        GameObject keeptexturelookCheckSprite = keeptexturelookTog.CreateChild("CheckSprite");
                        keeptexturelookCheckSprite.SetLayer(laya);
                        UISprite keeptexturelookCheckSpriteUISprite = keeptexturelookCheckSprite.AddComponent<UISprite>();
                        keeptexturelookCheckSpriteUISprite.alpha = 1;
                        keeptexturelookCheckSpriteUISprite.depth = 7;
                        keeptexturelookCheckSpriteUISprite.atlas = bollInpuntCeckUISprit.atlas;
                        keeptexturelookCheckSpriteUISprite.color = bollInpuntCeckUISprit.color;
                        keeptexturelookCheckSpriteUISprite.height = 11;
                        keeptexturelookCheckSpriteUISprite.width = 11;
                        keeptexturelookCheckSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        keeptexturelookCheckSpriteUISprite.spriteName = "MenuButtonBackgroundWhite";
                        //divideXBool1TogUITog.checkSprite = divideXBool1CheckSpriteUISprite;
                        keeptexturelookTogUITog.activeSprite = keeptexturelookCheckSpriteUISprite;
                        //divideXBool1TogUITog.onChange.Clear();
                        keeptexturelookTogUIWig.ResizeCollider();
                        keeptexturelookTogBoxCol.size = new Vector3((float)(keeptexturelookTogBoxCol.size.x / 5), (float)(keeptexturelookTogBoxCol.size.y / 5), keeptexturelookTogBoxCol.size.z);

                        keeptexturelookTogP = keeptexturelookTog;


                        GameObject runtessbutton = tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("runTessButton");
                        runtessbutton.SetLayer(laya);
                        runtessbutton.GetComponent<Transform>().localPosition = new Vector3((float)33, (float)(-77.0-16-35), 0);
                        GameObject runtessbuttonLab = runtessbutton.CreateChild("NameLabel");
                        runtessbuttonLab.SetLayer(laya);
                        runtessbuttonLab.GetComponent<Transform>().localPosition = new Vector3((float)-38, (float)0.0, 0);
                        UILabel runtessbuttonLabUILab = runtessbuttonLab.AddComponent<UILabel>();
                        runtessbuttonLabUILab.ambigiousFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        runtessbuttonLabUILab.depth = 10;
                        runtessbuttonLabUILab.fontSize = 22;
                        runtessbuttonLabUILab.height = 16;
                        runtessbuttonLabUILab.maxLineCount = 1;
                        runtessbuttonLabUILab.multiLine = false;
                        runtessbuttonLabUILab.overflowMethod = UILabel.Overflow.ResizeFreely;
                        runtessbuttonLabUILab.supportEncoding = false;
                        runtessbuttonLabUILab.text = "RUN DIVIDER";
                        runtessbuttonLabUILab.color = new Color((float)(108/255.0), (float)(237 / 255.0), (float)(101 / 255.0));
                        runtessbuttonLabUILab.trueTypeFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        //divideXBool1LabUILab.width = intinputValueLabelUILabel.width; 
                        runtessbuttonLabUILab.aspectRatio = 7.875F;

                        GameObject runtessbuttonTog = runtessbutton.CreateChild("Button");
                        runtessbuttonTog.SetLayer(laya);
                        runtessbuttonTog.GetComponent<Transform>().localPosition = new Vector3((float)-38, (float)0, 0);
                        UIWidget runtessbuttonTogUIWig = runtessbuttonTog.AddComponent<UIWidget>();
                        runtessbuttonTogUIWig.depth = 4;
                        runtessbuttonTogUIWig.autoResizeBoxCollider = true;
                        runtessbuttonTogUIWig.aspectRatio = gpubuiw.aspectRatio;
                        runtessbuttonTogUIWig.width = 128 + 64;
                        runtessbuttonTogUIWig.height = 32;
                        BoxCollider runtessbuttonTogBoxCol = runtessbuttonTog.AddComponent<BoxCollider>();
                        runtessbuttonTogBoxCol.isTrigger = true;
                        UIToggle runtessbuttonTogUITog = runtessbuttonTog.AddComponent<UIToggle>();
                        runtessbuttonTogUITog.optionCanBeNone = true;
                        runtessbuttonTogUITog.value = true;
                        runtessbuttonTogUITog.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnToolRUN)));
                        //divideXBool1TogUITog.checkAnimation = boolInputTog.checkAnimation;

                        runtessbuttonTogUITog.functionName = "";
                        UIButton runtessbuttonTogUIBut = runtessbuttonTog.AddComponent<UIButton>();
                        runtessbuttonTogUIBut.normalSprite = bollInpuntUIbut.normalSprite;
                        runtessbuttonTogUIBut.hover = intinputfacadeButtonUIButton.hover;
                        runtessbuttonTogUIBut.disabledColor = intinputfacadeButtonUIButton.disabledColor;
                        runtessbuttonTogUIBut.pressed = intinputfacadeButtonUIButton.pressed;
                        runtessbuttonTogUIBut.mStartingColor = intinputfacadeButtonUIButton.defaultColor;
                        runtessbuttonTogUIBut.mDefaultColor = intinputfacadeButtonUIButton.defaultColor;
                        runtessbuttonTog.AddComponent<UIExMouseWheelScrollView>();

                        GameObject runtessbuttonBack = runtessbuttonTog.CreateChild("Background");
                        runtessbuttonBack.SetLayer(laya);
                        UISprite runtessbuttonBackSprite = runtessbuttonBack.AddComponent<UISprite>();
                        runtessbuttonBackSprite.atlas = bollInpuntBackUISprit.atlas;
                        runtessbuttonBackSprite.color = bollInpuntBackUISprit.color;
                        runtessbuttonBackSprite.depth = 6;
                        runtessbuttonBackSprite.height = 32;
                        runtessbuttonBackSprite.width = 128+64;
                        runtessbuttonBackSprite.type = UIBasicSprite.Type.Sliced;
                        runtessbuttonBackSprite.autoResizeBoxCollider = true;
                        runtessbuttonBackSprite.spriteName = "Light";
                        runtessbuttonTogUIBut.tweenTarget = runtessbuttonBack;
                        runtessbuttonTogUIBut.mWidget = runtessbuttonBackSprite;

                        runtessbuttonTogUIWig.ResizeCollider();
                        runtessbuttonTogBoxCol.size = new Vector3((float)(runtessbuttonTogBoxCol.size.x / 1), (float)(runtessbuttonTogBoxCol.size.y / 1), runtessbuttonTogBoxCol.size.z);


                        GameObject windowlabel = tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("windowLabel");
                        windowlabel.SetLayer(laya);
                        windowlabel.GetComponent<Transform>().localPosition = new Vector3((float)0, (float)88.0, 0);
                        UILabel windowlabelLabUILab = windowlabel.AddComponent<UILabel>();
                        windowlabelLabUILab.ambigiousFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        windowlabelLabUILab.depth = 10;
                        windowlabelLabUILab.fontSize = 16;
                        windowlabelLabUILab.height = 16;
                        windowlabelLabUILab.maxLineCount = 1;
                        windowlabelLabUILab.multiLine = false;
                        windowlabelLabUILab.overflowMethod = UILabel.Overflow.ResizeFreely;
                        windowlabelLabUILab.supportEncoding = false;
                        windowlabelLabUILab.text = "DIVIDER WINDOW";
                        //windowlabelLabUILab.color = new Color((float)(108 / 255.0), (float)(237 / 255.0), (float)(101 / 255.0));
                        windowlabelLabUILab.trueTypeFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        //divideXBool1LabUILab.width = intinputValueLabelUILabel.width; 
                        windowlabelLabUILab.aspectRatio = 7.875F;

                        
                        GameObject closewinbut = tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("closeWindowButton");
                        closewinbut.SetLayer(laya);
                        closewinbut.GetComponent<Transform>().localPosition = new Vector3((float)230, (float)(92.0), 0);
                        GameObject closewinbutLab = closewinbut.CreateChild("NameLabel");
                        closewinbutLab.SetLayer(laya);
                        closewinbutLab.GetComponent<Transform>().localPosition = new Vector3((float)-38.5, (float)-3.5, 0);
                        UILabel closewinbutLabUILab = closewinbutLab.AddComponent<UILabel>();
                        closewinbutLabUILab.ambigiousFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        closewinbutLabUILab.depth = 10;
                        closewinbutLabUILab.fontSize = 22;
                        closewinbutLabUILab.height = 16;
                        closewinbutLabUILab.maxLineCount = 1;
                        closewinbutLabUILab.multiLine = false;
                        closewinbutLabUILab.overflowMethod = UILabel.Overflow.ResizeFreely;
                        closewinbutLabUILab.supportEncoding = false;
                        closewinbutLabUILab.text = "˟";
                        closewinbutLabUILab.color = new Color((float)(204 / 255.0), (float)(26 / 255.0), (float)(26 / 255.0));
                        closewinbutLabUILab.trueTypeFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        //divideXBool1LabUILab.width = intinputValueLabelUILabel.width; 
                        closewinbutLabUILab.aspectRatio = 7.875F;

                        GameObject closewinbutTog = closewinbut.CreateChild("Button");
                        closewinbutTog.SetLayer(laya);
                        closewinbutTog.GetComponent<Transform>().localPosition = new Vector3((float)-38, (float)0, 0);
                        UIWidget closewinbutTogUIWig = closewinbutTog.AddComponent<UIWidget>();
                        closewinbutTogUIWig.depth = 4;
                        closewinbutTogUIWig.autoResizeBoxCollider = true;
                        closewinbutTogUIWig.aspectRatio = gpubuiw.aspectRatio;
                        closewinbutTogUIWig.width = 16;
                        closewinbutTogUIWig.height = 16;
                        BoxCollider closewinbutTogBoxCol = closewinbutTog.AddComponent<BoxCollider>();
                        closewinbutTogBoxCol.isTrigger = true;
                        UIToggle closewinbutTogUITog = closewinbutTog.AddComponent<UIToggle>();
                        closewinbutTogUITog.optionCanBeNone = true;
                        closewinbutTogUITog.value = false;
                        closewinbutTogUITog.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnToolCloze)));
                        //divideXBool1TogUITog.checkAnimation = boolInputTog.checkAnimation;

                        closewinbutTogUITog.functionName = "";
                        UIButton closewinbutTogUIBut = closewinbutTog.AddComponent<UIButton>();
                        closewinbutTogUIBut.normalSprite = bollInpuntUIbut.normalSprite;
                        closewinbutTogUIBut.hover = bollInpuntUIbut.hover;
                        closewinbutTogUIBut.disabledColor = bollInpuntUIbut.disabledColor;
                        closewinbutTogUIBut.pressed = bollInpuntUIbut.pressed;
                        closewinbutTogUIBut.mStartingColor = bollInpuntUIbut.mStartingColor;
                        closewinbutTogUIBut.mDefaultColor = bollInpuntUIbut.mStartingColor;
                        closewinbutTog.AddComponent<UIExMouseWheelScrollView>();

                        GameObject closewinbutBack = closewinbutTog.CreateChild("Background");
                        closewinbutBack.SetLayer(laya);
                        UISprite closewinbutBackSprite = closewinbutBack.AddComponent<UISprite>();
                        closewinbutBackSprite.atlas = bollInpuntBackUISprit.atlas;
                        closewinbutBackSprite.color = bollInpuntBackUISprit.color;
                        closewinbutBackSprite.depth = 6;
                        closewinbutBackSprite.height = 16;
                        closewinbutBackSprite.width = 16;
                        closewinbutBackSprite.type = UIBasicSprite.Type.Sliced;
                        closewinbutBackSprite.autoResizeBoxCollider = true;
                        closewinbutBackSprite.spriteName = "Light";
                        closewinbutTogUIBut.tweenTarget = closewinbutBack;
                        closewinbutTogUIBut.mWidget = closewinbutBackSprite;

                        closewinbutTogUIWig.ResizeCollider();
                        closewinbutTogBoxCol.size = new Vector3((float)(closewinbutTogBoxCol.size.x / 1), (float)(closewinbutTogBoxCol.size.y / 1), closewinbutTogBoxCol.size.z);


                        GameObject dividerspacing = tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("Spacing");
                        dividerspacing.SetLayer(laya);
                        dividerspacing.GetComponent<Transform>().localPosition = new Vector3((float)104, (float)-63, 0);
                        GameObject dividerspacingLabel = dividerspacing.CreateChild("NameLabel");
                        dividerspacingLabel.SetLayer(laya);
                        dividerspacingLabel.GetComponent<Transform>().localPosition = new Vector3((float)-229, (float)1.0, 0);
                        UILabel dividerspacingLabUILab = dividerspacingLabel.AddComponent<UILabel>();
                        dividerspacingLabUILab.ambigiousFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        dividerspacingLabUILab.depth = 10;
                        dividerspacingLabUILab.fontSize = 16;
                        dividerspacingLabUILab.height = 16;
                        dividerspacingLabUILab.maxLineCount = 1;
                        dividerspacingLabUILab.multiLine = false;
                        dividerspacingLabUILab.overflowMethod = UILabel.Overflow.ResizeFreely;
                        dividerspacingLabUILab.supportEncoding = false;
                        dividerspacingLabUILab.text = "PART SEPARATION (XYZ):";
                        dividerspacingLabUILab.trueTypeFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        //divideXBool1LabUILab.width = intinputValueLabelUILabel.width; 
                        dividerspacingLabUILab.aspectRatio = 7.875F;



                        GameObject dividerspacingInputX = dividerspacing.CreateChild("SpacingX");
                        dividerspacingInputX.SetLayer(laya);
                        int dividerspacingInputXWidth = 84;
                        int dividerspacingInputXHeight = 20;
                        dividerspacingInputX.GetComponent<Transform>().localPosition = new Vector3((float)-125, (float)0.0, 0);
                        dividerspacingInputX.AddComponent<UIWidget>();
                        dividerspacingInputX.GetComponent<UIWidget>().depth = intinputwig.depth;

                        dividerspacingInputX.GetComponent<UIWidget>().aspectRatio = intinputwig.aspectRatio;
                        dividerspacingInputX.GetComponent<UIWidget>().height = dividerspacingInputXHeight;
                        dividerspacingInputX.GetComponent<UIWidget>().width = dividerspacingInputXWidth;

                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAlphaFrameID = 994;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAnchorsCached = true;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = gppuiw.mCam;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateAnchors = false;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateFrame = 944;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mMoved = true;

                        dividerspacingInputX.AddComponent<BoxCollider>();
                        dividerspacingInputX.GetComponent<BoxCollider>().isTrigger = true;
                        dividerspacingInputX.GetComponent<UIWidget>().autoResizeBoxCollider = true;
                        dividerspacingInputX.GetComponent<UIWidget>().ResizeCollider();
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>().size = new Vector3(intinputwig.width,intinputwig.height,0);

                        string lastXSpacingLabel = lastXSpacing + "";
                        if (!lastXSpacingLabel.Contains("."))
                        {
                            lastXSpacingLabel = lastXSpacingLabel +".0";
                        }

                        dividerspacingInputX.AddComponent<UIExNumericInput>();
                        dividerspacingInputX.GetComponent<UIExNumericInput>().value = lastXSpacingLabel;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().cursorPosition = 0;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().defaultText = lastXSpacingLabel;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().enabled = true;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().selectionStart = 0;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().selectionEnd = 0;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().characterLimit = 256;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().facadeButton_ = intinmputcomp.facadeButton_;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().label = intinmputcomp.label;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().mDefaultText = lastXSpacingLabel;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().mDoInit = false;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().mLoadSavedValue = true;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().mPosition = 63;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().caretColor = intinmputcomp.caretColor;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().Min_ = 0;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().min_ = 0;
                        dividerspacingInputX.GetComponent<UIExNumericInput>().mPivot = UIWidget.Pivot.Center;

                        dividerspacingInputX.GetComponent<UIExNumericInput>().onChange_.Add(new EventDelegate(new EventDelegate.Callback(this.OnFloatChangeSpacingX)));
                        dividerspacingInputX.GetComponent<UIExNumericInput>().onFinish_.Add(new EventDelegate(new EventDelegate.Callback(this.OnFloatChangeSpacingX)));
                        dividerspacingInputX.AddComponent<UIExDisableOnSelect>();
                        dividerspacingInputX.GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { };
                        dividerspacingInputX.GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { };
                        dividerspacingInputX.GetComponent<UIExDisableOnSelect>().disableOnSelect_ = new Behaviour[] { };

                        dividerspacingInputX.AddComponent<UIExMouseWheelScrollView>();

                        dividerspacingInputXP = dividerspacingInputX;

                        GameObject sdixleft = dividerspacingInputX.CreateChild("LeftArrow");
                        sdixleft.SetLayer(laya);
                        sdixleft.GetComponent<Transform>().localPosition = new Vector3(dividerspacingInputXWidth * -0.414682539683F, 0, 0);
                        sdixleft.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget sdixleftUIWig = sdixleft.AddComponent<UIWidget>();
                        sdixleftUIWig.depth = 6;
                        sdixleftUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        sdixleftUIWig.height = 18;
                        sdixleftUIWig.width = 18;
                        sdixleftUIWig.autoResizeBoxCollider = true;
                        BoxCollider sdixleftUIBoxCol = sdixleft.AddComponent<BoxCollider>();
                        sdixleftUIBoxCol.isTrigger = true;
                        sdixleftUIWig.ResizeCollider();
                        UIButton sdixleftUIBut = sdixleft.AddComponent<UIButton>();
                        sdixleftUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        sdixleftUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        sdixleftUIBut.hover = leftarrowintinUIBut.hover;
                        sdixleftUIBut.pressed = leftarrowintinUIBut.pressed;
                        sdixleftUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        sdixleftUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        sdixleft.AddComponent<UIExMouseWheelScrollView>();
                        GameObject sdixleftSprite = sdixleft.CreateChild("Sprite");
                        sdixleftSprite.SetLayer(laya);
                        UISprite sdixleftSpriteUISprite = sdixleftSprite.AddComponent<UISprite>();
                        sdixleftSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        sdixleftSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        sdixleftSpriteUISprite.depth = 7;
                        sdixleftSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        sdixleftSpriteUISprite.color = leftarrowintinUISprite.color;
                        sdixleftSpriteUISprite.height = leftarrowintinUISprite.height;
                        sdixleftSpriteUISprite.width = leftarrowintinUISprite.width;
                        sdixleftSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        sdixleftUIBut.tweenTarget = sdixleftSprite;
                        sdixleftUIBut.mWidget = sdixleftSpriteUISprite;
                        sdixleftUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnLeftXSpacingInputPress)));

                        GameObject sdixright = dividerspacingInputX.CreateChild("RightArrow");
                        sdixright.SetLayer(laya);
                        sdixright.GetComponent<Transform>().localPosition = new Vector3(dividerspacingInputXWidth * 0.414682539683F, 0, 0);
                        //intin1right.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget sdixrightUIWig = sdixright.AddComponent<UIWidget>();
                        sdixrightUIWig.depth = 6;
                        sdixrightUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        sdixrightUIWig.height = 18;
                        sdixrightUIWig.width = 18;
                        sdixrightUIWig.autoResizeBoxCollider = true;
                        BoxCollider sdixrightUIBoxCol = sdixright.AddComponent<BoxCollider>();
                        sdixrightUIBoxCol.isTrigger = true;
                        sdixrightUIWig.ResizeCollider();
                        UIButton sdixrightUIBut = sdixright.AddComponent<UIButton>();
                        sdixrightUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        sdixrightUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        sdixrightUIBut.hover = leftarrowintinUIBut.hover;
                        sdixrightUIBut.pressed = leftarrowintinUIBut.pressed;
                        sdixrightUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        sdixrightUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        sdixright.AddComponent<UIExMouseWheelScrollView>();
                        GameObject sdixrightSprite = sdixright.CreateChild("Sprite");
                        sdixrightSprite.SetLayer(laya);
                        UISprite sdixrightSpriteUISprite = sdixrightSprite.AddComponent<UISprite>();
                        sdixrightSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        sdixrightSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        sdixrightSpriteUISprite.depth = 7;
                        sdixrightSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        sdixrightSpriteUISprite.color = leftarrowintinUISprite.color;
                        sdixrightSpriteUISprite.height = leftarrowintinUISprite.height;
                        sdixrightSpriteUISprite.width = leftarrowintinUISprite.width;
                        sdixrightSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        sdixrightUIBut.tweenTarget = sdixrightSprite;
                        sdixrightUIBut.mWidget = sdixrightSpriteUISprite;
                        sdixrightUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnRightXSpacingInputPress)));

                        GameObject sdixValLab = dividerspacingInputX.CreateChild("ValueLabel");
                        sdixValLab.SetLayer(laya);
                        sdixValLab.AddComponent<UILabel>();
                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().alignment = NGUIText.Alignment.Center;
                        sdixValLab.GetComponent<UILabel>().ambigiousFont = intinputValueLabelUILabel.ambigiousFont; //->PetitaBold
                        sdixValLab.GetComponent<UILabel>().depth = intinputValueLabelUILabel.depth; //0->3
                        sdixValLab.GetComponent<UILabel>().fontSize = intinputValueLabelUILabel.fontSize; //16->12
                        sdixValLab.GetComponent<UILabel>().height = intinputValueLabelUILabel.height; //100->16
                        sdixValLab.GetComponent<UILabel>().maxLineCount = intinputValueLabelUILabel.maxLineCount; //0->1
                        sdixValLab.GetComponent<UILabel>().multiLine = intinputValueLabelUILabel.multiLine; //True->false
                        sdixValLab.GetComponent<UILabel>().overflowMethod = UILabel.Overflow.ClampContent;//intinputValueLabelUILabel.overflowMethod; //Shrinkcontent->Clampcontent
                        sdixValLab.GetComponent<UILabel>().supportEncoding = intinputValueLabelUILabel.supportEncoding; //True->false
                        sdixValLab.GetComponent<UILabel>().text = lastXSpacingLabel;
                        sdixValLab.GetComponent<UILabel>().trueTypeFont = intinputValueLabelUILabel.trueTypeFont; //->PetitaBold
                        sdixValLab.GetComponent<UILabel>().width = (int)(126 / 1.5); //100->126
                        sdixValLab.GetComponent<UILabel>().aspectRatio = intinputValueLabelUILabel.aspectRatio; //1->7.875

                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().mPivot = UIWidget.Pivot.Center;
                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().rawPivot = UIWidget.Pivot.Center;
                        //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(0, 0, 0);
                        //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(63, 10, 0);


                        GameObject sdixBackgroundSprite = dividerspacingInputX.CreateChild("BackgroundSprite");
                        sdixBackgroundSprite.SetLayer(laya);
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().position = intinputbg.GetComponent<Transform>().position;
                        sdixBackgroundSprite.GetComponent<Transform>().localScale = intinputbg.GetComponent<Transform>().localScale;
                        sdixBackgroundSprite.AddComponent<UISprite>();
                        sdixBackgroundSprite.GetComponent<UISprite>().depth = intinputbgsprite.depth; //2
                        sdixBackgroundSprite.GetComponent<UISprite>().atlas = intinputbgsprite.atlas;
                        sdixBackgroundSprite.GetComponent<UISprite>().autoResizeBoxCollider = intinputbgsprite.autoResizeBoxCollider;
                        sdixBackgroundSprite.GetComponent<UISprite>().spriteName = intinputbgsprite.spriteName;
                        sdixBackgroundSprite.GetComponent<UISprite>().aspectRatio = intinputbgsprite.aspectRatio;
                        sdixBackgroundSprite.GetComponent<UISprite>().type = intinputbgsprite.type;
                        sdixBackgroundSprite.GetComponent<UISprite>().mWidth = dividerspacingInputXWidth; //126
                        sdixBackgroundSprite.GetComponent<UISprite>().mHeight = dividerspacingInputXHeight; //20
                        sdixBackgroundSprite.GetComponent<UISprite>().pivot = UIWidget.Pivot.Center;
                        sdixBackgroundSprite.GetComponent<UISprite>().rawPivot = UIWidget.Pivot.Center;
                        sdixBackgroundSprite.GetComponent<UISprite>().mPivot = UIWidget.Pivot.Center;
                        sdixBackgroundSprite.GetComponent<UISprite>().color = intinputbgsprite.color;
                        sdixBackgroundSprite.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);


                        GameObject sdixFacadeButton = dividerspacingInputX.CreateChild("FacadeButton");
                        sdixFacadeButton.SetLayer(laya);
                        sdixFacadeButton.AddComponent<UIWidget>();
                        sdixFacadeButton.GetComponent<UIWidget>().depth = 3;//3 //intinputfacadeButtonWidget.depth; //1
                        sdixFacadeButton.GetComponent<UIWidget>().aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        sdixFacadeButton.GetComponent<UIWidget>().height = dividerspacingInputXHeight; //20
                        sdixFacadeButton.GetComponent<UIWidget>().width = dividerspacingInputXWidth; //126
                        sdixFacadeButton.GetComponent<UIWidget>().mAlphaFrameID = intinputfacadeButtonWidget.mAlphaFrameID;
                        sdixFacadeButton.GetComponent<UIWidget>().mAnchorsCached = intinputfacadeButtonWidget.mAnchorsCached;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = intinputfacadeButtonWidget.mCam;
                        sdixFacadeButton.GetComponent<UIWidget>().mUpdateAnchors = intinputfacadeButtonWidget.mUpdateAnchors;
                        sdixFacadeButton.GetComponent<UIWidget>().mUpdateFrame = intinputfacadeButtonWidget.mUpdateFrame;
                        sdixFacadeButton.GetComponent<UIWidget>().mMoved = intinputfacadeButtonWidget.mMoved;

                        sdixFacadeButton.AddComponent<BoxCollider>();
                        sdixFacadeButton.GetComponent<BoxCollider>().size = new Vector3(dividerspacingInputXWidth, dividerspacingInputXHeight, 0); //126w, 20h.
                        sdixFacadeButton.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);//intinputfacadeButtonBoxCollider.center; 
                        sdixFacadeButton.GetComponent<BoxCollider>().contactOffset = intinputfacadeButtonBoxCollider.contactOffset;
                        sdixFacadeButton.GetComponent<BoxCollider>().isTrigger = intinputfacadeButtonBoxCollider.isTrigger;

                        sdixFacadeButton.AddComponent<UIButton>();
                        sdixFacadeButton.GetComponent<UIButton>().normalSprite = intinputfacadeButtonUIButton.normalSprite;
                        sdixFacadeButton.GetComponent<UIButton>().disabledColor = intinputfacadeButtonUIButton.disabledColor;
                        sdixFacadeButton.GetComponent<UIButton>().hover = intinputfacadeButtonUIButton.hover;
                        sdixFacadeButton.GetComponent<UIButton>().pressed = intinputfacadeButtonUIButton.pressed;
                        sdixFacadeButton.GetComponent<UIButton>().mDefaultColor = intinputfacadeButtonUIButton.mDefaultColor;
                        sdixFacadeButton.GetComponent<UIButton>().mStartingColor = intinputfacadeButtonUIButton.mStartingColor;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mSprite = intinputfacadeButtonUIButton.mSprite;
                        sdixFacadeButton.GetComponent<UIButton>().tweenTarget = sdixBackgroundSprite;
                        sdixFacadeButton.GetComponent<UIButton>().mSprite = sdixBackgroundSprite.GetComponent<UISprite>();
                        sdixFacadeButton.GetComponent<UIButton>().mWidget = sdixBackgroundSprite.GetComponent<UISprite>();
                        dividerspacingInputX.GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { sdixFacadeButton.GetComponent<UIButton>() };
                        dividerspacingInputX.GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { sdixFacadeButton };
                        //dividerspacingInputX.GetComponent<UIExDisableOnSelect>().
                        sdixFacadeButton.AddComponent<UIExMouseWheelScrollView>();

                        dividerspacingInputX.GetComponent<UIExNumericInput>().label = sdixValLab.GetComponent<UILabel>();
                        dividerspacingInputX.GetComponent<UIExNumericInput>().facadeButton_ = sdixFacadeButton.GetComponent<UIButton>();
                        dividerspacingInputX.GetComponent<UIExNumericInput>().onChange.Clear();

                        dividerspacingInputX.GetComponent<UIExNumericInput>().Awake();



                        GameObject dividerspacingInputY = dividerspacing.CreateChild("SpacingY");
                        dividerspacingInputY.SetLayer(laya);
                        int dividerspacingInputYWidth = 84;
                        int dividerspacingInputYHeight = 20;
                        dividerspacingInputY.GetComponent<Transform>().localPosition = new Vector3((float)-38, (float)0, 0);
                        dividerspacingInputY.AddComponent<UIWidget>();
                        dividerspacingInputY.GetComponent<UIWidget>().depth = intinputwig.depth;

                        dividerspacingInputY.GetComponent<UIWidget>().aspectRatio = intinputwig.aspectRatio;
                        dividerspacingInputY.GetComponent<UIWidget>().height = dividerspacingInputYHeight;
                        dividerspacingInputY.GetComponent<UIWidget>().width = dividerspacingInputYWidth;

                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAlphaFrameID = 994;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAnchorsCached = true;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = gppuiw.mCam;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateAnchors = false;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateFrame = 944;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mMoved = true;

                        dividerspacingInputY.AddComponent<BoxCollider>();
                        dividerspacingInputY.GetComponent<BoxCollider>().isTrigger = true;
                        dividerspacingInputY.GetComponent<UIWidget>().autoResizeBoxCollider = true;
                        dividerspacingInputY.GetComponent<UIWidget>().ResizeCollider();
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>().size = new Vector3(intinputwig.width,intinputwig.height,0);

                        string lastYSpacingLabel = lastYSpacing + "";
                        if (!lastYSpacingLabel.Contains("."))
                        {
                            lastYSpacingLabel = lastYSpacingLabel + ".0";
                        }

                        dividerspacingInputY.AddComponent<UIExNumericInput>();
                        dividerspacingInputY.GetComponent<UIExNumericInput>().value = lastYSpacingLabel;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().cursorPosition = 0;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().defaultText = lastYSpacingLabel;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().enabled = true;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().selectionStart = 0;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().selectionEnd = 0;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().characterLimit = 256;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().facadeButton_ = intinmputcomp.facadeButton_;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().label = intinmputcomp.label;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().mDefaultText = lastYSpacingLabel;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().mDoInit = false;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().mLoadSavedValue = true;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().mPosition = 63;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().caretColor = intinmputcomp.caretColor;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().Min_ = 0;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().min_ = 0;
                        dividerspacingInputY.GetComponent<UIExNumericInput>().mPivot = UIWidget.Pivot.Center;

                        dividerspacingInputY.GetComponent<UIExNumericInput>().onChange_.Add(new EventDelegate(new EventDelegate.Callback(this.OnFloatChangeSpacingY)));
                        dividerspacingInputY.GetComponent<UIExNumericInput>().onFinish_.Add(new EventDelegate(new EventDelegate.Callback(this.OnFloatChangeSpacingY)));
                        dividerspacingInputY.AddComponent<UIExDisableOnSelect>();
                        dividerspacingInputY.GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { };
                        dividerspacingInputY.GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { };
                        dividerspacingInputY.GetComponent<UIExDisableOnSelect>().disableOnSelect_ = new Behaviour[] { };

                        dividerspacingInputY.AddComponent<UIExMouseWheelScrollView>();

                        dividerspacingInputYP = dividerspacingInputY;

                        GameObject sdiyleft = dividerspacingInputY.CreateChild("LeftArrow");
                        sdiyleft.SetLayer(laya);
                        sdiyleft.GetComponent<Transform>().localPosition = new Vector3(dividerspacingInputYWidth * -0.414682539683F, 0, 0);
                        sdiyleft.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget sdiyleftUIWig = sdiyleft.AddComponent<UIWidget>();
                        sdiyleftUIWig.depth = 6;
                        sdiyleftUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        sdiyleftUIWig.height = 18;
                        sdiyleftUIWig.width = 18;
                        sdiyleftUIWig.autoResizeBoxCollider = true;
                        BoxCollider sdiyleftUIBoxCol = sdiyleft.AddComponent<BoxCollider>();
                        sdiyleftUIBoxCol.isTrigger = true;
                        sdiyleftUIWig.ResizeCollider();
                        UIButton sdiyleftUIBut = sdiyleft.AddComponent<UIButton>();
                        sdiyleftUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        sdiyleftUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        sdiyleftUIBut.hover = leftarrowintinUIBut.hover;
                        sdiyleftUIBut.pressed = leftarrowintinUIBut.pressed;
                        sdiyleftUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        sdiyleftUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        sdiyleft.AddComponent<UIExMouseWheelScrollView>();
                        GameObject sdiyleftSprite = sdiyleft.CreateChild("Sprite");
                        sdiyleftSprite.SetLayer(laya);
                        UISprite sdiyleftSpriteUISprite = sdiyleftSprite.AddComponent<UISprite>();
                        sdiyleftSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        sdiyleftSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        sdiyleftSpriteUISprite.depth = 7;
                        sdiyleftSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        sdiyleftSpriteUISprite.color = leftarrowintinUISprite.color;
                        sdiyleftSpriteUISprite.height = leftarrowintinUISprite.height;
                        sdiyleftSpriteUISprite.width = leftarrowintinUISprite.width;
                        sdiyleftSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        sdiyleftUIBut.tweenTarget = sdiyleftSprite;
                        sdiyleftUIBut.mWidget = sdiyleftSpriteUISprite;
                        sdiyleftUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnLeftYSpacingInputPress)));

                        GameObject sdiyright = dividerspacingInputY.CreateChild("RightArrow");
                        sdiyright.SetLayer(laya);
                        sdiyright.GetComponent<Transform>().localPosition = new Vector3(dividerspacingInputYWidth * 0.414682539683F, 0, 0);
                        //intin1right.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget sdiyrightUIWig = sdiyright.AddComponent<UIWidget>();
                        sdiyrightUIWig.depth = 6;
                        sdiyrightUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        sdiyrightUIWig.height = 18;
                        sdiyrightUIWig.width = 18;
                        sdiyrightUIWig.autoResizeBoxCollider = true;
                        BoxCollider sdiyrightUIBoxCol = sdiyright.AddComponent<BoxCollider>();
                        sdiyrightUIBoxCol.isTrigger = true;
                        sdiyrightUIWig.ResizeCollider();
                        UIButton sdiyrightUIBut = sdiyright.AddComponent<UIButton>();
                        sdiyrightUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        sdiyrightUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        sdiyrightUIBut.hover = leftarrowintinUIBut.hover;
                        sdiyrightUIBut.pressed = leftarrowintinUIBut.pressed;
                        sdiyrightUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        sdiyrightUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        sdiyright.AddComponent<UIExMouseWheelScrollView>();
                        GameObject sdiyrightSprite = sdiyright.CreateChild("Sprite");
                        sdiyrightSprite.SetLayer(laya);
                        UISprite sdiyrightSpriteUISprite = sdiyrightSprite.AddComponent<UISprite>();
                        sdiyrightSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        sdiyrightSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        sdiyrightSpriteUISprite.depth = 7;
                        sdiyrightSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        sdiyrightSpriteUISprite.color = leftarrowintinUISprite.color;
                        sdiyrightSpriteUISprite.height = leftarrowintinUISprite.height;
                        sdiyrightSpriteUISprite.width = leftarrowintinUISprite.width;
                        sdiyrightSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        sdiyrightUIBut.tweenTarget = sdiyrightSprite;
                        sdiyrightUIBut.mWidget = sdiyrightSpriteUISprite;
                        sdiyrightUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnRightYSpacingInputPress)));

                        GameObject sdiyValLab = dividerspacingInputY.CreateChild("ValueLabel");
                        sdiyValLab.SetLayer(laya);
                        sdiyValLab.AddComponent<UILabel>();
                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().alignment = NGUIText.Alignment.Center;
                        sdiyValLab.GetComponent<UILabel>().ambigiousFont = intinputValueLabelUILabel.ambigiousFont; //->PetitaBold
                        sdiyValLab.GetComponent<UILabel>().depth = intinputValueLabelUILabel.depth; //0->3
                        sdiyValLab.GetComponent<UILabel>().fontSize = intinputValueLabelUILabel.fontSize; //16->12
                        sdiyValLab.GetComponent<UILabel>().height = intinputValueLabelUILabel.height; //100->16
                        sdiyValLab.GetComponent<UILabel>().maxLineCount = intinputValueLabelUILabel.maxLineCount; //0->1
                        sdiyValLab.GetComponent<UILabel>().multiLine = intinputValueLabelUILabel.multiLine; //True->false
                        sdiyValLab.GetComponent<UILabel>().overflowMethod = UILabel.Overflow.ClampContent;//intinputValueLabelUILabel.overflowMethod; //Shrinkcontent->Clampcontent
                        sdiyValLab.GetComponent<UILabel>().supportEncoding = intinputValueLabelUILabel.supportEncoding; //True->false
                        sdiyValLab.GetComponent<UILabel>().text = lastYSpacingLabel;
                        sdiyValLab.GetComponent<UILabel>().trueTypeFont = intinputValueLabelUILabel.trueTypeFont; //->PetitaBold
                        sdiyValLab.GetComponent<UILabel>().width = (int)(126 / 1.5); //100->126
                        sdiyValLab.GetComponent<UILabel>().aspectRatio = intinputValueLabelUILabel.aspectRatio; //1->7.875

                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().mPivot = UIWidget.Pivot.Center;
                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().rawPivot = UIWidget.Pivot.Center;
                        //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(0, 0, 0);
                        //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(63, 10, 0);


                        GameObject sdiyBackgroundSprite = dividerspacingInputY.CreateChild("BackgroundSprite");
                        sdiyBackgroundSprite.SetLayer(laya);
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().position = intinputbg.GetComponent<Transform>().position;
                        sdiyBackgroundSprite.GetComponent<Transform>().localScale = intinputbg.GetComponent<Transform>().localScale;
                        sdiyBackgroundSprite.AddComponent<UISprite>();
                        sdiyBackgroundSprite.GetComponent<UISprite>().depth = intinputbgsprite.depth; //2
                        sdiyBackgroundSprite.GetComponent<UISprite>().atlas = intinputbgsprite.atlas;
                        sdiyBackgroundSprite.GetComponent<UISprite>().autoResizeBoxCollider = intinputbgsprite.autoResizeBoxCollider;
                        sdiyBackgroundSprite.GetComponent<UISprite>().spriteName = intinputbgsprite.spriteName;
                        sdiyBackgroundSprite.GetComponent<UISprite>().aspectRatio = intinputbgsprite.aspectRatio;
                        sdiyBackgroundSprite.GetComponent<UISprite>().type = intinputbgsprite.type;
                        sdiyBackgroundSprite.GetComponent<UISprite>().mWidth = dividerspacingInputYWidth; //126
                        sdiyBackgroundSprite.GetComponent<UISprite>().mHeight = dividerspacingInputYHeight; //20
                        sdiyBackgroundSprite.GetComponent<UISprite>().pivot = UIWidget.Pivot.Center;
                        sdiyBackgroundSprite.GetComponent<UISprite>().rawPivot = UIWidget.Pivot.Center;
                        sdiyBackgroundSprite.GetComponent<UISprite>().mPivot = UIWidget.Pivot.Center;
                        sdiyBackgroundSprite.GetComponent<UISprite>().color = intinputbgsprite.color;
                        sdiyBackgroundSprite.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);


                        GameObject sdiyFacadeButton = dividerspacingInputY.CreateChild("FacadeButton");
                        sdiyFacadeButton.SetLayer(laya);
                        sdiyFacadeButton.AddComponent<UIWidget>();
                        sdiyFacadeButton.GetComponent<UIWidget>().depth = 3;//3 //intinputfacadeButtonWidget.depth; //1
                        sdiyFacadeButton.GetComponent<UIWidget>().aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        sdiyFacadeButton.GetComponent<UIWidget>().height = dividerspacingInputYHeight; //20
                        sdiyFacadeButton.GetComponent<UIWidget>().width = dividerspacingInputYWidth; //126
                        sdiyFacadeButton.GetComponent<UIWidget>().mAlphaFrameID = intinputfacadeButtonWidget.mAlphaFrameID;
                        sdiyFacadeButton.GetComponent<UIWidget>().mAnchorsCached = intinputfacadeButtonWidget.mAnchorsCached;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = intinputfacadeButtonWidget.mCam;
                        sdiyFacadeButton.GetComponent<UIWidget>().mUpdateAnchors = intinputfacadeButtonWidget.mUpdateAnchors;
                        sdiyFacadeButton.GetComponent<UIWidget>().mUpdateFrame = intinputfacadeButtonWidget.mUpdateFrame;
                        sdiyFacadeButton.GetComponent<UIWidget>().mMoved = intinputfacadeButtonWidget.mMoved;

                        sdiyFacadeButton.AddComponent<BoxCollider>();
                        sdiyFacadeButton.GetComponent<BoxCollider>().size = new Vector3(dividerspacingInputYWidth, dividerspacingInputYHeight, 0); //126w, 20h.
                        sdiyFacadeButton.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);//intinputfacadeButtonBoxCollider.center; 
                        sdiyFacadeButton.GetComponent<BoxCollider>().contactOffset = intinputfacadeButtonBoxCollider.contactOffset;
                        sdiyFacadeButton.GetComponent<BoxCollider>().isTrigger = intinputfacadeButtonBoxCollider.isTrigger;

                        sdiyFacadeButton.AddComponent<UIButton>();
                        sdiyFacadeButton.GetComponent<UIButton>().normalSprite = intinputfacadeButtonUIButton.normalSprite;
                        sdiyFacadeButton.GetComponent<UIButton>().disabledColor = intinputfacadeButtonUIButton.disabledColor;
                        sdiyFacadeButton.GetComponent<UIButton>().hover = intinputfacadeButtonUIButton.hover;
                        sdiyFacadeButton.GetComponent<UIButton>().pressed = intinputfacadeButtonUIButton.pressed;
                        sdiyFacadeButton.GetComponent<UIButton>().mDefaultColor = intinputfacadeButtonUIButton.mDefaultColor;
                        sdiyFacadeButton.GetComponent<UIButton>().mStartingColor = intinputfacadeButtonUIButton.mStartingColor;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mSprite = intinputfacadeButtonUIButton.mSprite;
                        sdiyFacadeButton.GetComponent<UIButton>().tweenTarget = sdiyBackgroundSprite;
                        sdiyFacadeButton.GetComponent<UIButton>().mSprite = sdiyBackgroundSprite.GetComponent<UISprite>();
                        sdiyFacadeButton.GetComponent<UIButton>().mWidget = sdiyBackgroundSprite.GetComponent<UISprite>();
                        dividerspacingInputY.GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { sdiyFacadeButton.GetComponent<UIButton>() };
                        dividerspacingInputY.GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { sdiyFacadeButton };
                        //dividerspacingInputY.GetComponent<UIExDisableOnSelect>().
                        sdiyFacadeButton.AddComponent<UIExMouseWheelScrollView>();

                        dividerspacingInputY.GetComponent<UIExNumericInput>().label = sdiyValLab.GetComponent<UILabel>();
                        dividerspacingInputY.GetComponent<UIExNumericInput>().facadeButton_ = sdiyFacadeButton.GetComponent<UIButton>();
                        dividerspacingInputY.GetComponent<UIExNumericInput>().onChange.Clear();

                        dividerspacingInputY.GetComponent<UIExNumericInput>().Awake();



                        GameObject dividerspacingInputZ = dividerspacing.CreateChild("SpacingZ");
                        dividerspacingInputZ.SetLayer(laya);
                        int dividerspacingInputZWidth = 84;
                        int dividerspacingInputZHeight = 20;
                        dividerspacingInputZ.GetComponent<Transform>().localPosition = new Vector3((float)49, (float)0, 0);
                        dividerspacingInputZ.AddComponent<UIWidget>();
                        dividerspacingInputZ.GetComponent<UIWidget>().depth = intinputwig.depth;

                        dividerspacingInputZ.GetComponent<UIWidget>().aspectRatio = intinputwig.aspectRatio;
                        dividerspacingInputZ.GetComponent<UIWidget>().height = dividerspacingInputZHeight;
                        dividerspacingInputZ.GetComponent<UIWidget>().width = dividerspacingInputZWidth;

                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAlphaFrameID = 994;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mAnchorsCached = true;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = gppuiw.mCam;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateAnchors = false;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mUpdateFrame = 944;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mMoved = true;

                        dividerspacingInputZ.AddComponent<BoxCollider>();
                        dividerspacingInputZ.GetComponent<BoxCollider>().isTrigger = true;
                        dividerspacingInputZ.GetComponent<UIWidget>().autoResizeBoxCollider = true;
                        dividerspacingInputZ.GetComponent<UIWidget>().ResizeCollider();
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().AddComponent<BoxCollider>().size = new Vector3(intinputwig.width,intinputwig.height,0);

                        string lastZSpacingLabel = lastZSpacing + "";
                        if (!lastZSpacingLabel.Contains("."))
                        {
                            lastZSpacingLabel = lastZSpacingLabel + ".0";
                        }

                        dividerspacingInputZ.AddComponent<UIExNumericInput>();
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().value = lastZSpacingLabel;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().cursorPosition = 0;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().defaultText = lastZSpacingLabel;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().enabled = true;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().selectionStart = 0;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().selectionEnd = 0;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().characterLimit = 256;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().facadeButton_ = intinmputcomp.facadeButton_;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetComponent<UIExIntegerInput>().label = intinmputcomp.label;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().mDefaultText = lastZSpacingLabel;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().mDoInit = false;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().mLoadSavedValue = true;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().mPosition = 63;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().caretColor = intinmputcomp.caretColor;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().Min_ = 0;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().min_ = 0;
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().mPivot = UIWidget.Pivot.Center;

                        dividerspacingInputZ.GetComponent<UIExNumericInput>().onChange_.Add(new EventDelegate(new EventDelegate.Callback(this.OnFloatChangeSpacingZ)));
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().onFinish_.Add(new EventDelegate(new EventDelegate.Callback(this.OnFloatChangeSpacingZ)));
                        dividerspacingInputZ.AddComponent<UIExDisableOnSelect>();
                        dividerspacingInputZ.GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { };
                        dividerspacingInputZ.GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { };
                        dividerspacingInputZ.GetComponent<UIExDisableOnSelect>().disableOnSelect_ = new Behaviour[] { };

                        dividerspacingInputZ.AddComponent<UIExMouseWheelScrollView>();

                        dividerspacingInputZP = dividerspacingInputZ;

                        GameObject sdizleft = dividerspacingInputZ.CreateChild("LeftArrow");
                        sdizleft.SetLayer(laya);
                        sdizleft.GetComponent<Transform>().localPosition = new Vector3(dividerspacingInputZWidth * -0.414682539683F, 0, 0);
                        sdizleft.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget sdizleftUIWig = sdizleft.AddComponent<UIWidget>();
                        sdizleftUIWig.depth = 6;
                        sdizleftUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        sdizleftUIWig.height = 18;
                        sdizleftUIWig.width = 18;
                        sdizleftUIWig.autoResizeBoxCollider = true;
                        BoxCollider sdizleftUIBoxCol = sdizleft.AddComponent<BoxCollider>();
                        sdizleftUIBoxCol.isTrigger = true;
                        sdizleftUIWig.ResizeCollider();
                        UIButton sdizleftUIBut = sdizleft.AddComponent<UIButton>();
                        sdizleftUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        sdizleftUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        sdizleftUIBut.hover = leftarrowintinUIBut.hover;
                        sdizleftUIBut.pressed = leftarrowintinUIBut.pressed;
                        sdizleftUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        sdizleftUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        sdizleft.AddComponent<UIExMouseWheelScrollView>();
                        GameObject sdizleftSprite = sdizleft.CreateChild("Sprite");
                        sdizleftSprite.SetLayer(laya);
                        UISprite sdizleftSpriteUISprite = sdizleftSprite.AddComponent<UISprite>();
                        sdizleftSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        sdizleftSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        sdizleftSpriteUISprite.depth = 7;
                        sdizleftSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        sdizleftSpriteUISprite.color = leftarrowintinUISprite.color;
                        sdizleftSpriteUISprite.height = leftarrowintinUISprite.height;
                        sdizleftSpriteUISprite.width = leftarrowintinUISprite.width;
                        sdizleftSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        sdizleftUIBut.tweenTarget = sdizleftSprite;
                        sdizleftUIBut.mWidget = sdizleftSpriteUISprite;
                        sdizleftUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnLeftZSpacingInputPress)));

                        GameObject sdizright = dividerspacingInputZ.CreateChild("RightArrow");
                        sdizright.SetLayer(laya);
                        sdizright.GetComponent<Transform>().localPosition = new Vector3(dividerspacingInputZWidth * 0.414682539683F, 0, 0);
                        //intin1right.GetComponent<Transform>().localRotation = new Quaternion(0, 1F, 0, 0F);
                        UIWidget sdizrightUIWig = sdizright.AddComponent<UIWidget>();
                        sdizrightUIWig.depth = 6;
                        sdizrightUIWig.aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        sdizrightUIWig.height = 18;
                        sdizrightUIWig.width = 18;
                        sdizrightUIWig.autoResizeBoxCollider = true;
                        BoxCollider sdizrightUIBoxCol = sdizright.AddComponent<BoxCollider>();
                        sdizrightUIBoxCol.isTrigger = true;
                        sdizrightUIWig.ResizeCollider();
                        UIButton sdizrightUIBut = sdizright.AddComponent<UIButton>();
                        sdizrightUIBut.normalSprite = leftarrowintinUIBut.normalSprite;
                        sdizrightUIBut.disabledColor = leftarrowintinUIBut.disabledColor;
                        sdizrightUIBut.hover = leftarrowintinUIBut.hover;
                        sdizrightUIBut.pressed = leftarrowintinUIBut.pressed;
                        sdizrightUIBut.mStartingColor = leftarrowintinUIBut.mStartingColor;
                        sdizrightUIBut.mDefaultColor = leftarrowintinUIBut.mDefaultColor;
                        sdizright.AddComponent<UIExMouseWheelScrollView>();
                        GameObject sdizrightSprite = sdizright.CreateChild("Sprite");
                        sdizrightSprite.SetLayer(laya);
                        UISprite sdizrightSpriteUISprite = sdizrightSprite.AddComponent<UISprite>();
                        sdizrightSpriteUISprite.alpha = leftarrowintinUISprite.alpha;
                        sdizrightSpriteUISprite.atlas = leftarrowintinUISprite.atlas;
                        sdizrightSpriteUISprite.depth = 7;
                        sdizrightSpriteUISprite.spriteName = leftarrowintinUISprite.spriteName;
                        sdizrightSpriteUISprite.color = leftarrowintinUISprite.color;
                        sdizrightSpriteUISprite.height = leftarrowintinUISprite.height;
                        sdizrightSpriteUISprite.width = leftarrowintinUISprite.width;
                        sdizrightSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        sdizrightUIBut.tweenTarget = sdizrightSprite;
                        sdizrightUIBut.mWidget = sdizrightSpriteUISprite;
                        sdizrightUIBut.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnRightZSpacingInputPress)));

                        GameObject sdizValLab = dividerspacingInputZ.CreateChild("ValueLabel");
                        sdizValLab.SetLayer(laya);
                        sdizValLab.AddComponent<UILabel>();
                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().alignment = NGUIText.Alignment.Center;
                        sdizValLab.GetComponent<UILabel>().ambigiousFont = intinputValueLabelUILabel.ambigiousFont; //->PetitaBold
                        sdizValLab.GetComponent<UILabel>().depth = intinputValueLabelUILabel.depth; //0->3
                        sdizValLab.GetComponent<UILabel>().fontSize = intinputValueLabelUILabel.fontSize; //16->12
                        sdizValLab.GetComponent<UILabel>().height = intinputValueLabelUILabel.height; //100->16
                        sdizValLab.GetComponent<UILabel>().maxLineCount = intinputValueLabelUILabel.maxLineCount; //0->1
                        sdizValLab.GetComponent<UILabel>().multiLine = intinputValueLabelUILabel.multiLine; //True->false
                        sdizValLab.GetComponent<UILabel>().overflowMethod = UILabel.Overflow.ClampContent;//intinputValueLabelUILabel.overflowMethod; //Shrinkcontent->Clampcontent
                        sdizValLab.GetComponent<UILabel>().supportEncoding = intinputValueLabelUILabel.supportEncoding; //True->false
                        sdizValLab.GetComponent<UILabel>().text = lastZSpacingLabel;
                        sdizValLab.GetComponent<UILabel>().trueTypeFont = intinputValueLabelUILabel.trueTypeFont; //->PetitaBold
                        sdizValLab.GetComponent<UILabel>().width = (int)(126 / 1.5); //100->126
                        sdizValLab.GetComponent<UILabel>().aspectRatio = intinputValueLabelUILabel.aspectRatio; //1->7.875

                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().mPivot = UIWidget.Pivot.Center;
                        //tabSystemUIRootIntInputValLab.GetComponent<UILabel>().rawPivot = UIWidget.Pivot.Center;
                        //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(0, 0, 0);
                        //tabSystemUIRootIntInputValLab.GetComponent<Transform>().position = new Vector3(63, 10, 0);


                        GameObject sdizBackgroundSprite = dividerspacingInputZ.CreateChild("BackgroundSprite");
                        sdizBackgroundSprite.SetLayer(laya);
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<Transform>().position = intinputbg.GetComponent<Transform>().position;
                        sdizBackgroundSprite.GetComponent<Transform>().localScale = intinputbg.GetComponent<Transform>().localScale;
                        sdizBackgroundSprite.AddComponent<UISprite>();
                        sdizBackgroundSprite.GetComponent<UISprite>().depth = intinputbgsprite.depth; //2
                        sdizBackgroundSprite.GetComponent<UISprite>().atlas = intinputbgsprite.atlas;
                        sdizBackgroundSprite.GetComponent<UISprite>().autoResizeBoxCollider = intinputbgsprite.autoResizeBoxCollider;
                        sdizBackgroundSprite.GetComponent<UISprite>().spriteName = intinputbgsprite.spriteName;
                        sdizBackgroundSprite.GetComponent<UISprite>().aspectRatio = intinputbgsprite.aspectRatio;
                        sdizBackgroundSprite.GetComponent<UISprite>().type = intinputbgsprite.type;
                        sdizBackgroundSprite.GetComponent<UISprite>().mWidth = dividerspacingInputZWidth; //126
                        sdizBackgroundSprite.GetComponent<UISprite>().mHeight = dividerspacingInputZHeight; //20
                        sdizBackgroundSprite.GetComponent<UISprite>().pivot = UIWidget.Pivot.Center;
                        sdizBackgroundSprite.GetComponent<UISprite>().rawPivot = UIWidget.Pivot.Center;
                        sdizBackgroundSprite.GetComponent<UISprite>().mPivot = UIWidget.Pivot.Center;
                        sdizBackgroundSprite.GetComponent<UISprite>().color = intinputbgsprite.color;
                        sdizBackgroundSprite.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);


                        GameObject sdizFacadeButton = dividerspacingInputZ.CreateChild("FacadeButton");
                        sdizFacadeButton.SetLayer(laya);
                        sdizFacadeButton.AddComponent<UIWidget>();
                        sdizFacadeButton.GetComponent<UIWidget>().depth = 3;//3 //intinputfacadeButtonWidget.depth; //1
                        sdizFacadeButton.GetComponent<UIWidget>().aspectRatio = intinputfacadeButtonWidget.aspectRatio;
                        sdizFacadeButton.GetComponent<UIWidget>().height = dividerspacingInputZHeight; //20
                        sdizFacadeButton.GetComponent<UIWidget>().width = dividerspacingInputZWidth; //126
                        sdizFacadeButton.GetComponent<UIWidget>().mAlphaFrameID = intinputfacadeButtonWidget.mAlphaFrameID;
                        sdizFacadeButton.GetComponent<UIWidget>().mAnchorsCached = intinputfacadeButtonWidget.mAnchorsCached;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIWidget>().mCam = intinputfacadeButtonWidget.mCam;
                        sdizFacadeButton.GetComponent<UIWidget>().mUpdateAnchors = intinputfacadeButtonWidget.mUpdateAnchors;
                        sdizFacadeButton.GetComponent<UIWidget>().mUpdateFrame = intinputfacadeButtonWidget.mUpdateFrame;
                        sdizFacadeButton.GetComponent<UIWidget>().mMoved = intinputfacadeButtonWidget.mMoved;

                        sdizFacadeButton.AddComponent<BoxCollider>();
                        sdizFacadeButton.GetComponent<BoxCollider>().size = new Vector3(dividerspacingInputZWidth, dividerspacingInputZHeight, 0); //126w, 20h.
                        sdizFacadeButton.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);//intinputfacadeButtonBoxCollider.center; 
                        sdizFacadeButton.GetComponent<BoxCollider>().contactOffset = intinputfacadeButtonBoxCollider.contactOffset;
                        sdizFacadeButton.GetComponent<BoxCollider>().isTrigger = intinputfacadeButtonBoxCollider.isTrigger;

                        sdizFacadeButton.AddComponent<UIButton>();
                        sdizFacadeButton.GetComponent<UIButton>().normalSprite = intinputfacadeButtonUIButton.normalSprite;
                        sdizFacadeButton.GetComponent<UIButton>().disabledColor = intinputfacadeButtonUIButton.disabledColor;
                        sdizFacadeButton.GetComponent<UIButton>().hover = intinputfacadeButtonUIButton.hover;
                        sdizFacadeButton.GetComponent<UIButton>().pressed = intinputfacadeButtonUIButton.pressed;
                        sdizFacadeButton.GetComponent<UIButton>().mDefaultColor = intinputfacadeButtonUIButton.mDefaultColor;
                        sdizFacadeButton.GetComponent<UIButton>().mStartingColor = intinputfacadeButtonUIButton.mStartingColor;
                        //tabSystemUIRoot.gameObject.GetChildren().Last().GetChildren().Last().GetChildren().Last().GetComponent<UIButton>().mSprite = intinputfacadeButtonUIButton.mSprite;
                        sdizFacadeButton.GetComponent<UIButton>().tweenTarget = sdizBackgroundSprite;
                        sdizFacadeButton.GetComponent<UIButton>().mSprite = sdizBackgroundSprite.GetComponent<UISprite>();
                        sdizFacadeButton.GetComponent<UIButton>().mWidget = sdizBackgroundSprite.GetComponent<UISprite>();
                        dividerspacingInputZ.GetComponent<UIExDisableOnSelect>().disableButtonsOnSelect_ = new UIButton[] { sdizFacadeButton.GetComponent<UIButton>() };
                        dividerspacingInputZ.GetComponent<UIExDisableOnSelect>().deactivateOnSelect_ = new GameObject[] { sdizFacadeButton };
                        //dividerspacingInputZ.GetComponent<UIExDisableOnSelect>().
                        sdizFacadeButton.AddComponent<UIExMouseWheelScrollView>();

                        dividerspacingInputZ.GetComponent<UIExNumericInput>().label = sdizValLab.GetComponent<UILabel>();
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().facadeButton_ = sdizFacadeButton.GetComponent<UIButton>();
                        dividerspacingInputZ.GetComponent<UIExNumericInput>().onChange.Clear();

                        dividerspacingInputZ.GetComponent<UIExNumericInput>().Awake();


                        GameObject cubetoplanes = tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("CubesToPlanes");
                        cubetoplanes.SetLayer(laya);
                        cubetoplanes.GetComponent<Transform>().localPosition = new Vector3((float)33, (float)(-85.0), 0);
                        GameObject cubetoplanesLab = cubetoplanes.CreateChild("NameLabel");
                        cubetoplanesLab.SetLayer(laya);
                        cubetoplanesLab.GetComponent<Transform>().localPosition = new Vector3((float)-98, (float)0.0, 0);
                        UILabel cubetoplanesLabUILab = cubetoplanesLab.AddComponent<UILabel>();
                        cubetoplanesLabUILab.ambigiousFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        cubetoplanesLabUILab.depth = 10;
                        cubetoplanesLabUILab.fontSize = 16;
                        cubetoplanesLabUILab.height = 16;
                        cubetoplanesLabUILab.maxLineCount = 1;
                        cubetoplanesLabUILab.multiLine = false;
                        cubetoplanesLabUILab.overflowMethod = UILabel.Overflow.ResizeFreely;
                        cubetoplanesLabUILab.supportEncoding = false;
                        cubetoplanesLabUILab.text = "CUBES TO PLANES:";
                        cubetoplanesLabUILab.trueTypeFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        //divideXBool1LabUILab.width = intinputValueLabelUILabel.width; 
                        cubetoplanesLabUILab.aspectRatio = 7.875F;

                        GameObject cubetoplanesTog = cubetoplanes.CreateChild("Toggle");
                        cubetoplanesTog.SetLayer(laya);
                        cubetoplanesTog.GetComponent<Transform>().localPosition = new Vector3((float)-38, (float)0, 0);
                        UIWidget cubetoplanesTogUIWig = cubetoplanesTog.AddComponent<UIWidget>();
                        cubetoplanesTogUIWig.depth = 4;
                        cubetoplanesTogUIWig.autoResizeBoxCollider = true;
                        cubetoplanesTogUIWig.aspectRatio = gpubuiw.aspectRatio;
                        BoxCollider cubetoplanesTogBoxCol = cubetoplanesTog.AddComponent<BoxCollider>();
                        cubetoplanesTogBoxCol.isTrigger = true;
                        UIToggle cubetoplanesTogUITog = cubetoplanesTog.AddComponent<UIToggle>();
                        cubetoplanesTogUITog.optionCanBeNone = true;
                        cubetoplanesTogUITog.value = CTP;
                        cubetoplanesTogUITog.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnTogCTPChange)));
                        //divideXBool1TogUITog.checkAnimation = boolInputTog.checkAnimation;

                        cubetoplanesTogUITog.functionName = "";
                        UIButton cubetoplanesTogUIBut = cubetoplanesTog.AddComponent<UIButton>();
                        cubetoplanesTogUIBut.normalSprite = bollInpuntUIbut.normalSprite;
                        cubetoplanesTogUIBut.hover = bollInpuntUIbut.hover;
                        cubetoplanesTogUIBut.disabledColor = bollInpuntUIbut.disabledColor;
                        cubetoplanesTogUIBut.pressed = bollInpuntUIbut.pressed;
                        cubetoplanesTogUIBut.mStartingColor = bollInpuntUIbut.mStartingColor;
                        cubetoplanesTogUIBut.mDefaultColor = bollInpuntUIbut.mStartingColor;
                        cubetoplanesTog.AddComponent<UIExMouseWheelScrollView>();

                        GameObject cubetoplanesBack = cubetoplanesTog.CreateChild("Background");
                        cubetoplanesBack.SetLayer(laya);
                        UISprite cubetoplanesBackSprite = cubetoplanesBack.AddComponent<UISprite>();
                        cubetoplanesBackSprite.atlas = bollInpuntBackUISprit.atlas;
                        cubetoplanesBackSprite.color = bollInpuntBackUISprit.color;
                        cubetoplanesBackSprite.depth = 6;
                        cubetoplanesBackSprite.height = 16;
                        cubetoplanesBackSprite.width = 16;
                        cubetoplanesBackSprite.type = UIBasicSprite.Type.Sliced;
                        cubetoplanesBackSprite.autoResizeBoxCollider = true;
                        cubetoplanesBackSprite.spriteName = "Highlight";
                        cubetoplanesTogUIBut.tweenTarget = cubetoplanesBack;
                        cubetoplanesTogUIBut.mWidget = cubetoplanesBackSprite;


                        GameObject cubetoplanesCheckSprite = cubetoplanesTog.CreateChild("CheckSprite");
                        cubetoplanesCheckSprite.SetLayer(laya);
                        UISprite cubetoplanesCheckSpriteUISprite = cubetoplanesCheckSprite.AddComponent<UISprite>();
                        cubetoplanesCheckSpriteUISprite.alpha = 1;
                        cubetoplanesCheckSpriteUISprite.depth = 7;
                        cubetoplanesCheckSpriteUISprite.atlas = bollInpuntCeckUISprit.atlas;
                        cubetoplanesCheckSpriteUISprite.color = bollInpuntCeckUISprit.color;
                        cubetoplanesCheckSpriteUISprite.height = 11;
                        cubetoplanesCheckSpriteUISprite.width = 11;
                        cubetoplanesCheckSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        cubetoplanesCheckSpriteUISprite.spriteName = "MenuButtonBackgroundWhite";
                        //divideXBool1TogUITog.checkSprite = divideXBool1CheckSpriteUISprite;
                        cubetoplanesTogUITog.activeSprite = cubetoplanesCheckSpriteUISprite;
                        //divideXBool1TogUITog.onChange.Clear();
                        cubetoplanesTogUIWig.ResizeCollider();
                        cubetoplanesTogBoxCol.size = new Vector3((float)(cubetoplanesTogBoxCol.size.x / 5), (float)(cubetoplanesTogBoxCol.size.y / 5), cubetoplanesTogBoxCol.size.z);

                        cubetoplanesTogP = cubetoplanesTog;


                        GameObject noplaneoverlap = tabSystemUIRoot.gameObject.GetChildren().Last().CreateChild("NoPlaneOverlap");
                        noplaneoverlap.SetLayer(laya);
                        noplaneoverlap.GetComponent<Transform>().localPosition = new Vector3((float)33, (float)(-85.0-16), 0);
                        GameObject noplaneoverlapLab = noplaneoverlap.CreateChild("NameLabel");
                        noplaneoverlapLab.SetLayer(laya);
                        noplaneoverlapLab.GetComponent<Transform>().localPosition = new Vector3((float)-98, (float)0.0, 0);
                        UILabel noplaneoverlapLabUILab = noplaneoverlapLab.AddComponent<UILabel>();
                        noplaneoverlapLabUILab.ambigiousFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        noplaneoverlapLabUILab.depth = 10;
                        noplaneoverlapLabUILab.fontSize = 16;
                        noplaneoverlapLabUILab.height = 16;
                        noplaneoverlapLabUILab.maxLineCount = 1;
                        noplaneoverlapLabUILab.multiLine = false;
                        noplaneoverlapLabUILab.overflowMethod = UILabel.Overflow.ResizeFreely;
                        noplaneoverlapLabUILab.supportEncoding = false;
                        noplaneoverlapLabUILab.text = "NO PLANE OVERLAP:";
                        noplaneoverlapLabUILab.trueTypeFont = G.Sys.ResourceManager_.GetResource<Font>("BebasNeue"); //->PetitaBold
                        //divideXBool1LabUILab.width = intinputValueLabelUILabel.width; 
                        noplaneoverlapLabUILab.aspectRatio = 7.875F;

                        GameObject noplaneoverlapTog = noplaneoverlap.CreateChild("Toggle");
                        noplaneoverlapTog.SetLayer(laya);
                        noplaneoverlapTog.GetComponent<Transform>().localPosition = new Vector3((float)-38, (float)0, 0);
                        UIWidget noplaneoverlapTogUIWig = noplaneoverlapTog.AddComponent<UIWidget>();
                        noplaneoverlapTogUIWig.depth = 4;
                        noplaneoverlapTogUIWig.autoResizeBoxCollider = true;
                        noplaneoverlapTogUIWig.aspectRatio = gpubuiw.aspectRatio;
                        BoxCollider noplaneoverlapTogBoxCol = noplaneoverlapTog.AddComponent<BoxCollider>();
                        noplaneoverlapTogBoxCol.isTrigger = true;
                        UIToggle noplaneoverlapTogUITog = noplaneoverlapTog.AddComponent<UIToggle>();
                        noplaneoverlapTogUITog.optionCanBeNone = true;
                        noplaneoverlapTogUITog.value = NPO;
                        noplaneoverlapTogUITog.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnTogNPOChange)));
                        //divideXBool1TogUITog.checkAnimation = boolInputTog.checkAnimation;

                        noplaneoverlapTogUITog.functionName = "";
                        UIButton noplaneoverlapTogUIBut = noplaneoverlapTog.AddComponent<UIButton>();
                        noplaneoverlapTogUIBut.normalSprite = bollInpuntUIbut.normalSprite;
                        noplaneoverlapTogUIBut.hover = bollInpuntUIbut.hover;
                        noplaneoverlapTogUIBut.disabledColor = bollInpuntUIbut.disabledColor;
                        noplaneoverlapTogUIBut.pressed = bollInpuntUIbut.pressed;
                        noplaneoverlapTogUIBut.mStartingColor = bollInpuntUIbut.mStartingColor;
                        noplaneoverlapTogUIBut.mDefaultColor = bollInpuntUIbut.mStartingColor;
                        noplaneoverlapTog.AddComponent<UIExMouseWheelScrollView>();

                        GameObject noplaneoverlapBack = noplaneoverlapTog.CreateChild("Background");
                        noplaneoverlapBack.SetLayer(laya);
                        UISprite noplaneoverlapBackSprite = noplaneoverlapBack.AddComponent<UISprite>();
                        noplaneoverlapBackSprite.atlas = bollInpuntBackUISprit.atlas;
                        noplaneoverlapBackSprite.color = bollInpuntBackUISprit.color;
                        noplaneoverlapBackSprite.depth = 6;
                        noplaneoverlapBackSprite.height = 16;
                        noplaneoverlapBackSprite.width = 16;
                        noplaneoverlapBackSprite.type = UIBasicSprite.Type.Sliced;
                        noplaneoverlapBackSprite.autoResizeBoxCollider = true;
                        noplaneoverlapBackSprite.spriteName = "Highlight";
                        noplaneoverlapTogUIBut.tweenTarget = noplaneoverlapBack;
                        noplaneoverlapTogUIBut.mWidget = noplaneoverlapBackSprite;


                        GameObject noplaneoverlapCheckSprite = noplaneoverlapTog.CreateChild("CheckSprite");
                        noplaneoverlapCheckSprite.SetLayer(laya);
                        UISprite noplaneoverlapCheckSpriteUISprite = noplaneoverlapCheckSprite.AddComponent<UISprite>();
                        noplaneoverlapCheckSpriteUISprite.alpha = 1;
                        noplaneoverlapCheckSpriteUISprite.depth = 7;
                        noplaneoverlapCheckSpriteUISprite.atlas = bollInpuntCeckUISprit.atlas;
                        noplaneoverlapCheckSpriteUISprite.color = bollInpuntCeckUISprit.color;
                        noplaneoverlapCheckSpriteUISprite.height = 11;
                        noplaneoverlapCheckSpriteUISprite.width = 11;
                        noplaneoverlapCheckSpriteUISprite.type = UIBasicSprite.Type.Sliced;
                        noplaneoverlapCheckSpriteUISprite.spriteName = "MenuButtonBackgroundWhite";
                        //divideXBool1TogUITog.checkSprite = divideXBool1CheckSpriteUISprite;
                        noplaneoverlapTogUITog.activeSprite = noplaneoverlapCheckSpriteUISprite;
                        //divideXBool1TogUITog.onChange.Clear();
                        noplaneoverlapTogUIWig.ResizeCollider();
                        noplaneoverlapTogBoxCol.size = new Vector3((float)(noplaneoverlapTogBoxCol.size.x / 5), (float)(noplaneoverlapTogBoxCol.size.y / 5), noplaneoverlapTogBoxCol.size.z);

                        noplaneoverlapP = noplaneoverlap;
                        noplaneoverlapTogP = noplaneoverlapTog;


                        tespopwinP = tespopwin;

                    }
                }
            }
            return true;
        }

        private void OnToolCloze()
        {
            if(!justCreated)
            {
                tespopwinP.Destroy();
                doesIsPresent = false;
            }
            else
            {
                justCreated = false;
            }
        }

        private void OnToolRUN()
        {
            if(!justCreated)
            {
                List<GameObject> trackNodeObjects = G.Sys.LevelEditor_.SelectedNonTrackNodeObjects_;
                bool canDivideAny = false;
                foreach (GameObject gameObject in trackNodeObjects)
                {
                    if (DivideAction.isDividableObj(gameObject, lastXDivide, lastYDivide, lastZDivide))
                    {
                        canDivideAny = true;
                        break;
                    }
                }
                if (canDivideAny)
                {
                    int dividableObjectsCount = 0;
                    int truexdivide = lastXDivide;
                    int trueydivide = lastYDivide;
                    int truezdivide = lastZDivide;
                    if (!divideXBool1TogP.GetComponent<UIToggle>().value)
                    {
                        trueydivide = 0;
                    }
                    if (!divideYBool2TogP.GetComponent<UIToggle>().value)
                    {
                        truezdivide = 0;
                    }
                    if (!divideZBool3TogP.GetComponent<UIToggle>().value)
                    {
                        truexdivide = 0;
                    }
                    DivideAction action = new DivideAction(trackNodeObjects.ToArray(), truexdivide, trueydivide, truezdivide, KTL, lastXSpacing, lastYSpacing, lastZSpacing, CTP, NPO, out dividableObjectsCount);
                    //G.Sys.LevelEditor_.ClearSelectedList();
                    action.DivideObjects(true);
                    action.FinishAndAddToLevelEditorActions();
                    LevelEditorTool.PrintFormattedCountMessage("{0} object{1} were Divided.", dividableObjectsCount);
                }
                else
                {
                    LevelEditorTool.PrintErrorMessage("No Objects Were Divided");
                }

                OnToolCloze();
            }
        }

        private void OnTogKTLChange()
        {
            if (!IsGameObjNull(keeptexturelookTogP))
            {
                if(keeptexturelookTogP.GetComponent<UIToggle>().value)
                {
                    KTL = true;
                }
                else
                {
                    KTL = false;
                }
            }
        }

        private void OnTogSDNOEAChange()
        {
            if(!IsGameObjNull(tabSystemUIRootIntInput1P) && !IsGameObjNull(tabSystemUIRootIntInput2P) && !IsGameObjNull(tabSystemUIRootIntInput3P) && !IsGameObjNull(lockdividesTogP))
            {
                if(lockdividesTogP.GetComponent<UIToggle>().value)
                {
                    SDNOEA = true;
                    if(!justCreated)
                    {
                        SDNOEA_Set3();
                    }
                    
                }
                else
                {
                    SDNOEA = false;
                }
                
            }
        }

        private void OnTogCTPChange()
        {
            if (!IsGameObjNull(cubetoplanesTogP))
            {
                if (cubetoplanesTogP.GetComponent<UIToggle>().value)
                {
                    CTP = true;
                    noplaneoverlapP.SetActive(true);
                }
                else
                {
                    CTP = false;
                    noplaneoverlapP.SetActive(false);
                }
            }
        }

        private void OnTogNPOChange()
        {
            if (!IsGameObjNull(noplaneoverlapTogP))
            {
                if (noplaneoverlapTogP.GetComponent<UIToggle>().value)
                {
                    NPO = true;
                }
                else
                {
                    NPO = false;
                }
            }
        }

        private void OnFloatChangeSpacingX()
        {
            if (!IsGameObjNull(dividerspacingInputXP))
            {
                UIExNumericInput floatinputcomp = dividerspacingInputXP.GetComponent<UIExNumericInput>();
                lastXSpacing = floatinputcomp.Value_;
            }
        }

        private void OnFloatChangeSpacingY()
        {
            if (!IsGameObjNull(dividerspacingInputYP))
            {
                UIExNumericInput floatinputcomp = dividerspacingInputYP.GetComponent<UIExNumericInput>();
                lastYSpacing = floatinputcomp.Value_;
            }
        }

        private void OnFloatChangeSpacingZ()
        {
            if (!IsGameObjNull(dividerspacingInputZP))
            {
                UIExNumericInput floatinputcomp = dividerspacingInputZP.GetComponent<UIExNumericInput>();
                lastZSpacing = floatinputcomp.Value_;
            }
        }

        private void OnLeftXSpacingInputPress()
        {
            if (!IsGameObjNull(dividerspacingInputXP))
            {
                UIExNumericInput inputcomp = dividerspacingInputXP.GetComponent<UIExNumericInput>();
                UILabel uilabelinput = FOONIC(dividerspacingInputXP, "ValueLabel").GetComponent<UILabel>();
                float floatvalu = inputcomp.Value_;
                uilabelinput.text = (floatvalu - .1F) + "";
                inputcomp.value = (floatvalu - .1F) + "";
                inputcomp.Value_ = floatvalu - .1F;
                lastXSpacing = inputcomp.Value_;
            }
        }

        private void OnRightXSpacingInputPress()
        {
            if (!IsGameObjNull(dividerspacingInputXP))
            {
                UIExNumericInput inputcomp = dividerspacingInputXP.GetComponent<UIExNumericInput>();
                UILabel uilabelinput = FOONIC(dividerspacingInputXP, "ValueLabel").GetComponent<UILabel>();
                float floatvalu = inputcomp.Value_;
                uilabelinput.text = (floatvalu + .1F) + "";
                inputcomp.value = (floatvalu + .1F) + "";
                inputcomp.Value_ = floatvalu + .1F;
                lastXSpacing = inputcomp.Value_;
            }
        }

        private void OnLeftYSpacingInputPress()
        {
            if (!IsGameObjNull(dividerspacingInputXP))
            {
                UIExNumericInput inputcomp = dividerspacingInputYP.GetComponent<UIExNumericInput>();
                UILabel uilabelinput = FOONIC(dividerspacingInputYP, "ValueLabel").GetComponent<UILabel>();
                float floatvalu = inputcomp.Value_;
                uilabelinput.text = (floatvalu - .1F) + "";
                inputcomp.value = (floatvalu - .1F) + "";
                inputcomp.Value_ = floatvalu - .1F;
                lastYSpacing = inputcomp.Value_;
            }
        }

        private void OnRightYSpacingInputPress()
        {
            if (!IsGameObjNull(dividerspacingInputYP))
            {
                UIExNumericInput inputcomp = dividerspacingInputYP.GetComponent<UIExNumericInput>();
                UILabel uilabelinput = FOONIC(dividerspacingInputYP, "ValueLabel").GetComponent<UILabel>();
                float floatvalu = inputcomp.Value_;
                uilabelinput.text = (floatvalu + .1F) + "";
                inputcomp.value = (floatvalu + .1F) + "";
                inputcomp.Value_ = floatvalu + .1F;
                lastYSpacing = inputcomp.Value_;
            }
        }

        private void OnLeftZSpacingInputPress()
        {
            if (!IsGameObjNull(dividerspacingInputZP))
            {
                UIExNumericInput inputcomp = dividerspacingInputZP.GetComponent<UIExNumericInput>();
                UILabel uilabelinput = FOONIC(dividerspacingInputZP, "ValueLabel").GetComponent<UILabel>();
                float floatvalu = inputcomp.Value_;
                uilabelinput.text = (floatvalu - .1F) + "";
                inputcomp.value = (floatvalu - .1F) + "";
                inputcomp.Value_ = floatvalu - .1F;
                lastZSpacing = inputcomp.Value_;
            }
        }

        private void OnRightZSpacingInputPress()
        {
            if (!IsGameObjNull(dividerspacingInputYP))
            {
                UIExNumericInput inputcomp = dividerspacingInputZP.GetComponent<UIExNumericInput>();
                UILabel uilabelinput = FOONIC(dividerspacingInputZP, "ValueLabel").GetComponent<UILabel>();
                float floatvalu = inputcomp.Value_;
                uilabelinput.text = (floatvalu + .1F) + "";
                inputcomp.value = (floatvalu + .1F) + "";
                inputcomp.Value_ = floatvalu + .1F;
                lastZSpacing = inputcomp.Value_;
            }
        }

        private void OnTogChange1()
        {

            if (!IsGameObjNull(divideXBool1TogP) && !IsGameObjNull(tabSystemUIRootIntInput1P))
            {
                if (divideXBool1TogP.GetComponent<UIToggle>().value)
                {
                    FOONIC(tabSystemUIRootIntInput1P, "FacadeButton").GetComponent<UIButton>().isEnabled = true;
                    FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>().color = new Color(1, 1, 1);
                    FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(1, 1, 1);
                    FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>().gradientTop = new Color(1, 1, 1);
                }
                else
                {
                    FOONIC(tabSystemUIRootIntInput1P, "FacadeButton").GetComponent<UIButton>().isEnabled = false;
                    FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>().color = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>().gradientTop = new Color(.3F, .3F, .3F);
                }
            }
        }

        private void OnTogChange2()
        {

            if (!IsGameObjNull(divideYBool2TogP) && !IsGameObjNull(tabSystemUIRootIntInput2P))
            {
                if (divideYBool2TogP.GetComponent<UIToggle>().value)
                {
                    FOONIC(tabSystemUIRootIntInput2P, "FacadeButton").GetComponent<UIButton>().isEnabled = true;
                    FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>().color = new Color(1, 1, 1);
                    FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(1, 1, 1);
                    FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>().gradientTop = new Color(1, 1, 1);
                }
                else
                {
                    FOONIC(tabSystemUIRootIntInput2P, "FacadeButton").GetComponent<UIButton>().isEnabled = false;
                    FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>().color = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>().gradientTop = new Color(.3F, .3F, .3F);
                }
            }
        }

        private void OnTogChange3()
        {

            if (!IsGameObjNull(divideZBool3TogP) && !IsGameObjNull(tabSystemUIRootIntInput3P))
            {
                if (divideZBool3TogP.GetComponent<UIToggle>().value)
                {
                    FOONIC(tabSystemUIRootIntInput3P, "FacadeButton").GetComponent<UIButton>().isEnabled = true;
                    FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>().color = new Color(1, 1, 1);
                    FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(1, 1, 1);
                    FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>().gradientTop = new Color(1, 1, 1);
                }
                else
                {
                    FOONIC(tabSystemUIRootIntInput3P, "FacadeButton").GetComponent<UIButton>().isEnabled = false;
                    FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>().color = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>().gradientTop = new Color(.3F, .3F, .3F);
                }
            }
        }

        private void SDNOEA_Set1()
        {
            if (SDNOEA && !IsGameObjNull(tabSystemUIRootIntInput2P) && !IsGameObjNull(tabSystemUIRootIntInput3P))
            {
                UIExIntegerInput intinputcomp1 = tabSystemUIRootIntInput1P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput1 = FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>();
                UIExIntegerInput intinputcomp2 = tabSystemUIRootIntInput2P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput2 = FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>();
                UIExIntegerInput intinputcomp3 = tabSystemUIRootIntInput3P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput3 = FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>();
                int numval = intinputcomp1.Value_;
                string sval = intinputcomp1.value;
                lastXDivide = numval;
                lastZDivide = numval;
                uilabelintinput2.text = sval;
                uilabelintinput3.text = sval;
                intinputcomp2.value = numval + "";
                intinputcomp3.value = numval + "";
                intinputcomp2.Value_ = numval;
                intinputcomp3.Value_ = numval;
                if (!divideYBool2TogP.GetComponent<UIToggle>().value)
                {
                    FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>().color = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(.3F, .3F, .3F);
                }
                if (!divideZBool3TogP.GetComponent<UIToggle>().value)
                {
                    FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>().color = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(.3F, .3F, .3F);
                }
            }
        }

        private void SDNOEA_Set2()
        {
            if (SDNOEA && !IsGameObjNull(tabSystemUIRootIntInput1P) && !IsGameObjNull(tabSystemUIRootIntInput3P))
            {
                UIExIntegerInput intinputcomp1 = tabSystemUIRootIntInput1P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput1 = FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>();
                UIExIntegerInput intinputcomp2 = tabSystemUIRootIntInput2P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput2 = FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>();
                UIExIntegerInput intinputcomp3 = tabSystemUIRootIntInput3P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput3 = FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>();
                int numval = intinputcomp2.Value_;
                string sval = intinputcomp2.value;
                lastXDivide = numval;
                lastYDivide = numval;
                uilabelintinput1.text = sval;
                uilabelintinput3.text = sval;
                intinputcomp1.value = numval + "";
                intinputcomp3.value = numval + "";
                intinputcomp1.Value_ = numval;
                intinputcomp3.Value_ = numval;
                if (!divideXBool1TogP.GetComponent<UIToggle>().value)
                {
                    FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>().color = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(.3F, .3F, .3F);
                }
                if (!divideZBool3TogP.GetComponent<UIToggle>().value)
                {
                    FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>().color = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(.3F, .3F, .3F);
                }
            }
        }

        private void SDNOEA_Set3()
        {
            if (SDNOEA && !IsGameObjNull(tabSystemUIRootIntInput1P) && !IsGameObjNull(tabSystemUIRootIntInput2P))
            {
                UIExIntegerInput intinputcomp1 = tabSystemUIRootIntInput1P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput1 = FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>();
                UIExIntegerInput intinputcomp2 = tabSystemUIRootIntInput2P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput2 = FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>();
                UIExIntegerInput intinputcomp3 = tabSystemUIRootIntInput3P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput3 = FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>();
                int numval = intinputcomp3.Value_;
                string sval = intinputcomp3.value;
                lastYDivide = numval;
                lastZDivide = numval;
                uilabelintinput2.text = sval;
                uilabelintinput1.text = sval;
                intinputcomp2.value = numval + "";
                intinputcomp1.value = numval + "";
                intinputcomp2.Value_ = numval;
                intinputcomp1.Value_ = numval;
                if (!divideYBool2TogP.GetComponent<UIToggle>().value)
                {
                    FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>().color = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(.3F, .3F, .3F);
                }
                if (!divideXBool1TogP.GetComponent<UIToggle>().value)
                {
                    FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>().color = new Color(.3F, .3F, .3F);
                    FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>().mColor = new Color(.3F, .3F, .3F);
                }
            }
        }

        private void OnIntInputChange1()
        {
            if ((UnityEngine.Object)tabSystemUIRootIntInput1P != (UnityEngine.Object)null)
            {
                UIExIntegerInput intinputcomp = tabSystemUIRootIntInput1P.GetComponent<UIExIntegerInput>();
                lastYDivide = intinputcomp.Value_;
                if (intinputcomp.Value_ < 0)
                {
                    intinputcomp.value = "0";
                    lastYDivide = 0;
                }
                
                SDNOEA_Set1();
            }
        }

        private void OnIntInputChange2()
        {
            if ((UnityEngine.Object)tabSystemUIRootIntInput2P != (UnityEngine.Object)null)
            {
                UIExIntegerInput intinputcomp = tabSystemUIRootIntInput2P.GetComponent<UIExIntegerInput>();
                lastZDivide = intinputcomp.Value_;
                if (intinputcomp.Value_ < 0)
                {
                    intinputcomp.value = "0";
                    lastZDivide = 0;
                }
                SDNOEA_Set2();
            }
        }

        private void OnIntInputChange3()
        {
            if ((UnityEngine.Object)tabSystemUIRootIntInput3P != (UnityEngine.Object)null)
            {
                UIExIntegerInput intinputcomp = tabSystemUIRootIntInput3P.GetComponent<UIExIntegerInput>();
                lastXDivide = intinputcomp.Value_;
                if (intinputcomp.Value_ < 0)
                {
                    intinputcomp.value = "0";
                    lastXDivide = 0;
                }
                SDNOEA_Set3();
            }
        }

        private void OnLeft1Press()
        {
            if ((UnityEngine.Object)tabSystemUIRootIntInput1P != (UnityEngine.Object)null)
            {
                UIExIntegerInput intinputcomp = tabSystemUIRootIntInput1P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput = FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>();
                if ((intinputcomp.Value_ - 1) >= 0)
                {
                    int intvalu = intinputcomp.Value_;
                    uilabelintinput.text = (intvalu - 1) + "";
                    intinputcomp.value = (intvalu - 1) + "";
                    intinputcomp.Value_ = intvalu - 1;
                    lastYDivide = intinputcomp.Value_;
                    SDNOEA_Set1();
                }
                
            }
        }

        private void OnRight1Press()
        {
            if ((UnityEngine.Object)tabSystemUIRootIntInput1P != (UnityEngine.Object)null)
            {
                UIExIntegerInput intinputcomp = tabSystemUIRootIntInput1P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput = FOONIC(tabSystemUIRootIntInput1P, "ValueLabel").GetComponent<UILabel>();
                int intvalu = intinputcomp.Value_;
                uilabelintinput.text = (intvalu + 1) + "";
                intinputcomp.value = (intvalu + 1) + "";
                intinputcomp.Value_ = intvalu + 1;
                lastYDivide = intinputcomp.Value_;
                SDNOEA_Set1();
            }
        }

        private void OnLeft2Press()
        {
            if ((UnityEngine.Object)tabSystemUIRootIntInput2P != (UnityEngine.Object)null)
            {
                UIExIntegerInput intinputcomp = tabSystemUIRootIntInput2P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput = FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>();
                if ((intinputcomp.Value_ - 1) >= 0)
                {
                    int intvalu = intinputcomp.Value_;
                    uilabelintinput.text = (intvalu - 1) + "";
                    intinputcomp.value = (intvalu - 1) + "";
                    intinputcomp.Value_ = intvalu - 1;
                    lastZDivide = intinputcomp.Value_;
                    SDNOEA_Set2();
                }
                
            }
        }

        private void OnRight2Press()
        {
            if ((UnityEngine.Object)tabSystemUIRootIntInput2P != (UnityEngine.Object)null)
            {
                UIExIntegerInput intinputcomp = tabSystemUIRootIntInput2P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput = FOONIC(tabSystemUIRootIntInput2P, "ValueLabel").GetComponent<UILabel>();
                int intvalu = intinputcomp.Value_;
                uilabelintinput.text = (intvalu + 1) + "";
                intinputcomp.value = (intvalu + 1) + "";
                intinputcomp.Value_ = intvalu + 1;
                lastZDivide = intinputcomp.Value_;
                SDNOEA_Set2();
            }
        }

        private void OnLeft3Press()
        {
            if ((UnityEngine.Object)tabSystemUIRootIntInput3P != (UnityEngine.Object)null)
            {
                UIExIntegerInput intinputcomp = tabSystemUIRootIntInput3P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput = FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>();
                if ((intinputcomp.Value_ - 1) >= 0)
                {
                    int intvalu = intinputcomp.Value_;
                    uilabelintinput.text = (intvalu - 1) + "";
                    intinputcomp.value = (intvalu - 1) + "";
                    intinputcomp.Value_ = intvalu - 1;
                    lastXDivide = intinputcomp.Value_;
                    SDNOEA_Set3();
                }
            }
        }

        private void OnRight3Press()
        {
            if ((UnityEngine.Object)tabSystemUIRootIntInput3P != (UnityEngine.Object)null)
            {
                UIExIntegerInput intinputcomp = tabSystemUIRootIntInput3P.GetComponent<UIExIntegerInput>();
                UILabel uilabelintinput = FOONIC(tabSystemUIRootIntInput3P, "ValueLabel").GetComponent<UILabel>();
                int intvalu = intinputcomp.Value_;
                uilabelintinput.text = (intvalu + 1) + "";
                intinputcomp.value = (intvalu + 1) + "";
                intinputcomp.Value_ = intvalu + 1;
                lastXDivide = intinputcomp.Value_;
                SDNOEA_Set3();
            }
        }

        private bool IsGameObjNull(GameObject objec)
        {
            return ((UnityEngine.Object)objec == (UnityEngine.Object)null);
        }

        private bool IsGameCompNull<T>(T objec)
        {
            return (objec == null);
        }

        private GameObject FOONIC(GameObject parentObjec, string name) //FindObjectOfNameInChildren
        {
            if((UnityEngine.Object)parentObjec != (UnityEngine.Object)null)
            {
                foreach (GameObject gameObject in parentObjec.gameObject.GetChildren())
                {
                    if (gameObject.name == name)
                    {
                        return gameObject;
                        break;
                    }
                }
            }
            return null;
        }

        private GameObject FOONIC(GameObject[] childObjecRay, string name) //For use with main object list
        {
            foreach (GameObject gameObject in childObjecRay)
            {
                if (gameObject.transform.parent == null && gameObject.name == name)
                {
                    return gameObject;
                    break;
                }
            }
            return null;
        }

        private void printPropsAndFields(UIExNumericInput comp)
        {
            Mod.Logger.Info("<PROPS>");
            Mod.Logger.Info("cursorPosition: " + comp.cursorPosition);
            Mod.Logger.Info("defaultText: " + comp.defaultText);
            Mod.Logger.Info("Dragging_: " + comp.Dragging_);
            Mod.Logger.Info("enabled: " + comp.enabled);
            Mod.Logger.Info("ExpStepSize_: " + comp.ExpStepSize_);
            Mod.Logger.Info("hideFlags: " + comp.hideFlags);
            Mod.Logger.Info("isSelected: " + comp.isSelected);
            Mod.Logger.Info("Max_: " + comp.Max_);
            Mod.Logger.Info("Min_: " + comp.Min_);
            //Mod.Logger.Info("enabled: " + comp.selected);
            Mod.Logger.Info("selectionEnd: " + comp.selectionEnd);
            Mod.Logger.Info("selectionStart: " + comp.selectionStart);
            Mod.Logger.Info("StepSize_: " + comp.StepSize_);
            Mod.Logger.Info("tag: " + comp.tag);
            //Mod.Logger.Info("enabled: " + comp.text);
            Mod.Logger.Info("useGUILayout: " + comp.useGUILayout);
            Mod.Logger.Info("value: " + comp.value);
            Mod.Logger.Info("Value_: " + comp.Value_);
            Mod.Logger.Info("<FIELDS>");
            Mod.Logger.Info("activeTextColor: " + comp.activeTextColor);
            Mod.Logger.Info("caretColor: " + comp.caretColor);
            Mod.Logger.Info("characterLimit: " + comp.characterLimit);
            Mod.Logger.Info("customString_: " + comp.customString_);
            Mod.Logger.Info("dragDelta_: " + comp.dragDelta_);
            Mod.Logger.Info("expressionTree_: " + comp.expressionTree_);
            Mod.Logger.Info("expStepSize_: " + comp.expStepSize_);
            Mod.Logger.Info("facadeButton_: " + comp.facadeButton_);
            Mod.Logger.Info("hideInput: " + comp.hideInput);
            Mod.Logger.Info("ignoreNextInputOnChangeEvent_: " + comp.ignoreNextInputOnChangeEvent_);
            Mod.Logger.Info("inactiveColor_: " + comp.inactiveColor_);
            Mod.Logger.Info("inputType: " + comp.inputType);
            Mod.Logger.Info("keyboardType: " + comp.keyboardType);
            Mod.Logger.Info("label: " + comp.label);
            Mod.Logger.Info("max_: " + comp.max_);
            Mod.Logger.Info("min_: " + comp.min_);
            Mod.Logger.Info("onChange: " + comp.onChange);
            Mod.Logger.Info("onChange_: " + comp.onChange_);
            Mod.Logger.Info("onFinish_: " + comp.onFinish_);
            Mod.Logger.Info("onReturnKey: " + comp.onReturnKey);
            Mod.Logger.Info("onStart_: " + comp.onStart_);
            Mod.Logger.Info("onSubmit: " + comp.onSubmit);
            Mod.Logger.Info("onValidate: " + comp.onValidate);
            Mod.Logger.Info("openedKeyboard_: " + comp.openedKeyboard_);
            Mod.Logger.Info("previousValue_: " + comp.previousValue_);
            Mod.Logger.Info("savedAs: " + comp.savedAs);
            Mod.Logger.Info("selectAllTextOnFocus: " + comp.selectAllTextOnFocus);
            Mod.Logger.Info("selectionColor: " + comp.selectionColor);
            Mod.Logger.Info("selectOnSubmit_: " + comp.selectOnSubmit_);
            Mod.Logger.Info("selectOnTab: " + comp.selectOnTab);
            Mod.Logger.Info("stepSize_: " + comp.stepSize_);
            Mod.Logger.Info("submitted_: " + comp.submitted_);
            Mod.Logger.Info("touchID_: " + comp.touchID_);
            Mod.Logger.Info("validation: " + comp.validation);
            Mod.Logger.Info("value_: " + comp.value_);
            Mod.Logger.Info("<FIELDSM>");
            Mod.Logger.Info("mBlankTex: " + comp.mBlankTex);
            Mod.Logger.Info("mCached: " + comp.mCached);
            Mod.Logger.Info("mCam: " + comp.mCam);
            Mod.Logger.Info("mCaret: " + comp.mCaret);
            Mod.Logger.Info("mDefaultColor: " + comp.mDefaultColor);
            Mod.Logger.Info("mDefaultText: " + comp.mDefaultText);
            Mod.Logger.Info("mDoInit: " + comp.mDoInit);
            Mod.Logger.Info("mHighlight: " + comp.mHighlight);
            Mod.Logger.Info("mLastAlpha: " + comp.mLastAlpha);
            Mod.Logger.Info("mLoadSavedValue: " + comp.mLoadSavedValue);
            Mod.Logger.Info("mNextBlink: " + comp.mNextBlink);
            Mod.Logger.Info("mOnGUI: " + comp.mOnGUI);
            Mod.Logger.Info("mPivot: " + comp.mPivot);
            Mod.Logger.Info("mPosition: " + comp.mPosition);
            Mod.Logger.Info("mSelectionEnd: " + comp.mSelectionEnd);
            Mod.Logger.Info("mSelectionStart: " + comp.mSelectionStart);
            Mod.Logger.Info("mSelectMe: " + comp.mSelectMe);
            Mod.Logger.Info("mSelectTime: " + comp.mSelectTime);
            Mod.Logger.Info("mValue: " + comp.mValue);
        }

        private void printPropsAndFields(UIToggle comp)
        {
            Mod.Logger.Info("<PROPS>");
            Mod.Logger.Info("enabled: " + comp.enabled);
            Mod.Logger.Info("hideFlags: " + comp.hideFlags);
            //Mod.Logger.Info("enabled: " + comp.isChecked);
            Mod.Logger.Info("tag: " + comp.tag);
            Mod.Logger.Info("useGUILayout: " + comp.useGUILayout);
            Mod.Logger.Info("value: " + comp.value);
            Mod.Logger.Info("<FIELDS>");
            Mod.Logger.Info("activeAnimation: " + comp.activeAnimation);
            Mod.Logger.Info("activeSprite: " + comp.activeSprite);
            Mod.Logger.Info("animator: " + comp.animator);
            Mod.Logger.Info("checkAnimation: " + comp.checkAnimation);
            Mod.Logger.Info("checkSprite: " + comp.checkSprite);
            Mod.Logger.Info("eventReceiver: " + comp.eventReceiver);
            Mod.Logger.Info("functionName: " + comp.functionName);
            Mod.Logger.Info("group: " + comp.group);
            Mod.Logger.Info("instantTween: " + comp.instantTween);
            Mod.Logger.Info("mIsActive: " + comp.mIsActive);
            Mod.Logger.Info("mStarted: " + comp.mStarted);
            Mod.Logger.Info("onChange: " + comp.onChange);
            Mod.Logger.Info("optionCanBeNone: " + comp.optionCanBeNone);
            Mod.Logger.Info("startsActive: " + comp.startsActive);
            Mod.Logger.Info("startsChecked: " + comp.startsChecked);
            Mod.Logger.Info("validator: " + comp.validator);
        }

        private void printPropsAndFields(UIExDisableOnSelect comp)
        {
            Mod.Logger.Info("<PROPS>");
            Mod.Logger.Info("enabled: " + comp.enabled);
            Mod.Logger.Info("hideFlags: " + comp.hideFlags);
            Mod.Logger.Info("isActiveAndEnabled: " + comp.isActiveAndEnabled);
            Mod.Logger.Info("tag: " + comp.tag);
            Mod.Logger.Info("useGUILayout: " + comp.useGUILayout);
            Mod.Logger.Info("<FIELDS>");
            Mod.Logger.Info("deactivateOnSelect_: " + comp.deactivateOnSelect_);
            Mod.Logger.Info("disableButtonsOnSelect_: " + comp.disableButtonsOnSelect_);
            Mod.Logger.Info("disableOnSelect_: " + comp.disableOnSelect_);
            
        }

            private void printPropsAndFields(UILabel comp)
        {
            Mod.Logger.Info("<PROPS>");
            Mod.Logger.Info("alignment: " + comp.alignment);
            Mod.Logger.Info("alpha: " + comp.alpha);
            Mod.Logger.Info("ambigiousFont: " + comp.ambigiousFont);
            Mod.Logger.Info("applyGradient: " + comp.applyGradient);
            Mod.Logger.Info("bitmapFont: " + comp.bitmapFont);
            Mod.Logger.Info("border: " + comp.border);
            Mod.Logger.Info("color: " + comp.color);
            Mod.Logger.Info("depth: " + comp.depth);
            Mod.Logger.Info("drawRegion: " + comp.drawRegion);
            Mod.Logger.Info("effectColor: " + comp.effectColor);
            Mod.Logger.Info("effectDistance: " + comp.effectDistance);
            Mod.Logger.Info("effectStyle: " + comp.effectStyle);
            Mod.Logger.Info("enabled: " + comp.enabled);
            Mod.Logger.Info("floatSpacingX: " + comp.floatSpacingX);
            Mod.Logger.Info("floatSpacingY: " + comp.floatSpacingY);
            //Mod.Logger.Info("enabled: " + comp.font);
            Mod.Logger.Info("fontSize: " + comp.fontSize);
            Mod.Logger.Info("fontStyle: " + comp.fontStyle);
            Mod.Logger.Info("gradientBottom: " + comp.gradientBottom);
            Mod.Logger.Info("gradientTop: " + comp.gradientTop);
            Mod.Logger.Info("height: " + comp.height);
            Mod.Logger.Info("hideFlags: " + comp.hideFlags);
            //Mod.Logger.Info("enabled: " + comp.lineHeight);
            //Mod.Logger.Info("enabled: " + comp.lineWidth);
            Mod.Logger.Info("mainTexture: " + comp.mainTexture);
            Mod.Logger.Info("material: " + comp.material);
            Mod.Logger.Info("maxLineCount: " + comp.maxLineCount);
            Mod.Logger.Info("multiLine: " + comp.multiLine);
            Mod.Logger.Info("onRender: " + comp.onRender);
            Mod.Logger.Info("overflowMethod: " + comp.overflowMethod);
            Mod.Logger.Info("pivot: " + comp.pivot);
            Mod.Logger.Info("rawPivot: " + comp.rawPivot);
            Mod.Logger.Info("shader: " + comp.shader);
            Mod.Logger.Info("shouldBeProcessed: " + comp.shouldBeProcessed);
            //Mod.Logger.Info("enabled: " + comp.shrinkToFit);
            Mod.Logger.Info("spacingX: " + comp.spacingX);
            Mod.Logger.Info("spacingY: " + comp.spacingY);
            Mod.Logger.Info("supportEncoding: " + comp.supportEncoding);
            Mod.Logger.Info("symbolStyle: " + comp.symbolStyle);
            Mod.Logger.Info("tag: " + comp.tag);
            Mod.Logger.Info("text: " + comp.text);
            Mod.Logger.Info("trueTypeFont: " + comp.trueTypeFont);
            Mod.Logger.Info("useFloatSpacing: " + comp.useFloatSpacing);
            Mod.Logger.Info("useGUILayout: " + comp.useGUILayout);
            Mod.Logger.Info("width: " + comp.width);
            Mod.Logger.Info("<FIELDS>");
            Mod.Logger.Info("aspectRatio: " + comp.aspectRatio);
            Mod.Logger.Info("autoResizeBoxCollider: " + comp.autoResizeBoxCollider);
            Mod.Logger.Info("bottomAnchor: " + comp.bottomAnchor);
            Mod.Logger.Info("drawCall: " + comp.drawCall);
            Mod.Logger.Info("fillGeometry: " + comp.fillGeometry);
            Mod.Logger.Info("finalAlpha: " + comp.finalAlpha);
            Mod.Logger.Info("geometry: " + comp.geometry);
            Mod.Logger.Info("hideIfOffScreen: " + comp.hideIfOffScreen);
            Mod.Logger.Info("hitCheck: " + comp.hitCheck);
            Mod.Logger.Info("keepAspectRatio: " + comp.keepAspectRatio);
            Mod.Logger.Info("keepCrispWhenShrunk: " + comp.keepCrispWhenShrunk);
            Mod.Logger.Info("leftAnchor: " + comp.leftAnchor);
            Mod.Logger.Info("onChange: " + comp.onChange);
            Mod.Logger.Info("onPostFill: " + comp.onPostFill);
            Mod.Logger.Info("panel: " + comp.panel);
            Mod.Logger.Info("rightAnchor: " + comp.rightAnchor);
            Mod.Logger.Info("topAnchor: " + comp.topAnchor);
            Mod.Logger.Info("updateAnchors: " + comp.updateAnchors);
            Mod.Logger.Info("<FIELDSM>");
            Mod.Logger.Info("mActiveTTF: " + comp.mActiveTTF);
            Mod.Logger.Info("mAlignment: " + comp.mAlignment);
            Mod.Logger.Info("mAlphaFrameID: " + comp.mAlphaFrameID);
            Mod.Logger.Info("mAnchorsCached: " + comp.mAnchorsCached);
            Mod.Logger.Info("mApplyGradient: " + comp.mApplyGradient);
            Mod.Logger.Info("mCalculatedSize: " + comp.mCalculatedSize);
            Mod.Logger.Info("mCam: " + comp.mCam);
            Mod.Logger.Info("mChanged: " + comp.mChanged);
            Mod.Logger.Info("mChildren: " + comp.mChildren);
            Mod.Logger.Info("mColor: " + comp.mColor);
            Mod.Logger.Info("mCorners: " + comp.mCorners);
            Mod.Logger.Info("mDensity: " + comp.mDensity);
            Mod.Logger.Info("mDepth: " + comp.mDepth);
            Mod.Logger.Info("mDrawRegion: " + comp.mDrawRegion);
            Mod.Logger.Info("mEffectColor: " + comp.mEffectColor);
            Mod.Logger.Info("mEffectDistance: " + comp.mEffectDistance);
            Mod.Logger.Info("mEffectStyle: " + comp.mEffectStyle);
            Mod.Logger.Info("mEncoding: " + comp.mEncoding);
            Mod.Logger.Info("mFinalFontSize: " + comp.mFinalFontSize);
            Mod.Logger.Info("mFloatSpacingX: " + comp.mFloatSpacingX);
            Mod.Logger.Info("mFloatSpacingY: " + comp.mFloatSpacingY);
            Mod.Logger.Info("mFont: " + comp.mFont);
            Mod.Logger.Info("mFontSize: " + comp.mFontSize);
            Mod.Logger.Info("mFontStyle: " + comp.mFontStyle);
            Mod.Logger.Info("mGo: " + comp.mGo);
            Mod.Logger.Info("mGradientBottom: " + comp.mGradientBottom);
            Mod.Logger.Info("mGradientTop: " + comp.mGradientTop);
            Mod.Logger.Info("mHeight: " + comp.mHeight);
            Mod.Logger.Info("mIsInFront: " + comp.mIsInFront);
            Mod.Logger.Info("mIsVisibleByAlpha: " + comp.mIsVisibleByAlpha);
            Mod.Logger.Info("mIsVisibleByPanel: " + comp.mIsVisibleByPanel);
            Mod.Logger.Info("mLastAlpha: " + comp.mLastAlpha);
            Mod.Logger.Info("mLastHeight: " + comp.mLastHeight);
            Mod.Logger.Info("mLastWidth: " + comp.mLastWidth);
            Mod.Logger.Info("mLineWidth: " + comp.mLineWidth);
            Mod.Logger.Info("mLocalToPanel: " + comp.mLocalToPanel);
            Mod.Logger.Info("mMaterial: " + comp.mMaterial);
            Mod.Logger.Info("mMatrixFrame: " + comp.mMatrixFrame);
            Mod.Logger.Info("mMaxLineCount: " + comp.mMaxLineCount);
            Mod.Logger.Info("mMaxLineHeight: " + comp.mMaxLineHeight);
            Mod.Logger.Info("mMaxLineWidth: " + comp.mMaxLineWidth);
            Mod.Logger.Info("mMoved: " + comp.mMoved);
            Mod.Logger.Info("mMultiline: " + comp.mMultiline);
            Mod.Logger.Info("mOldV0: " + comp.mOldV0);
            Mod.Logger.Info("mOldV1: " + comp.mOldV1);
            Mod.Logger.Info("mOnRender: " + comp.mOnRender);
            Mod.Logger.Info("mOverflow: " + comp.mOverflow);
            Mod.Logger.Info("mOverflowEllipsis: " + comp.mOverflowEllipsis);
            Mod.Logger.Info("mParent: " + comp.mParent);
            Mod.Logger.Info("mParentFound: " + comp.mParentFound);
            Mod.Logger.Info("mPivot: " + comp.mPivot);
            Mod.Logger.Info("mPlayMode: " + comp.mPlayMode);
            Mod.Logger.Info("mPremultiply: " + comp.mPremultiply);
            Mod.Logger.Info("mProcessedText: " + comp.mProcessedText);
            Mod.Logger.Info("mRoot: " + comp.mRoot);
            Mod.Logger.Info("mRootSet: " + comp.mRootSet);
            Mod.Logger.Info("mScale: " + comp.mScale);
            Mod.Logger.Info("mShouldBeProcessed: " + comp.mShouldBeProcessed);
            Mod.Logger.Info("mShrinkToFit: " + comp.mShrinkToFit);
            Mod.Logger.Info("mSpacingX: " + comp.mSpacingX);
            Mod.Logger.Info("mSpacingY: " + comp.mSpacingY);
            Mod.Logger.Info("mStarted: " + comp.mStarted);
            Mod.Logger.Info("mSymbols: " + comp.mSymbols);
            Mod.Logger.Info("mText: " + comp.mText);
            Mod.Logger.Info("mTrans: " + comp.mTrans);
            Mod.Logger.Info("mTrueTypeFont: " + comp.mTrueTypeFont);
            Mod.Logger.Info("mUpdateAnchors: " + comp.mUpdateAnchors);
            Mod.Logger.Info("mUpdateFrame: " + comp.mUpdateFrame);
            Mod.Logger.Info("mUseFloatSpacing: " + comp.mUseFloatSpacing);
            Mod.Logger.Info("mWidth: " + comp.mWidth);

        }


            private void printPropsAndFields(UIButton comp)
        {
            Mod.Logger.Info("<PROPS>");
            Mod.Logger.Info("enabled: " + comp.enabled);
            Mod.Logger.Info("hideFlags: " + comp.hideFlags);
            Mod.Logger.Info("isEnabled: " + comp.isEnabled);
            Mod.Logger.Info("normalSprite: " + comp.normalSprite);
            Mod.Logger.Info("normalSprite2D: " + comp.normalSprite2D);
            Mod.Logger.Info("state: " + comp.state);
            Mod.Logger.Info("tag: " + comp.tag);
            Mod.Logger.Info("useGUILayout: " + comp.useGUILayout);
            Mod.Logger.Info("<FIELDS>");
            Mod.Logger.Info("disabledColor: " + comp.disabledColor);
            Mod.Logger.Info("disabledSprite: " + comp.disabledSprite);
            Mod.Logger.Info("disabledSprite2D: " + comp.disabledSprite2D);
            Mod.Logger.Info("dragHighlight: " + comp.dragHighlight);
            Mod.Logger.Info("duration: " + comp.duration);
            Mod.Logger.Info("hover: " + comp.hover);
            Mod.Logger.Info("hoverSprite: " + comp.hoverSprite);
            Mod.Logger.Info("hoverSprite2D: " + comp.hoverSprite2D);
            Mod.Logger.Info("onClick: " + comp.onClick);
            Mod.Logger.Info("pixelSnap: " + comp.pixelSnap);
            Mod.Logger.Info("pressed: " + comp.pressed);
            Mod.Logger.Info("pressedSprite: " + comp.pressedSprite);
            Mod.Logger.Info("pressedSprite2D: " + comp.pressedSprite2D);
            Mod.Logger.Info("tweenTarget: " + comp.tweenTarget);
            Mod.Logger.Info("<FIELDSM>");
            Mod.Logger.Info("mDefaultColor: " + comp.mDefaultColor);
            Mod.Logger.Info("mInitDone: " + comp.mInitDone);
            Mod.Logger.Info("mNormalSprite: " + comp.mNormalSprite);
            Mod.Logger.Info("mNormalSprite2D: " + comp.mNormalSprite2D);
            Mod.Logger.Info("mSprite: " + comp.mSprite);
            Mod.Logger.Info("mSprite2D: " + comp.mSprite2D);
            Mod.Logger.Info("mStartingColor: " + comp.mStartingColor);
            Mod.Logger.Info("mState: " + comp.mState);
            Mod.Logger.Info("mWidget: " + comp.mWidget);
        }

            private void printPropsAndFields(UIExIntegerInput comp)
        {
            Mod.Logger.Info("<PROPS>");
            Mod.Logger.Info("cursorPosition: " + comp.cursorPosition);
            Mod.Logger.Info("defaultText: " + comp.defaultText);
            Mod.Logger.Info("Dragging_: " + comp.Dragging_);
            Mod.Logger.Info("enabled: " + comp.enabled);
            Mod.Logger.Info("ExpStepSize_: " + comp.ExpStepSize_);
            Mod.Logger.Info("hideFlags: " + comp.hideFlags);
            Mod.Logger.Info("isSelected: " + comp.isSelected);
            Mod.Logger.Info("Max_: " + comp.Max_);
            Mod.Logger.Info("Min_: " + comp.Min_);
            //Mod.Logger.Info("alpha: " + comp.selected);
            Mod.Logger.Info("selectionEnd: " + comp.selectionEnd);
            Mod.Logger.Info("selectionStart: " + comp.selectionStart);
            Mod.Logger.Info("StepSize_: " + comp.StepSize_);
            Mod.Logger.Info("tag: " + comp.tag);
            //Mod.Logger.Info("alpha: " + comp.text);
            Mod.Logger.Info("useGUILayout: " + comp.useGUILayout);
            Mod.Logger.Info("value: " + comp.value);
            Mod.Logger.Info("Value_: " + comp.Value_);
            Mod.Logger.Info("<FIELDS>");
            Mod.Logger.Info("activeTextColor: " + comp.activeTextColor);
            Mod.Logger.Info("caretColor: " + comp.caretColor);
            Mod.Logger.Info("characterLimit: " + comp.characterLimit);
            Mod.Logger.Info("customString_: " + comp.customString_);
            Mod.Logger.Info("dragDelta_: " + comp.dragDelta_);
            Mod.Logger.Info("expressionTree_: " + comp.expressionTree_);
            Mod.Logger.Info("expStepSize_: " + comp.expStepSize_);
            Mod.Logger.Info("facadeButton_: " + comp.facadeButton_);
            Mod.Logger.Info("hideInput: " + comp.hideInput);
            Mod.Logger.Info("ignoreNextInputOnChangeEvent_: " + comp.ignoreNextInputOnChangeEvent_);
            Mod.Logger.Info("inactiveColor_: " + comp.inactiveColor_);
            Mod.Logger.Info("inputType: " + comp.inputType);
            Mod.Logger.Info("keyboardType: " + comp.keyboardType);
            Mod.Logger.Info("label: " + comp.label);
            Mod.Logger.Info("max_: " + comp.max_);
            Mod.Logger.Info("min_: " + comp.min_);
            Mod.Logger.Info("onChange: " + comp.onChange);
            Mod.Logger.Info("onChange_: " + comp.onChange_);
            Mod.Logger.Info("onFinish_: " + comp.onFinish_);
            Mod.Logger.Info("onReturnKey: " + comp.onReturnKey);
            Mod.Logger.Info("onStart_: " + comp.onStart_);
            Mod.Logger.Info("onSubmit: " + comp.onSubmit);
            Mod.Logger.Info("onValidate: " + comp.onValidate);
            Mod.Logger.Info("openedKeyboard_: " + comp.openedKeyboard_);
            Mod.Logger.Info("previousValue_: " + comp.previousValue_);
            Mod.Logger.Info("savedAs: " + comp.savedAs);
            Mod.Logger.Info("selectAllTextOnFocus: " + comp.selectAllTextOnFocus);
            Mod.Logger.Info("selectionColor: " + comp.selectionColor);
            Mod.Logger.Info("selectOnSubmit_: " + comp.selectOnSubmit_);
            Mod.Logger.Info("selectOnTab: " + comp.selectOnTab);
            Mod.Logger.Info("stepSize_: " + comp.stepSize_);
            Mod.Logger.Info("submitted_: " + comp.submitted_);
            Mod.Logger.Info("touchID_: " + comp.touchID_);
            Mod.Logger.Info("validation: " + comp.validation);
            Mod.Logger.Info("value_: " + comp.value_);
            Mod.Logger.Info("<FIELDSM>");
            Mod.Logger.Info("mBlankTex: " + comp.mBlankTex);
            Mod.Logger.Info("mCached: " + comp.mCached);
            Mod.Logger.Info("mCam: " + comp.mCam);
            Mod.Logger.Info("mCaret: " + comp.mCaret);
            Mod.Logger.Info("mDefaultColor: " + comp.mDefaultColor);
            Mod.Logger.Info("mDefaultText: " + comp.mDefaultText);
            Mod.Logger.Info("mDoInit: " + comp.mDoInit);
            Mod.Logger.Info("mHighlight: " + comp.mHighlight);
            Mod.Logger.Info("mLastAlpha: " + comp.mLastAlpha);
            Mod.Logger.Info("mLoadSavedValue: " + comp.mLoadSavedValue);
            Mod.Logger.Info("mNextBlink: " + comp.mNextBlink);
            Mod.Logger.Info("mOnGUI: " + comp.mOnGUI);
            Mod.Logger.Info("mPivot: " + comp.mPivot);
            Mod.Logger.Info("mPosition: " + comp.mPosition);
            Mod.Logger.Info("mSelectionEnd: " + comp.mSelectionEnd);
            Mod.Logger.Info("mSelectionStart: " + comp.mSelectionStart);
            Mod.Logger.Info("mSelectMe: " + comp.mSelectMe);
            Mod.Logger.Info("mSelectTime: " + comp.mSelectTime);
            Mod.Logger.Info("mValue: " + comp.mValue);

        }

            private void printPropsAndFields(UIWidget comp)
        {
            Mod.Logger.Info("<PROPS>");
            Mod.Logger.Info("alpha: " + comp.alpha);
            Mod.Logger.Info("color: " + comp.color);
            Mod.Logger.Info("depth: " + comp.depth);
            Mod.Logger.Info("drawRegion: " + comp.drawRegion);
            Mod.Logger.Info("enabled: " + comp.enabled);
            Mod.Logger.Info("height: " + comp.height);
            Mod.Logger.Info("hideFlags: " + comp.hideFlags);
            Mod.Logger.Info("mainTexture: " + comp.mainTexture);
            Mod.Logger.Info("name: " + comp.name);
            Mod.Logger.Info("onRender: " + comp.onRender);
            Mod.Logger.Info("pivot: " + comp.pivot);
            Mod.Logger.Info("rawPivot: " + comp.rawPivot);
            Mod.Logger.Info("shader: " + comp.shader);
            Mod.Logger.Info("tag: " + comp.tag);
            Mod.Logger.Info("useGUILayout: " + comp.useGUILayout);
            Mod.Logger.Info("width: " + comp.width);
            Mod.Logger.Info("<FIELDS>");
            Mod.Logger.Info("aspectRatio: " + comp.aspectRatio);
            Mod.Logger.Info("autoResizeBoxCollider: " + comp.autoResizeBoxCollider);
            Mod.Logger.Info("bottomAnchor: " + comp.bottomAnchor);
            Mod.Logger.Info("drawCall: " + comp.drawCall);
            Mod.Logger.Info("fillGeometry: " + comp.fillGeometry);
            Mod.Logger.Info("finalAlpha: " + comp.finalAlpha);
            Mod.Logger.Info("geometry: " + comp.geometry);
            Mod.Logger.Info("hideIfOffScreen: " + comp.hideIfOffScreen);
            Mod.Logger.Info("hitCheck: " + comp.hitCheck);
            Mod.Logger.Info("keepAspectRatio: " + comp.keepAspectRatio);
            Mod.Logger.Info("leftAnchor: " + comp.leftAnchor);
            Mod.Logger.Info("onChange: " + comp.onChange);
            Mod.Logger.Info("onPostFill: " + comp.onPostFill);
            Mod.Logger.Info("panel: " + comp.panel);
            Mod.Logger.Info("rightAnchor: " + comp.rightAnchor);
            Mod.Logger.Info("topAnchor: " + comp.topAnchor);
            Mod.Logger.Info("updateAnchors: " + comp.updateAnchors);
            Mod.Logger.Info("<M>");
            Mod.Logger.Info("mAlphaFrameID: " + comp.mAlphaFrameID);
            Mod.Logger.Info("mAnchorsCached: " + comp.mAnchorsCached);
            Mod.Logger.Info("mCam: " + comp.mCam);
            Mod.Logger.Info("mChanged: " + comp.mChanged);
            Mod.Logger.Info("mChildren: " + comp.mChildren);
            Mod.Logger.Info("mColor: " + comp.mColor);
            Mod.Logger.Info("mCorners: " + comp.mCorners);
            Mod.Logger.Info("mDepth: " + comp.mDepth);
            Mod.Logger.Info("mDrawRegion: " + comp.mDrawRegion);
            Mod.Logger.Info("mGo: " + comp.mGo);
            Mod.Logger.Info("mHeight: " + comp.mHeight);
            Mod.Logger.Info("mIsInFront: " + comp.mIsInFront);
            Mod.Logger.Info("mIsVisibleByAlpha: " + comp.mIsVisibleByAlpha);
            Mod.Logger.Info("mIsVisibleByPanel: " + comp.mIsVisibleByPanel);
            Mod.Logger.Info("mLastAlpha: " + comp.mLastAlpha);
            Mod.Logger.Info("mLocalToPanel: " + comp.mLocalToPanel);
            Mod.Logger.Info("mMatrixFrame: " + comp.mMatrixFrame);
            Mod.Logger.Info("mMoved: " + comp.mMoved);
            Mod.Logger.Info("mOldV0: " + comp.mOldV0);
            Mod.Logger.Info("mOldV1: " + comp.mOldV1);
            Mod.Logger.Info("mOnRender: " + comp.mOnRender);
            Mod.Logger.Info("mParent: " + comp.mParent);
            Mod.Logger.Info("mParentFound: " + comp.mParentFound);
            Mod.Logger.Info("mPivot: " + comp.mPivot);
            Mod.Logger.Info("mPlayMode: " + comp.mPlayMode);
            Mod.Logger.Info("mRoot: " + comp.mRoot);
            Mod.Logger.Info("mRootSet: " + comp.mRootSet);
            Mod.Logger.Info("mStarted: " + comp.mStarted);
            Mod.Logger.Info("mTrans: " + comp.mTrans);
            Mod.Logger.Info("mUpdateAnchors: " + comp.mUpdateAnchors);
            Mod.Logger.Info("mUpdateFrame: " + comp.mUpdateFrame);
            Mod.Logger.Info("mWidth: " + comp.mWidth);
        }

            private void printPropsAndFields(UISprite comp)
        {
            Mod.Logger.Info("<PROPS>");
            Mod.Logger.Info("alpha: " + comp.alpha);
            Mod.Logger.Info("atlas: " + comp.atlas);
            Mod.Logger.Info("color: " + comp.color);
            Mod.Logger.Info("depth: " + comp.depth);
            Mod.Logger.Info("drawRegion: " + comp.drawRegion);
            Mod.Logger.Info("enabled: " + comp.enabled);
            Mod.Logger.Info("fillAmount: " + comp.fillAmount);
            Mod.Logger.Info("centerType: " + comp.centerType);
            Mod.Logger.Info("fillDirection: " + comp.fillDirection);
            Mod.Logger.Info("flip: " + comp.flip);
            Mod.Logger.Info("height: " + comp.height);
            Mod.Logger.Info("hideFlags: " + comp.hideFlags);
            Mod.Logger.Info("invert: " + comp.invert);
            Mod.Logger.Info("mainTexture: " + comp.mainTexture);
            Mod.Logger.Info("name: " + comp.name);
            Mod.Logger.Info("onRender: " + comp.onRender);
            Mod.Logger.Info("pivot: " + comp.pivot);
            Mod.Logger.Info("rawPivot: " + comp.rawPivot);
            Mod.Logger.Info("shader: " + comp.shader);
            Mod.Logger.Info("spriteName: " + comp.spriteName);
            Mod.Logger.Info("tag: " + comp.tag);
            Mod.Logger.Info("type: " + comp.type);
            Mod.Logger.Info("useGUILayout: " + comp.useGUILayout);
            Mod.Logger.Info("width: " + comp.width);
            Mod.Logger.Info("<FIELDS>");
            Mod.Logger.Info("aspectRatio: " + comp.aspectRatio);
            Mod.Logger.Info("autoResizeBoxCollider: " + comp.autoResizeBoxCollider);
            Mod.Logger.Info("bottomAnchor: " + comp.bottomAnchor);
            Mod.Logger.Info("bottomType: " + comp.bottomType);
            Mod.Logger.Info("centerType: " + comp.centerType);
            Mod.Logger.Info("drawCall: " + comp.drawCall);
            Mod.Logger.Info("fillGeometry: " + comp.fillGeometry);
            Mod.Logger.Info("finalAlpha: " + comp.finalAlpha);
            Mod.Logger.Info("geometry: " + comp.geometry);
            Mod.Logger.Info("hideIfOffScreen: " + comp.hideIfOffScreen);
            Mod.Logger.Info("hitCheck: " + comp.hitCheck);
            Mod.Logger.Info("keepAspectRatio: " + comp.keepAspectRatio);
            Mod.Logger.Info("leftAnchor: " + comp.leftAnchor);
            Mod.Logger.Info("leftType: " + comp.leftType);
            Mod.Logger.Info("onChange: " + comp.onChange);
            Mod.Logger.Info("onPostFill: " + comp.onPostFill);
            Mod.Logger.Info("panel: " + comp.panel);
            Mod.Logger.Info("rightAnchor: " + comp.rightAnchor);
            Mod.Logger.Info("rightType: " + comp.rightType);
            Mod.Logger.Info("topAnchor: " + comp.topAnchor);
            Mod.Logger.Info("topType: " + comp.topType);
            Mod.Logger.Info("updateAnchors: " + comp.updateAnchors);
            Mod.Logger.Info("<M>");
            Mod.Logger.Info("mAlphaFrameID: " + comp.mAlphaFrameID);
            Mod.Logger.Info("mAnchorsCached: " + comp.mAnchorsCached);
            Mod.Logger.Info("mAtlas: " + comp.mAtlas);
            Mod.Logger.Info("mCam: " + comp.mCam);
            Mod.Logger.Info("mChanged: " + comp.mChanged);
            Mod.Logger.Info("mChildren: " + comp.mChildren);
            Mod.Logger.Info("mColor: " + comp.mColor);
            Mod.Logger.Info("mCorners: " + comp.mCorners);
            Mod.Logger.Info("mDepth: " + comp.mDepth);
            Mod.Logger.Info("mDrawRegion: " + comp.mDrawRegion);
            Mod.Logger.Info("mFillAmount: " + comp.mFillAmount);
            Mod.Logger.Info("mFillCenter: " + comp.mFillCenter);
            Mod.Logger.Info("mFillDirection: " + comp.mFillDirection);
            Mod.Logger.Info("mFlip: " + comp.mFlip);
            Mod.Logger.Info("mGo: " + comp.mGo);
            Mod.Logger.Info("mHeight: " + comp.mHeight);
            Mod.Logger.Info("mInnerUV: " + comp.mInnerUV);
            Mod.Logger.Info("mInvert: " + comp.mInvert);
            Mod.Logger.Info("mIsInFront: " + comp.mIsInFront);
            Mod.Logger.Info("mIsVisibleByAlpha: " + comp.mIsVisibleByAlpha);
            Mod.Logger.Info("mIsVisibleByPanel: " + comp.mIsVisibleByPanel);
            Mod.Logger.Info("mLastAlpha: " + comp.mLastAlpha);
            Mod.Logger.Info("mLocalToPanel: " + comp.mLocalToPanel);
            Mod.Logger.Info("mMatrixFrame: " + comp.mMatrixFrame);
            Mod.Logger.Info("mMoved: " + comp.mMoved);
            Mod.Logger.Info("mOldV0: " + comp.mOldV0);
            Mod.Logger.Info("mOldV1: " + comp.mOldV1);
            Mod.Logger.Info("mOnRender: " + comp.mOnRender);
            Mod.Logger.Info("mOuterUV: " + comp.mOuterUV);
            Mod.Logger.Info("mParent: " + comp.mParent);
            Mod.Logger.Info("mParentFound: " + comp.mParentFound);
            Mod.Logger.Info("mPivot: " + comp.mPivot);
            Mod.Logger.Info("mPlayMode: " + comp.mPlayMode);
            Mod.Logger.Info("mRoot: " + comp.mRoot);
            Mod.Logger.Info("mRootSet: " + comp.mRootSet);
            Mod.Logger.Info("mSprite: " + comp.mSprite);
            Mod.Logger.Info("mSpriteName: " + comp.mSpriteName);
            Mod.Logger.Info("mSpriteSet: " + comp.mSpriteSet);
            Mod.Logger.Info("mStarted: " + comp.mStarted);
            Mod.Logger.Info("mTrans: " + comp.mTrans);
            Mod.Logger.Info("mType: " + comp.mType);
            Mod.Logger.Info("mUpdateAnchors: " + comp.mUpdateAnchors);
            Mod.Logger.Info("mUpdateFrame: " + comp.mUpdateFrame);
            Mod.Logger.Info("mWidth: " + comp.mWidth);
        }

        private void printPropsAndFields(BoxCollider comp)
        {
            Mod.Logger.Info("<BB>");
            Mod.Logger.Info("attachedRigidbody: " + comp.attachedRigidbody);
            Mod.Logger.Info("bounds: " + comp.bounds);
            Mod.Logger.Info("center: " + comp.center);
            Mod.Logger.Info("contactOffset: " + comp.contactOffset);
            Mod.Logger.Info("enabled: " + comp.enabled);
            //Mod.Logger.Info("alpha: " + comp.extents);
            Mod.Logger.Info("hideFlags: " + comp.hideFlags);
            Mod.Logger.Info("isTrigger: " + comp.isTrigger);
            Mod.Logger.Info("material: " + comp.material);
            Mod.Logger.Info("name: " + comp.name);
            Mod.Logger.Info("sharedMaterial: " + comp.sharedMaterial);
            Mod.Logger.Info("size: " + comp.size);
            Mod.Logger.Info("tag: " + comp.tag);
            Mod.Logger.Info("transform: " + comp.transform);
        }

        private void printPropsAndFields(UIExInput comp)
        {
            Mod.Logger.Info("<PROPS>");
            Mod.Logger.Info("cursorPosition: " + comp.cursorPosition);
            Mod.Logger.Info("defaultText: " + comp.defaultText);
            Mod.Logger.Info("enabled: " + comp.enabled);
            Mod.Logger.Info("hideFlags: " + comp.hideFlags);
            Mod.Logger.Info("isSelected: " + comp.isSelected);
            Mod.Logger.Info("name: " + comp.name);
            Mod.Logger.Info("selectionEnd: " + comp.selectionEnd);
            Mod.Logger.Info("selectionStart: " + comp.selectionStart);
            Mod.Logger.Info("tag: " + comp.tag);
            Mod.Logger.Info("useGUILayout: " + comp.useGUILayout);
            Mod.Logger.Info("value: " + comp.value);
            Mod.Logger.Info("Value_: " + comp.Value_);
            Mod.Logger.Info("<FIELDS>");
            Mod.Logger.Info("activeTextColor: " + comp.activeTextColor);
            Mod.Logger.Info("caretColor: " + comp.caretColor);
            Mod.Logger.Info("characterLimit: " + comp.characterLimit);
            Mod.Logger.Info("customString_: " + comp.customString_);
            Mod.Logger.Info("facadeButton_: " + comp.facadeButton_);
            Mod.Logger.Info("hideInput: " + comp.hideInput);
            Mod.Logger.Info("ignoreNextInputOnChangeEvent_: " + comp.ignoreNextInputOnChangeEvent_);
            Mod.Logger.Info("inactiveColor_: " + comp.inactiveColor_);
            Mod.Logger.Info("inputType: " + comp.inputType);
            Mod.Logger.Info("keyboardType: " + comp.keyboardType);
            Mod.Logger.Info("label: " + comp.label);
            Mod.Logger.Info("onChange: " + comp.onChange);
            Mod.Logger.Info("onChange_: " + comp.onChange_);
            Mod.Logger.Info("onFinish_: " + comp.onFinish_);
            Mod.Logger.Info("onReturnKey: " + comp.onReturnKey);
            Mod.Logger.Info("onStart_: " + comp.onStart_);
            Mod.Logger.Info("onSubmit: " + comp.onSubmit);
            Mod.Logger.Info("onValidate: " + comp.onValidate);
            Mod.Logger.Info("openedKeyboard_: " + comp.openedKeyboard_);
            Mod.Logger.Info("previousValue_: " + comp.previousValue_);
            Mod.Logger.Info("savedAs: " + comp.savedAs);
            Mod.Logger.Info("selectAllTextOnFocus: " + comp.selectAllTextOnFocus);
            Mod.Logger.Info("selectionColor: " + comp.selectionColor);
            Mod.Logger.Info("selectOnSubmit_: " + comp.selectOnSubmit_);
            Mod.Logger.Info("selectOnTab: " + comp.selectOnTab);
            Mod.Logger.Info("submitted_: " + comp.submitted_);
            Mod.Logger.Info("validation: " + comp.validation);
            Mod.Logger.Info("value_: " + comp.value_);
            Mod.Logger.Info("<FIELDSM>");
            Mod.Logger.Info("mBlankTex: " + comp.mBlankTex);
            Mod.Logger.Info("mCached: " + comp.mCached);
            Mod.Logger.Info("mCam: " + comp.mCam);
            Mod.Logger.Info("mCaret: " + comp.mCaret);
            Mod.Logger.Info("mDefaultColor: " + comp.mDefaultColor);
            Mod.Logger.Info("mDefaultText: " + comp.mDefaultText);
            Mod.Logger.Info("mDoInit: " + comp.mDoInit);
            Mod.Logger.Info("mHighlight: " + comp.mHighlight);
            Mod.Logger.Info("mLastAlpha: " + comp.mLastAlpha);
            Mod.Logger.Info("mLoadSavedValue: " + comp.mLoadSavedValue);
            Mod.Logger.Info("mNextBlink: " + comp.mNextBlink);
            Mod.Logger.Info("mOnGUI: " + comp.mOnGUI);
            Mod.Logger.Info("mPivot: " + comp.mPivot);
            Mod.Logger.Info("mPosition: " + comp.mPosition);
            Mod.Logger.Info("mSelectionEnd: " + comp.mSelectionEnd);
            Mod.Logger.Info("mSelectionStart: " + comp.mSelectionStart);
            Mod.Logger.Info("mSelectMe: " + comp.mSelectMe);
            Mod.Logger.Info("mSelectTime: " + comp.mSelectTime);
            Mod.Logger.Info("mValue: " + comp.mValue);
        }

        private void OnSelect(string valu, bool success)
        {

        }

        private void OnOk()
        {

        }

        private void OnCancel()
        {

        }
    }
}
