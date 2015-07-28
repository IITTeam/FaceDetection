using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;
using Microsoft.Speech.Synthesis;

namespace FaceDetection.Core
{
    public class VoiceAssistantService
    {
        private readonly SpeechSynthesizer _synthesizer;
        public VoiceAssistantService()
        {
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SetOutputToDefaultAudioDevice();
            _synthesizer.Volume = 100;
            //Path.GetFullPath("Resources/Lexicon.pls");
            _synthesizer.AddLexicon(new Uri(Path.GetFullPath("Resources/Lexicon.pls")), "application/pls+xml");
        }

        public void SayText(string text, int voice)
        {
            _synthesizer.SelectVoice(_synthesizer.GetInstalledVoices().ToList()[voice].VoiceInfo.Name);
            _synthesizer.SpeakAsync(text);
        }


        public List<string> GetAllVoices()
        {
            return _synthesizer.GetInstalledVoices().ToList().Select(installedVoice => installedVoice.VoiceInfo.Name).ToList();
        }
    }
}
