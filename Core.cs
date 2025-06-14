﻿using HarmonyLib;
using MelonLoader;

[assembly: MelonInfo(typeof(ayumod1.Core), "ayumod1", "1.0.1", "arinalee", null)]
[assembly: MelonGame("semiwork", "REPO")]

namespace ayumod1
{
    public class Core : MelonMod
    {
        // Local player gets fully healed by the truck
       [HarmonyPatch(typeof(PlayerAvatar), "FinalHealRPC")]
        private static class FinalHealRPCPatch
        {
            private static void Prefix(PlayerAvatar __instance)
            {
                MelonLogger.Msg("FinalHealRPC called.");
                // Heal the player for their maxHealth amount
                __instance.playerHealth.Heal(1000, true);
            }
        }

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("ayumod1 initialized.");
        }

        //public override void OnLateUpdate()
        //{

        //    if (Input.GetKeyDown(KeyCode.H))
        //    {
        //        LoggerInstance.Msg("H pressed.");
        //        PlayerHealth playerHealth = GameObject.Find("Player Avatar Controller").GetComponent<PlayerHealth>();
        //        FieldInfo healthField = typeof(PlayerHealth).GetField("health", BindingFlags.NonPublic | BindingFlags.Instance);
        //        healthField.SetValue(playerHealth, 20);

        //    }

        //    if (Input.GetKeyDown(KeyCode.K))
        //    {
        //        LoggerInstance.Msg("K pressed.");
        //        // Go over batteries in the scene and set their batteryLife to 1000
        //        GameObject.FindObjectsOfType<ItemBattery>().ToList().ForEach(x =>
        //        {
        //            ItemBattery itemBattery = x.GetComponent<ItemBattery>();
        //            FieldInfo batteryBarsField = typeof(ItemBattery).GetField("batteryBars", BindingFlags.Public | BindingFlags.Instance);
        //            batteryBarsField.SetValue(itemBattery, 96);

        //            FieldInfo batteryLifeField = typeof(ItemBattery).GetField("batteryLife", BindingFlags.Public | BindingFlags.Instance);
        //            batteryLifeField.SetValue(itemBattery, 1000f);
        //        });
        //    }
        //}

        // Make all the batteries in the game infinite
        [HarmonyPatch(typeof(ItemBattery), "FixedUpdate")]
        private static class ItemBatteryPatch
        {
            static void Postfix(ItemBattery __instance)
            {
                if (__instance.batteryLife < 96)
                {
                    MelonLogger.Msg($"Correcting battery life from {__instance.batteryLife} to 1000f");
                    __instance.batteryLife = 1000f;
                }
            }
        }

        // Disable save deletion if you leave mid game
        [HarmonyPatch(typeof(DataDirector), "SaveDeleteCheck")]
        private static class SaveDeleteOnLeavePatch
        {
            static void Prefix(bool _leaveGame)
            {
                _leaveGame = false;
            }
        }

        // Enable dev mode
        //[HarmonyPatch(typeof(SemiFunc), nameof(SemiFunc.DebugDev))]
        //private static class DebugDevPatch
        //{
        //    static bool Postfix(bool devMode) => true;
        //}

        //[HarmonyPatch(typeof(SemiFunc), nameof(SemiFunc.Command))]
        //private static class CommandPatch
        //{
        //    static void Prefix(string _command)
        //    {
        //        if (_command == "/tester")
        //        {
        //            RunManager.instance.TesterToggle();
        //            return;
        //        }
        //        else if (_command == "/shop")
        //        {
        //            RunManager.instance.ChangeLevel(true, false, RunManager.ChangeLevelType.Shop);
        //        }
        //    }
        //}

        // Backport shop healthpack exploit
        [HarmonyPatch(typeof(SemiFunc), nameof(SemiFunc.RunIsShop))]
        private static class RunIsShopPatch
        {
            static bool Prefix()
            {
                return (UnityEngine.Object)RunManager.instance.levelCurrent == (UnityEngine.Object)RunManager.instance.levelShop;
            }
        }

    }
}