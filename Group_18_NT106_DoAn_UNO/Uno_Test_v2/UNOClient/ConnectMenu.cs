using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace CLIENT
{
    public partial class ConnectMenu : Form
    {
        //no static at first 
        public static WaitingRoom WaitingRoom;
        public ConnectMenu()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // TODO: Adding checking for texbox's emptyness, legal values ...
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(textBoxIP.Text), 11000);
            ProcessSocket.Data = "CONNECT";
            ProcessSocket.LINK(serverEP);
            WaitingRoom = new WaitingRoom();
            ProcessSocket.SENDER(textBoxName.Text);

            User.ID = textBoxName.Text;

            WaitingRoom.FormClosed += new FormClosedEventHandler(lobby_FormClosed);
            WaitingRoom.ShowStartButton();
            this.Hide();
            WaitingRoom.Show();
        }

        void lobby_FormClosed(object sender, EventArgs e)
        {
            ProcessSocket.Data = "DISCONNECT";
            ProcessSocket.SENDER(User.ID);
            ProcessSocket.SKCL.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            ProcessSocket.SKCL.Close();
            this.Show();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            // TODO: Adding checking for texbox's emptyness, legal values ...
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(textBoxIP.Text), 11000);
            ProcessSocket.Data = "CONNECT";
            ProcessSocket.LINK(serverEP);
            WaitingRoom = new WaitingRoom();
            ProcessSocket.SENDER(textBoxName.Text);

            User.ID = textBoxName.Text;

            WaitingRoom.FormClosed += new FormClosedEventHandler(lobby_FormClosed);
            this.Hide();
            WaitingRoom.Show();
        }

        private void btnRules_Click(object sender, EventArgs e)
        {           
        }
    }
}
