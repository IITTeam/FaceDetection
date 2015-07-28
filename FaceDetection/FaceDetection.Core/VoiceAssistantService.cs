using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            _synthesizer.Rate = 1;
            _synthesizer.AddLexicon(new Uri(Path.GetFullPath("Resources/Lexicon.pls")), "application/pls+xml");
        }

        public void SayText(string text)
        {
            var voice =
                _synthesizer.GetInstalledVoices()
                    .FirstOrDefault(installedVoice => installedVoice.VoiceInfo.Culture.Name.Contains("ru-RU"));
            _synthesizer.SelectVoice(voice != null
                ? voice.VoiceInfo.Name
                : _synthesizer.GetInstalledVoices().FirstOrDefault().VoiceInfo.Name);
            _synthesizer.Speak(text);
        }

        public List<string> GetAllVoices()
        {
            return
                _synthesizer.GetInstalledVoices()
                    .ToList()
                    .Select(installedVoice => installedVoice.VoiceInfo.Name)
                    .ToList();
        }
    }
}