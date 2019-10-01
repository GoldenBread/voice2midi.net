using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using voice2midiAPI.net.Managers;

namespace voice2midiAPI.net.Tools
{
    public class MelodiaManager : ExternalManager// Melodia, transcriber script that convert .wav to .midi
    {
        public MelodiaManager(bool redirectStdOutput = true):
            base(redirectStdOutput)
        {
            PStartInfo.FileName = @"/Users/thierry/Projects/voice2midi_microservice_simplified/audio_to_midi_melodia.py";
        }

        public override async Task run(string FilePathIn, string FilePathOut)
        {
            PStartInfo.Arguments = FilePathIn + " " + FilePathOut + " 60";

            using (Process process = Process.Start(PStartInfo))
            {
                // Read output
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = await reader.ReadToEndAsync();
                    Console.WriteLine(result);
                }
            }
        }

    }
}
