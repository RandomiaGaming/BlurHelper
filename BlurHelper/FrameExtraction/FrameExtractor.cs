namespace BlurHelper
{
    public static class FrameExtractor
    {
        public static void ExtractFrames(string sourceVideoPath, string outputDirectoryPath, FrameFormat frameFormat)
        {
            if (sourceVideoPath is null)
            {
                throw new System.Exception("sourceVideoPath cannot be null.");
            }
            else if (sourceVideoPath is "")
            {
                throw new System.Exception("sourceVideoPath cannot be empty.");
            }
            else if (!System.IO.File.Exists(sourceVideoPath))
            {
                throw new System.Exception("sourceVideoPath does not exist.");
            }
            sourceVideoPath = System.IO.Path.GetFullPath(sourceVideoPath);
            if (outputDirectoryPath is null)
            {
                throw new System.Exception("outputDirectoryPath cannot be null.");
            }
            else if (outputDirectoryPath is "")
            {
                throw new System.Exception("outputDirectoryPath cannot be empty.");
            }
            else if (!System.IO.Directory.Exists(outputDirectoryPath))
            {
                System.IO.Directory.CreateDirectory(outputDirectoryPath);
            }
            outputDirectoryPath = System.IO.Path.GetFullPath(outputDirectoryPath);
            if (outputDirectoryPath[outputDirectoryPath.Length - 1] is '\\')
            {
                outputDirectoryPath = outputDirectoryPath.Substring(0, outputDirectoryPath.Length - 1);
            }
            string extension;
            if (frameFormat is FrameFormat.PNG)
            {
                extension = ".png";
            }
            else if (frameFormat is FrameFormat.JPEG)
            {
                extension = ".png";
            }
            else if (frameFormat is FrameFormat.BMP)
            {
                extension = ".png";
            }
            else
            {
                throw new System.Exception("frameFormat was not valid.");
            }
            string pathUnsplit = System.Environment.GetEnvironmentVariable("Path", System.EnvironmentVariableTarget.Machine);
            string[] paths = pathUnsplit.Split(';');
            string ffmpegPath = null;
            foreach (string path in paths)
            {
                if (System.IO.Directory.Exists(path))
                {
                    string possibleFfmpegPath = $"{path}\\ffmpeg.exe";
                    if (System.IO.File.Exists(possibleFfmpegPath))
                    {
                        ffmpegPath = possibleFfmpegPath;
                        break;
                    }
                }
            }
            System.Diagnostics.ProcessStartInfo ffmpegStartInfo = new System.Diagnostics.ProcessStartInfo(ffmpegPath);
            ffmpegStartInfo.Arguments = $"-i \"{sourceVideoPath}\" \"{outputDirectoryPath}\\%d{extension}\"";
            ffmpegStartInfo.RedirectStandardError = true;
            ffmpegStartInfo.RedirectStandardOutput = true;
            ffmpegStartInfo.UseShellExecute = false;
            System.Diagnostics.Process ffmpeg = System.Diagnostics.Process.Start(ffmpegStartInfo);
            System.Console.ForegroundColor = System.ConsoleColor.Cyan;
            System.Console.WriteLine($"Extracting frames from video \"{sourceVideoPath}\" to \"{outputDirectoryPath}\"...");
            System.Console.WriteLine();
            System.Threading.Thread stdOutRelayThread = new System.Threading.Thread(() =>
            {
                while (!ffmpeg.HasExited)
                {
                    System.Console.ForegroundColor = System.ConsoleColor.White;
                    System.Console.WriteLine("ffmpeg> " + ffmpeg.StandardOutput.ReadLine());
                }
            });
            stdOutRelayThread.Start();
            System.Threading.Thread stdErrorRelayThread = new System.Threading.Thread(() =>
            {
                while (!ffmpeg.HasExited)
                {
                    System.Console.ForegroundColor = System.ConsoleColor.White;
                    System.Console.WriteLine("ffmpeg> " + ffmpeg.StandardError.ReadLine());
                }
            });
            stdErrorRelayThread.Start();
            while (!ffmpeg.HasExited)
            {
                System.Threading.Thread.Sleep(100);
            }
            System.Console.ForegroundColor = System.ConsoleColor.Cyan;
            System.Console.WriteLine();
            System.Console.WriteLine($"Extracted frames from video \"{sourceVideoPath}\" to \"{outputDirectoryPath}\"!");
            System.Console.ForegroundColor = System.ConsoleColor.White;
            System.Console.ReadLine();
        }
    }
}