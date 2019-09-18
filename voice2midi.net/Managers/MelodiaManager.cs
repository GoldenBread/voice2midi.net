using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace voice2midiAPI.net.Tools
{
    public class MelodiaManager// Melodia, transcriber script that convert .wav to .midi
    {
        private ProcessStartInfo PStartInfo = new ProcessStartInfo();

        public MelodiaManager(bool redirectStdOutput = true)
        {
            redirectStandardOutput(redirectStdOutput);
            PStartInfo.FileName = @"/Users/thierry/Projects/voice2midi_microservice_simplified/audio_to_midi_melodia.py";
        }

        public void redirectStandardOutput(bool redirection)// Redirect stdout to app output console
        {
            PStartInfo.UseShellExecute = !redirection;
            PStartInfo.RedirectStandardOutput = redirection;
        }

        public async Task runMelodia(string FilePathIn, string FilePathOut)
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
