using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using System;
using System.Collections.Generic;

namespace VulnCheck.VulnCheckCode;


// TB Based on what I'm seeing in BetterSpire this Patches file is used to add behavior to the game on a specific event
// in our case we're currently adding the show capability to the F4 key and the navigation of the buff display
// we could maybe combine this into the BuffTracker itself but I'm also fine keeping it separate
// TODO this patch needs to be set up using harmony in the mod Init in MainFile.  Will need to call CreateClassProcessor I think
// Need to read through the info on patching here: https://harmony.pardeike.net/articles/patching.html
[HarmonyPatch(typeof(NGame), "_Input")]
public class InputPatch
{

    static void Postfix(InputEvent inputEvent)
    {
        try
        {

            if (inputEvent is InputEventKey f4Event
                && f4Event.Keycode == Key.F4
                && f4Event.Pressed
                && !f4Event.IsEcho())
            {
                BuffTracker.Toggle();
                return;
            }

            if (inputEvent is InputEventKey pgEvent && pgEvent.Pressed && !pgEvent.IsEcho())
            {
                if (pgEvent.Keycode == Key.Pagedown) { BuffTracker.NextPage(); return; }
                if (pgEvent.Keycode == Key.Pageup) { BuffTracker.PrevPage(); return; }
            }

            // Mouse events for panel drag/close
            if (inputEvent is InputEventMouseButton or InputEventMouseMotion)
            {
                BuffTracker.HandleMouseInput(inputEvent);
            }
        }
        catch (Exception ex) { ModLog.Error("InputPatch", ex); }
    }
}
