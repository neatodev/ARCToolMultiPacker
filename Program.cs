using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

        private static string[] folderList;

        private static string GameArgs;
        private static bool unpack;
        private static bool tex;

        /// <summary>
        /// Format: .\ARCToolMultiPacker.exe [game] [tex(optional)] [unpack/repack] [folders] 
        /// Written by Mario Schweidler (frofoo/neatodev)
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            DefineParams(args);
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

        private static void DefineParams(string[] args)
        {
            string[] argList = args;
            DefineGame(argList[0]);

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
