using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Chess_Cabs.Game;
using Chess_Cabs.Utils;


namespace ConsoleChess
{

    public class ChessBoard
    {
        public static bool whiteQueensideRookHasMoved = false;
        public static bool whiteKingsideRookHasMoved = false;
        public static bool whiteKingHasMoved = false;
        public static bool blackKingsideRookHasMoved = false;
        public static bool blackQueensideRookHasMoved = false;
        public static bool blackKingHasMoved = false;
        public static bool hasCastledThisMove;
        public static int enPassantX = -1;
        public static int enPassantY = -1;
        public static bool enPassantAvailable;
        private const int size = 8;
        public char[,] _board = new char[size, size]
        {
            {'♖', '♘', '♗', '♕', '♔', '♗', '♘', '♖'},
            {'♙', '♙', '♙', '♙', '♙', '♙', '♙', '♙'},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {'♟', '♟', '♟', '♟', '♟', '♟', '♟', '♟'},
            {'♜', '♞', '♝', '♛', '♚', '♝', '♞', '♜'}
        };

        public int GetAdvantage(bool isWhite)
        {
            int advantage = 0;

            if(IsCheckMate(isWhite))
            {
                return 1000;
            }

            foreach(char ch in _board)
            {
                switch(ch)
                {
                    case '♕':
                        advantage += 9;
                        break;

                    case '♛':
                        advantage -= 9;
                        break;

                    case '♖':
                        advantage += 5;
                        break;

                    case '♜':
                        advantage -= 5;
                        break;

                    case '♗':
                        advantage += 3;
                        break;

                    case '♝':
                        advantage -= 3;
                        break;

                    case '♘':
                        advantage += 3;
                        break;

                    case '♞':
                        advantage -= 3;
                        break;

                    case '♙':
                        advantage += 1;
                        break;

                    case '♟':
                        advantage -= 1;
                        break;
                }
            }

            return isWhite ? advantage : -1 * advantage;
        }

        public ChessBoard Clone()
        {
            ChessBoard clone = new();

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++) 
                {
                    clone._board[x, y] = _board[x, y];
                }
            }

            return clone;
        }

        public void MoveBack(int startX, int startY, int endX, int endY)
        {
            char piece = _board[endY, endX];
            _board[endY, endX] = _board[startY, startX];
            _board[startY, startX] = piece;
        }

        public bool Move(int startX, int startY, int endX, int endY, bool isWhite, bool analyse)
        {
            char piece = _board[startY, startX];

            if (piece == ' ')
            {
                return false;
            }

            if (!IsValidMove(startX, startY, endX, endY, isWhite))
            {
                return false;
            }

            if (startX == 0 && startY == 0)
            {
                whiteQueensideRookHasMoved = true;
            }
            else if (startX == 7 && startY == 0)
            {
                whiteKingsideRookHasMoved = true;
            }
            else if (startX == 0 && startY == 7)
            {
                blackQueensideRookHasMoved = true;
            }
            else if (startX == 7 && startY == 7)
            {
                blackKingsideRookHasMoved = true;
            }

            if (piece == '♟' && endY == 0)
            {
                piece = '♛';
            }
            else if (piece == '♙' && endY == 7)
            {
                piece = '♕';
            }

            _board[startY, startX] = ' ';
            _board[endY, endX] = piece;

            return true;
        }

        public char GetChar(int y, int x)
        {
            return _board[y, x];
        }

        private bool IsValidMove(int startX, int startY, int endX, int endY, bool isWhite)
        {
            if (isWhite)
            {
                switch (_board[startY, startX])
                {
                    case '♙':
                        return IsValidPawnMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                    case '♖':
                        return IsValidRookMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                    case '♘':
                        return IsValidKnightMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                    case '♗':
                        return IsValidBishopMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                    case '♕':
                        return IsValidQueenMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                    case '♔':
                        return IsValidKingMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                }
            }
            else
            {
                switch (_board[startY, startX])
                {
                    case '♟':
                        return IsValidPawnMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                    case '♜':
                        return IsValidRookMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                    case '♞':
                        return IsValidKnightMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                    case '♝':
                        return IsValidBishopMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                    case '♛':
                        return IsValidQueenMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                    case '♚':
                        return IsValidKingMove(startX, startY, endX, endY) && !IsCheckAfterwards(startX, startY, endX, endY, isWhite);
                }
            }

            if (enPassantAvailable)
            {
                enPassantAvailable = false;
            }
            else
            {
                enPassantX = -1;
                enPassantY = -1;
            }

            return false;
        }

        private bool IsCheckAfterwards(int startX, int startY, int endX, int endY, bool isWhite)
        {

            char endPiece = _board[endY, endX];
            char startPiece = _board[startY, startX];
            _board[endY, endX] = _board[startY, startX];
            _board[startY, startX] = ' ';

            bool isCheckAfterwards = IsCheck(isWhite);

            _board[endY, endX] = endPiece;
            _board[startY, startX] = startPiece;

            return isCheckAfterwards;
        }

        private bool IsValidPawnMove(int startX, int startY, int endX, int endY)
        {
            int deltaX = endX - startX;
            int deltaY = endY - startY;

            char piece = _board[startY, startX];
            int direction = IsPieceBlack(piece) ? -1 : 1;

            if (deltaX == 0 && deltaY == direction)
            {
                return _board[endY, endX] == ' ';
            }

            if (deltaX == 0 && deltaY == 2 * direction && startY == (IsPieceBlack(piece) ? 6 : 1))
            {
                int middleY = startY + direction;
                enPassantX = startX;
                enPassantY = middleY;
                enPassantAvailable = true;
                return _board[middleY, startX] == ' ' && _board[endY, endX] == ' ';
            }

            if (Math.Abs(deltaX) == 1 && deltaY == direction)
            {
                if (endX == enPassantX && endY == enPassantY)
                {
                    _board[enPassantY + (-1 * direction), enPassantX] = ' ';
                    return true;
                }

                return IsPieceBlack(piece) ? IsPieceWhite(_board[endY, endX]) : IsPieceBlack(_board[endY, endX]);
            }

            return false;
        }

        private bool IsValidRookMove(int startX, int startY, int endX, int endY)
        {
            int deltaX = endX - startX;
            int deltaY = endY - startY;

            if (deltaX != 0 && deltaY != 0)
            {
                return false;
            }

            int stepX = Math.Sign(deltaX);
            int stepY = Math.Sign(deltaY);

            int x = startX + stepX;
            int y = startY + stepY;

            while (x != endX || y != endY)
            {
                if (_board[y, x] != ' ')
                {
                    return false;
                }

                x += stepX;
                y += stepY;
            }

            if (ChessConsole.isWhite ? IsPieceWhite(_board[endY, endX]) : IsPieceBlack(_board[endY, endX]))
            {
                return false;
            }

            return true;
        }

        private bool IsValidKnightMove(int startX, int startY, int endX, int endY)
        {
            int deltaX = Math.Abs(endX - startX);
            int deltaY = Math.Abs(endY - startY);
            bool isWhite = ChessConsole.isWhite;

            if ((deltaX == 1 && deltaY == 2) || (deltaX == 2 && deltaY == 1))
            {
                return (isWhite ? IsPieceBlack(_board[endY, endX]) : IsPieceWhite(_board[endY, endX])) || _board[endY, endX] == ' ';
            }

            return false;
        }

        private bool IsValidBishopMove(int startX, int startY, int endX, int endY)
        {
            int deltaX = endX - startX;
            int deltaY = endY - startY;

            if (Math.Abs(deltaX) != Math.Abs(deltaY))
            {
                return false;
            }

            int stepX = Math.Sign(deltaX);
            int stepY = Math.Sign(deltaY);
            int x = startX + stepX;
            int y = startY + stepY;

            while (x != endX || y != endY)
            {
                if (_board[y, x] != ' ')
                {
                    return false;
                }

                x += stepX;
                y += stepY;
            }

            if (ChessConsole.isWhite ? IsPieceWhite(_board[endY, endX]) : IsPieceBlack(_board[endY, endX]))
            {
                return false;
            }

            return true;
        }

        private bool IsValidQueenMove(int startX, int startY, int endX, int endY)
        {
            return IsValidRookMove(startX, startY, endX, endY) || IsValidBishopMove(startX, startY, endX, endY);
        }

        private bool IsValidKingMove(int startX, int startY, int endX, int endY)
        {
            int deltaX = Math.Abs(endX - startX);
            int deltaY = Math.Abs(endY - startY);
            bool isWhite = ChessConsole.isWhite;

            if (deltaX <= 1 && deltaY <= 1)
            {
                return (isWhite ? IsPieceBlack(_board[endY, endX]) : IsPieceWhite(_board[endY, endX])) || _board[endY, endX] == ' ';
            }

            if (isWhite)
            {
                if (endX == 7 && endY == 0 && startX == 4 && startY == 0 && !IsCheck(ChessConsole.isWhite))
                {
                    if (!whiteKingHasMoved && !whiteKingsideRookHasMoved)
                    {
                        if (_board[startY, startX + 1] == ' ' && _board[startY, startX + 2] == ' ')
                        {
                            hasCastledThisMove = true;
                            _board[startY, startX] = ' ';
                            _board[startY, startX + 2] = '♔';
                            _board[startY, startX + 3] = ' ';
                            _board[startY, startX + 1] = '♖';
                            return false;
                        }
                    }
                }
                else if (endX == 0 && endY == 0 && startX == 4 && startY == 0 && !IsCheck(ChessConsole.isWhite));
                {
                    if (!whiteKingHasMoved && !whiteQueensideRookHasMoved)
                    {
                        if (_board[startY, startX - 1] == ' ' && _board[startY, startX - 2] == ' ' && _board[startY, startX - 3] == ' ')
                        {
                            hasCastledThisMove = true;
                            _board[startY, startX] = ' ';
                            _board[startY, startX - 2] = '♔';
                            _board[startY, startX - 4] = ' ';
                            _board[startY, startX - 1] = '♖';
                            return false;
                        }
                    }
                }
            }
            else
            {
                if (endX == 7 && endY == 7 && startX == 4 && startY == 7 && !IsCheck(ChessConsole.isWhite))
                {
                    if (!blackKingHasMoved && !blackKingsideRookHasMoved)
                    {
                        if (_board[startY, startX + 1] == ' ' && _board[startY, startX + 2] == ' ')
                        {
                            hasCastledThisMove = true;
                            _board[startY, startX] = ' ';
                            _board[startY, startX + 2] = '♚';
                            _board[startY, startX + 3] = ' ';
                            _board[startY, startX + 1] = '♜';
                            return false;
                        }
                    }
                }
                else if (endX == 0 && endY == 7 && startX == 4 && startY == 7 && !IsCheck(ChessConsole.isWhite))
                {
                    if (!blackKingHasMoved && !blackQueensideRookHasMoved)
                    {
                        if (_board[startY, startX - 1] == ' ' && _board[startY, startX - 2] == ' ' && _board[startY, startX - 3] == ' ')
                        {
                            hasCastledThisMove = true;
                            _board[startY, startX] = ' ';
                            _board[startY, startX - 2] = '♚';
                            _board[startY, startX - 4] = ' ';
                            _board[startY, startX - 1] = '♜';
                            return false;
                        }
                    }
                }
            }

            return false;
        }

        public void DrawBoard()
        {
            Console.Clear();
            Console.WriteLine("    a   b   c   d   e   f   g   h  ");
            Console.WriteLine("  +---+---+---+---+---+---+---+---+");
            for (int y = 7; y >= 0; y--)
            {
                Console.Write($"{y + 1} ");
                for (int x = 0; x < 8; x++)
                {
                    Console.Write($"| {_board[y, x]} ");
                }
                Console.WriteLine($"| {y + 1}");
                Console.WriteLine("  +---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("    a   b   c   d   e   f   g   h  ");
        }

        private static bool ContainsXY(int[,] selectedXY, int x, int y)
        {
            for(int i = 0; i < selectedXY.GetLength(0); i++)
            {
                if (selectedXY[i,0] == x && selectedXY[i, 1] == y)
                {
                    return true;
                }
            }

            return false;
        }

        public void DrawBoardSelected(int[,] selectedXY, int selectedX, int selectedY, ConsoleColor color)
        {
            Console.Clear();
            Console.WriteLine("    a   b   c   d   e   f   g   h  ");
            Console.WriteLine("  +---+---+---+---+---+---+---+---+");
            for (int y = 7; y >= 0; y--)
            {
                Console.Write($"{y + 1} ");
                for (int x = 0; x < 8; x++)
                {
                    if(selectedX == x && selectedY == y)
                    {
                        Console.Write("| ");
                        Console.BackgroundColor = color;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"{_board[y, x]}");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    else if (ContainsXY(selectedXY, x, y))
                    {
                        Console.Write("| ");
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{_board[y, x]}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write($"| {_board[y, x]} ");
                    }
                }
                Console.WriteLine($"| {y + 1}");
                Console.WriteLine("  +---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("    a   b   c   d   e   f   g   h  ");
        }

        public bool IsCheck(bool isWhite)
        {
            int kingRow = -1;
            int kingCol = -1;
            char king = isWhite ? '♔' : '♚';

            // Find king's position
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_board[i, j] == king)
                    {
                        kingRow = i;
                        kingCol = j;
                        break;
                    }
                }
            }

            if(kingCol > 7 || kingCol < 0)
            {
                return false;
            }

            // Check for attacks from rooks and queens in horizontal and vertical directions
            for (int i = kingRow - 1; i >= 0; i--)
            {
                if (_board[i, kingCol] != ' ')
                {
                    if ((isWhite && (_board[i, kingCol] == '♜' || _board[i, kingCol] == '♛'))
                        || (!isWhite && (_board[i, kingCol] == '♖' || _board[i, kingCol] == '♕')))
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = kingRow + 1; i < 8; i++)
            {
                if (_board[i, kingCol] != ' ')
                {
                    if ((isWhite && (_board[i, kingCol] == '♜' || _board[i, kingCol] == '♛'))
                        || (!isWhite && (_board[i, kingCol] == '♖' || _board[i, kingCol] == '♕')))
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int j = kingCol - 1; j >= 0; j--)
            {
                if (_board[kingRow, j] != ' ')
                {
                    if ((isWhite && (_board[kingRow, j] == '♜' || _board[kingRow, j] == '♛'))
                        || (!isWhite && (_board[kingRow, j] == '♖' || _board[kingRow, j] == '♕')))
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int j = kingCol + 1; j < 8; j++)
            {
                if (_board[kingRow, j] != ' ')
                {
                    if ((isWhite && _board[kingRow, j] == '♜' || _board[kingRow, j] == '♛')
                        || (!isWhite && (_board[kingRow, j] == '♖' || _board[kingRow, j] == '♕')))
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // Check for attacks from bishops and queens in diagonal directions
            int[] diagRow = { -1, -1, 1, 1 };
            int[] diagCol = { -1, 1, -1, 1 };
            for (int i = 0; i < 4; i++)
            {
                int row = kingRow + diagRow[i];
                int col = kingCol + diagCol[i];
                while (row >= 0 && row < 8 && col >= 0 && col < 8)
                {
                    if (_board[row, col] != ' ')
                    {
                        if ((isWhite && (_board[row, col] == '♝' || _board[row, col] == '♛'))
                            || (!isWhite && (_board[row, col] == '♗' || _board[row, col] == '♕')))
                        {
                            return true;
                        }
                        else
                        {
                            break;
                        }
                    }
                    row += diagRow[i];
                    col += diagCol[i];
                }
            }

            // Check for attacks from knights
            int[] knightRow = { -2, -2, -1, -1, 1, 1, 2, 2 };
            int[] knightCol = { -1, 1, -2, 2, -2, 2, -1, 1 };
            for (int i = 0; i < 8; i++)
            {
                int row = kingRow + knightRow[i];
                int col = kingCol + knightCol[i];
                if (row >= 0 && row < 8 && col >= 0 && col < 8)
                {
                    if ((isWhite && _board[row, col] == '♞') || (!isWhite && (_board[row, col] == '♘')))
                    {
                        return true;
                    }
                }
            }

            // Check for attacks from pawns
            int pawnDirection = isWhite ? 1 : -1;
            int[] pawnCol = { -1, 1 };
            for (int i = 0; i < 2; i++)
            {
                int row = kingRow + pawnDirection;
                int col = kingCol + pawnCol[i];
                if (row >= 0 && row < 8 && col >= 0 && col < 8)
                {
                    if ((isWhite && _board[row, col] == '♟') || (!isWhite && _board[row, col] == '♙'))
                    {
                        return true;
                    }
                }
            }

            // Check for attacks of the opponents king
            int[] dirsX = new int[] { 1, 1, 1, 0, 0, -1, -1, -1 };
            int[] dirsY = new int[] { 1, 0, -1, 1, -1, 1, 0, -1 };

            for (int i = 0; i < 8; i++)
            {
                int row = kingRow + dirsY[i];
                int col = kingRow + dirsX[i];

                if (row <= 7 && row >= 0 && col <= 7 && col >= 0)
                {
                    if ((isWhite && _board[row, col] == '♚') || (!isWhite && _board[row, col] == '♔'))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<Tuple<int,int,int,int>> GetValidMoves(bool isWhite)
        {
            List<Tuple<int, int, int, int>> validMoves = new();

            for (int startRow = 0; startRow < 8; startRow++)
            {
                for(int startCol = 0; startCol < 8; startCol++)
                {
                    for(int endRow = 0; endRow < 8; endRow++)
                    {
                        for(int endCol = 0; endCol < 8; endCol++)
                        {
                            if(IsValidMove(startCol, startRow, endCol, endRow, isWhite))
                            {
                                validMoves.Add(new Tuple<int,int,int,int> (startCol, startRow, endCol, endRow));
                            }
                        }
                    }
                }
            }

            return validMoves;
        }

        public int[,] GetValidSquaresOfPiece(bool isWhite, int x, int y)
        {
            List<Tuple<int,int>> squares = new();

            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(IsValidMove(x,y,i,j, isWhite))
                    {
                        squares.Add(new Tuple<int, int>(i, j));
                    }
                }
            }



            return ToTwoDArray(squares);
        }

        private static int[,] ToTwoDArray(List<Tuple<int, int>> squares)
        {
            int[,] squareArray = new int[squares.Count, 2];

            foreach(Tuple<int,int> square in squares)
            {
                squareArray[squares.IndexOf(square), 0] = square.Item1;
                squareArray[squares.IndexOf(square), 1] = square.Item2;
            }

            return squareArray;
        }

        public bool IsCheckMate(bool isWhite)
        {
            return GetValidMoves(isWhite).Count == 0 && IsCheck(isWhite);
        }

        public bool IsStaleMate(bool isWhite)
        {
            return GetValidMoves(isWhite).Count == 0 && !IsCheck(isWhite);
        }

        private static bool IsPieceWhite(char piece)
        {
            return piece == '♔' || piece == '♕' || piece == '♖' || piece == '♘' || piece == '♗' || piece == '♙';
        }

        private static bool IsPieceBlack(char piece)
        {
            return piece == '♚' || piece == '♛' || piece == '♜' || piece == '♞' || piece == '♝' || piece == '♟';
        }
    }
}
