using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Office.App
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            pageMessages.Visible = false;
            pageClients.Visible = false;
        }

        #region Control box buttons

        private void btnClose_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion

        #region Dragging support

        Bunifu.Framework.UI.Drag dragWindow = new Bunifu.Framework.UI.Drag();

        private void controlBoxPanel_MouseDown(object sender, MouseEventArgs e)
        {
            dragWindow.Grab(this);
        }

        private void controlBoxPanel_MouseMove(object sender, MouseEventArgs e)
        {
            dragWindow.MoveObject(true, true);
        }

        private void controlBoxPanel_MouseUp(object sender, MouseEventArgs e)
        {
            dragWindow.Release();
        }

        #endregion

        private void btnHome_Click(object sender, EventArgs e)
        {
            lblMainHeader.Text = "Poruke";
            pageMessages.Visible = true;
            pageMessages.BringToFront();
        }

        private void btnClients_Click(object sender, EventArgs e)
        {
            lblMainHeader.Text = "Klijenti";
            pageClients.Visible = true;
            pageClients.BringToFront();
        }
    }
}
