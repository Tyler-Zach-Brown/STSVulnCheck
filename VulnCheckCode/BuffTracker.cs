using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace VulnCheck.VulnCheckCode;


/// <summary>
/// Panel that shows available buffs and debuffs in players hands for multiplayer coordination
/// </summary>


[HarmonyPatch]
class BuffTracker
{
    [HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.Draw))]
    [HarmonyPatch([typeof(PlayerChoiceContext), typeof(Decimal), typeof(Player), typeof(bool)])]
    [HarmonyPostfix]
    static async void GetCardBuffs(Task<IEnumerable<CardModel>> __result)
    {
        try
        {
            await __result;
            var buffCards = __result.Result;
            foreach (var card in buffCards)
            {
                try
                {
                    if (card.DynamicVars.Vulnerable.BaseValue > 0)
                        MainFile.Logger.Info("Found vuln: " + card.Title);
                } catch (KeyNotFoundException) {/* Do nothing if vuln not found */}
            }
        }
        catch (Exception ex)
        {
            MainFile.Logger.Error("oopsie: " + ex);
        }
    }
}
