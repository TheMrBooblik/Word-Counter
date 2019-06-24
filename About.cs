using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Проект_курсовая_
{
    public partial class About : Form
    {
        public int N = 0;
        public About()
        {
            InitializeComponent();
        }

        private void tAbout_Tick(object sender, EventArgs e) // таймер
        {
            progressBar1.Maximum = 95;
            if (progressBar1.Value < 95)
                progressBar1.Value++;
            N++;
            if (N==100)
            {
                Close();
                N = 0;
                progressBar1.Value = 0;

            }
           

        }

        private void About_KeyPress(object sender, KeyPressEventArgs e)
        
            {
                if (e.KeyChar == (char)27)
                    Close();
            }

        private void button1_Click(object sender, EventArgs e) // кнопка закрыть
        {
            progressBar1.Value = 0;
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
