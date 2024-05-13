using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace UNOServer
{

    class Program
    {
        private static Socket SKSV;
        private static Socket SKCL;
        private static Thread Thread1;
        private static List<USER> USERLIST = new List<USER>();
        private static int MacDinh = 1;
        private static bool TakeTurn = true;
        private static int RUT = 0;
        private static string MAU = "";









        static void Main(string[] args)
        {
            SKSV = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
            SKSV.Bind(serverEP);
            SKSV.Listen(4);
            Console.WriteLine("Server Duoc tao thanh cong");

            while (true)
            {
                SKCL = SKSV.Accept();
             
                Thread1 = new Thread(() => HandleNewUser(SKCL));
                Thread1.Start();
            }
        }

        public static void HandleNewUser(Socket client)
        {
            USER User = new USER();
            User.UserSK = client;
            USERLIST.Add(User);

            byte[] data = new byte[1024];

            while (User.UserSK.Connected)
            {
                if (User.UserSK.Available > 0)
                {
                    string receivedata = "";

                    while (User.UserSK.Available > 0)
                    {
                        int bRead = User.UserSK.Receive(data);
                        receivedata += Encoding.UTF8.GetString(data, 0, bRead);
                    }

                    Console.WriteLine(User.UserSK.RemoteEndPoint + ": " + receivedata);
                    ENCRYTINGREADER(receivedata, User);
                }
            }
      
        }


        //Setting up turn for each user
        public static void SettingUpTurn()
        {
            int[] turns = new int[USERLIST.Count];

            for (int i = 1; i <= USERLIST.Count; i++)
            {
                turns[i - 1] = i;
            }

            Random rd = new Random();
            foreach (var user in USERLIST)
            {
                int pick = rd.Next(turns.Length);
                user.Luot = turns[pick];
                turns = turns.Where(val => val != turns[pick]).ToArray();


                user.SoLuongBai = 7;
            }
        }

        public static void XaoBai()
        {
            Random rd = new Random();
            XAPBAI.CardName = XAPBAI.CardName.OrderBy(x => rd.Next()).ToArray();
        }

        public static string CreateUserCard()
        {
            Random rd = new Random();
            string usercard = "";
            for (int i = 0; i < 7; i++)
            {
                int pick = rd.Next(XAPBAI.CardName.Length);
                usercard += XAPBAI.CardName[pick] + ";";
                XAPBAI.CardName = XAPBAI.CardName.Where(val => val != XAPBAI.CardName[pick]).ToArray();
            }
            return usercard;
        }

        public static string ShowPileCard()
        {

            string temp = XAPBAI.CardName[0];
            XAPBAI.CardName = XAPBAI.CardName.Where(val => val != XAPBAI.CardName[0]).ToArray();
            MoBai.mobai.Add(temp);
            return temp;
        }

        public static bool ISOVER()
        {
            if (XAPBAI.CardName.Length == 0)
                return true;
            return false;
        }

        public static void BroadcastBack(string type, string receivedata)
        {
            foreach (var user in USERLIST)
            {
                byte[] data = Encoding.UTF8.GetBytes(type + receivedata);
                user.UserSK.Send(data);
            }
        }




        private static void HandleConnect(string[] Signal, USER User)
        {
            User.ID = Signal[1];
            foreach (var user in USERLIST)
            {
                byte[] data = Encoding.UTF8.GetBytes("Case1;" + user.ID);
                User.UserSK.Send(data);
                Thread.Sleep(210);
            }
            foreach (var user in USERLIST)
            {
                if (user.UserSK != User.UserSK)
                {
                    byte[] data = Encoding.UTF8.GetBytes("Case1;" + User.ID);
                    user.UserSK.Send(data);
                    Thread.Sleep(210);
                }
            }
        }

        private static void HandleDisconnect(string[] Signal)
        {
            foreach (var user in USERLIST.ToList())
            {
                if (user.ID == Signal[1])
                {
                    user.UserSK.Shutdown(SocketShutdown.Both);
                    user.UserSK.Close();
                    USERLIST.Remove(user);
                }
            }
        }

        private static void HandleStart(string[] Signal, USER User)
        {
            SettingUpTurn();
            USERLIST.Sort((x, y) => x.Luot.CompareTo(y.Luot));
            XaoBai();
            XAPBAI.currentCard = ShowPileCard();
            foreach (var user in USERLIST)
            {
               
                string SendData = "Case2;" + user.ID + ";" + user.Luot + ";" + user.SoLuongBai + ";" + CreateUserCard() + XAPBAI.currentCard;
                byte[] data = Encoding.UTF8.GetBytes(SendData);
                user.UserSK.Send(data);
              
                Thread.Sleep(100);
            }


            foreach (var user in USERLIST)
            {
                foreach (var player_ in USERLIST)
                {
                    if (user.ID != player_.ID)
                    {
                        string SendData = "Case3;" + player_.ID + ";" + player_.Luot + ";" + player_.SoLuongBai;
                        byte[] data = Encoding.UTF8.GetBytes(SendData);
                        user.UserSK.Send(data);
                   
                        Thread.Sleep(100);
                    }
                }
            }

            foreach (var user in USERLIST)
            {
                string SendData = "Case4;" + user.ID;
                byte[] data = Encoding.UTF8.GetBytes(SendData);
                user.UserSK.Send(data);
         
                Thread.Sleep(100);
            }

            foreach (var user in USERLIST)
            {
                string SendData_ = "Case6;" + USERLIST[MacDinh - 1].ID;
                byte[] buffer_ = Encoding.UTF8.GetBytes(SendData_);
                user.UserSK.Send(buffer_);
               
                Thread.Sleep(100);
            }
        }

        private static void HandleDanhBai(string[] Signal, USER User)
        {
            if (int.Parse(Signal[2]) == 0)
            {
                foreach (var user in USERLIST)
                {
                    string SendData = "Case9;" + Signal[1] + ";" + Signal[2] + ";" + Signal[3];
                    byte[] data = Encoding.UTF8.GetBytes(SendData);
                    user.UserSK.Send(data);

                    Thread.Sleep(100);
                }
            }
            else { 
            XAPBAI.currentCard = Signal[3];
            MoBai.mobai.Add(Signal[3]);
            USERLIST[MacDinh - 1].SoLuongBai = int.Parse(Signal[2]);

            if (USERLIST[MacDinh - 1].SoLuongBai == 0)
            {
                foreach (var user in USERLIST)
                {
                    string SendData = "Case9;" + Signal[1] + ";" + Signal[2] + ";" + Signal[3];
                    byte[] data = Encoding.UTF8.GetBytes(SendData);
                    user.UserSK.Send(data);

                    Thread.Sleep(100);
                }
            }
            else
            {
                foreach (var user in USERLIST)
                {
                    if (user.Luot != MacDinh)
                    {
                        string SendData = "Case5;" + Signal[1] + ";" + Signal[2] + ";" + Signal[3];
                        if (Signal[3].Contains("df"))
                        {
                            SendData += ";" + Signal[4];
                        }
                        byte[] data = Encoding.UTF8.GetBytes(SendData);
                        user.UserSK.Send(data);

                        Thread.Sleep(100);
                    }
                }

                if (Signal[3].Contains("dt"))
                    RUT += 2;
                if (Signal[3].Contains("df"))
                {
                    MAU = Signal[4];
                    RUT += 4;
                }


                if (Signal[3].Contains("rv"))
                {
                    if (TakeTurn == true)
                        TakeTurn = false;
                    else
                        TakeTurn = true;
                }


                if (TakeTurn == true)
                {
                    if (Signal[3].Contains("s"))
                    {
                        if (MacDinh == USERLIST.Count)
                        {
                            MacDinh = 2;
                        }
                        else
                        {
                            MacDinh = MacDinh + 2;
                        }
                    }
                    else
                    {
                        MacDinh++;
                    }
                }
                else
                {
                    if (Signal[3].Contains("s"))
                    {
                        if (MacDinh == 1)
                        {
                            MacDinh = USERLIST.Count - 1;
                        }
                        else
                        {
                            MacDinh = MacDinh - 2;
                        }
                    }
                    else
                    {
                        MacDinh--;
                    }
                }

                if (MacDinh > USERLIST.Count)
                    MacDinh = 1;

                if (MacDinh < 1)
                    MacDinh = USERLIST.Count;

                foreach (var user in USERLIST)
                {
                    string SendData_ = "Case6;" + USERLIST[MacDinh - 1].ID;
                    byte[] buffer_ = Encoding.UTF8.GetBytes(SendData_);
                    user.UserSK.Send(buffer_);

                    Thread.Sleep(100);
                }
            }
        }

        }

        private static void AddNewCard(string[] Signal, USER User)
        {
            USERLIST[MacDinh - 1].SoLuongBai = int.Parse(Signal[2]);
            string mkmsg = "Case7;" + Signal[1] + ";" + XAPBAI.CardName[0];
            XAPBAI.CardName = XAPBAI.CardName.Where(val => val != XAPBAI.CardName[0]).ToArray();
            // To add: check if drawpile is out of cards
            byte[] bf = Encoding.UTF8.GetBytes(mkmsg);
            USERLIST[MacDinh - 1].UserSK.Send(bf);

            foreach (var user in USERLIST)
            {
                if (user.Luot != MacDinh)
                {
                    string SendData = "Case5;" + Signal[1] + ";" + Signal[2];
                    byte[] data = Encoding.UTF8.GetBytes(SendData);
                    user.UserSK.Send(data);
                
                    Thread.Sleep(100);
                }
            }

            if (TakeTurn == true)
            {
                MacDinh++;
            }
            else
            {
                MacDinh--;
            }

            if (MacDinh > USERLIST.Count)
                MacDinh = 1;

            if (MacDinh < 1)
                MacDinh = USERLIST.Count;

            foreach (var user in USERLIST)
            {
                string SendData_ = "Case6;" + USERLIST[MacDinh - 1].ID;
                byte[] buffer_ = Encoding.UTF8.GetBytes(SendData_);
                user.UserSK.Send(buffer_);
               
                Thread.Sleep(100);
            }
        }

        private static void HandleSpecialCard(string[] Signal, USER User)
        {
            USERLIST[MacDinh - 1].SoLuongBai += RUT;
            string cardstack = "Case8;" + USERLIST[MacDinh - 1].ID + ";" + USERLIST[MacDinh - 1].SoLuongBai + ";";
            for (int i = 0; i < RUT; i++)
            {
                cardstack += XAPBAI.CardName[0] + ";";
                XAPBAI.CardName = XAPBAI.CardName.Where(val => val != XAPBAI.CardName[0]).ToArray();
            }

            if (Signal[2] == "wd")
                cardstack += MAU;

            byte[] buff = Encoding.UTF8.GetBytes(cardstack);
            USERLIST[MacDinh - 1].UserSK.Send(buff);
            RUT = 0;
            Console.WriteLine("Sendback: " + cardstack);

            foreach (var user in USERLIST)
            {
                if (user.Luot != MacDinh)
                {
                    string SendData = "Case5;" + Signal[1] + ";" + USERLIST[MacDinh - 1].SoLuongBai;
                    if (Signal[2] == "wd")
                        SendData += ";" + MAU;

                    byte[] data = Encoding.UTF8.GetBytes(SendData);
                    user.UserSK.Send(data);
                   
                    Thread.Sleep(100);
                }
            }
            MAU = "";
            UpdateTurnAndNotifyUsers();
        }
        private static void UpdateTurnAndNotifyUsers()
        {
            if (TakeTurn == true)
            {
                MacDinh++;
            }
            else
            {
                MacDinh--;
            }

            if (MacDinh > USERLIST.Count)
                MacDinh = 1;

            if (MacDinh < 1)
                MacDinh = USERLIST.Count;

            foreach (var user in USERLIST)
            {
                string SendData_ = "Case6;" + USERLIST[MacDinh - 1].ID;
                byte[] buffer_ = Encoding.UTF8.GetBytes(SendData_);
                user.UserSK.Send(buffer_);

                Thread.Sleep(100);
            }
        }


        public static void ENCRYTINGREADER(string receivedata, USER User)
        {
            string[] Signal = receivedata.Split(';');

            switch (Signal[0])
            {
                case "CONNECT":
                    HandleConnect(Signal, User);
                    break;
                case "DISCONNECT":
                    HandleDisconnect(Signal);
                    break;
                case "START":
                    HandleStart(Signal, User);
                    break;
                case "DanhBai":
                    HandleDanhBai(Signal, User);
                    break;
                case "AddNewCard":
                    AddNewCard(Signal, User);
                    break;
                case "SpecialCardDT":
                    HandleSpecialCard(Signal, User);
                    break;
                default:
                    break;
            }
        }



    }



    //addition classes to store the card deck, the discarded pile of cards and the user's information
    // Class to store the card deck
    class XAPBAI
    {
        public static string currentCard = "";
        public static string[] CardName =
            {
                "r0", "r1", "r2", "r3", "r4", "r5", "r1_", "r2_", "r3_", "r4_", "r5_",
                "b0", "b1", "b2", "b3", "b4", "b5", "b1_", "b2_", "b3_", "b4_", "b5_",
                "y0", "y1", "y2", "y3", "y4", "y5", "y1_", "y2_", "y3_", "y4_", "y5_",
                "g0", "g1", "g2", "g3", "g4", "g5", "g1_", "g2_", "g3_", "g4_", "y5_",
                "Rv_r", "Rv_r_X", "Rv_b", "Rv_b_X", "Rv_y", "Rv_y_X", "Rv_g", "Rv_g_X",
                "s_r", "s_r_X", "s_b", "s_b_X", "s_y", "s_y_X", "s_g", "s_g_X",
                "wd", "wd_X", "wd_Y", "wd_Z",
                "df", "df_X", "df_Y", "df_Z",
                "dt_r", "dt_r_X", "dt_b", "dt_b_X", "dt_y", "dt_y_X", "dt_g", "dt_g_X"
            };
    }

    // Class to store the discarded pile of cards
    class MoBai
    {
        public static List<string> mobai = new List<string>();
    }
    // Class to store the user's information
    class USER
    {
        public string ID { get; set; }
        public int SoLuongBai { get; set; }
        public int Luot { get; set; }
        public bool RoomOwner { get; set; }
        public Socket UserSK { get; set; }
    }

    
}
