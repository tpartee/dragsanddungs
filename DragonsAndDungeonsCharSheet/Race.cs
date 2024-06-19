using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonsAndDungeonsCharSheet
{
    // Race      Mins           Mods             Ability               Classes
    // Dwarf     CON 9          -1 CHA, +1 CON   Infravision           Bard, Cleric, Fighter, Thief
    // Elf       INT 9          -1 CON, +1 DEX   Detect Secret Doors   Cleric, Druid, Fighter, Wizard, Ranger, Thief
    // Gnome     CON 9, INT 9   None             Defensive Bonus       Cleric, Fighter, Thief, Wizard
    // Halfling  CON 9, DEX 9   +1 DEX, -1 STR   Initiative Bonus      Bard, Druid, Fighter, Thief
    // Human     None           None             None                  All

    public class Race
    {
        public string Name { get; }
        public List<Stat> StatMins { get; }
        public List<StatMod> StatMods { get; }
        public string Ability { get; }
        public List<string> Classes { get; }

        // Constructor
        public Race(string name, List<Stat> statMins, List<StatMod> statMods, string ability, List<string> classes) {
            Name = name;
            StatMins = statMins;
            StatMods = statMods;
            Ability = ability;
            Classes = classes;
        }

    }
}
