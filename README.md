# Dragons & Dungeons

Dragons &amp; Dungeons Tools (Please don't sue me WotC!)

## Abstract / Update

This was a project given as a take-home code assignment for a job interview process. I want to point out that the instructions (in a PDF file inside the character sheet folder) expressly said NOT to take more than 3-4 hours. So I followed those instructions. I wrote 4 hours of code, then did around 90 minutes of testing, debugging and this documentation.

Today I received a rejection letter letting me know they accepted a candidate who clearly spent 20+ hours on the assignment given the features the hiring manager extolled. Which was well, well beyond the 4 hours given. So they hired someone who clearly had no self-respect, didn't respect their own time let alone the company's, and if it had been a Project Manager who had said "you have 4 hours to work on it" and I spent 20, I'd be in some deep shit for going over the given time by a factor of 5. Clearly the company was fishing for people to abuse, so I hope the person who was hired enjoys their abuse, and I hope they grow a pair and don't stick with the studio for long.

## Character Sheet

Inside the folder for the character sheet project you will find the files (including pre-compiled debug executable) necessary to build and/or run the project. There is also a PDF file containing the instructions for the project in the folder.

### Architecture

I chose to use WPF/XAML for this project as it's the fastest UI prototyping tool framework I know and allowed me to get a lot of solid UI work done quickly and easily. I chose C# as the language as C# is one of my core langugaes and typically the smartest choice for a project of this nature with WPF/XAML. There are many much heavier frameworks and languages I could have used, but the goal was to get this banged out fast and I used the tools I know best for that!

### Building

This project was built on Visual Studio 2022 Community Edition with a build target of .Net 8.0 (not Core or Framework, please note). You will need VS2022 of any edition installed in order to load and compile this project. If you don't have access to VS2022, a pre-compiled executable for Windows can be found in the `bin` folder.

To load and build the project, simply double-click the .sln file - VS2022 should load the project up. From this point, you can either click the Quick Play button to run it immediately, or setup a build for a specific mode (Debug/Prod) and architecture. Output will be in the `bin` folder.

### Assumptions

I spent around 5 hours working on this project, it's for a code exercise, and it's missing a vast amount of polish and good practice - those things take considerable time. But this project meets the requirements laid-out in the PDF file.

I spent a fair amount of time architecting the data/classes so they would be as flexible and re-usable as possible for future considerations, including the ask that classes and races be editable and users would eventually want features to add new ones. A lot more stability and accessor restrictions could have been added but again those take time.

Rolling character stats was an area where I also didn't have time to offer a wide selection of options to users such as the Player's Handbook offers for the real game. I went with a fairly old school "RNG Hell" attribute rolling method, with a little concession for class primary stat (best of 3 rolls) and racial minimums.

### Loading & Saving

I really didn't have time to do this "the right way", which for me would be either a truly binary format or something like JSON or XML for compatibility and the potential for human-readability. I just used a quick and dirty text file write/read with the raw stats in a specific order which is really undesirable IMO, but gets the job done fast. Good file formatting and sanity takes time!

I really took a shortcut on loading, I wanted to do a whole file-picker thing, but instead if you want to load a character, you put the character's name in the name field, tab away from it (this is annoying, sorry!) then click the Load menu item. There's an error text display at the bottom of the window which will let you know if there's an issue.

Saving is pretty simple, once you've rolled a character you can save it and it uses the name of the character as the save file name, with `.sheet` appended to the end. The user never needs to know about this `.sheet` extension, it's transparent unless you're looking at the filesystem.

Right now loads and saves only operate from the same folder as the executable, so you will need to move any sheets around with your `.exe` if you change where you put it! But having the `.sheets` files satisfies the ability to share your characters with others.

### Testing

I could have spent a couple hours easily doing self-testing and debugging, instead I really just did a rough pass on core functionality without pushing for any edge cases - so please be aware some edge cases may be a bit broken!

### Thanks

For taking the time to review this little project!
