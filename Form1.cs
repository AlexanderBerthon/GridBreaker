namespace GridBreaker {

    /// <summary>
    /// goal of the game is to get as many points as possible in the given turns you have
    /// the board will repopulate cells a limited number of times
    /// to get the most points, the player has to plan ahead to try to completely clear the board
    /// bonus points the larger the chain?
    /// negative points for left over boxes?
    /// is there a way to prevent user from clicking combo's less than 3?
    /// or maybe just offer low points
    /// or negative points?
    /// </summary>

    public partial class Form1 : Form {
        //global variables :(
        Button[] btnArray = new Button[100];
        Random random = new Random();
        int count = 50;


        public Form1() {
            InitializeComponent();
            flowLayoutPanel1.Controls.CopyTo(btnArray, 0);

            //randomly iterate through all buttons and assign them a random color?
            foreach(Button btn in btnArray) {
                int color = random.Next(1, 11);
                switch (color) {
                    case 1: btn.BackColor = Color.Red; break;
                    case 2: btn.BackColor = Color.Red; break;
                    case 3: btn.BackColor = Color.Red; break;
                    case 4: btn.BackColor = Color.Green; break;
                    case 5: btn.BackColor = Color.Green; break;
                    case 6: btn.BackColor = Color.Green; break;
                    case 7: btn.BackColor = Color.Blue; break;
                    case 8: btn.BackColor = Color.Blue; break;
                    case 9: btn.BackColor = Color.Blue; break;
                    case 10: btn.BackColor = Color.Yellow; break;
                }
            }
        }


        private void grid_Click(object sender, EventArgs e) {



            count--;
            Button clicked = (Button)sender;
            destroy(clicked);
            adjust();
            if (count > 0 && count%5 == 0) {
                repopulate();
            }
        }

        private void destroy(Button cell) {
            String color = cell.BackColor.ToString();
            cell.BackColor = Color.White;
            this.Refresh();
            //check left
            try {
                if (btnArray[cell.TabIndex - 1].BackColor.ToString() == color) {
                    destroy(btnArray[cell.TabIndex - 1]);
                }
            }
            catch (IndexOutOfRangeException e) {}
            //check right
            try {
            if (btnArray[cell.TabIndex + 1].BackColor.ToString() == color) {
                    destroy(btnArray[cell.TabIndex + 1]);
                }
            }
            catch (IndexOutOfRangeException e) {}
            try {
            //check up
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

        private void repopulate() {
            Boolean populating = true;
            while (populating) {
                populating = false;
                foreach (Button btn in btnArray) {
                    if (btn.BackColor == Color.White) {
                        populating = true;
                        int color = random.Next(1, 11);
                        switch (color) {
                            case 1: btn.BackColor = Color.Red; break;
                            case 2: btn.BackColor = Color.Red; break;
                            case 3: btn.BackColor = Color.Red; break;
                            case 4: btn.BackColor = Color.Green; break;
                            case 5: btn.BackColor = Color.Green; break;
                            case 6: btn.BackColor = Color.Green; break;
                            case 7: btn.BackColor = Color.Blue; break;
                            case 8: btn.BackColor = Color.Blue; break;
                            case 9: btn.BackColor = Color.Blue; break;
                            case 10: btn.BackColor = Color.Yellow; break;
                        }
                    }
                }
            }
        }


        private void repopulate_Old() {
            for (int i = 0; i < 10; i++) {
                int color = random.Next(1, 11);
                switch (color) {
                    case 1: btnArray[i].BackColor = Color.Red; break;
                    case 2: btnArray[i].BackColor = Color.Red; break;
                    case 3: btnArray[i].BackColor = Color.Red; break;
                    case 4: btnArray[i].BackColor = Color.Green; break;
                    case 5: btnArray[i].BackColor = Color.Green; break;
                    case 6: btnArray[i].BackColor = Color.Green; break;
                    case 7: btnArray[i].BackColor = Color.Blue; break;
                    case 8: btnArray[i].BackColor = Color.Blue; break;
                    case 9: btnArray[i].BackColor = Color.Blue; break;
                    case 10: btnArray[i].BackColor = Color.Yellow; break;
                }
            }

        }


    }
}