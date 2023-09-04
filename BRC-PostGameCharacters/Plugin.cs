using BepInEx;
using HarmonyLib;

namespace BRC_PostGameCharacters
{
	[BepInPlugin("com.MandM.BRC-PostGameCharacters", "BRC-PostGameCharacters", "1.0.0")]
	[BepInProcess("Bomb Rush Cyberfunk.exe")]

	public class Plugin : BaseUnityPlugin
	{
		public void Awake()
		{
			Harmony harmony = new Harmony("MandM.BRC-PostGameCharacters.Harmony");
			harmony.PatchAll();
		}
	}
}
