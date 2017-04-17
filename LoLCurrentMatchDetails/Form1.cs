using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoLCurrentMatchDetails
{
    public partial class Form1 : Form, ApiEvents
    {
        public Form1()
        {
            
            InitializeComponent();
            textBox1.Text = ApiFetcher.getSummonerName(ApiFetcher.CURRENT_SUMMONER);
            ApiFetcher.favlist.ForEach(item => comboBox1.Items.Add(ApiFetcher.getSummonerName(item)));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void onLeagueStarted()
        {

        }

        public void onLeagueStopped()
        {

        }

        public void onStartedUpdate()
        {
            changeaccess(false);
        }

        public void onFinishedUpdate(string data)
        {
            updateLabel(data);
            changeaccess(true);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox item = (ComboBox) sender;
            string selectedItem = (string) item.SelectedItem;
            textBox1.Text = selectedItem;
            ApiFetcher.CURRENT_SUMMONER = ApiFetcher.getSummonerID(selectedItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateLabel("loading");
            ApiFetcher.CURRENT_SUMMONER = ApiFetcher.getSummonerID(textBox1.Text);
            ApiFetcher.getData(1, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updateLabel("loading");
            RiotSharp.FeaturedGamesEndpoint.FeaturedGames testgame = ApiFetcher.api.GetFeaturedGames(RiotSharp.Region.euw);
            string sname = testgame.GameList[0].Participants[0].SummonerName;
            ApiFetcher.CURRENT_SUMMONER = ApiFetcher.api.GetSummoner(RiotSharp.Region.euw, sname).Id;
            //ApiFetcher.CURRENT_SUMMONER = 60077041;
            int result = ApiFetcher.getData();
            ApiFetcher.getData(1, 1);
        }

        private void updateLabel(string data)
        {
            // Running on the worker thread
            this.data.Invoke((MethodInvoker)delegate {
                // Running on the UI thread
                this.data.Text = data;
                this.RaisePaintEvent(null, null);
            });
            // Back on the worker thread  
        }

        private void changeaccess(bool boolean)
        {
            // Running on the worker thread
            this.data.Invoke((MethodInvoker)delegate {
                button1.Enabled = boolean;
                button2.Enabled = boolean;
                textBox1.Enabled = boolean;
                comboBox1.Enabled = boolean;
            });
            // Back on the worker thread 
        }
    }
}
