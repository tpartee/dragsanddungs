using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragonsAndDungeonsCharSheet
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class MainWindow : Window
    {
        protected RaceManager raceMgr   = new RaceManager();    // Race helper class
        protected ClassManager classMgr = new ClassManager();   // Class helper class
        private bool isCharacterRolled  = false;
        private bool isCharacterSaved   = false;



        #region Dependency Properties

        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }
        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register("ErrorText", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public bool CanCharacterRoll
        {
            get { return (bool)GetValue(CanCharacterRollProperty); }
            set { SetValue(CanCharacterRollProperty, value); }
        }
        public static readonly DependencyProperty CanCharacterRollProperty =
            DependencyProperty.Register("CanCharacterRoll", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public Character CurrentCharacter
        {
            get { return (Character)GetValue(CurrentCharacterProperty); }
            set { SetValue(CurrentCharacterProperty, value); }
        }
        public static readonly DependencyProperty CurrentCharacterProperty =
            DependencyProperty.Register("CurrentCharacter", typeof(Character), typeof(MainWindow), new PropertyMetadata(null));

        public List<string> RaceOptions
        {
            get { return (List<string>)GetValue(RaceOptionsProperty); }
            set { SetValue(RaceOptionsProperty, value); }
        }
        public static readonly DependencyProperty RaceOptionsProperty =
            DependencyProperty.Register("RaceOptions", typeof(List<string>), typeof(MainWindow), new PropertyMetadata(null));

        public List<string> ClassOptions
        {
            get { return (List<string>)GetValue(ClassOptionsProperty); }
            set { SetValue(ClassOptionsProperty, value); }
        }
        public static readonly DependencyProperty ClassOptionsProperty =
            DependencyProperty.Register("ClassOptions", typeof(List<string>), typeof(MainWindow), new PropertyMetadata(null));

        #endregion Dependency Properties



        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;   // Self-referential data context so data in this window can bind to the UI
            CurrentCharacter = new Character();
            RaceOptions = raceMgr.GetAllRaceNames();
        }



        /// <summary> Called when a user clicks on the Load menu item </summary>
        /// <param name="sender"> The object which spawned this event </param>
        /// <param name="e"> Auto-filled </param>
        private void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            // This is a super quick and dirty save and load functionality, ideally I would use JSON or XML but that would take far too long for this code exercise and require third party libs

            // TODO: Validation - ensure we're not stomping on an unsaved character, etc.

            // Verify the sheet exists on disk first
            string exePath  = System.Reflection.Assembly.GetEntryAssembly().Location;
            string safeName = CurrentCharacter.Name.Replace(' ', '_') + ".sheet";
            string path     = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(exePath), safeName);
            if (!File.Exists(path)) { ErrorText = $"Character sheet not found: {path}"; return; }

            // Sheet exists, let's load it!
            string charData;
            try {
                charData = File.ReadAllText(path);
            } catch (Exception ex) {
                ErrorText = $"Failed to load character sheet: {ex.Message}"; return;
            }

            // Process the data and set visual state to display
            CurrentCharacter = new Character(charData);
            isCharacterRolled = true;
            isCharacterSaved  = true;
            SelectionView.Visibility = Visibility.Collapsed;
            DisplayView.Visibility = Visibility.Visible; 
        }

        /// <summary> Called when a user clicks on the Save menu item </summary>
        /// <param name="sender"> The object which spawned this event </param>
        /// <param name="e"> Auto-filled </param>
        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            // This is a super quick and dirty save and load functionality, ideally I would use JSON or XML or object serialization but that would take far too long for this code exercise and require third party libs

            // TODO: Validation - ensure we're not stomping on an unsaved character, etc.
            //if (!isCharacterRolled) { ErrorText = "Can't save an unrolled character!"; return; }

            // Try to write the character sheet out
            string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string safeName = CurrentCharacter.Name.Replace(' ', '_') + ".sheet";
            string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(exePath), safeName);
            string charData = CurrentCharacter.GetSaveString();

            try {
                File.WriteAllText(path, charData.ToString());
            } catch(Exception ex) {
                ErrorText = $"Failed to save character: {ex.Message}";
            }
        }

        private void OnNewClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Validation - ensure we're not stomping on an unsaved character, etc.
            //if (isCharacterRolled && !isCharacterSaved) { ErrorText = "You have an unsaved character. Aborting! (TODO: Make this a pop-up question instead.)"; return; }

            // Reset the state so user can make a new character
            isCharacterRolled = false;
            isCharacterSaved  = false;
            SelectionView.Visibility = Visibility.Visible;
            DisplayView.Visibility   = Visibility.Collapsed;
            RaceCombo.SelectedIndex = -1;
            ClassCombo.SelectedIndex = -1;
            CurrentCharacter = new Character();
        }

        private void OnExitClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Validation - ensure we're not stomping on an unsaved character, etc.
            //if (isCharacterRolled && !isCharacterSaved) { ErrorText = "Don't exit without saving! (TODO: Make this a pop-up question instead.)"; return; }

            this.Close();
        }



        private void OnRaceChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null == (sender as ComboBox)) return;
            ComboBox raceCombo = sender as ComboBox;
            if (null == raceCombo.SelectedValue) return;
            CurrentCharacter.Race = raceCombo.SelectedValue.ToString();
            ClassOptions = raceMgr.GetClassesWithPrimeForRace(CurrentCharacter.Race, classMgr);
        }

        private void OnClassChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null == (sender as ComboBox)) return;
            ComboBox classCombo = sender as ComboBox;
            if (classCombo.SelectedIndex < 0) return;   // Indicates a state reset, bail
            CurrentCharacter.Class = classMgr.ParseSelection( classCombo.SelectedValue.ToString() );
            CanCharacterRoll = true;
        }

        /// <summary> Called when the user clicks the Roll button, which is only active after a race and class is selected </summary>
        /// <param name="sender"> The Roll button </param>
        /// <param name="e"> Auto-filled </param>
        private void RollClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentCharacter.Name)) { ErrorText = "Please Choose a Name Before Rolling!"; return; }

            // There are SO many different ways one can go with how rolls are assigned, even in official D&D books many methods are available - the most common method used in my experience
            //   is allowing a player to roll 6 sets of stats, then choosing where to assign each roll themselves. However for this tool I'm going to go with a more traditional "victim of RNG"
            //   rolling system where each attribute is hard-rolled, and then any attributes falling short of race requirements get re-rolls, and the Primary Stat gets best of 3 rolls

            // Roll main stats
            Dice statDice = new Dice(3,6);   // 3d6 for each stat
            CurrentCharacter.SetStat(StatTypes.STR, statDice.Roll());
            CurrentCharacter.SetStat(StatTypes.CON, statDice.Roll());
            CurrentCharacter.SetStat(StatTypes.DEX, statDice.Roll());
            CurrentCharacter.SetStat(StatTypes.INT, statDice.Roll());
            CurrentCharacter.SetStat(StatTypes.WIS, statDice.Roll());
            CurrentCharacter.SetStat(StatTypes.CHA, statDice.Roll());

            // Class Primary Stat should take best of 3, so let's do that here
            StatTypes priStat = classMgr.ClassesLUT[ CurrentCharacter.Class ].PrimaryStat;
            for (int i = 0; i<2; i++) {
                int newVal = statDice.Roll();
                if (newVal > CurrentCharacter.GetStat(priStat)) CurrentCharacter.SetStat(priStat, newVal);
            }

            // Re-roll any racial minimums as-needed (good planning in class setup let me do this so easily)
            foreach (Stat min in raceMgr.RacesLUT[ CurrentCharacter.Race ].StatMins ) {
                while ( CurrentCharacter.GetStat(min.StatType) < min.Value ) CurrentCharacter.SetStat( min.StatType, statDice.Roll() );
            }

            // Apply stat modifiers
            foreach (StatMod mod in raceMgr.RacesLUT[ CurrentCharacter.Race ].StatMods) {
                int curVal = CurrentCharacter.GetStat(mod.StatType);
                CurrentCharacter.SetStat(mod.StatType, curVal + mod.Value, curVal);   // The third parameter instructs to show the natural stat in parens after the modified stat for the sheet
            }

            // Set racial ability
            CurrentCharacter.Ability = raceMgr.RacesLUT[ CurrentCharacter.Race ].Ability;

            // Roll HP
            CurrentCharacter.MaxHitPoints = classMgr.ClassesLUT[ CurrentCharacter.Class ].HitDice.Roll();
            if      (CurrentCharacter.Constitution <  4) CurrentCharacter.MaxHitPoints -= 3;
            else if (CurrentCharacter.Constitution <  6) CurrentCharacter.MaxHitPoints -= 2;
            else if (CurrentCharacter.Constitution <  9) CurrentCharacter.MaxHitPoints -= 1;
            else if (CurrentCharacter.Constitution < 13) CurrentCharacter.MaxHitPoints += 0;
            else if (CurrentCharacter.Constitution < 16) CurrentCharacter.MaxHitPoints += 1;
            else if (CurrentCharacter.Constitution < 18) CurrentCharacter.MaxHitPoints += 2;
            else                                         CurrentCharacter.MaxHitPoints += 3;
            if (CurrentCharacter.MaxHitPoints < 1) CurrentCharacter.MaxHitPoints = 1;   // Can't go below 1

            // Roll Gold
            CurrentCharacter.Gold = statDice.Roll() * 10;

            // Hide the selection view and show the display view
            isCharacterRolled = true;
            SelectionView.Visibility = Visibility.Collapsed;
            DisplayView.Visibility = Visibility.Visible;
        }

    }
}