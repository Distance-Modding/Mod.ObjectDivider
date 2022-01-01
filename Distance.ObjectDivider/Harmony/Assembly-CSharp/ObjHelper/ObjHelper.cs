using LevelEditorActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.ObjectDivider.Harmony
{
    class ObjHelper
    {
        public static void TranslateLocalWhileKeepingTextureAsIfWorldmapped(ref GameObject gameObject, Vector3 localTranslate)
        {
            if (gameObject.HasComponent<GoldenSimples>())
            {
                if (!gameObject.GetComponent<GoldenSimples>().worldMapped_)
                {
                    TranslateLocal(ref gameObject, localTranslate);
                    Vector3 objscale = gameObject.GetComponent<Transform>().localScale;
                    GoldenSimples objgs = gameObject.GetComponent<GoldenSimples>();
                    Vector3 defaultObjSize = getDefaultObjSize(gameObject.name);
                    float texOffsetX = 0;
                    float texOffsetY = 0;
                    float texOffsetZ = 0;
                    if (defaultObjSize.x != 0)
                    {
                        texOffsetX = ((-1 / (defaultObjSize.x * objscale.x)) * localTranslate.x - objgs.textureScale_.x + objgs.textureOffset_.x);
                    }
                    if (defaultObjSize.y != 0)
                    {
                        texOffsetY = ((-1 / (defaultObjSize.y * objscale.y)) * localTranslate.y - objgs.textureScale_.y + objgs.textureOffset_.y);
                    }
                    if (defaultObjSize.z != 0)
                    {
                        texOffsetZ = ((-1 / (defaultObjSize.z * objscale.z)) * localTranslate.z - objgs.textureScale_.z + objgs.textureOffset_.z);
                    }
                    objgs.textureOffset_ = new Vector3(texOffsetX, texOffsetY, texOffsetZ);
                }
            }
        }

        public static void ScaleWhileKeepingTextureAsIfWorldmapped(ref GameObject gameObject)
        {
            if (gameObject.HasComponent<GoldenSimples>())
            {
                if (!gameObject.GetComponent<GoldenSimples>().worldMapped_)
                {

                }
            }
        }

        public static Vector3 TranslateLocal(ref GameObject gameObject, Vector3 localTranslate)
        {
            Quaternion objrot = gameObject.GetComponent<Transform>().rotation;
            Vector3 xAxis = objrot * (new Vector3(1F, 0, 0));
            Vector3 yAxis = objrot * (new Vector3(0, 1F, 0));
            Vector3 zAxis = objrot * (new Vector3(0, 0, 1F));
            Vector3 xAxisTranslation = xAxis * localTranslate.x;
            Vector3 yAxisTranslation = yAxis * localTranslate.y;
            Vector3 zAxisTranslation = zAxis * localTranslate.z;
            Vector3 fullTranslation = xAxisTranslation + yAxisTranslation + zAxisTranslation;
            gameObject.GetComponent<Transform>().position = gameObject.GetComponent<Transform>().position + fullTranslation;
            return fullTranslation;
        }

        public static Vector3 getRealDefaultObjSize(GameObject gameObject)
        {
            GroupAction groupAction_ = Group.CreateGroupAction(new GameObject[] { gameObject }, gameObject);
            Group group = groupAction_.GroupObjects();
            Vector3 objsize = group.localBounds_.size;
            groupAction_.Undo();
            return objsize;
        }

        public static Vector3 getConvenientObjPosition(GameObject gameObject)
        {
            GroupAction groupAction_ = Group.CreateGroupAction(new GameObject[] { gameObject }, gameObject);
            Group group = groupAction_.GroupObjects();
            Vector3 objpos = group.gameObject.GetComponent<Transform>().position;
            groupAction_.Undo();
            return objpos;
        }

        public static void DontHaveAdd<TComponent>(ref GameObject gameObject) where TComponent : AddedComponent
        {
            if (!gameObject.HasComponent<TComponent>())
            {
                gameObject.AddComponent<TComponent>();
            }
        }

        public static void copyAddedComponentsOverWithTexture(ref GameObject gameObject, GameObject gameObject2, bool doTrackAttach)
        {
            if (gameObject2.HasComponent<GoldenSimples>())
            {
                copyGoldenSimpleOverWithColor(ref gameObject, gameObject2);
            }
            if (gameObject2.HasComponent<Animated>())
            {
                copyAnimatorOver(ref gameObject, gameObject2);
            }
            if (gameObject2.HasComponent<EngageBrokenPieces>())
            {
                copyEngageBrokenPiecesOver(ref gameObject, gameObject2.GetComponent<EngageBrokenPieces>());
            }
            if (gameObject2.HasComponent<ExcludeFromEMP>())
            {
                copyExcludeFromEMPOver(ref gameObject, gameObject2.GetComponent<ExcludeFromEMP>());
            }
            if (gameObject2.HasComponent<FadeOut>())
            {
                copyFadeOutOver(ref gameObject, gameObject2.GetComponent<FadeOut>());
            }
            if (gameObject2.HasComponent<GoldenAnimator>())
            {
                copyGoldenAnimatorOver(ref gameObject, gameObject2.GetComponent<GoldenAnimator>());
            }
            if (gameObject2.HasComponent<IgnoreInCullGroups>())
            {
                copyIgnoreInCullGroupsOver(ref gameObject, gameObject2.GetComponent<IgnoreInCullGroups>());
            }
            if (gameObject2.HasComponent<PulseAll>())
            {
                copyPulseAllOver(ref gameObject, gameObject2.GetComponent<PulseAll>());
            }
            if (gameObject2.HasComponent<SetActiveAfterWarp>())
            {
                copySetActiveAfterWarpOver(ref gameObject, gameObject2.GetComponent<SetActiveAfterWarp>());
            }
            if (gameObject2.HasComponent<ShowDuringGlitch>())
            {
                copyShowDuringGlitchOver(ref gameObject, gameObject2.GetComponent<ShowDuringGlitch>());
            }
            if (gameObject2.HasComponent<TurnLightOnNearCar>())
            {
                copyTurnLightOnNearCarOver(ref gameObject, gameObject2.GetComponent<TurnLightOnNearCar>());
            }
            if (gameObject2.HasComponent<TrackAttachment>() && doTrackAttach)
            {
                copyTrackAttachmentOver(ref gameObject, gameObject2.GetComponent<TrackAttachment>());
            }
        }

        public static ReferenceMap.Handle<GameObject> ReplaceWithDuplicate(ref GameObject gameObject)
        {
            ReferenceMap.Handle<GameObject> duplicatedObjectHandle_ = new ReferenceMap.Handle<GameObject>();
            GameObject newObj = DuplicateObjectsAction.Duplicate(gameObject);
            LevelEditor levelEditor = G.Sys.LevelEditor_;
            LevelLayer layerOfObject = levelEditor.WorkingLevel_.GetLayerOfObject(gameObject);
            levelEditor.DeleteGameObject(gameObject);
            levelEditor.AddGameObject(ref duplicatedObjectHandle_, newObj, layerOfObject);
            levelEditor.SelectObject(newObj);
            gameObject = newObj;
            return duplicatedObjectHandle_;
        }

        public static ReferenceMap.Handle<GameObject> ReplaceWithDuplicate(ref GameObject gameObject, ref ReferenceMap.Handle<GameObject> duplicatedObjectHandle_)
        {
            GameObject newObj = DuplicateObjectsAction.Duplicate(gameObject);
            LevelEditor levelEditor = G.Sys.LevelEditor_;
            LevelLayer layerOfObject = levelEditor.WorkingLevel_.GetLayerOfObject(gameObject);
            levelEditor.DeleteGameObject(gameObject);
            levelEditor.AddGameObject(ref duplicatedObjectHandle_, newObj, layerOfObject);
            levelEditor.SelectObject(newObj);
            gameObject = newObj;
            return duplicatedObjectHandle_;
        }

        public static TrackAttachment copyTrackAttachmentOver(ref GameObject gameObject, TrackAttachment comp)
        {
            if (gameObject.HasComponent<TrackAttachment>())
            {
                gameObject.RemoveComponent<TrackAttachment>();
            }
            AttachToTrackAction action = new AttachToTrackAction(comp.attachedTo_, gameObject, comp.subsegmentIndex_);
            TrackAttachment gocomp = action.AddComponent(gameObject);
            gocomp.copyRot_ = comp.copyRot_;
            gocomp.copyScale_ = comp.copyScale_;
            ReplaceWithDuplicate(ref gameObject);

            return gocomp;
        }

        public static TurnLightOnNearCar copyTurnLightOnNearCarOver(ref GameObject gameObject, TurnLightOnNearCar comp)
        {
            DontHaveAdd<TurnLightOnNearCar>(ref gameObject);
            TurnLightOnNearCar gocomp = gameObject.GetComponent<TurnLightOnNearCar>();
            gocomp.triggerDistance_ = comp.triggerDistance_;
            //gocomp.addDelay_ = comp.addDelay_;
            gocomp.delayRangeLow_ = comp.delayRangeLow_;
            gocomp.delayRangeHigh_ = comp.delayRangeHigh_;
            gocomp.randomDelay_ = comp.randomDelay_;
            gocomp.delayTime_ = comp.delayTime_;
            gocomp.setRGB_ = comp.setRGB_;
            gocomp.affectColorProperty_ = comp.affectColorProperty_;
            gocomp.playSound_ = comp.playSound_;
            gocomp.turnOffOnTriggerExit_ = comp.turnOffOnTriggerExit_;

            return gocomp;
        }

        public static ShowDuringGlitch copyShowDuringGlitchOver(ref GameObject gameObject, ShowDuringGlitch comp)
        {
            DontHaveAdd<ShowDuringGlitch>(ref gameObject);
            ShowDuringGlitch gocomp = gameObject.GetComponent<ShowDuringGlitch>();
            gocomp.glitchFieldID_ = comp.glitchFieldID_;
            gocomp.hideOnStart_ = comp.hideOnStart_;
            return gocomp;
        }

        public static SetActiveAfterWarp copySetActiveAfterWarpOver(ref GameObject gameObject, SetActiveAfterWarp comp)
        {
            DontHaveAdd<SetActiveAfterWarp>(ref gameObject);
            SetActiveAfterWarp gocomp = gameObject.GetComponent<SetActiveAfterWarp>();
            gocomp.index_ = comp.index_;
            gocomp.activeState_ = comp.activeState_;
            gocomp.warpEvent_ = comp.warpEvent_;
            gocomp.setActive_ = comp.setActive_;

            return gocomp;
        }

        public static PulseAll copyPulseAllOver(ref GameObject gameObject, PulseAll comp)
        {
            DontHaveAdd<PulseAll>(ref gameObject);
            PulseAll gocomp = gameObject.GetComponent<PulseAll>();
            gocomp.pulseProperty_ = comp.pulseProperty_;
            gocomp.pulseToMode_ = comp.pulseToMode_;
            gocomp.minAlpha_ = comp.minAlpha_;
            gocomp.degradeTime_ = comp.degradeTime_;
            gocomp.channel_ = comp.channel_;
            gocomp.colorChannel_ = comp.colorChannel_;
            gocomp.intervalFrequency_ = comp.intervalFrequency_;
            gocomp.intervalOffset_ = comp.intervalOffset_;
            gocomp.intervalCurveType_ = comp.intervalCurveType_;
            gocomp.usePulseCore_ = comp.usePulseCore_;
            gocomp.pulseMaterials_ = comp.pulseMaterials_;
            gocomp.pulseLights_ = comp.pulseLights_;
            gocomp.minLightBrightness_ = comp.minLightBrightness_;
            gocomp.pulseParticles_ = comp.pulseParticles_;
            gocomp.minParticleBrightness_ = comp.minParticleBrightness_;

            return gocomp;
        }

        public static LookAtCamera copyLookAtCameraOver(ref GameObject gameObject, LookAtCamera comp)
        {
            DontHaveAdd<LookAtCamera>(ref gameObject);
            LookAtCamera gocomp = gameObject.GetComponent<LookAtCamera>();
            gocomp.rotationSpeed_ = comp.rotationSpeed_;
            gocomp.flipZAxis_ = comp.flipZAxis_;
            gocomp.targetType_ = comp.targetType_;
            gocomp.billboard_ = comp.billboard_;

            return gocomp;
        }

        public static IgnoreInCullGroups copyIgnoreInCullGroupsOver(ref GameObject gameObject, IgnoreInCullGroups comp)
        {
            DontHaveAdd<IgnoreInCullGroups>(ref gameObject);
            IgnoreInCullGroups gocomp = gameObject.GetComponent<IgnoreInCullGroups>();


            return gocomp;
        }

        public static GoldenAnimator copyGoldenAnimatorOver(ref GameObject gameObject, GoldenAnimator comp)
        {
            DontHaveAdd<GoldenAnimator>(ref gameObject);
            GoldenAnimator goganim = gameObject.GetComponent<GoldenAnimator>();
            goganim.scale_ = comp.scale_;
            goganim.scaleExponent_ = comp.scaleExponent_;
            goganim.translate_ = comp.translate_;
            goganim.translateVector_ = comp.translateVector_;
            goganim.delay_ = comp.delay_;
            goganim.duration_ = comp.duration_;
            goganim.timeOffset_ = comp.timeOffset_;
            goganim.loop_ = comp.loop_;
            goganim.curveType_ = comp.curveType_;
            goganim.editorAnimationT_ = comp.editorAnimationT_;
            goganim.customPongValues_ = comp.customPongValues_;
            goganim.pongDelay_ = comp.pongDelay_;
            goganim.pongDuration_ = comp.pongDuration_;
            goganim.pongCurveType_ = comp.pongCurveType_;
            goganim.triggerOptionsLabel_ = comp.triggerOptionsLabel_;
            goganim.triggerOff_.action_ = comp.triggerOff_.action_;
            goganim.triggerOff_.waitForAnimationFinish_ = comp.triggerOff_.waitForAnimationFinish_;
            goganim.triggerOff_.reset_ = comp.triggerOff_.reset_;
            goganim.triggerOn_.action_ = comp.triggerOn_.action_;
            goganim.triggerOn_.waitForAnimationFinish_ = comp.triggerOn_.waitForAnimationFinish_;
            goganim.triggerOn_.reset_ = comp.triggerOn_.reset_;

            return goganim;
        }

        public static FadeOut copyFadeOutOver(ref GameObject gameObject, FadeOut comp)
        {
            DontHaveAdd<FadeOut>(ref gameObject);
            FadeOut gofo = gameObject.GetComponent<FadeOut>();
            gofo.oneShot_ = comp.oneShot_;
            gofo.triggerDistance_ = comp.triggerDistance_;
            gofo.delayTime_ = comp.delayTime_;
            gofo.curveType_ = comp.curveType_;
            gofo.setRGB_ = comp.setRGB_;
            gofo.affectColorProperty_ = comp.affectColorProperty_;

            return gofo;
        }

        public static ExcludeFromEMP copyExcludeFromEMPOver(ref GameObject gameObject, ExcludeFromEMP comp)
        {
            DontHaveAdd<ExcludeFromEMP>(ref gameObject);
            ExcludeFromEMP goefemp = gameObject.GetComponent<ExcludeFromEMP>();
            goefemp.excludeRenderer_ = comp.excludeRenderer_;
            goefemp.excludeLights_ = comp.excludeLights_;
            goefemp.excludeParticles_ = comp.excludeParticles_;

            return goefemp;
        }

        public static EngageBrokenPieces copyEngageBrokenPiecesOver(ref GameObject gameObject, EngageBrokenPieces comp)
        {
            DontHaveAdd<EngageBrokenPieces>(ref gameObject);
            EngageBrokenPieces goebp = gameObject.GetComponent<EngageBrokenPieces>();
            goebp.move_ = comp.move_;
            goebp.moveSpeed_ = comp.moveSpeed_;
            goebp.rotate_ = comp.rotate_;
            goebp.rotateSpeed_ = comp.rotateSpeed_;
            goebp.explode_ = comp.explode_;
            goebp.explodeSpeed_ = comp.explodeSpeed_;

            return goebp;
        }

        public static AnimatorCameraShake copyAnimatorCamShakeOver(ref GameObject gameObject, AnimatorCameraShake comp)
        {
            DontHaveAdd<AnimatorCameraShake>(ref gameObject);
            AnimatorCameraShake goanimcs = gameObject.GetComponent<AnimatorCameraShake>();
            goanimcs.playIntensity_ = comp.playIntensity_;
            goanimcs.stopIntensity_ = comp.stopIntensity_;
            goanimcs.pongPlayIntensity_ = comp.pongPlayIntensity_;
            goanimcs.pongStopIntensity_ = comp.stopIntensity_;
            goanimcs.effectiveNear_ = comp.effectiveNear_;
            goanimcs.falloffDistance_ = comp.falloffDistance_;
            goanimcs.centerPoint_ = comp.centerPoint_;
            return goanimcs;
        }

        public static AnimatorAudio copyAnimatorAudioOver(ref GameObject gameObject, AnimatorAudio comp)
        {
            DontHaveAdd<AnimatorAudio>(ref gameObject);
            AnimatorAudio goanimaud = gameObject.GetComponent<AnimatorAudio>();
            goanimaud.play_ = comp.play_;
            goanimaud.stop_ = comp.stop_;
            goanimaud.pongPlay_ = comp.pongPlay_;
            goanimaud.pongStop_ = comp.pongStop_;
            goanimaud.centerPoint_ = comp.centerPoint_;
            return goanimaud;
        }

        public static Animated copyAnimatorOver(ref GameObject gameObject, GameObject gameObject2)
        {
            if (gameObject2.HasComponent<Animated>())
            {
                if (gameObject2.HasComponent<AnimatorAudio>())
                {
                    if (gameObject2.HasComponent<AnimatorCameraShake>())
                    {
                        return copyAnimatorOver(ref gameObject, gameObject2.GetComponent<Animated>(), gameObject2.GetComponent<AnimatorAudio>(), gameObject2.GetComponent<AnimatorCameraShake>());
                    }
                    else
                    {
                        return copyAnimatorOver(ref gameObject, gameObject2.GetComponent<Animated>(), gameObject2.GetComponent<AnimatorAudio>(), null);
                    }
                }
                if (gameObject2.HasComponent<AnimatorCameraShake>())
                {
                    return copyAnimatorOver(ref gameObject, gameObject2.GetComponent<Animated>(), null, gameObject2.GetComponent<AnimatorCameraShake>());
                }
                return copyAnimatorOver(ref gameObject, gameObject2.GetComponent<Animated>(), null, null);
            }
            return null;
        }

        public static Animated copyAnimatorOver(ref GameObject gameObject, Animated comp, AnimatorAudio comp2, AnimatorCameraShake comp3)
        {
            DontHaveAdd<Animated>(ref gameObject);
            Animated goanim = gameObject.GetComponent<Animated>();
            goanim.alwaysAnimate_ = comp.alwaysAnimate_;
            goanim.animatePhysics_ = comp.animatePhysics_;
            goanim.animatingPhysics_ = comp.animatingPhysics_;
            //goanim.attachmentXformPS_ = comp.attachmentXformPS_;
            ////goanim.audio_ = comp.audio_;
            if (comp2 != null)
            {
                goanim.audio_ = copyAnimatorAudioOver(ref gameObject, comp2);
            }
            ////goanim.cameraShake_ = comp.cameraShake_;
            if (comp3 != null)
            {
                goanim.cameraShake_ = copyAnimatorCamShakeOver(ref gameObject, comp3);
            }
            goanim.centerPoint_ = comp.centerPoint_;
            goanim.currentAction_ = comp.currentAction_;
            goanim.curveType_ = comp.curveType_;
            goanim.customPongValues_ = comp.customPongValues_;
            goanim.defaultAction_ = comp.defaultAction_;
            goanim.delay_ = comp.delay_;
            goanim.doublePivotDistance_ = comp.doublePivotDistance_;
            goanim.duration_ = comp.duration_;
            goanim.editorAnimationT_ = comp.editorAnimationT_;
            goanim.extend_ = comp.extend_;
            goanim.followDistance_ = comp.followDistance_;
            goanim.followPercentOfTrack_ = comp.followPercentOfTrack_;
            //goanim.isFollowingTrack_ = comp.isFollowingTrack_;
            //goanim.isOffCenter_ = comp.isOffCenter_;
            //goanim.isProjectile_ = comp.isProjectile_;
            //goanim.isRotating_ = comp.isRotating_;
            //goanim.isScaling_ = comp.isScaling_;
            //goanim.isTranslating_ = comp.isTranslating_;
            //goanim.loopInfo_ = comp.loopInfo_;
            goanim.loop_ = comp.loop_;
            goanim.motion_ = comp.motion_;
            //goanim.origCenterOffset_ = comp.origCenterOffset_;
            //goanim.origCenter_ = comp.origCenter_;
            //goanim.orig_ = comp.orig_;
            //goanim.ping_ = comp.ping_;
            goanim.pongCurveType_ = comp.pongCurveType_;
            goanim.pongDelay_ = comp.pongDelay_;
            goanim.pongDuration_ = comp.pongDuration_;
            //goanim.pong_ = comp.pong_;
            goanim.projectileGravity_ = comp.projectileGravity_;
            goanim.projectileVelocity_ = comp.projectileVelocity_;
            goanim.rotateAxis_ = comp.rotateAxis_;
            goanim.rotateGlobal_ = comp.rotateGlobal_;
            goanim.rotateMagnitude_ = comp.rotateMagnitude_;
            goanim.rotate_ = comp.rotate_;
            //goanim.rotationAxisNrm_ = comp.rotationAxisNrm_;
            goanim.scaleExponent_ = comp.scaleExponent_;
            goanim.scale_ = comp.scale_;
            //goanim.setPhysicsLastFrame_ = comp.setPhysicsLastFrame_;
            //goanim.setPos_ = comp.setPos_;
            //goanim.setRot_ = comp.setRot_;
            //goanim.setScale_ = comp.setScale_;
            //goanim.startDistance_ = comp.startDistance_;
            goanim.timeOffset_ = comp.timeOffset_;
            //goanim.timeOfStart_ = comp.timeOfStart_;
            goanim.trackAttachment_ = comp.trackAttachment_;
            goanim.trackLocation_ = comp.trackLocation_;
            goanim.trackSection_ = comp.trackSection_;
            goanim.translateType_ = comp.translateType_;
            goanim.translateVector_ = comp.translateVector_;
            goanim.translateVector_ = comp.transVec_;
            goanim.triggerOff_.action_ = comp.triggerOff_.action_;
            goanim.triggerOff_.waitForAnimationFinish_ = comp.triggerOff_.waitForAnimationFinish_;
            goanim.triggerOff_.reset_ = comp.triggerOff_.reset_;
            goanim.triggerOn_.action_ = comp.triggerOn_.action_;
            goanim.triggerOn_.waitForAnimationFinish_ = comp.triggerOn_.waitForAnimationFinish_;
            goanim.triggerOn_.reset_ = comp.triggerOn_.reset_;
            goanim.triggerOptionsLabel_ = comp.triggerOptionsLabel_;
            //goanim.updater_ = comp.updater_;
            //goanim.waitActionTime_ = comp.waitActionTime_;
            //goanim.waitAction_ = comp.waitAction_;
            goanim.wrapAround_ = comp.wrapAround_;
            //goanim.xformSS_ = comp.xformSS_;
            return goanim;
        }

        public static void copyGoldenSimpleOverWithColor(ref GameObject gameObject, GameObject gameObject2)
        {
            if (gameObject.HasComponent<GoldenSimples>() && gameObject2.HasComponent<GoldenSimples>())
            {
                copyGoldenSimpleOverWithColor(ref gameObject, gameObject2.GetComponent<GoldenSimples>());
            }
        }

        public static void copyGoldenSimpleOverWithColor(ref GameObject gameObject, GoldenSimples comp)
        {
            if (gameObject.HasComponent<GoldenSimples>())
            {
                GoldenSimples gogs = gameObject.GetComponent<GoldenSimples>();
                gogs.additiveTransparency_ = comp.additiveTransparency_;
                gogs.bumpStrength_ = comp.bumpStrength_;
                gogs.disableBump_ = comp.disableBump_;
                gogs.disableCollision_ = comp.disableCollision_;
                gogs.disableDiffuse_ = comp.disableDiffuse_;
                gogs.disableReflect_ = comp.disableReflect_;
                gogs.emitTextureIndex_ = comp.emitTextureIndex_;
                gogs.flipTextureUV_ = comp.flipTextureUV_;
                gogs.invertEmit_ = comp.invertEmit_;
                gogs.multiplicativeTransparency_ = comp.multiplicativeTransparency_;
                gogs.textureIndex_ = comp.textureIndex_;
                gogs.textureOffset_ = comp.textureOffset_;
                gogs.textureScale_ = comp.textureScale_;
                gogs.worldMapped_ = comp.worldMapped_;
                gogs.renderer_.material.SetColor("_Color", comp.renderer_.material.GetColor("_Color"));
                gogs.renderer_.material.SetColor("_EmitColor", comp.renderer_.material.GetColor("_EmitColor"));
                gogs.renderer_.material.SetColor("_ReflectColor", comp.renderer_.material.GetColor("_ReflectColor"));
                gogs.renderer_.material.SetColor("_SpecColor", comp.renderer_.material.GetColor("_SpecColor"));
            }
        }

        public static void copyGoldenSimpleOver(ref GameObject gameObject, GoldenSimples comp)
        {
            if (gameObject.HasComponent<GoldenSimples>())
            {
                GoldenSimples gogs = gameObject.GetComponent<GoldenSimples>();
                gogs.additiveTransparency_ = comp.additiveTransparency_;
                gogs.bumpStrength_ = comp.bumpStrength_;
                gogs.disableBump_ = comp.disableBump_;
                gogs.disableCollision_ = comp.disableCollision_;
                gogs.disableDiffuse_ = comp.disableDiffuse_;
                gogs.disableReflect_ = comp.disableReflect_;
                gogs.emitTextureIndex_ = comp.emitTextureIndex_;
                gogs.flipTextureUV_ = comp.flipTextureUV_;
                gogs.invertEmit_ = comp.invertEmit_;
                gogs.multiplicativeTransparency_ = comp.multiplicativeTransparency_;
                gogs.textureIndex_ = comp.textureIndex_;
                gogs.textureOffset_ = comp.textureOffset_;
                gogs.textureScale_ = comp.textureScale_;
                gogs.worldMapped_ = comp.worldMapped_;
            }
        }

        public static Vector3 getDefaultObjSize(string objName)
        {
            switch (objName)
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
            return new Vector3(-1, -1, -1);
        }

        
    }
}
