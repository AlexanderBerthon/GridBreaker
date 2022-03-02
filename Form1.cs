namespace GridBreaker {
    public partial class Form1 : Form {
        //global variables :(
        Button[] btnArray = new Button[100];
        
        public Form1() {
            InitializeComponent();
            flowLayoutPanel1.Controls.CopyTo(btnArray, 0);
        }


        private void grid_Click(object sender, EventArgs e) {
            //when clicked, run recursive function
            //get color of the cell, via tag (R, G, B, P)
            //
            Button clicked = (Button)sender;
            destroy(clicked);
            adjust();
            repopulate();
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

        }
    }
}