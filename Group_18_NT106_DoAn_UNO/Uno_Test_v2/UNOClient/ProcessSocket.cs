using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.ComponentModel.Design;

namespace CLIENT
{
    static class Program
    {
     
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConnectMenu());
        }
    }
    class User
    {
        public static string ID { get; set; }
        public static int LUOT { get; set; }
        public static int SOLUONGBAI { get; set; }
        public static List<string> BAI = new List<string>();
    }

    class USER2
    {
        public string ID { get; set; }
        public string LUOT { get; set; }
        public string SOLUONGBAI { get; set; }
    }
    class ProcessSocket
    {
        public static Socket SKCL;
        public static Thread Thread;
        public static string Data = "";

        public static void LINK(IPEndPoint serverEP)
        {
            SKCL = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SKCL.Connect(serverEP);
            Thread = new Thread(() => ANALYZE());
            Thread.Start();
        }
        public static GAMEROOM room;
        public static List<USER2> otheruser;
        
        // Xử lý tin nhắn khởi tạo
        private static void HandleCase2(string[] payload)
        {
            User.LUOT = int.Parse(payload[2]);
            User.SOLUONGBAI = int.Parse(payload[3]);
            for (int i = 4; i <= 10; i++)
            {
                User.BAI.Add(payload[i]);
            }

            room = new GAMEROOM();
            otheruser = new List<USER2>();
            ConnectMenu.WaitingRoom.Invoke((MethodInvoker)delegate ()
            {
                room.currentCard = payload[11];
                room.SettingUpCard();
                room.ShowCurrentCard();
                room.Show();
            });
        }
        // Xử lý tin nhắn thông tin người chơi khác
        private static void HandleCase3(string[] payload)
        {
            USER2 otherPlayer = new USER2();
            otherPlayer.ID = payload[1];
            otherPlayer.LUOT = payload[2];
            otherPlayer.SOLUONGBAI = payload[3];
            otheruser.Add(otherPlayer);
        }
        // Xử lý tin nhắn cập nhật
        private static void HandleUpdateMessage(string[] payload)
        {
            room.CARDSYNC(payload[1], payload[2]);
            if (payload.Length > 3)
            {
                room.currentCard = payload[3];
                room.ShowCurrentCard();
            }
        }
        // Xử lý tin nhắn lượt chơi
        private static void HandleTurnMessage(string[] payload)
        {
            if (payload[1] == User.ID)
                CK();

            room.NotTurn();
            room.ISTURN(payload[1]);
        }
        // Xử lý tin nhắn rút bài
        private static void ProcessRStack(string[] payload)
        {
            for (int i = 3; i < payload.Length; i++)
            {
                if (payload[i] == "r" || payload[i] == "r" || payload[i] == "r" || payload[i] == "r")
                    room.currentCard = payload[i];
                else
                {
                    room.ProcessBocBai(payload[i]);
                }
            }

            CK();
        }
        // Xử lý tin nhắn kết thúc ván chơi
        private static void HandleEndMessage(string[] payload)
        {
            string playerId = payload[1];
            string numCards = payload[2];
            
            if (User.ID == playerId)
            {
               
                    MessageBox.Show("You win!");
                
              
            }
            else
            {
              MessageBox.Show(playerId + " has won the game ");
            }

         
        }

        public static void SENDER(string data)
        {
            string SendDatastr = Data + ";" + data;
            byte[] SendData = Encoding.UTF8.GetBytes(SendDatastr);
            SKCL.Send(SendData);
        }

        public static void ANALYZE()
        {
            byte[] RData = new byte[1024];

            while (SKCL.Connected)
            {
                if (SKCL.Available > 0)
                {
                    string SendData = "";

                    while (SKCL.Available > 0)
                    {
                        int bRead = SKCL.Receive(RData);
                        SendData += Encoding.UTF8.GetString(RData, 0, bRead);
                    }

                    Process(SendData);
                   
                }
            }
        }
        // Xử lý các điều kiện để đánh bài
        public static void CK()
        {
            room.CANDRAW(); // Kích hoạt nút rút bài
            room.CANPLAY(); // Kích hoạt nút đánh bài

            // Tạo danh sách các loại thẻ cờ trên bàn
            List<string> cardTypes = new List<string> { "r", "y", "b", "g", "s", "Rv", "dt", "df" };

            foreach (var row in room.Card)
            {
                foreach (var bt in row)
                {
                    /*// Lấy số hoặc loại thẻ cờ từ ID của nút
                    string getnum = new String(bt.id.Where(Char.IsDigit).ToArray());
                    string getType = new String(bt.id.Where(Char.IsLetter).ToArray());

                    // Neu la la chuc nang
                    if ((bt.id.Contains("dt") || bt.id.Contains("rv") || bt.id.Contains("s")) && room.currentCard.Contains(getType))
                    {
                        bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                        bt.btn.Enabled = true;
                        continue;
                    }

                    // Nếu thẻ cờ trên bàn khớp với thẻ mở trên bàn
                    if (room.currentCard.Contains(getType) || room.currentCard.Contains(getnum))
                    {
                        bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                        bt.btn.Enabled = true;
                        continue;
                    }

                    // Nếu là thẻ màu Wild Draw Four
                    if (bt.id.Contains("wd"))
                    {
                        // Nếu không có thẻ Draw Two hoặc Draw Four trên bàn
                        if (!room.currentCard.Contains("dt") && !room.currentCard.Contains("df"))
                        {
                            bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                            bt.btn.Enabled = true;
                            continue;
                        }
                    }

                    // Nếu là thẻ màu Draw Four
                    if (bt.id.Contains("df"))
                    {
                        // Nếu không có thẻ Draw Two trên bàn
                        if (!room.currentCard.Contains("dt"))
                        {
                            bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                            bt.btn.Enabled = true;
                            continue;
                        }
                    }*/

                    string checknum = new String(room.currentCard.Where(Char.IsDigit).ToArray());
                    string getnum = new String(bt.id.Where(Char.IsDigit).ToArray());

                    if (checknum != "" && checknum == getnum)
                    {
                        bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                        bt.btn.Enabled = true;
                        continue;
                    }

                    if (room.currentCard.Contains("r"))
                    {
                        if (bt.id.Contains("r"))
                        {
                            bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                            bt.btn.Enabled = true;
                            continue;
                        }
                    }

                    if (room.currentCard.Contains("y"))
                    {
                        if (bt.id.Contains("y"))
                        {
                            bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                            bt.btn.Enabled = true;
                            continue;
                        }
                    }

                    if (room.currentCard.Contains("b"))
                    {
                        if (bt.id.Contains("b"))
                        {
                            bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                            bt.btn.Enabled = true;
                            continue;
                        }
                    }

                    if (room.currentCard.Contains("g"))
                    {
                        if (bt.id.Contains("g"))
                        {
                            bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                            bt.btn.Enabled = true;
                            continue;
                        }
                    }

                    if (room.currentCard.Contains("s"))
                    {
                        if (bt.id.Contains("s"))
                        {
                            bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                            bt.btn.Enabled = true;
                            continue;
                        }
                    }

                    if (room.currentCard.Contains("Rv"))
                    {
                        if (bt.id.Contains("Rv"))
                        {
                            bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                            bt.btn.Enabled = true;
                            continue;
                        }
                    }

                    if (room.currentCard.Contains("dt"))
                    {
                        if (bt.id.Contains("dt"))
                        {
                            bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                            bt.btn.Enabled = true;
                            continue;
                        }
                    }


                    if (bt.id.Contains("wd"))
                    {
                        if (!room.currentCard.Contains("dt") || !room.currentCard.Contains("df"))
                        {
                            bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                            bt.btn.Enabled = true;
                            continue;
                        }
                    }

                    if (bt.id.Contains("df"))
                    {
                        bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                        bt.btn.Enabled = true;
                        continue;
                    }
                    // Nếu không phù hợp với bất kỳ điều kiện nào, vô hiệu hóa nút
                    bt.btn.FlatAppearance.BorderColor = Color.Red;
                    bt.btn.Enabled = false;
                }
            }
        }

        private static void GAMEROOM_FormClosed(object sender, EventArgs e)
        {
            ConnectMenu.WaitingRoom.Show();
        }
        
        public static void Process(string sendData)
        {
            string[] arrPayload = sendData.Split(';');
            string messageType = arrPayload[0];

            switch (messageType)
            {
                case "Case1":
                    ConnectMenu.WaitingRoom.ShowCurrentUser(arrPayload[1]);
                    break;
                case "Case2":
                    HandleCase2(arrPayload);
                    break;
                case "Case3":
                    HandleCase3(arrPayload);
                    break;
                case "Case4":
                    room.SHOW();
                    break;
                case "Case5":
                    HandleUpdateMessage(arrPayload);
                    break;
                case "Case6":
                    HandleTurnMessage(arrPayload);
                    break;
                case "Case7":
                    room.Invoke((MethodInvoker)delegate ()
                    {
                      for(int i=0;i<room.soluongbaicanrut;i++)
                        { 
                        room.ProcessBocBai(arrPayload[2]);}
                    });
                    break;
                case "Case8":
                    ProcessRStack(arrPayload);
                    break;
                case "Case9":
                    HandleEndMessage(arrPayload);
                    break;
                default:
                    break;
            }
        }
    }

}
