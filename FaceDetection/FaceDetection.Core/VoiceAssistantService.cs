using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.ObjectModel;
using System.Speech.Synthesis;

namespace FaceDetection.Core
{
    public class VoiceAssistantService
    {
        private readonly SpeechSynthesizer _synthesizer;
        public VoiceAssistantService()
        {
            _synthesizer = new SpeechSynthesizer();
        }

        public void SayText(string text, int voice)
        {
            _synthesizer.SelectVoice(_synthesizer.GetInstalledVoices().ToList()[voice].VoiceInfo.Name);
            _synthesizer.SetOutputToDefaultAudioDevice();
            _synthesizer.SpeakAsync(text);
        }


        public List<string> GetAllVoices()
        {
            return _synthesizer.GetInstalledVoices().ToList().Select(installedVoice => installedVoice.VoiceInfo.Name).ToList();
        }
    }
}
