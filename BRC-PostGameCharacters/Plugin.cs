using BepInEx;
using HarmonyLib;
using Reptile;

namespace BRC_PostGameCharacters
{
	[BepInPlugin("com.MandM.BRC-PostGameCharacters", "BRC-PostGameCharacters", "1.0.1")]
	[BepInProcess("Bomb Rush Cyberfunk.exe")]

	public class Plugin : BaseUnityPlugin
	{
		public void Awake()
		{
			Harmony harmony = new Harmony("MandM.BRC-PostGameCharacters.Harmony");
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
                if ((int)player.character != 12) __instance.selectableCharacters.Add(Characters.headMan);
                if ((int)player.character != 25) __instance.selectableCharacters.Add(Characters.legendMetalHead);
                if ((int)player.character != 24) __instance.selectableCharacters.Add(Characters.eightBallBoss);

                __instance.Shuffle(__instance.selectableCharacters);
            }
        }
    }
}
