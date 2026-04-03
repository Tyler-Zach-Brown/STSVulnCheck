using HarmonyLib;
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
    
    private static Dictionary<String, Func<CardModel, bool>> EnabledChecks = new();
    
    [HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.Draw))]
    [HarmonyPatch([typeof(PlayerChoiceContext), typeof(Decimal), typeof(Player), typeof(bool)])]
    [HarmonyPostfix]
    static async void GetCardBuffs(Task<IEnumerable<CardModel>> __result)
    {
        EnabledChecks.Add("Vuln", HasVuln); // TODO move somewhere outside of this patch should be setting this when mod is configure/when run starts
        try
        {
            await __result;
            var buffCards = __result.Result;
            foreach (var card in buffCards)
            {
                foreach (var (buff, func) in EnabledChecks)
                {
                    if (func.Invoke(card))
                    {
                        MainFile.Logger.Info("Found " + buff);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MainFile.Logger.Error("oopsie: " + ex);
        }
    }
    

    private static bool HasVuln(CardModel card)
    {
        try
        {
            return card.DynamicVars.Vulnerable.BaseValue > 0;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }
}
