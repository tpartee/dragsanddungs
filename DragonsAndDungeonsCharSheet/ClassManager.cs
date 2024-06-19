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

    public class ClassManager
    {
        public List<Class> Classes = new List<Class>();
        public Dictionary<string, Class> ClassesLUT = new Dictionary<string, Class>();

        // Constructor
        public ClassManager() {
            // Add the default class data
            Class newClass = new Class("Bard", new Dice(1,6), StatTypes.CHA);
            Classes.Add(newClass);
            ClassesLUT.Add(newClass.Name, newClass);
            newClass = new Class("Cleric", new Dice(1,6), StatTypes.WIS);
            Classes.Add(newClass);
            ClassesLUT.Add(newClass.Name, newClass);
            newClass = new Class("Druid", new Dice(1,6), StatTypes.WIS);
            Classes.Add(newClass);
            ClassesLUT.Add(newClass.Name, newClass);
            newClass = new Class("Fighter", new Dice(1,8), StatTypes.STR);
            Classes.Add(newClass);
            ClassesLUT.Add(newClass.Name, newClass);
            newClass = new Class("Ranger", new Dice(1,8), StatTypes.STR);
            Classes.Add(newClass);
            ClassesLUT.Add(newClass.Name, newClass);
            newClass = new Class("Thief", new Dice(1,4), StatTypes.DEX);
            Classes.Add(newClass);
            ClassesLUT.Add(newClass.Name, newClass);
            newClass = new Class("Wizard", new Dice(1,4), StatTypes.INT);
            Classes.Add(newClass);
            ClassesLUT.Add(newClass.Name, newClass);
        }

        public static List<string> GetAllClasses()
        {
            return new List<string>(){ "Bard", "Cleric", "Druid", "Fighter", "Ranger", "Thief", "Wizard" };
        }

        /// <summary> Convert the class string from the drop-down with primary stat into the actual class </summary>
        /// <param name="v"> The selected combobox string </param>
        /// <returns> The correct class for the selection made </returns>
        public string ParseSelection(string? v)
        {
            if (v == null) return string.Empty;   // In theory this should never happen but keep the code safe!
            foreach (var c in ClassesLUT.Keys) if (v.StartsWith(c)) return c;
            return string.Empty;   // In theory this should never happen but keep the code safe!
        }
    }
}
