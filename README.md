Initial ideas:
- Two main efforts
  - Get vuln power off cards by checking dynamic vars
    - Initially check on draw for testing
    - Once validated add startup list generation checking cards and adding to list
      - Once generated make it so that we don't rescan cards already added
      - Side note on this will it be faster to look at an attribute on a card or do a lookup of the card in a list?
      - Shifting to this mod as template https://github.com/erasels/Minty-Spire-2/blob/main/src/AscHoverTooltips.cs
  - Add UI element showing Vuln in hand
    - little window to allow for toggling? (F keys)
    - https://alchyr.github.io/BaseLib-Wiki/docs/utilities/config-advanced.html


- Stretch Goals
  - Potion checking (clear distinction between potion and card availability)
  - Add other power scans
    - Weak
    - Team card draw/energy
    - Enemy strength loss
    - Team Double Damage
  - Add Number of times power applied
  - Add mod config to allow toggling of window
    - Toggle powers searched for
    - Toggle potions on/off


KNOWN ISSUES:
- Uppercut is a little bitch and doesn't set vulnerable as a dynamicVar cause it hates me
- Creating a card and adding it to your hand doesn't trigger the draw function either