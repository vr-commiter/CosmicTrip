using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.UnityEngine;
using HarmonyLib;
using MyTrueGear;
using System.Collections.Generic;
using UnityEngine;

namespace CosmicTrip_TruegGear;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    private static TrueGearMod _TrueGear = null;

    public override void Load()
    {
        // Plugin startup logic
        Log = base.Log;

        Harmony.CreateAndPatchAll(typeof(Plugin));
        _TrueGear = new TrueGearMod();

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    public static KeyValuePair<float, float> GetEnemyDirection(Transform player, Transform enemy)
    {
        Vector3 toEnemy = enemy.position - player.position;
        toEnemy.y = 0; // 忽略垂直方向

        if (toEnemy == Vector3.zero)
            return new KeyValuePair<float, float>(0, 0); ; // 敌人与玩家重合时的处理

        Vector3 playerForward = player.forward;
        playerForward.y = 0;
        playerForward.Normalize();
        toEnemy.Normalize();
        // 计算顺时针角度（-180 到 180）
        float clockwiseAngle = Vector3.SignedAngle(playerForward, toEnemy, Vector3.up);
        // 转换为逆时针角度（0 到 360）
        float counterClockwiseAngle = (360 - clockwiseAngle) % 360;
        float verticalDifference = enemy.position.y - player.position.y;
        return new KeyValuePair<float, float>(counterClockwiseAngle, verticalDifference);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputVR), "HoldObject")]
    private static void PlayerInputVR_HoldObject_Postfix(PlayerInputVR __instance, GameObject obj)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("HoldObject");
        if (__instance.Handedness == InputHandedness.HAND_LEFT)
        {
            Log.LogInfo("LeftHandHoldItem");
            _TrueGear.Play("LeftHandHoldItem");
        }
        else if (__instance.Handedness == InputHandedness.HAND_RIGHT)
        {
            Log.LogInfo("RightHandHoldItem");
            _TrueGear.Play("RightHandHoldItem");
        }
        Log.LogInfo(obj.name);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputVR), "PullObject")]
    private static void PlayerInputVR_PullObject_Postfix(PlayerInputVR __instance, GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("PullObject");
        if (__instance.Handedness == InputHandedness.HAND_LEFT)
        {
            Log.LogInfo("LeftHandPullItem");
        }
        else if (__instance.Handedness == InputHandedness.HAND_RIGHT)
        {
            Log.LogInfo("RightHandPullItem");
        }
        Log.LogInfo(obj.name);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputVR), "DropObject")]
    private static void PlayerInputVR_DropObject_Postfix(PlayerInputVR __instance)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("DropObject");
        if (__instance.Handedness == InputHandedness.HAND_LEFT)
        {
            Log.LogInfo("LeftHandDropItem");
            _TrueGear.Play("LeftHandDropItem");
        }
        else if (__instance.Handedness == InputHandedness.HAND_RIGHT)
        {
            Log.LogInfo("RightHandDropItem");
            _TrueGear.Play("RightHandDropItem");
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputGun), "Shoot")]
    private static void PlayerInputGun_Shoot_Postfix(PlayerInputGun __instance)
    {

        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("Shoot");
        if (__instance.input.Handedness == InputHandedness.HAND_LEFT)
        {
            Log.LogInfo("LeftHandGunShoot");
            _TrueGear.Play("LeftHandGunShoot");
        }
        else if (__instance.input.Handedness == InputHandedness.HAND_RIGHT)
        {
            Log.LogInfo("RightHandGunShoot");
            _TrueGear.Play("RightHandGunShoot");
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputGun), "ShootChargeShot")]
    private static void PlayerInputGun_ShootChargeShot_Postfix(PlayerInputGun __instance)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("ShootChargeShot");
        if (__instance.input.Handedness == InputHandedness.HAND_LEFT)
        {
            Log.LogInfo("LeftHandGunChargeShoot");
            _TrueGear.Play("LeftHandGunChargeShoot");
        }
        else if (__instance.input.Handedness == InputHandedness.HAND_RIGHT)
        {
            Log.LogInfo("RightHandGunChargeShoot");
            _TrueGear.Play("RightHandGunChargeShoot");
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(ToolFrisbee2), "ChargeLevelOne")]
    private static void ToolFrisbee2_ChargeLevelOne_Postfix(ToolFrisbee2 __instance, PlayerInput input)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("ChargeLevelOne");
        if (input.name.Contains("device_1"))
        {
            Log.LogInfo("LeftHandFrisbeeChargeLevelOne");
            _TrueGear.Play("LeftHandFrisbeeChargeLevelOne");
        }
        else if (input.name.Contains("device_2"))
        {
            Log.LogInfo("RightHandFrisbeeChargeLevelOne");
            _TrueGear.Play("RightHandFrisbeeChargeLevelOne");
        }
        Log.LogInfo(input.name);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(ToolFrisbee2), "ChargeLevelTwo")]
    private static void ToolFrisbee2_ChargeLevelTwo_Postfix(ToolFrisbee2 __instance, PlayerInput input)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("ChargeLevelTwo");
        if (input.name.Contains("device_1"))
        {
            Log.LogInfo("LeftHandFrisbeeChargeLevelTwo");
            _TrueGear.Play("LeftHandFrisbeeChargeLevelTwo");
        }
        else if (input.name.Contains("device_2"))
        {
            Log.LogInfo("RightHandFrisbeeChargeLevelTwo");
            _TrueGear.Play("RightHandFrisbeeChargeLevelTwo");
        }
        Log.LogInfo(input.name);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(ToolFrisbee2), "ChargeLevelThree")]
    private static void ToolFrisbee2_ChargeLevelThree_Postfix(ToolFrisbee2 __instance, PlayerInput input)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("ChargeLevelThree");
        if (input.name.Contains("device_1"))
        {
            Log.LogInfo("LeftHandFrisbeeChargeLevelThree");
            _TrueGear.Play("LeftHandFrisbeeChargeLevelThree");
        }
        else if (input.name.Contains("device_2"))
        {
            Log.LogInfo("RightHandFrisbeeChargeLevelThree");
            _TrueGear.Play("RightHandFrisbeeChargeLevelThree");
        }
        Log.LogInfo(input.name);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(ToolFrisbee2), "ChargeLevelFour")]
    private static void ToolFrisbee2_ChargeLevelFour_Postfix(ToolFrisbee2 __instance, PlayerInput input)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("ChargeLevelFour");
        if (input.name.Contains("device_1"))
        {
            Log.LogInfo("LeftHandFrisbeeChargeLevelFour");
            _TrueGear.Play("LeftHandFrisbeeChargeLevelFour");
        }
        else if (input.name.Contains("device_2"))
        {
            Log.LogInfo("RightHandFrisbeeChargeLevelFour");
            _TrueGear.Play("RightHandFrisbeeChargeLevelFour");
        }
        Log.LogInfo(input.name);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(ToolFrisbee2), "PullTriggerWhileMounted")]
    private static void ToolFrisbee2_PullTriggerWhileMounted_Postfix(ToolFrisbee2 __instance, PlayerInput input)
    {
        Log.LogInfo("---------------------------------------------");
        if (input.name.Contains("device_1"))
        {
            Log.LogInfo("StartLeftHandFrisbee");
            _TrueGear.StartLeftHandFrisbee();
        }
        else if (input.name.Contains("device_2"))
        {
            Log.LogInfo("StartRightHandFrisbee");
            _TrueGear.StartRightHandFrisbee();
        }
        Log.LogInfo(input.name);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(ToolFrisbee2), "ReleaseTriggerWhileMounted")]
    private static void ToolFrisbee2_ReleaseTriggerWhileMounted_Postfix(ToolFrisbee2 __instance, PlayerInput input)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("ReleaseTriggerWhileMounted");
        if (input.name.Contains("device_1"))
        {
            Log.LogInfo("StopLeftHandFrisbee");
            _TrueGear.StopLeftHandFrisbee();
            _TrueGear.Play("LeftHandReleaseFrisbee");
        }
        else if (input.name.Contains("device_2"))
        {
            Log.LogInfo("StopRightHandFrisbee");
            _TrueGear.StopRightHandFrisbee();
            _TrueGear.Play("RightHandReleaseFrisbee");
        }
        Log.LogInfo(input.name);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerCore), "OnDamage")]
    private static void PlayerCore_OnDamage_Postfix(PlayerCore __instance, DamageData data)
    {
        Log.LogInfo("---------------------------------------------");
        if (data.Source == null)
        {
            Log.LogInfo("NoDirectionDamage");
            _TrueGear.Play("NoDirectionDamage");
            return;
        }
        var angle = GetEnemyDirection(__instance.gameObject.transform, data.Source.gameObject.transform);
        TrueGearMod.angle = angle.Key;
        TrueGearMod.ver = angle.Value;
        Log.LogInfo("DefaultDamage");
        Log.LogInfo($"Angle :{angle.Key} ,Ver :{angle.Value}");
        Log.LogInfo(data.Source.name);
        _TrueGear.PlayAngle("DefaultDamage",angle.Key,angle.Value);

    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerCore), "OnDeath")]
    private static void PlayerCore_OnDeath_Postfix(PlayerCore __instance)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("PlayerDeath");
        _TrueGear.Play("PlayerDeath");
        _TrueGear.StopLeftHandInhale();
        _TrueGear.StopRightHandInhale();
        _TrueGear.StopLeftHandFrisbee();
        _TrueGear.StopRightHandFrisbee();
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerCore), "DoHealEffect")]
    private static void PlayerCore_DoHealEffect_Postfix(PlayerCore __instance)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("Healing");
        _TrueGear.Play("Healing");
    }

    [HarmonyPostfix, HarmonyPatch(typeof(ToolShield2), "MountTriggerStart")]
    private static void ToolShield2_MountTriggerStart_Postfix(ToolShield2 __instance, PlayerInput input)
    {
        Log.LogInfo("---------------------------------------------");
        if (input.name.Contains("device_1"))
        {
            Log.LogInfo("LeftHandOpenShield");
            _TrueGear.Play("LeftHandOpenShield");
        }
        else if (input.name.Contains("device_2"))
        {
            Log.LogInfo("RightHandOpenShield");
            _TrueGear.Play("RightHandOpenShield");
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(ToolShield2), "MountTriggerEnd")]
    private static void ToolShield2_MountTriggerEnd_Postfix(ToolShield2 __instance, PlayerInput input)
    {
        Log.LogInfo("---------------------------------------------");
        if (input.name.Contains("device_1"))
        {
            Log.LogInfo("LeftHandHideShield");
            _TrueGear.Play("LeftHandHideShield");
        }
        else if (input.name.Contains("device_2"))
        {
            Log.LogInfo("RightHandHideShield");
            _TrueGear.Play("RightHandHideShield");
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(ToolShield2), "DamageShield")]
    private static void ToolShield2_DamageShield_Postfix(ToolShield2 __instance)
    {
        Log.LogInfo("---------------------------------------------");
        if (__instance.gameObject.transform.parent.transform.parent.name.Contains("device_1"))
        {
            Log.LogInfo("LeftHandShieldDamage");
            _TrueGear.Play("LeftHandShieldDamage");
        }
        else if (__instance.gameObject.transform.parent.transform.parent.name.Contains("device_2"))
        {
            Log.LogInfo("RightHandShieldDamage");
            _TrueGear.Play("RightHandShieldDamage");
        }
    }



    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputVR), "DoHapticPulsate")]
    private static void PlayerInputVR_DoHapticPulsate_Postfix(PlayerInputVR __instance, float value)
    {
        //Log.LogInfo("---------------------------------------------");
        //Log.LogInfo("DoHapticPulsate");
        if (value == 4f)
        {
            if (__instance.Handedness == InputHandedness.HAND_LEFT)
            {
                Log.LogInfo("---------------------------------------------");
                Log.LogInfo("LeftHandBubbleGunShoot");
                _TrueGear.Play("LeftHandBubbleGunShoot");
            }
            else
            {
                Log.LogInfo("---------------------------------------------");
                Log.LogInfo("RightHandBubbleGunShoot");
                _TrueGear.Play("RightHandBubbleGunShoot");
            }
        }        
    }

    [HarmonyPostfix, HarmonyPatch(typeof(ToolBubbleGun), "OnFullyCharged")]
    private static void ToolBubbleGun_OnFullyCharged_Postfix(ToolBubbleGun __instance)
    {
        if (__instance.Input.Handedness == InputHandedness.HAND_LEFT)
        {
            Log.LogInfo("---------------------------------------------");
            Log.LogInfo("LeftHandBubbleGunFullyCharged");
            _TrueGear.Play("LeftHandBubbleGunFullyCharged");
        }
        else
        {
            Log.LogInfo("---------------------------------------------");
            Log.LogInfo("RightHandBubbleGunFullyCharged");
            _TrueGear.Play("RightHandBubbleGunFullyCharged");
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputVR), "StartInhaleEffects")]
    private static void PlayerInputVR_StartInhaleEffects_Postfix(PlayerInputVR __instance )
    {
        if (__instance.Handedness == InputHandedness.HAND_LEFT)
        {
            _TrueGear.StartLeftHandInhale();
        }
        else
        {
            _TrueGear.StartRightHandInhale();
        }
        //Log.LogInfo("---------------------------------------------");
        //Log.LogInfo("StartInhaleEffects");
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputVR), "StopInhaleEffects")]
    private static void PlayerInputVR_StopInhaleEffects_Postfix(PlayerInputVR __instance)
    {
        if (__instance.Handedness == InputHandedness.HAND_LEFT)
        {
            _TrueGear.StopLeftHandInhale();
        }
        else
        {
            _TrueGear.StopRightHandInhale();
        }
        //Log.LogInfo("---------------------------------------------");
        //Log.LogInfo("StopInhaleEffects");
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputVR), "SetGunModeOn")]
    private static void PlayerInputVR_SetGunModeOn_Postfix(PlayerInputVR __instance)
    {
        if (__instance.Handedness == InputHandedness.HAND_LEFT)
        {
            Log.LogInfo("---------------------------------------------");
            Log.LogInfo("LeftHandHoldItem");
            _TrueGear.Play("LeftHandHoldItem");
        }
        else
        {
            Log.LogInfo("---------------------------------------------");
            Log.LogInfo("RightHandHoldItem");
            _TrueGear.Play("RightHandHoldItem");
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputVR), "SetGunModeOff")]
    private static void PlayerInputVR_SetGunModeOff_Postfix(PlayerInputVR __instance)
    {
        //Log.LogInfo("---------------------------------------------");
        //Log.LogInfo("SetGunModeOff");
    }
    
    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputVR), "OnPointTriggerStart")]
    private static void PlayerInputVR_OnPointTriggerStart_Postfix(PlayerInputVR __instance)
    {
        Log.LogInfo("---------------------------------------------");
        if (__instance.Handedness == InputHandedness.HAND_LEFT)
        {
            Log.LogInfo("LeftHandPointTrigger");
            _TrueGear.Play("LeftHandPointTrigger");
        }
        else
        {
            Log.LogInfo("RightHandPointTrigger");
            _TrueGear.Play("RightHandPointTrigger");
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputVR), "OnPointTriggerEnd")]
    private static void PlayerInputVR_OnPointTriggerEnd_Postfix(PlayerInputVR __instance)
    {
        //Log.LogInfo("---------------------------------------------");
        //Log.LogInfo("OnPointTriggerEnd");
    }

    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputVR), "TouchTransitionWaitingStates")]
    private static void PlayerInputVR_TouchTransitionWaitingStates_Postfix(PlayerInputVR __instance)
    {
        if (__instance.Touch == null)
        {
            return;
        }
        Log.LogInfo("---------------------------------------------");
        if (__instance.Handedness == InputHandedness.HAND_LEFT)
        {
            Log.LogInfo("LeftHandTouch");
            _TrueGear.Play("LeftHandTouch");
        }
        else
        {
            Log.LogInfo("RightHandTouch");
            _TrueGear.Play("RightHandTouch");
        }
        Log.LogInfo(__instance.Touch.name);
    }


    [HarmonyPostfix, HarmonyPatch(typeof(StructureTurretMachineGun), "FireTurret")]
    private static void StructureTurretMachineGun_FireTurret_Postfix(StructureTurretMachineGun __instance)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("FireTurret");
        _TrueGear.Play("FireTurret");
    }


    [HarmonyPostfix, HarmonyPatch(typeof(ToolBuildingGunPlace), "OnMountTriggerStart")]
    private static void ToolBuildingGunPlace_OnMountTriggerStart_Postfix(ToolBuildingGunPlace __instance, PlayerInput input)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("OnMountTriggerStart");
        if (input.name.Contains("device_1"))
        {
            Log.LogInfo("LeftHandBuilding");
            _TrueGear.Play("LeftHandBuilding");
        }
        else if (input.name.Contains("device_2"))
        {
            Log.LogInfo("RightHandBuilding");
            _TrueGear.Play("RightHandBuilding");
        }
    }


    [HarmonyPostfix, HarmonyPatch(typeof(PlayerInputRewardCollector), "OnTriggerEnter")]
    private static void PlayerInputRewardCollector_OnTriggerEnter_Postfix(PlayerInputRewardCollector __instance, Collider collider)
    {
        if (collider.name.Contains("Reward"))
        {
            Log.LogInfo("---------------------------------------------");
            if (__instance.owner.name.Contains("device_1"))
            {
                Log.LogInfo("LeftHandRewardCollected");
                _TrueGear.Play("LeftHandRewardCollected");
            }
            else if (__instance.owner.name.Contains("device_2"))
            {
                Log.LogInfo("RightHandRewardCollected");
                _TrueGear.Play("RightHandRewardCollected");
            }
        }        
    }


    [HarmonyPostfix, HarmonyPatch(typeof(InteractiveTeleporterRunePull), "DoTeleport")]
    private static void InteractiveTeleporterRunePull_DoTeleport_Postfix(InteractiveTeleporterRunePull __instance)
    {
        Log.LogInfo("---------------------------------------------");
        Log.LogInfo("Teleport");
        _TrueGear.Play("Teleport");
    }

}
