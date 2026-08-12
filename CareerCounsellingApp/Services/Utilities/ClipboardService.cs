using System;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.Utilities
{
    /// <summary>
    /// Simple clipboard service wrapper
    /// For cross-platform compatibility, this provides basic text copying
    /// </summary>
    public class ClipboardService
    {
        /// <summary>
        /// Copies text to clipboard using system clipboard
        /// Falls back gracefully if clipboard is not available
        /// </summary>
        public static async Task<bool> CopyToClipboardAsync(string text)
        {
            try
            {
                // Try using system clipboard - works on Windows, macOS, Linux
                #if WINDOWS
                System.Windows.Forms.Clipboard.SetText(text);
                return true;
                #elif MACCATALYST || __MACCATALYST__
                // macOS clipboard using Cocoa
                return await CopyToMacClipboardAsync(text);
                #else
                // Linux clipboard using xclip or xsel
                return await CopyToLinuxClipboardAsync(text);
                #endif
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Clipboard error: {ex.Message}");
                return false;
            }
        }

        #if MACCATALYST || __MACCATALYST__
        private static Task<bool> CopyToMacClipboardAsync(string text)
        {
            try
            {
                // This would use Objective-C interop for macOS
                // For now, return false to fallback
                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
        #endif

        private static Task<bool> CopyToLinuxClipboardAsync(string text)
        {
            try
            {
                // Try using xclip or xsel on Linux
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"echo '{text}' | xclip -selection clipboard\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}





