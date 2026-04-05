using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace VulnCheck.VulnCheckCode;


/// <summary>
/// Panel that shows available buffs and debuffs in players hands for multiplayer coordination
/// </summary>


[HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.Draw))]
[HarmonyPatch([typeof(PlayerChoiceContext), typeof(Decimal), typeof(Player), typeof(bool)])]
class BuffTracker
{
    
    //TODO Should be setting this dictionary up based off mod config settings
    private static Dictionary<String, Func<CardModel, bool>> EnabledChecks = new Dictionary<string, Func<CardModel, bool>>(){{"Vuln", HasVuln}};
    private static Player playerDrawing = null;

    [HarmonyPrefix]
    static void Prefix(Player player)
    {
        playerDrawing = player;
    }
    
    
    [HarmonyPostfix]
    static async void GetCardBuffs(Task<IEnumerable<CardModel>> __result)
    {
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
                        ThinkCmd.Play(new LocString("combat_messages", "VULN"), playerDrawing.Creature, 2.0);
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
