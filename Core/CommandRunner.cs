using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using System.Management;

namespace Core
{
    public interface ICommandRunner
    {
        Task<ProcessResult> RunCommandAsync(string cmd, bool runAsAdmin, CancellationToken cancellationToken);
    }
    public class CommandRunner : ICommandRunner
    {
        
        public async Task<ProcessResult> RunCommandAsync(string cmd, bool runAsAdmin, CancellationToken cancellationToken)
        {
            ProcessStartInfo pStartInfo = new ProcessStartInfo();
            pStartInfo.FileName = "cmd.exe";
            pStartInfo.Arguments = "/c " + cmd;
            pStartInfo.UseShellExecute = true;
            pStartInfo.CreateNoWindow = true;
            //pStartInfo.RedirectStandardOutput = true;
            //pStartInfo.RedirectStandardError = true;
            pStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            if (runAsAdmin)
                pStartInfo.Verb = "runas";
 
            return await RunProcessAsync(pStartInfo, cancellationToken);
        }

        private static async Task<ProcessResult> RunProcessAsync(ProcessStartInfo processStartInfo, CancellationToken cancellationToken)
        {

            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.EnableRaisingEvents = true;
 
            try
            {
                process.Start();
                await process.WaitForExitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {

                KillProcessAndChildren(process.Id);
                return new ProcessResult(process, process.ExitCode, new string[] {}, new string[] { "process was canceled by user"}, true);
            }

            var processResult = new ProcessResult(
                        process,
                        process.ExitCode, null, null,
                        //new[] { process.StandardOutput.ReadToEnd() },
                        //new[] { process.StandardError.ReadToEnd() },
                        false);

            //dispose the handle
            process.Dispose();

            return processResult;
        }


        //https://stackoverflow.com/questions/5901679/kill-process-tree-programmatically-in-c-sharp/5921994
        private static void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }

            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                    ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }
    }

    

    public sealed class ProcessResult : IDisposable
    {
        public ProcessResult(Process process, int exitError, string[] standardOutput, string[] standardError, bool isProcessCancelled)
        {
            Process = process;
            ExitCode = exitError;
            StandardOutput = standardOutput;
            StandardError = standardError;
            IsProcessCancelled = isProcessCancelled;
        }

        public bool IsProcessCancelled { get; }
        public Process Process { get; }
        public int ExitCode { get; }
        public string[] StandardOutput { get; }
        public string[] StandardError { get; }
        public void Dispose() { Process.Dispose(); }
    }
}
