using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Reptile;

namespace BRC_PostGameCharacters
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Bomb Rush Cyberfunk.exe")]

    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> BoostpacklessFaux = null!;

        public void Awake()
        {
            BoostpacklessFaux = Config.Bind("General", "Boostpackless Faux", false, "Adds boostpackless Faux to the Cypher.");

            Harmony harmony = new(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(CharacterSelect))]
    public class CharacterSelectPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("PopulateListOfSelectableCharacters")]
        public static void PopulateListOfSelectableCharacters_Postfix(CharacterSelect __instance, Player player)
        {
            if (Story.GetCurrentObjectiveInfo().chapter == Story.Chapter.CHAPTER_6)
            {
                // Faux
                if ((int)player.character != 12)
                    __instance.selectableCharacters.Add(Characters.headMan);

                // Red Felix
                if ((int)player.character != 25 && !__instance.selectableCharacters.Contains((Characters)25))
                    __instance.selectableCharacters.Add(Characters.legendMetalHead);

                // DOT.EXE (Boss)
                if ((int)player.character != 24)
                    __instance.selectableCharacters.Add(Characters.eightBallBoss);

                // Faux (boostpackless)
                if (Plugin.BoostpacklessFaux.Value && (int)player.character != 23)
                    __instance.selectableCharacters.Add(Characters.headManNoJetpack);

                __instance.Shuffle(__instance.selectableCharacters);
            }
        }
    }
}
