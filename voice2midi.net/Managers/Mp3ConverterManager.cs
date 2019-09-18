using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace voice2midiAPI.net.Managers
{
    public class Mp3ConverterManager : ExternalManager
    {
        public Mp3ConverterManager(bool redirectStdOutput = true):
            base(redirectStdOutput)
        {
            PStartInfo.FileName = @"/usr/local/bin/fluidsynth";
            PStartInfo.Arguments = @"-i /app/GeneralUser_GS.sf2 ";
        }

        public override async Task run(string FilePathIn, string FilePathOut)
        {
            PStartInfo.Arguments += FilePathIn + " -F " + FilePathOut;

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
