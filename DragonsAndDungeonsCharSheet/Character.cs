using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DragonsAndDungeonsCharSheet
{
    public enum Races { Human, Halfling, Gnome, Elf, Dwarf }

    public enum Classes { Bard, Cleric, Druid, Fighter, Ranger, Thief, Wizard }

    // Race      Mins           Mods             Ability               Classes
    // Dwarf     CON 9          -1 CHA, +1 CON   Infravision           Bard, Cleric, Fighter, Thief
    // Elf       INT 9          -1 CON, +1 DEX   Detect Secret Doors   Cleric, Druid, Fighter, Wizard, Ranger, Thief
    // Gnome     CON 9, INT 9   None             Defensive Bonus       Cleric, Fighter, Thief, Wizard
    // Halfling  CON 9, DEX 9   +1 DEX, -1 STR   Initiative Bonus      Bard, Druid, Fighter, Thief
    // Human     None           None             None                  All

    /// <summary> This class holds all of the data for a single character plus all of the utility methods for working with it </summary>
    public class Character : DependencyObject
    {

        #region Dependency Properties

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Character), new PropertyMetadata(string.Empty));

        public string Race
        {
            get { return (string)GetValue(RaceProperty); }
            set { SetValue(RaceProperty, value); }
        }
        public static readonly DependencyProperty RaceProperty =
            DependencyProperty.Register("Race", typeof(string), typeof(Character), new PropertyMetadata(string.Empty));

        public string Ability
        {
            get { return (string)GetValue(AbilityProperty); }
            set { SetValue(AbilityProperty, value); }
        }
        public static readonly DependencyProperty AbilityProperty =
            DependencyProperty.Register("Ability", typeof(string), typeof(Character), new PropertyMetadata(string.Empty));

        public string Class
        {
            get { return (string)GetValue(ClassProperty); }
            set { SetValue(ClassProperty, value); }
        }
        public static readonly DependencyProperty ClassProperty =
            DependencyProperty.Register("Class", typeof(string), typeof(Character), new PropertyMetadata(string.Empty));

        public int Strength
        {
            get { return (int)GetValue(StrengthProperty); }
            set { SetValue(StrengthProperty, value); }
        }
        public static readonly DependencyProperty StrengthProperty =
            DependencyProperty.Register("Strength", typeof(int), typeof(Character), new PropertyMetadata(0));

        public int Constitution
        {
            get { return (int)GetValue(ConstitutionProperty); }
            set { SetValue(ConstitutionProperty, value); }
        }
        public static readonly DependencyProperty ConstitutionProperty =
            DependencyProperty.Register("Constitution", typeof(int), typeof(Character), new PropertyMetadata(0));

        public int Dexterity
        {
            get { return (int)GetValue(DexterityProperty); }
            set { SetValue(DexterityProperty, value); }
        }
        public static readonly DependencyProperty DexterityProperty =
            DependencyProperty.Register("Dexterity", typeof(int), typeof(Character), new PropertyMetadata(0));

        public int Intelligence
        {
            get { return (int)GetValue(IntelligenceProperty); }
            set { SetValue(IntelligenceProperty, value); }
        }
        public static readonly DependencyProperty IntelligenceProperty =
            DependencyProperty.Register("Intelligence", typeof(int), typeof(Character), new PropertyMetadata(0));

        public int Wisdom
        {
            get { return (int)GetValue(WisdomProperty); }
            set { SetValue(WisdomProperty, value); }
        }
        public static readonly DependencyProperty WisdomProperty =
            DependencyProperty.Register("Wisdom", typeof(int), typeof(Character), new PropertyMetadata(0));

        public int Charisma
        {
            get { return (int)GetValue(CharismaProperty); }
            set { SetValue(CharismaProperty, value); }
        }
        public static readonly DependencyProperty CharismaProperty =
            DependencyProperty.Register("Charisma", typeof(int), typeof(Character), new PropertyMetadata(0));

        public int MaxHitPoints
        {
            get { return (int)GetValue(MaxHitPointsProperty); }
            set { SetValue(MaxHitPointsProperty, value); }
        }
        public static readonly DependencyProperty MaxHitPointsProperty =
            DependencyProperty.Register("MaxHitPoints", typeof(int), typeof(Character), new PropertyMetadata(0));

        public int Gold
        {
            get { return (int)GetValue(GoldProperty); }
            set { SetValue(GoldProperty, value); }
        }
        public static readonly DependencyProperty GoldProperty =
            DependencyProperty.Register("Gold", typeof(int), typeof(Character), new PropertyMetadata(0));


        public string DisplayStr
        {
            get { return (string)GetValue(DisplayStrProperty); }
            set { SetValue(DisplayStrProperty, value); }
        }
        public static readonly DependencyProperty DisplayStrProperty =
            DependencyProperty.Register("DisplayStr", typeof(string), typeof(Character), new PropertyMetadata(string.Empty));

        public string DisplayCon
        {
            get { return (string)GetValue(DisplayConProperty); }
            set { SetValue(DisplayConProperty, value); }
        }
        public static readonly DependencyProperty DisplayConProperty =
            DependencyProperty.Register("DisplayCon", typeof(string), typeof(Character), new PropertyMetadata(string.Empty));

        public string DisplayDex
        {
            get { return (string)GetValue(DisplayDexProperty); }
            set { SetValue(DisplayDexProperty, value); }
        }
        public static readonly DependencyProperty DisplayDexProperty =
            DependencyProperty.Register("DisplayDex", typeof(string), typeof(Character), new PropertyMetadata(string.Empty));

        public string DisplayInt
        {
            get { return (string)GetValue(DisplayIntProperty); }
            set { SetValue(DisplayIntProperty, value); }
        }
        public static readonly DependencyProperty DisplayIntProperty =
            DependencyProperty.Register("DisplayInt", typeof(string), typeof(Character), new PropertyMetadata(string.Empty));

        public string DisplayWis
        {
            get { return (string)GetValue(DisplayWisProperty); }
            set { SetValue(DisplayWisProperty, value); }
        }
        public static readonly DependencyProperty DisplayWisProperty =
            DependencyProperty.Register("DisplayWis", typeof(string), typeof(Character), new PropertyMetadata(string.Empty));

        public string DisplayCha
        {
            get { return (string)GetValue(DisplayChaProperty); }
            set { SetValue(DisplayChaProperty, value); }
        }
        public static readonly DependencyProperty DisplayChaProperty =
            DependencyProperty.Register("DisplayCha", typeof(string), typeof(Character), new PropertyMetadata(string.Empty));

        #endregion Dependency Properties


        /// <summary> Instantiate a new blank character </summary>
        public Character() { }

        /// <summary> Load a character from a file on disk </summary>
        /// <param name="filePath"> The path to the file to open </param>
        public Character(string filePath) {
            ParseFromFile(filePath);
        }

        public int GetStat(StatTypes stat)
        {
            switch (stat) {
                case StatTypes.STR: return Strength;
                case StatTypes.CON: return Constitution;
                case StatTypes.DEX: return Dexterity;
                case StatTypes.INT: return Intelligence;
                case StatTypes.WIS: return Wisdom;
                case StatTypes.CHA: return Charisma;
                default: return 0;
            }
        }

        public void SetStat(StatTypes stat, int val, int nat = -1)   // 'nat' means the natural number before modifiers
        {
            string display = (nat > -1 && nat != val) ? $"{val} ({nat})" : val.ToString();
            switch (stat) {
                case StatTypes.STR:
                    Strength = val;
                    DisplayStr = display;
                    break;
                case StatTypes.CON:
                    Constitution = val;
                    DisplayCon = display;
                    break;
                case StatTypes.DEX:
                    Dexterity = val;
                    DisplayDex = display;
                    break;
                case StatTypes.INT:
                    Intelligence = val;
                    DisplayInt = display;
                    break;
                case StatTypes.WIS:
                    Wisdom = val;
                    DisplayWis = display;
                    break;
                case StatTypes.CHA:
                    Charisma = val;
                    DisplayCha = display;
                    break;
            }
        }

        public string GetSaveString()
        {
            StringBuilder charData = new StringBuilder();
            charData.AppendLine(Name);
            charData.AppendLine(Race);
            charData.AppendLine(Class);
            charData.AppendLine(Ability);
            charData.AppendLine(Strength.ToString());
            charData.AppendLine(DisplayStr);
            charData.AppendLine(Constitution.ToString());
            charData.AppendLine(DisplayCon);
            charData.AppendLine(Dexterity.ToString());
            charData.AppendLine(DisplayDex);
            charData.AppendLine(Intelligence.ToString());
            charData.AppendLine(DisplayInt);
            charData.AppendLine(Wisdom.ToString());
            charData.AppendLine(DisplayWis);
            charData.AppendLine(Charisma.ToString());
            charData.AppendLine(DisplayCha);
            charData.AppendLine(MaxHitPoints.ToString());
            charData.AppendLine(Gold.ToString());

            return charData.ToString();
        }

        private void ParseFromFile(string charData)
        {
            // Parse character data into fields
            int outInt;
            string[] fields = charData.Split("\n");
            if (fields.Length < 18) return;   // WARN: Bad to bail without an error like this, fix later - this is just for sanity and to avoid errors for now
            Name         = fields[0];
            Race         = fields[1];
            Class        = fields[2];
            Ability      = fields[3];
            Strength     = (int.TryParse( fields[4], out outInt)) ? outInt : 0;
            DisplayStr   = fields[5];
            Constitution = (int.TryParse( fields[6], out outInt)) ? outInt : 0;
            DisplayCon   = fields[7];
            Dexterity    = (int.TryParse( fields[8], out outInt)) ? outInt : 0;
            DisplayDex   = fields[9];
            Intelligence = (int.TryParse(fields[10], out outInt)) ? outInt : 0;
            DisplayInt   = fields[11];
            Wisdom       = (int.TryParse(fields[12], out outInt)) ? outInt : 0;
            DisplayWis   = fields[13];
            Charisma     = (int.TryParse(fields[14], out outInt)) ? outInt : 0;
            DisplayCha   = fields[15];
            MaxHitPoints = (int.TryParse(fields[16], out outInt)) ? outInt : 0;
            Gold         = (int.TryParse(fields[17], out outInt)) ? outInt : 0;
        }

    }
}
