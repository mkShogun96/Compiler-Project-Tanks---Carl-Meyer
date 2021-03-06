﻿using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    /**
     * This class is responsible for the Command Line Interface and calling the appropriate methods
     * for this Project.
     */
    public class Menu
    {

        private string _sourceCode = "";
        private Scanner _scanner;
        private Program _program;
        public void StartMenu()
        {
            Console.Clear();
            UI.Strong("Welcome to Carl's Project Tanks Solution!");
            UI.Info("\nPlease enter a command:\n");
            
            ParseCommand(Console.ReadLine());
        }

        private void ParseCommand(string command)
        {
            if (command == null)
            {
                UI.Error("Can not process empty command!");
                ParseCommand(Console.ReadLine());
                return;
            }

            switch (command.ToLower())
            {
                case "read":
                    ReadCode();
                    break;
                case "load":
                    LoadSourceCode();
                    break;
                case "save":
                    SaveSourceCode();
                    break;
                case "print":
                    if (!HasSourceCode())
                        break;
                    PrintSourceCode();
                    break;
                case "scan":
                    if (!HasSourceCode())
                        break;
                    Scan();
                    break;
                case "display tokens":
                    if (!HasSourceCode())
                        break;
                    if (!SourceCodeScanned())
                        break;
                    DisplayTokens();
                    break;
                case "parse":
                    Parse();
                    break;
                case "context":
                    ContextualAnalysis();
                    break;
                case "help":
                    ShowHelp();
                    break;
                case "quit":
                    Quit();
                    return;
                // case "run": - dont have to run the code
                //     break;
            }

            UI.Info("\nPlease enter a command:\n");
            ParseCommand(Console.ReadLine());
        }

        // =============================================================================================================
        // Validation Methods for using CLI

        private bool HasSourceCode()
        {
            if (_sourceCode == "")
            {
                UI.Error("No source code found. Please ensure that you have source code in memory.");
                return false;
            }

            return true;
        }

        private bool SourceCodeScanned()
        {
            if (_scanner == null)
            {
                UI.Error("Source code has not been scanned yet.");
                UI.Info("Do you want to scan now? (y/n)");
                Console.WriteLine();
                if (Console.ReadKey().Equals("y"))
                {
                    Scan();
                    return true;
                }
                UI.Error("\nScanner has not been initialized. Please scan the source code.");
                return false;
            }

            if (!_scanner._Sentence.Equals(_sourceCode))
            {
                UI.Warn("The source code that was scanned does not match the source code in memory.");
                UI.Info("Do you want to scan the new source code now? (y/n)");
                if (Console.ReadKey().Equals("y"))
                {
                    Scan();
                    return true;
                }
                UI.Warn("\nWarning! Scanner is using old source code.");
            }

            return true;
        }
        // -------------------------------------------------------------------------------------------------------------
        
        // =============================================================================================================
        // CLI Command Methods

        /**
         * Enter source code to be saved in a text file so we can just simply load the source code
         * again using the load command.
         * (This is so you don't have to type in the source code every time)
         */
        private void SaveSourceCode()
        {
            try
            {
                StreamWriter sw = new StreamWriter(@"C:\Users\NMU\Google Drive\NMU BSc Honours\Compiler Theory\Project TANKS\Compiler Project Tanks - Carl Meyer\Compiler Project Tanks - Carl Meyer\SourceCode.txt");
                
                UI.Info("\nPlease enter source code:\n");
                
                string sourceCode = Console.ReadLine();
                
                sw.WriteLine(sourceCode);
                
                sw.Close();
                
                UI.Info("Successfully Saved Source Code!");
                
            }
            catch(Exception e)
            {
                UI.Error("Exception: " + e.Message);
            }
        }
        
        private void LoadSourceCode()
        {
            try
            {
                
                StreamReader sr = new StreamReader(@"C:\Users\NMU\Google Drive\NMU BSc Honours\Compiler Theory\Project TANKS\Compiler Project Tanks - Carl Meyer\Compiler Project Tanks - Carl Meyer\SourceCode.txt");
                
                string line = sr.ReadLine();
                
                if (line == null)
                {
                    UI.Error("Error! No source code found. Try using the save command to save your source code first.");
                    sr.Close();
                    return;
                }

                _sourceCode = line;
                sr.Close();
                UI.Info("Successfully loaded source code from save file.");
                PrintSourceCode();
            }
            catch(Exception e)
            {
                UI.Error("Error! Could not load source code: " + e.Message);
            }
        }
        
        /**
         * Create a new instance of the scanner with the source code that is currently loaded in memory
         */
        private void Scan()
        {
            try
            {
                if (_sourceCode == "")
                {
                    UI.Error("No source code found. Please ensure that you have source code in memory.");
                    return;
                }
            
                _scanner = new Scanner(_sourceCode);
                
                UI.Info("Successfully scanned source code.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not execute scan command due to error: {0}",e);
            }
            
        }

        /**
         * Displays the tokens that were identified after scanning the source code.
         * (Requires source code to be loaded in memory and scan command to have been called)
         */
        private void DisplayTokens()
        {
            if (_sourceCode == "")
            {
                UI.Error("No source code found. Please ensure that you have source code in memory.");
                return;
            }

            UI.Info("The following tokens have been identified in the source code:");
            _scanner.DisplayTokens();
        }

        private void Parse()
        {
            if (_sourceCode == "")
            {
                ReadCode();
            }
            UI.Info("\nParsing Source Code...\n");
            Parser parser = new Parser(_sourceCode);
            _program = parser._program;
        }
        
        private void ContextualAnalysis()
        {
            Analyser analyser = new Analyser();
            analyser.Check(_program);
        }
        
        private void Quit()
        {
            Environment.Exit(0);
        }


        private void ShowHelp()
        {
            Console.WriteLine("\nList of possible commands:");
            Console.WriteLine("read - Read the Source Code entered by the user. The Source Code is kept in memory until you use the read command again.");
            Console.WriteLine("save - save source code that can be loaded to memory using the load command.");
            Console.WriteLine("load - load source code that has previously been saved.");
            Console.WriteLine("print - displays the contents of the Source Code");
            Console.WriteLine("scan - scans the Source Code using the scanner. (keeps the scanner in memory for further usage)");
            Console.WriteLine("display tokens - Display the tokens that have been identified by the scanner");
            Console.WriteLine("parse - parses the Source Code and then displays the tokens to the console.");
            Console.WriteLine("context - perform a contextual analysis of the AST built from the Source Code.");
            // Console.WriteLine("run - the Source Code"); - dont have to run the code
            Console.WriteLine("help - displays the help message.");
            Console.WriteLine("quit - quit the program.");
            
        }

        private void ReadCode()
        {
            UI.Info("\nPlease enter source code:\n");

            _sourceCode = Console.ReadLine();
        }

        private void PrintSourceCode()
        {
            // Display the file contents to the console. Variable text is a string.
            UI.Info("\nContents of source code:");
            UI.Strong(_sourceCode);
        }
    }
    
    // -------------------------------------------------------------------------------------------------------------
}