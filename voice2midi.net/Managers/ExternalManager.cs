using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace voice2midiAPI.net.Managers
{
    public abstract class ExternalManager
    {
        protected ProcessStartInfo PStartInfo = new ProcessStartInfo();

        public ExternalManager(bool redirectStdOutput = true)
        {
            redirectStandardOutput(redirectStdOutput);
        }

        public void redirectStandardOutput(bool redirection)// Redirect stdout to app output console
        {
            PStartInfo.UseShellExecute = !redirection;
            PStartInfo.RedirectStandardOutput = redirection;
        }

        public abstract Task run(string FilePathIn, string FilePathOut);

    }
}
