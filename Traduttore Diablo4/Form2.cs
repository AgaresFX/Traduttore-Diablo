using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Traduttore_Diablo4
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr uno, int due, int tre, int quattro);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            RelaseCapture();
        }

        private void RelaseCapture()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        private void lblMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string email = "dimassimoweb@gmail.com";
            string subject = "Oggetto della mail";
            string body = "Corpo della mail";
            string mailto = $"mailto:{email}?subject={Uri.EscapeUriString(subject)}&body={Uri.EscapeUriString(body)}";

            try
            {
                Process.Start(mailto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossibile aprire il client di posta: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
