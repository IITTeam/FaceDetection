using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Speech.Synthesis;

namespace FaceDetection.Core
{
    public class VoiceAssistantService
    {
        public void SayText(string text,string voice)
        {
            // Initialize a new instance of the SpeechSynthesizer.
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoice(voice);

            // Configure the audio output. 
            synth.SetOutputToDefaultAudioDevice();

            // Speak a string.
            synth.Speak(text);
        }
    }
}
