using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using OpenUtau.Core.Util;

namespace OpenUtau.Classic {
    public class Plugin : IPlugin {
        public string Name;
        public string Executable;
        public bool AllNotes;
        public bool UseShell;
        private string encoding = "shift_jis";
        public string Shortcut;

        public string Encoding { get => encoding; set => encoding = value; }

        public async Task Run(string tempFile) {
            if (!File.Exists(Executable)) {
                throw new FileNotFoundException($"Executable {Executable} not found.");
            }
            string winePath = Preferences.Default.WinePath;
            string ext = Path.GetExtension(Executable).ToLowerInvariant();
            bool useWine = !OS.IsWindows()
                && !string.IsNullOrEmpty(winePath)
                && (ext == ".exe" || ext == ".bat");
            string resolvedWinePath = string.Empty;
            if (useWine) {
                string[] wineNames = { "wine", "wine64", "wine32", "wine32on64" };
                foreach (string wineName in wineNames) {
                    string candidate = OS.WhereIs(wineName);
                    if (!string.IsNullOrEmpty(candidate)
                        && Path.IsPathRooted(candidate)
                        && File.Exists(candidate)
                        && candidate.EndsWith(wineName, StringComparison.Ordinal)) {
                        resolvedWinePath = Path.GetFullPath(candidate);
                        break;
                    }
                }
                if (string.IsNullOrEmpty(resolvedWinePath)) {
                    throw new FileNotFoundException("Wine executable not found.");
                }
            }
            var startInfo = new ProcessStartInfo() {
                FileName = useWine ? resolvedWinePath : Executable,
                Arguments = useWine ? $"\"{Executable}\" \"{tempFile}\"" : $"\"{tempFile}\"",
                Environment = {{"LANG", "ja_JP.utf8"}},
            string ext = Path.GetExtension(Executable).ToLower();
            bool useWine = !OS.IsWindows() && !string.IsNullOrEmpty(winePath) && ( ext == ".exe" || ext == ".bat");
            var startInfo = new ProcessStartInfo() {
                WorkingDirectory = Path.GetDirectoryName(Executable),
            };
            if (useWine) {
                startInfo.FileName = winePath;
                startInfo.Arguments = $"\"{Executable}\" \"{tempFile}\"";
                startInfo.UseShellExecute = false;
                startInfo.Environment.Add("LANG", "ja_JP.utf8");
            } else {
                startInfo.FileName = Executable;
                startInfo.Arguments = $"\"{tempFile}\"";
                startInfo.UseShellExecute = UseShell;
            }
            using (var process = Process.Start(startInfo)) {
                await process.WaitForExitAsync();
            }
        }

        public override string ToString() => Name;
    }
}
