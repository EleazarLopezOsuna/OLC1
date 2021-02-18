using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OLC1_Proyecto2
{
    public partial class VisorAST : Form
    {
        public VisorAST()
        {
            InitializeComponent();
            pictureBox1.Left = (this.Width - pictureBox1.Width) / 2;
            pictureBox1.Top = (this.Height - pictureBox1.Height) / 2;

            pictureBox1.BackgroundImage = Image.FromFile(@"C:\Users\JavierG\Pictures\AST.jpg");
        }

        private void VisorAST_Load(object sender, EventArgs e)
        {

        }

        private void VisorAST_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
