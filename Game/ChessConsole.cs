using System;
using Chess_Cabs.Engines;
using Chess_Cabs.Utils;
using ConsoleChess;

namespace Chess_Cabs.Game
{
    public static class ChessConsole
    {
        public static readonly ChessBoard _board = new ChessBoard();
        public static readonly ChessEngine engine = new ChessEngine(3);
        public static bool isWhite = true;
        public static bool exit = false;

        public static void Start(int engineSkillLevel)
        {
            _board.DrawBoard();

            while (!exit)
            {
                Again:
                int[] start;
                int[] startPixel;

                do
                {
                    startPixel = Mouse.WaitForInput();
                    start = Mouse.GetCoords(startPixel[0], startPixel[1]);
                }
                while (start[0] == -1);

                int[,] selectedXY = _board.GetValidSquaresOfPiece(isWhite, start[1], 7 - start[0]);

                _board.DrawBoardSelected(selectedXY, start[1], 7 - start[0], ConsoleColor.DarkRed);
                Thread.Sleep(100);

                int[] end;
                do
                {
                    int[] endPixel = Mouse.WaitForInput();
                    end = Mouse.GetCoords(endPixel[0], endPixel[1]);
                }
                while (end[0] == -1);

                int startX = start[1];
                int startY = 7 - start[0];
                int endX = end[1];
                int endY = 7 - end[0];


                if (_board.Move(startX, startY, endX, endY, isWhite, true))
                {
                    _board.DrawBoard();

                    if (_board.Clone().IsCheck(!isWhite))
                    {
                        Console.WriteLine("Check!");
                    }

                    if (_board.Clone().IsCheckMate(!isWhite))
                    {
                        Console.WriteLine("Checkmate!");
                        exit = true;
                    }

                    if (_board.Clone().IsStaleMate(!isWhite))
                    {
                        Console.WriteLine("Stalemate!");
                        exit = true;
                    }
                }
                else if (ChessBoard.hasCastledThisMove)
                {
                    _board.DrawBoard();
                    ChessBoard.hasCastledThisMove = false;
                }
                else
                {
                    _board.DrawBoard();
                    Console.WriteLine("Invalid move...");
                    goto Again;
                }

                if (!(engineSkillLevel == 0))
                {
                    isWhite = !isWhite;
                }

                if (!exit)
                {
                    if(engineSkillLevel == 1)
                    {
                        engine.Move(_board.Clone(), isWhite);
                    }
                    else if(engineSkillLevel == 2)
                    {
                        Tuple<int, int, int, int> move = StockfishAI.GetBestMove(StockfishAI.ConvertGameToFEN(_board._board), 500);
                        _board.Move(move.Item1, move.Item2, move.Item3, move.Item4, isWhite, true);
                        _board.DrawBoard();
                    }

                    if(engineSkillLevel is 1 or 2)
                    {
                        if (_board.Clone().IsCheck(!isWhite))
                        {
                            Console.WriteLine("Check!");
                        }

                        if (_board.Clone().IsCheckMate(!isWhite))
                        {
                            Console.WriteLine("Checkmate!");
                            exit = true;
                        }

                        if (_board.Clone().IsStaleMate(!isWhite))
                        {
                            Console.WriteLine("Stalemate!");
                            exit = true;
                        }
                    }
                }

                isWhite = !isWhite;
            }

            Console.WriteLine();
            Console.WriteLine("Thanks for playing!");
        }
    }
}