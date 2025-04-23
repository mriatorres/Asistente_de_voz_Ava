using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace AsistenteDeVozAva
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer Toro = new SpeechSynthesizer();
        SpeechRecognitionEngine startlistening = new SpeechRecognitionEngine();
        Random rnd = new Random();
        int RecTimeOut = 0;
        DateTime TimeNow = DateTime.Now;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
            _recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(_recognizer_SpeechRecognized);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);

            startlistening.SetInputToDefaultAudioDevice();
            startlistening.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            startlistening.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(startlistening_SpeechRecognized);
        }

        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int ranNum;
            string speech = e.Result.Text;

            if (speech == "Hola") {
                Toro.SpeakAsync("Hola, ¿Que pasa?");
            }
            if (speech == "Como estas") {
                Toro.SpeakAsync("De pie bailando");
            }
            if (speech == "Que hora es") {
                Toro.SpeakAsync(DateTime.Now.ToString("h mm tt"));
            }
            if (speech == "Para de hablar")
            {
                Toro.SpeakAsyncCancelAll();
                ranNum = rnd.Next(1, 2);
                if (ranNum == 1)
                {
                    Toro.SpeakAsync("okay, esta bien");
                }
                if (ranNum == 2) {
                    Toro.SpeakAsync("ups, lo siento, dejare de hablar");
                }
            }

            if(speech == "Para de escuchar")
            {
                Toro.SpeakAsync("Si me necesitas, aqui estoy");
                _recognizer.RecognizeAsyncCancel();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);

            }

            if (speech == "Muestra los comandos") ;
            {
                string[] commands = (File.ReadAllLines(@"DefaultCommands.txt"));
                LstCommands.Items.Clear();
                LstCommands.SelectionMode = SelectionMode.None;
                LstCommands.Visible = true;
                foreach (string command in commands) {
                    LstCommands.Items.Add(command);
                }
            }

            if(speech == "Oculta los comandos")
            {
                LstCommands.Visible = false;
            }

        }
        private void _recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeOut = 0;
        }
        private void startlistening_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;  

            if(speech == "Despierta")
            {
                startlistening.RecognizeAsyncCancel();
                Toro.SpeakAsync("Hola, aqui estoy");
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);

            }
        }

       

       

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TmrSpeaking_Tick(object sender, EventArgs e)
        {
            if (RecTimeOut == 10)
            {
                _recognizer.RecognizeAsyncCancel();
            }
            else if (RecTimeOut == 11) {
                TmrSpeaking.Stop();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
                RecTimeOut = 0;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void LstCommands_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
