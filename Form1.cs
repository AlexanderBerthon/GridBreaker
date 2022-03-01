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
            solve(clicked);

        }

        private void solve(Button cell) {
            String color = cell.BackColor.ToString();
            cell.BackColor = Color.White;
            this.Refresh();
            //check left
            try {
                if (btnArray[cell.TabIndex - 1].BackColor.ToString() == color) {
                    solve(btnArray[cell.TabIndex - 1]);
                }
            }
            catch (IndexOutOfRangeException e) {}
            //check right
            try {
            if (btnArray[cell.TabIndex + 1].BackColor.ToString() == color) {
                    solve(btnArray[cell.TabIndex + 1]);
                }
            }
            catch (IndexOutOfRangeException e) {}
            try {
            //check up
            if (btnArray[cell.TabIndex - 10].BackColor.ToString() == color) {
                    solve(btnArray[cell.TabIndex - 10]);
                }
            }
            catch (IndexOutOfRangeException e) { }
            //check down
            try {
            if (btnArray[cell.TabIndex + 10].BackColor.ToString() == color) {
                    solve(btnArray[cell.TabIndex + 10]);
                }
            }
            catch (IndexOutOfRangeException e) { }
        }

    }
}