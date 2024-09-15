using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ARCToolMultiPacker
{
    internal class Program
    {
        private static readonly string Dir = Directory.GetCurrentDirectory();
        private static string CustomArgs = "";

        private static string[] folderList;
        private static List<string> CustomFolderList = new List<string>();
        private static string[] AllArgs = { "-dd", "-texRE6", "-alwayscomp", "-pc", "-texRE5", "-dmc4se", "-txt", "-v", "7", "-rev2", "-rev", "-re6", "-re5", "-rehd", "-re0", "-ddo", "-dd", "-noxmemmagic", "-texDMC4SE", "-xmem", "-tex", "-dmc4" };

        private static bool unpack;

        /// <summary>
        /// Format: .\ARCToolMultiPacker.exe [unpack/repack] [ARCTool args] [folders (name can't have spaces!)]
        /// Written by github.com/neatodev
        /// </summary>
        private static void Main(string[] args)
        {
            DefineUnpack(args[0]);
            DefineParams(args);
            int SDirs = 0;
            int Delay = 0;
            string Key = "";

            foreach (string Folder in CustomFolderList)
            {
                var SubDirs = Directory.GetDirectories(Folder, "*", SearchOption.AllDirectories);
                SDirs += SubDirs.Length;
            }

            if (SDirs > 0)
            {
                Console.WriteLine("Detected a total of " + SDirs + " subfolder(s). Would you like to use ARCTool recursively? (May heavily increase runtime/running processes.) (Y/N)");
                Key = Console.ReadLine();
                while (Key != "Y".ToLower() && Key != "N".ToLower())
                {
                    Key = Console.ReadLine();
                }
            }

            Console.WriteLine("Would you like to set a delay between the launch of ARCTool instances? (Y/N)");
            string DelayKey = Console.ReadLine();
            while (DelayKey != "Y".ToLower() && DelayKey != "N".ToLower())
            {
                DelayKey = Console.ReadLine();
            }
            if (DelayKey == "Y".ToLower())
            {
                Console.WriteLine("Please specify the delay from a range of 0 to 25 (seconds).");
                do
                {
                    Int32.TryParse(Console.ReadLine(), out Delay);
                    if (Delay > 25 || Delay < 0)
                        Console.WriteLine("Please enter a number between 0 and 25.");

                } while (Delay > 25 || Delay < 0);
                Delay *= 1000;
            }
            if (unpack) { 
            if (Key == "Y".ToLower())
            {
                foreach (string Folder in CustomFolderList)
                {
                    foreach (string File in Directory.EnumerateFiles(Folder, "*", SearchOption.AllDirectories))
                    {
                        Thread.Sleep(Delay);
                        Process.Start(".\\ARCtool.exe", CustomArgs + " " + File);
                    }
                }
            }
            else
            {
                foreach (string Folder in CustomFolderList)
                {
                    foreach (string File in Directory.EnumerateFiles(Folder, "*", SearchOption.TopDirectoryOnly))
                    {
                        Thread.Sleep(Delay);
                        Process.Start(".\\ARCtool.exe", CustomArgs + " " + File);
                    }
                }
                }
            } 
            else
            {
                if (Key == "Y".ToLower())
                {
                    foreach (string Folder in CustomFolderList)
                    {
                        Process.Start(".\\ARCtool.exe", CustomArgs + " " + Folder);
                        foreach (string Dir in Directory.EnumerateDirectories(Folder, "*", SearchOption.AllDirectories))
                        {
                            Thread.Sleep(Delay);
                            Process.Start(".\\ARCtool.exe", CustomArgs + " " + Dir);
                        }
                    }
                }
                else
                {
                    foreach (string Folder in CustomFolderList)
                    {
                        Process.Start(".\\ARCtool.exe", CustomArgs + " " + Folder);
                        foreach (string Dir in Directory.EnumerateDirectories(Folder, "*", SearchOption.TopDirectoryOnly))
                        {
                            Thread.Sleep(Delay);
                            Process.Start(".\\ARCtool.exe", CustomArgs + " " + Dir);
                        }
                    }
                }
            }
        }

        private static void DefineUnpack(string arg)
        {
            if (arg.ToLower() == "unpack")
            {
                unpack = true;           
            }
            else if (arg.ToLower() == "repack")
            {
                unpack = false; 
            }
            else
            {
                throw new Exception("Failed to specify \"unpack\" or \"repack\"");
            }
            
        }

        private static void DefineParams(string[] args)
        {
            string[] argList = args;
            for (int i = 1; i < argList.Length; i++)
            {
                bool IsFolder = true;
                foreach (string GArg in AllArgs)
                {
                    if (GArg.ToLower() == argList[i].ToLower())
                    {
                        CustomArgs = CustomArgs + " " + argList[i];
                        IsFolder = false;
                    }
                }
                if (IsFolder)
                {
                    CustomFolderList.Add(argList[i]);
                }
                IsFolder = true;
            }
        }
    }
}
