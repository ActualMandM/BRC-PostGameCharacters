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
                if (IsValid(Characters.headMan, __instance, player))
                    __instance.selectableCharacters.Add(Characters.headMan);

                // Red Felix
                if (IsValid(Characters.legendMetalHead, __instance, player))
                    __instance.selectableCharacters.Add(Characters.legendMetalHead);

                // DOT.EXE (Boss)
                if (IsValid(Characters.eightBallBoss, __instance, player))
                    __instance.selectableCharacters.Add(Characters.eightBallBoss);

                // Faux (boostpackless)
                if (Plugin.BoostpacklessFaux.Value && IsValid(Characters.headManNoJetpack, __instance, player))
                    __instance.selectableCharacters.Add(Characters.headManNoJetpack);

                __instance.Shuffle(__instance.selectableCharacters);
            }
        }

        public static bool IsValid(Characters character, CharacterSelect __instance, Player player)
        {
            return !__instance.selectableCharacters.Contains(character) && player.character != character;
        }
    }
}
