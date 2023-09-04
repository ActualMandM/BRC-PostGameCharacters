using HarmonyLib;
using Reptile;

namespace BRC_PostGameCharacters
{
	[HarmonyPatch(typeof(CharacterSelect))]
	public class CharacterSelectPatch
	{
		[HarmonyPostfix]
		[HarmonyPatch("PopulateListOfSelectableCharacters")]
		public static void PopulateListOfSelectableCharacters_Postfix(CharacterSelect __instance, Player player)
		{
			__instance.selectableCharacters.Clear();
			for (int i = 0; i < 26; i++)
			{
				Characters character = (Characters)i;

				if (character != Characters.headManNoJetpack && player.character != character && IsCharacterUnlocked(__instance, character))
					__instance.selectableCharacters.Add(character);
			}
			__instance.Shuffle(__instance.selectableCharacters);
		}

		public static bool IsCharacterUnlocked(CharacterSelect __instance, Characters character)
		{
			switch (character)
			{
				// DLC
				case Characters.robot:
				case Characters.skate:
					return __instance.hasCharacterDownloadableContent;

				// Not unlockable
				case Characters.headMan:
				case Characters.legendMetalHead:
				case Characters.eightBallBoss:
					return Story.GetCurrentObjectiveInfo().chapter == Story.Chapter.CHAPTER_6;

				// Unlockable
				default:
					return Core.Instance.SaveManager.CurrentSaveSlot.GetCharacterProgress(character).unlocked;
			}
		}
	}
}
