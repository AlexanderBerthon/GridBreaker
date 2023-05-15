using System.Globalization;
using static System.Formats.Asn1.AsnWriter;
using System.Text.RegularExpressions;
/// Improvements
/// implement highscore menu?
namespace GridBreaker {

    /// <summary>
    /// goal of the game is to get as many points as possible within 30 seconds
    /// The grid will drop in new cells every 2 turns but only up to 10 times
    /// the number of points you get scales exponentially with the number of cells you combo together
    /// to get the highest score, a player must balance speed and strategy
    /// Aim for large combos but act quickly to utilize free refills and time limit 
    /// </summary>
    public partial class Form1 : Form {
        Button[] btnArray = new Button[100];
        Random random = new Random();
        int count = 50;
        int totalpoints = 0;
        int turnPoints = 0;

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        int clock = 300;

        Highscore[] highScores;


        /// <summary>
        /// Function that controls the timer
        /// </summary>
        /// <param name="anObject"></param> The timer object
        /// <param name="eventArgs"></param> The timer interval event
        private void TimerEventProcessor(Object anObject, EventArgs eventArgs) {
            clock--;
            if (clock <= 0) {
                timer.Stop();

                for (int i = 0; i < btnArray.Length; i++) {
                    btnArray[i].Enabled = false;
                }

                displayGameOver();
                this.Refresh();
            }
            else {
                //reset event clock
                timer.Stop();
                timer.Start();
            }

            if (clock % 60 < 10) {
                label4.Text = clock / 60 + ":0" + clock % 60;
            }
            else {
                label4.Text = clock / 60 + ":" + clock % 60;
            }
        }

        /// <summary>
        /// Form initialization. Initializes timer, button controls, and randomly creates the initial gameplay grid. 
        /// </summary>
        public Form1() {
            InitializeComponent();
            timer.Interval = 1000;
            timer.Start();
            timer.Tick += new EventHandler(TimerEventProcessor);
            flowLayoutPanel1.Controls.CopyTo(btnArray, 0);

            //Initialize highscore variables
            string[] inputData;
            highScores = new Highscore[5];

            //create the highscore file if it doesn't exist
            if (!File.Exists("C:\\Users\\" + Environment.UserName + "\\Desktop\\GridBreaker_Highscores.txt")) {
                string[] temp = { "Jeff 0", "Kenny 0", "Taylor 0", "Alex 0", "Martin 0" };
                File.WriteAllLines("C:\\Users\\" + Environment.UserName + "\\Desktop\\GridBreaker_Highscores.txt", temp); //creates files and populates with dummy data
            }

            inputData = System.IO.File.ReadAllLines("C:\\Users\\" + Environment.UserName + "\\Desktop\\GridBreaker_Highscores.txt");

            if (inputData.Length > 0) {
                for (int i = 0; i < inputData.Length; i++) {
                    string[] split = new string[2];
                    split = inputData[i].Split(" ");
                    highScores[i] = new Highscore(split[0], int.Parse(split[1]));
                }
            }


            //randomly iterate through all buttons and assign them a random color?
            foreach (Button btn in btnArray) {
                int color = random.Next(1, 10);
                switch (color) {
                    case 1: btn.BackColor = Color.DarkOrange; break;
                    case 2: btn.BackColor = Color.DarkOrange; break;
                    case 3: btn.BackColor = Color.DarkOrange; break;
                    case 4: btn.BackColor = Color.DarkGreen; break;
                    case 5: btn.BackColor = Color.DarkGreen; break;
                    case 6: btn.BackColor = Color.DarkGreen; break;
                    case 7: btn.BackColor = Color.Firebrick; break;
                    case 8: btn.BackColor = Color.Firebrick; break;
                    case 9: btn.BackColor = Color.Firebrick; break;
                    //case 10: btn.BackColor = Color.BlueViolet; break;
                }
            }

            label1.Text = "Refill in: " + ((count % 2)+2)+" moves";
            label2.Text = count / 2 + " Refills remaining";
        }

        /// <summary>
        /// Button click event function.
        /// 
        /// This function is responsible for the majority of the gameplay logic and control
        /// 
        /// On click the function will check for valid input before calling the necessary helper functions.
        /// 
        /// Good input will call the recursive destroy function to process the player move,
        /// then clean up the board by dropping cells into any empty spaces created by the destroy function
        /// 
        /// Also contains control for the repopulate/refill function 
        /// by tracking the number of turns that have occured and refilling as needed
        /// </summary>
        /// <param name="sender"></param> Button object
        /// <param name="e"></param> Button click event
        private void grid_Click(object sender, EventArgs e) {
            Button clicked = (Button)sender;
            if (clicked.BackColor != Color.White) {
                destroy(clicked);
                totalpoints += turnPoints;
                turnPoints = 0;
                label3.Text = "Score: "+totalpoints.ToString();
                adjust();
                if (count > 0) {
                    count--;
                    label1.Text = "Refill in: " + count % 2 + " moves";
                    if (count == 0) {
                        repopulate();
                        label1.Text = "Refill in: " + 0 + " moves";
                        label2.Text = 0 + " Refills remaining";
                    }
                    else if (count % 2 == 0) {
                        repopulate();
                        label1.Text = "Refill in: " + ((count % 2) + 2) + " moves";
                        label2.Text = count / 2 + " Refills remaining";
                    }
                }
            }
        }

        /// <summary>
        /// Recursive function responsible for clearing matching cells
        /// When a player clicks a colored cell on the grid, this function will check all adjacent cells
        /// Cells that match will be marked for deletion and similarly run through this function, checking those cells for matches as well.
        /// This creates a chain reaction that will destroy all matching adjacent tiles
        /// </summary>
        /// <param name="cell"></param> button cell to be compared
        private void destroy(Button cell) {
            turnPoints += 5;
            turnPoints = (int)Math.Floor(turnPoints * 1.05);
            String color = cell.BackColor.ToString();
            cell.BackColor = Color.White;
            this.Refresh();
            //check left
            try {
                if (btnArray[cell.TabIndex - 1].BackColor.ToString() == color) {
                    if (btnArray[cell.TabIndex - 1].Location.Y == cell.Location.Y) {
                        destroy(btnArray[cell.TabIndex - 1]);
                    }
                }
            }
            catch (IndexOutOfRangeException e) { }
            //check right
            try {
                if (btnArray[cell.TabIndex + 1].BackColor.ToString() == color) {
                    if (btnArray[cell.TabIndex + 1].Location.Y == cell.Location.Y) {
                        destroy(btnArray[cell.TabIndex + 1]);
                    }
                }
            }
            catch (IndexOutOfRangeException e) { }
            //check up
            try {
                if (btnArray[cell.TabIndex - 10].BackColor.ToString() == color) {
                    destroy(btnArray[cell.TabIndex - 10]);
                }
            }
            catch (IndexOutOfRangeException e) { }
            //check down
            try {
                if (btnArray[cell.TabIndex + 10].BackColor.ToString() == color) {
                    destroy(btnArray[cell.TabIndex + 10]);
                }
            }
            catch (IndexOutOfRangeException e) { }
        }

        /// <summary>
        /// Function responsible for filling in holes in the grid caused by the destroy function
        /// moves cells down one if the cell below is empty, simulates gravity
        /// </summary>
        private void adjust() {
            Boolean moving = true;
            while (moving) {
                this.Refresh();
                moving = false;
                for(int i = 89; i >= 0; i--) {
                    if(btnArray[i].BackColor != Color.White && btnArray[i+10].BackColor == Color.White) {
                        moving = true;
                        btnArray[i + 10].BackColor = btnArray[i].BackColor;
                        btnArray[i].BackColor = Color.White;
                    }
                }
            }
        }

        /// <summary>
        /// Function responsible for the refill mechaninc
        /// randomly assigns colored cells to the top row when called
        /// these cells remain in the top row for one turn, before ajust is called to give the player a chance to react
        /// to the new incoming cells
        /// </summary>
        private void repopulate() {
            for (int i = 0; i < 10; i++) {
                int color = random.Next(1, 10);
                switch (color) {
                    case 1: btnArray[i].BackColor = Color.DarkOrange; break;
                    case 2: btnArray[i].BackColor = Color.DarkOrange; break;
                    case 3: btnArray[i].BackColor = Color.DarkOrange; break;
                    case 4: btnArray[i].BackColor = Color.DarkGreen; break;
                    case 5: btnArray[i].BackColor = Color.DarkGreen; break;
                    case 6: btnArray[i].BackColor = Color.DarkGreen; break;
                    case 7: btnArray[i].BackColor = Color.Firebrick; break;
                    case 8: btnArray[i].BackColor = Color.Firebrick; break;
                    case 9: btnArray[i].BackColor = Color.Firebrick; break;
                    //case 10: btnArray[i].BackColor = Color.BlueViolet; break;
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContinueButton_Click(object sender, EventArgs e) {
            highscorePanel.Visible = false;

            for (int i = 0; i < btnArray.Length; i++) {
                btnArray[i].Enabled = true;
            }

            count = 20;
            totalpoints = 0;
            turnPoints = 0;
            clock = 30;

            label1.Text = "Refill in: " + ((count % 2) + 2) + " moves";
            label2.Text = count / 2 + " Refills remaining";
            label3.Text = "0:30";
            label4.Text = "Score: 0";

            //randomly iterate through all buttons and assign them a random color?
            foreach (Button btn in btnArray) {
                int color = random.Next(1, 10);
                switch (color) {
                    case 1: btn.BackColor = Color.DarkOrange; break;
                    case 2: btn.BackColor = Color.DarkOrange; break;
                    case 3: btn.BackColor = Color.DarkOrange; break;
                    case 4: btn.BackColor = Color.DarkGreen; break;
                    case 5: btn.BackColor = Color.DarkGreen; break;
                    case 6: btn.BackColor = Color.DarkGreen; break;
                    case 7: btn.BackColor = Color.Firebrick; break;
                    case 8: btn.BackColor = Color.Firebrick; break;
                    case 9: btn.BackColor = Color.Firebrick; break;
                    //case 10: btn.BackColor = Color.BlueViolet; break;
                }
            }

            timer.Start();
        }

        /// <summary>
        /// Exits the application when the button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        /// <summary>
        /// Helper function to update highscore sheet
        /// error checking on user input to ensure proper format
        /// </summary>
        private void confirmUserInputButton_Click(object sender, EventArgs e) {
            String userInput = "";
            Regex regex = new Regex("[0-9]");
            if (newHighscoreTextbox.Text != null) {
                userInput = newHighscoreTextbox.Text;

                if (regex.IsMatch(userInput)) {
                    userInputErrorLabel.Text = "Error: no numbers allowed";
                    userInputErrorLabel.Visible = true;
                }
                else if (userInput.Contains(" ")) {
                    userInputErrorLabel.Text = "Error: no spaces allowed";
                    userInputErrorLabel.Visible = true;
                }
                else if (userInput.Length < 1) {
                    userInputErrorLabel.Text = "Error: please enter a name";
                    userInputErrorLabel.Visible = true;
                }
                else {
                    //add new highscore to list
                    /*
                    //convert score into readable version
                    String score = totalpoints.ToString(); //convert score to a string
                    String format = "";                    //temp variable for formatting
                    int count = 0;                         //temp variable for formatting

                    //add commas
                    for (int i = score.Count(); i > 0; i--) {
                        format += score[i - 1];
                        count++;
                        if (count == 3) {
                            count = 0;
                            format += ",";
                        }
                    }

                    score = "";

                    //reverse
                    for (int i = format.Count(); i > 0; i--) {
                        score += format[i - 1];
                    }

                    //remove leading comma where score % 3 = 0
                    //it's ugly but easier than adding an exception to the logic above
                    if (score.Substring(0, 1) == ",") {
                        score = score.Remove(0, 1);
                    }
                    */



                    highScores[4] = new Highscore(newHighscoreTextbox.Text, totalpoints);

                    Array.Sort(highScores, Highscore.SortScoreAcending());

                    //close new highscore menu
                    newHighscorePanel.Visible = false;

                    //populate highscore board
                    highscoreName1.Text = highScores[0].getName();
                    highscoreName2.Text = highScores[1].getName();
                    highscoreName3.Text = highScores[2].getName();
                    highscoreName4.Text = highScores[3].getName();
                    highscoreName5.Text = highScores[4].getName();
                    highscore1.Text = highScores[0].getScore().ToString();
                    highscore2.Text = highScores[1].getScore().ToString();
                    highscore3.Text = highScores[2].getScore().ToString();
                    highscore4.Text = highScores[3].getScore().ToString();
                    highscore5.Text = highScores[4].getScore().ToString();

                    //display highscore board
                    highscorePanel.Visible = true;
                    continueButton.Visible = true;
                    exitButton.Visible = true;
                    playAgainLabel.Visible = true;

                    String[] temp = new string[5];

                    //write to file
                    for (int i = 0; i < 5; i++) {
                        temp[i] = highScores[i].getName() + " " + highScores[i].getScore().ToString();
                    }

                    File.WriteAllLines("C:\\Users\\" + Environment.UserName + "\\Desktop\\Gridbreaker_Highscores.txt", temp);
                }
            }
        }

        /// <summary>
        /// Helper function that clears error message upon user interaction on text box
        /// Prevents a permanent error message showing , makes it more clear that format is incorrect on multiple user attempts at adding a new highscore
        /// </summary>
        private void NewHighScoreTextBox_TextChanged(object sender, EventArgs e) {
            userInputErrorLabel.Visible = false;
        }

        private void displayGameOver() {
            Boolean newHighScore = false;

            //check for new highscore
            for (int i = 0; i < 5; i++) {
                if (totalpoints >= highScores[i].getScore()) {
                    newHighScore = true;
                }
            }

            if (newHighScore) {
                //display new highscore UI
                newHighscorePanel.Visible = true;
            }
            else {
                //populate highscore board
                highscoreName1.Text = highScores[0].getName();
                highscoreName2.Text = highScores[1].getName();
                highscoreName3.Text = highScores[2].getName();
                highscoreName4.Text = highScores[3].getName();
                highscoreName5.Text = highScores[4].getName();
                highscore1.Text = highScores[0].getScore().ToString();
                highscore2.Text = highScores[1].getScore().ToString();
                highscore3.Text = highScores[2].getScore().ToString();
                highscore4.Text = highScores[3].getScore().ToString();
                highscore5.Text = highScores[4].getScore().ToString();

                //display gameover UI
                highscorePanel.Visible = true;
                playAgainLabel.Visible = true;
                continueButton.Visible = true;
                exitButton.Visible = true;
            }
        }


    }
}

