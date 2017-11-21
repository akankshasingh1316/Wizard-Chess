using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.IO;

namespace ChessClient
{
    class SpeechRecog
    {
        SpeechRecognitionEngine recognizer;
        Grammar algebraic_notation;
        static string project_directory = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString() + "\\";
        static string an_filepath = project_directory + "algebraic_grammar.xml";

        public SpeechRecog()
        {
            this.recognizer = new SpeechRecognitionEngine();
            this.algebraic_notation = new Grammar(SpeechRecog.an_filepath);
            this.recognizer.LoadGrammar(this.algebraic_notation);
        }

        public string recognize()
        {
            string response = "";
            try
            {
                this.recognizer.SetInputToDefaultAudioDevice();
                RecognitionResult result = this.recognizer.Recognize();
                response = result.Text;
            }
            catch (Exception ex)
            {
                response = "Invalid command";
                //response = "Error " + ex.ToString();
                // Console.WriteLine(String.Format("Could not recognize input from default aduio device. Is a microphone or sound card available?\r\n{0} - {1}.", exception.Source, exception.Message));
            }
            return response;
        }
    }
}
