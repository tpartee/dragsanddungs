using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DragonsAndDungeonsCharSheet
{
    public enum DefaultRaces { Human, Halfling, Gnome, Elf, Dwarf }

    /// <summary> This class helps to manage the races currently loaded and allow loading, saving, updating, etc. </summary>
    public class RaceManager
    {
        private bool isInitialized = false;
        public List<Race>               Races    = new List<Race>();
        public Dictionary<string, Race> RacesLUT = new Dictionary<string, Race>();

        // Race      Mins           Mods             Ability               Classes
        // Dwarf     CON 9          -1 CHA, +1 CON   Infravision           Bard, Cleric, Fighter, Thief
        // Elf       INT 9          -1 CON, +1 DEX   Detect Secret Doors   Cleric, Druid, Fighter, Wizard, Ranger, Thief
        // Gnome     CON 9, INT 9   None             Defensive Bonus       Cleric, Fighter, Thief, Wizard
        // Halfling  CON 9, DEX 9   +1 DEX, -1 STR   Initiative Bonus      Bard, Druid, Fighter, Thief
        // Human     None           None             None                  All

        // Constructor
        public RaceManager() {
            // Add the default race data
            Race newRace = new Race("Human", new List<Stat>(), new List<StatMod>(), "None", ClassManager.GetAllClasses());
            Races.Add(newRace);
            RacesLUT.Add(newRace.Name, newRace);
            newRace = new Race("Halfling", new List<Stat>() { new Stat(StatTypes.CON, 9), new Stat(StatTypes.DEX, 9) }, new List<StatMod>() { new StatMod(StatTypes.DEX, 1), new StatMod(StatTypes.STR, -1) }, "Initiative Bonus", new List<string>() { "Bard", "Druid", "Fighter", "Thief" });
            Races.Add(newRace);
            RacesLUT.Add(newRace.Name, newRace);
            newRace = new Race("Gnome", new List<Stat>() { new Stat(StatTypes.CON, 9), new Stat(StatTypes.INT, 9) }, new List<StatMod>(), "Defensive Bonus", new List<string>() { "Cleric", "Fighter", "Thief", "Wizard" });
            Races.Add(newRace);
            RacesLUT.Add(newRace.Name, newRace);
            newRace = new Race("Elf", new List<Stat>() { new Stat(StatTypes.INT, 9) }, new List<StatMod>() { new StatMod(StatTypes.DEX, 1), new StatMod(StatTypes.CON, -1) }, "Detect Secret Doors", new List<string>() { "Cleric", "Druid", "Fighter", "Wizard", "Ranger", "Thief" });
            Races.Add(newRace);
            RacesLUT.Add(newRace.Name, newRace);
            newRace = new Race("Dwarf", new List<Stat>() { new Stat(StatTypes.CON, 9) }, new List<StatMod>() { new StatMod(StatTypes.CHA, -1), new StatMod(StatTypes.CON, 1) }, "Infravision", new List<string>() { "Bard", "Cleric", "Fighter", "Thief" });
            Races.Add(newRace);
            RacesLUT.Add(newRace.Name, newRace);
            isInitialized = true;
        }

        public List<string> GetAllRaceNames()
        {
            if (!isInitialized) return new List<string>();
            return RacesLUT.Keys.ToList();
        }

        public List<string> GetClassesWithPrimeForRace(string race, ClassManager clsMgr)
        {
            if (!RacesLUT.ContainsKey(race)) return new List<string>();

            List<string> classList = new List<string>();
            foreach (string cls in RacesLUT[race].Classes) {
                classList.Add(cls + " (" + clsMgr.ClassesLUT[cls].PrimaryStat.ToString() + ")");
            }

            return classList;
        }

    }
}
