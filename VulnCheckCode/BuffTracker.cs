using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;

namespace VulnCheck.VulnCheckCode;


/// <summary>
/// Panel that shows available buffs and debuffs in players hands for multiplayer coordination
/// </summary>


[HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.Draw))]
class BuffTracker
{
    static IEnumerable<CardModel> Postfix(IEnumerable<CardModel> cards)
    {
        var buffCards = cards.Where(c => c.DynamicVars.Vulnerable.BaseValue > 0);
        foreach (var cardModel in buffCards)
        {
            MainFile.Logger.Info("Found vuln: " + cardModel.Title);
        }
        return cards;
    }
}
