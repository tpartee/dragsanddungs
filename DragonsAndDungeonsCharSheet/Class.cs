using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonsAndDungeonsCharSheet
{
    // Class    HP    Prime Stat
    // Bard     1d6   CHA
    // Cleric   1d6   WIS
    // Druid    1d6   WIS
    // Fighter  1d8   STR
    // Ranger   1d8   STR
    // Thief    1d4   DEX
    // Wizard   1d4   INT

    public class Class
    {
        public string Name { get; }
        public Dice HitDice { get; }
        public StatTypes PrimaryStat { get; }

        // Constructor
        public Class(string name, Dice hitDice, StatTypes primaryStat) {
            Name = name;
            HitDice = hitDice;
            PrimaryStat = primaryStat;
        }
    }
}
