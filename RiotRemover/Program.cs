using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RiotRemover
{
    class Program
    {
        public static string userStartupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup), riotDefaultPath = String.Format("{0}\\riot.pyw", userStartupPath), version = "1.0";
        public static List<string> regexs = new List<string>()
        {
            @"(?:http(?:s)?:\/\/)?(?:(canary|ptb).)?(?:(discord|discordapp)?.com)(?:)\/api\/(?:v\d\/)?webhooks\/(?:\d{16,18})\/(?:[A-Za-z0-9_\-]+)",
            @"(?:\d{16,18})\/(?:[A-Za-z0-9_\-]+)"
        };
        public static string[] omg = new string[] { ".py", ".pyw" }, ignore = new string[] { ".exe", ".bat", ".vbs" }; //Mmmmmmmmmmmmmmmmmmmmmmmmh
        static void Main(string[] args)
        {
            Console.WriteLine(String.Format("Version {0}\nScanned Extensions File: {1}\nAlso verifying extension through lnk files :)\n\n", version, string.Join(",", omg)));
            if (File.Exists(riotDefaultPath))
            {
                Logger.Warn("You are infected by Riot !");
                Logger.Info("The programm will delete the webhook and remove Riot from your computer.");
                killRiot(riotDefaultPath, false);
            } else
            {
                Logger.Info("Looks like you are not infected by Riot.. Watching for some renamed variant !");
                Logger.Info("Verifying lnk redirect, py and pyw files..");
                foreach (string file in Directory.GetFiles(userStartupPath))
                {
                    if (file.EndsWith(".lnk"))
                    {
                        Logger.Info("Found a lnk file. Resolving the target and verifying the file..");
                        IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)new IWshRuntimeLibrary.WshShell().CreateShortcut(file);
                        killRiot(link.TargetPath, true); //Mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmh
                    }
                    if(omg.Any(x => file.EndsWith(x)))
                    {
                        Logger.Info("Found a suspicious file. Scan launched on it ! :)");
                        killRiot(file, false);
                    }
                }
            }
            Console.WriteLine("Scan finished !\nEverything should be good now.");
            Console.ReadLine();
        }

        public static void killRiot(string path, bool lnk)
        {
            if(ignore.Any(x => path.EndsWith(x)))
            {
                Logger.Info("Ignored file extension. Passed the file.");
                return;
            }
            MatchCollection matchsRegex0 = Regex.Matches(File.ReadAllText(path), regexs[0], RegexOptions.None);
            MatchCollection matchsRegex1 = Regex.Matches(File.ReadAllText(path), regexs[1], RegexOptions.None);

            if(matchsRegex0.Count == 0 && matchsRegex1.Count == 0)
            {
                if (lnk && !omg.Any(x => path.EndsWith(x)))
                    Logger.Info("Clean lnk target file !");
                else if (omg.Any(x => path.EndsWith(x)))
                    Logger.Info("Clean file !");
                return;
            }

            if (matchsRegex0.Count > 0)
            {
                Logger.Warn("Found on or multiple entire webhook in the Riot script, they'll be deleted in a few seconds !");
                foreach (var x in matchsRegex0)
                {
                    string webhook = Regex.Match(x.ToString(), regexs[1], RegexOptions.None).ToString();
                    webhook = "https://discord.com/api/v9/webhooks/" + webhook;
                    //Console.WriteLine(webhook);
                    deleteWebhook(webhook);
                }
            }
            if (matchsRegex1.Count > 0)
            {
                Logger.Warn("Found on or multiple part of webhook in the Riot script, they'll be deleted in a few seconds !");
                foreach (var x in matchsRegex1)
                {
                    string webhook = "https://discord.com/api/v9/webhooks/" + x;
                    //Console.WriteLine(webhook);
                    deleteWebhook(webhook);
                }
            }
            File.Delete(path);
        }
        public static void deleteWebhook(string x)
        {
            using (HttpRequest req = new HttpRequest())
            {
                req.IgnoreProtocolErrors = true;
                try
                {
                    req.UserAgent = "HideakiAtsuyo";
                    req.Delete(x);
                }
                catch
                {
                }
            }
        }
    }
}
