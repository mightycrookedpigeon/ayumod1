using System.Reflection;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(ayumod1.Core), "ayumod1", "1.0.0", "user", null)]
[assembly: MelonGame("semiwork", "REPO")]

namespace ayumod1
{
    public class Core : MelonMod
    {
        // Patch the amount of health the local player gets when they are healed by the truck
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

        [HarmonyPatch(typeof(DataDirector), "SaveDeleteCheck")]
        private static class SaveDeleteOnLeavePatch
        {
            static void Prefix(bool _leaveGame)
            {
                _leaveGame = false;
            }
        }
    }
}