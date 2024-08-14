using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ARCToolMultiPacker
{
    internal class Program
    {
        private static readonly string Dir = Directory.GetCurrentDirectory();
        private static readonly string DdArgs = "-dd -texRE6 -alwayscomp -pc -txt -v 7";
        private static readonly string DdoArgs = "-ddo -texRE6 -alwayscomp -pc -txt -v 7";
        private static readonly string Dmc4Args = "-pc -dmc4 -xmem -noxmemmagic -texRE5 -alwayscomp";
        private static readonly string DmcSE4Args = "-pc -dmc4se -texDMC4SE -alwayscomp -txt -v 7";
        private static readonly string Re0Args = "-re0 -texRE6 -alwayscomp -pc -txt -v 7";
        private static readonly string ReHDArgs = "-rehd -texRE6 -alwayscomp -pc -txt -v 7";
        private static readonly string Re5Args = "-alwayscomp -re5 -texRE5 -v 7 -pc"; //tex
        private static readonly string Re6Args = "-re6 -alwayscomp -pc -txt -v 7"; //tex
        private static readonly string RevArgs = "-rev -alwayscomp -pc -txt -v 7"; //tex
        private static readonly string Rev2Args = "-rev2 -texRE6 -alwayscomp -pc -txt -v 7"; //tex
        private static string CustomArgs = "";

        private static string[] folderList;
        private static List<string> CustomFolderList = new List<string>();
        private static string[] AllArgs = { "-dd", "-texRE6", "-alwayscomp", "-pc", "-texRE5", "-dmc4se", "-txt", "-v", "7", "-rev2", "-rev", "-re6", "-re5", "-rehd", "-re0", "-ddo", "-dd", "-noxmemmagic", "-texDMC4SE", "-xmem", "-tex", "-dmc4" };

        private static string GameArgs;
        private static bool unpack;
        private static bool tex;
        private static bool custom = false;

        /// <summary>
        /// Format: .\ARCToolMultiPacker.exe [game] [tex(optional)] [unpack/repack] [folders] 
        /// Alternative: .\ARCToolMultiPacker.exe [custom] [write arguments as you would in ARCTool] [folders]
        /// Written by github.com/neatodev
        /// </summary>
        private static void Main(string[] args)
        {
            DefineParams(args);
            if (custom)
            {
                foreach (string Folder in CustomFolderList)
                {
                    var files = Directory.GetFiles(Dir + "\\" + Folder);
                    foreach (var file in files)
                        try
                        {
                            Process.Start(".\\ARCtool.exe", CustomArgs + " " + file);
                        }
                        catch (FileNotFoundException e)
                        {
                            Console.WriteLine("Could not extract file. Is the path valid? " + e);
                        }
                }
            }
            else
            {
                foreach (var arg in folderList)
                    if (unpack && tex)
                    {
                        var files = Directory.GetFiles(Dir + "\\" + arg);
                        foreach (var file in files)
                            try
                            {
                                Process.Start(".\\ARCtool.exe", GameArgs + " -tex " + file);
                            }
                            catch (FileNotFoundException e)
                            {
                                Console.WriteLine("Could not extract file. Is the path valid? " + e);
                            }
                    }
                    else if (unpack)
                    {
                        var files = Directory.GetFiles(Dir + "\\" + arg);
                        foreach (var file in files)
                            try
                            {
                                Process.Start(".\\ARCtool.exe", GameArgs + " " + file);
                            }
                            catch (FileNotFoundException e)
                            {
                                Console.WriteLine("Could not extract file. Is the path valid? " + e);
                            }
                    }
                    else if (!unpack || (!unpack && tex))
                    {
                        var folders = Directory.GetDirectories(Dir + "\\" + arg);
                        foreach (var folder in folders)
                            try
                            {
                                Process.Start(".\\ARCtool.exe", GameArgs + " " + folder);
                            }
                            catch (DirectoryNotFoundException e)
                            {
                                Console.WriteLine("Could not pack file. Is the path valid? " + e);
                            }
                    }
            }
        }

        private static void DefineParams(string[] args)
        {
            string[] argList = args;
            foreach (var arg in argList)
            {
                Console.WriteLine(arg);
            }

            DefineGame(argList[0]);

            if (custom)
            {
                for (int i = 1; i < argList.Length; i++)
                {
                    bool IsFolder = true;
                    foreach (string GArg in AllArgs)
                    {
                        if(GArg.ToLower() == argList[i].ToLower())
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
                return;
            }

            switch (argList[1])
            {
                case "tex":
                    tex = true;
                    break;

                case "unpack":
                    unpack = true;
                    folderList = argList.Skip(2).ToArray();
                    break;

                case "repack":
                    unpack = false;
                    folderList = argList.Skip(2).ToArray();

                    break;

                default:
                    throw new Exception();
            }

            if (!argList[1].Equals("unpack") && !argList[1].Equals("repack"))
            {

                switch (argList[2])
                {
                    case "unpack":
                        unpack = true;
                        folderList = argList.Skip(3).ToArray();

                        break;

                    case "repack":
                        unpack = false;
                        folderList = argList.Skip(3).ToArray();

                        break;

                    default:
                        throw new Exception();

                }
            }

        }

        private static void DefineGame(string arg)
        {
            switch (arg)
            {
                case "custom":
                    custom = true;
                    break;
                case "dd":
                    GameArgs = DdArgs;
                    break;
                case "ddo":
                    GameArgs = DdoArgs;
                    break;

                case "dmc4":
                    GameArgs = Dmc4Args;
                    break;

                case "dmc4se":
                    GameArgs = DmcSE4Args;
                    break;

                case "re0":
                    GameArgs = Re0Args;
                    break;

                case "re5":
                    GameArgs = Re5Args;
                    break;

                case "re6":
                    GameArgs = Re6Args;
                    break;

                case "reHD":
                    GameArgs = ReHDArgs;
                    break;

                case "rev":
                    GameArgs = RevArgs;
                    break;

                case "rev2":
                    GameArgs = Rev2Args;
                    break;

                default:
                    throw new Exception();
            }
        }

    }
}
