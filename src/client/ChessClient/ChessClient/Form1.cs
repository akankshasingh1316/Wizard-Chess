using System;
using System.Windows.Forms;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace ChessClient
{
    public partial class MainForm : Form
    {
        SpeechRecog chessRecognizer;
        static string server = "http://thunder.cise.ufl.edu:7777";
        static string move_url = server + "/move";
        static string undo_url = server + "/undo";
        static string board_img = server + "/board.png";
        static string board_hint = board_img + "?hint=true";
        private string turn = "white";
        bool showing_hints = false;


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            chessRecognizer = new SpeechRecog();
            get_board();
        }


        private void get_newgame()
        {
            WebRequest req = WebRequest.Create(server);
            req.Method = "GET";
            WebResponse res = req.GetResponse();
            get_board();
        }

        private void get_board(bool hint=false)
        {
            pbPicstatus.Style = ProgressBarStyle.Marquee;
            if (hint)
            {
                fetch_board(board_hint);
            }
            else
            {
                fetch_board(board_img);
            }
        }

        // Wow this is WAY faster than picBoard.Load() or picBoard.LoadAsync();
        private void fetch_board(string board_url)
        {
            try {
                using (HttpClient hc = new HttpClient())
                {
                    hc.BaseAddress = new Uri(board_url);
                    HttpResponseMessage result = hc.GetAsync("").Result;
                    byte[] payload = result.Content.ReadAsByteArrayAsync().Result;
                    Bitmap bmp;
                    using (MemoryStream ms = new MemoryStream(payload))
                    {
                        Image img = Image.FromStream(ms);
                        bmp = new Bitmap(img);
                    }
                    picBoard.Image = bmp;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.Text = "Connection to server failed!";
            }
        }

        private void post_move(string piece, string position)
        {
            if (piece.ToLower() == "knight")
            {
                piece = "n";
            }
            else
            {
                piece = piece[0].ToString().ToLower();
            }
            position = position.ToLower();

            using (HttpClient hc = new HttpClient())
            {
                hc.BaseAddress = new Uri(move_url);
                FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("piece", piece),
                    new KeyValuePair<string, string>("to", position)
                });
                // If you want to do this synchronously, use PostAsync(...).Result;

                HttpResponseMessage result = hc.PostAsync("", content).Result;
                string raw_json = result.Content.ReadAsStringAsync().Result;

                GameState gs = JsonConvert.DeserializeObject<GameState>(raw_json);
                //var ser = new JavaScriptSerializer();
                //ser.DeserializeObject(raw_json);




                //string movelist = result.Content.ReadAsStringAsync().Result;
                string movelist = gs.status;
                txtResponse.Text = "Valid moves: " + movelist;
                this.turn = gs.turn;
                lblTurn.Text = gs.turn + "'s turn";
                if (result.IsSuccessStatusCode)
                {

                }
                else
                {

                }
            }
        }

        private void post_undo()
        {
            using (WebClient wb = new WebClient())
            {
                NameValueCollection payload = new NameValueCollection();
                byte[] response = wb.UploadValues(undo_url, "POST", payload);
            }
        }

        private void parse_phrase(string phrase)
        {
            txtLog.AppendText(this.turn + ">> " + phrase + "\n");
            string[] bits = phrase.Split(' ');
            switch(bits[0])
            {
                case "Move":
                    string piece = bits[1];
                    string position = bits[3];
                    post_move(piece, position);
                    break;
                case "Hint":
                    if (showing_hints)
                        showing_hints = false;
                    else
                        showing_hints = true;
                    break;
                case "Undo":
                    post_undo();
                    break;
                case "New":
                    get_newgame();
                    break;
                case "Help":

                    break;
                default:

                    break;
            }
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            btnListen.Text = "Listening...";
            while(true) {
                lblState.Text = "listening";
                string chess_phrase = this.chessRecognizer.recognize();
                //lblState.Text = "processing";
                parse_phrase(chess_phrase);
                lblState.Text = "refreshing board";
                get_board(showing_hints);
            }
        }

        private void picBoard_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            pbPicstatus.Style = ProgressBarStyle.Blocks;
        }
    }

    public class GameState
    {
        public string turn { get; set; }
        public string status { get; set; }
        public string moves { get; set; }
    }
}
