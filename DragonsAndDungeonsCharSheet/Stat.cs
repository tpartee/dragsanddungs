using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DragonsAndDungeonsCharSheet
{
    // STR: Strength
    // INT: Intelligence
    // WIS: Wisdom
    // DEX: Dexterity
    // CON: Constitution
    // CHA: Charisma

    public enum StatTypes { STR, INT, WIS, DEX, CON, CHA }

    /// <summary> Stores a stat type and value combo, note this is only for positive values such as storing stat minimums, for pos/neg values use StatMod </summary>
    public class Stat
    {
        public StatTypes StatType;
        public uint Value;

        public Stat(StatTypes type, uint val) {
            StatType = type;
            Value    = val;
        }
    }

    /// <summary> Stores a stat type and value combo, note this is meant for pos/neg values such as storing stat modifiers, for positive values use Stat </summary>
    public class StatMod
    {
        public StatTypes StatType;
        public int Value;

        public StatMod(StatTypes type, int val)
        {
            StatType = type;
            Value = val;
        }
    }

}
