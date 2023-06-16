using Chess_Cabs.Game;
using Chess_Cabs.Utils;
using ConsoleChess;
using Stockfish.NET;
using Stockfish.NET.Exceptions;
using Stockfish.NET.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Chess_Cabs.Engines
{
    public static class StockfishAI
    {

        public static string ConvertGameToFEN(char[,] board)
        {
            // Generate the FEN string for the resulting board position
            string fen = "";
            for (int y = 0; y < 8; y++)
            {
                int emptySquares = 0;
                for (int x = 0; x < 8; x++)
                {
                    char piece = Utilities.ChangeColorOfPiece(Utilities.GetRealChar(board[7 - y, x]));
                    if (piece == ' ')
                    {
                        emptySquares++;
                    }
                    else
                    {
                        if (emptySquares > 0)
                        {
                            fen += emptySquares.ToString();
                            emptySquares = 0;
                        }
                        fen += piece;
                    }
                }
                if (emptySquares > 0)
                {
                    fen += emptySquares.ToString();
                }
                if (y < 7)
                {
                    fen += "/";
                }
            }

            if (ChessConsole.isWhite)
            {
                fen += " w - 0 1";
            }
            else
            {
                fen += " b - 0 1";
            }

            return fen;
        }

        public static Tuple<int, int, int, int> GetBestMove(string fen, int moveTime)
        {
            // Start Stockfish process
            Process stockfishProcess = new Process();
            stockfishProcess.StartInfo.FileName = @"D:\Chess_Cabs\Engines\stockfish-windows-2022-x86-64-modern.exe";
            stockfishProcess.StartInfo.UseShellExecute = false;
            stockfishProcess.StartInfo.RedirectStandardInput = true;
            stockfishProcess.StartInfo.RedirectStandardOutput = true;
            stockfishProcess.Start();

            stockfishProcess.StandardInput.WriteLine("position fen " + fen);

            stockfishProcess.StandardInput.WriteLine($"go movetime {moveTime}");

            Tuple<int, int, int, int> move = Tuple.Create(-1, -1, -1, -1);

            while (true)
            {
                string output = stockfishProcess.StandardOutput.ReadLine()!;
                if (output.StartsWith("bestmove"))
                {
                    output = output.Substring(9, 4);
                    move = Utilities.ConvertStringToTupleStockfish(output);
                    break;
                }
            }

            stockfishProcess.Kill();
            stockfishProcess.WaitForExit();
            stockfishProcess.Close();
            return move;
        }
    }
}
