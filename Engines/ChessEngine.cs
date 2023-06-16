using Chess_Cabs.Game;
using ConsoleChess;
using Chess_Cabs.Utils;

namespace Chess_Cabs.Engines
{
    public class ChessEngine
    {
        public int Depth { get; set; }
        public ChessEngine(int depth)
        {
            Depth = depth;
        }

        public Tuple<int, int, int, int> FindMove(ChessBoard board, bool isWhite)
        {
            Dictionary<Tuple<int, int, int, int>, int> MovesAndAdvantage = new();

            foreach (Tuple<int, int, int, int> validMove in board.GetValidMoves(ChessConsole.isWhite))
            {
                ChessBoard copiedBoard = board.Clone();
                copiedBoard.Move(validMove.Item1, validMove.Item2, validMove.Item3, validMove.Item4, isWhite, false);
                if (copiedBoard.IsCheckMate(!isWhite))
                {
                    return validMove;
                }
                int advantage = SearchDepth(copiedBoard, 3, isWhite);
                MovesAndAdvantage.Add(validMove, advantage);
            }

            var query = from move in MovesAndAdvantage
                        orderby move.Value descending
                        where move.Value == MovesAndAdvantage.Values.Max()
                        select move.Key;

            Random rnd = new Random();
            int index = rnd.Next(0, query.Count());

            return query.ElementAt(index);
        }

        public int SearchDepth(ChessBoard board, int currentDepth, bool isWhite)
        {
            int advantage = board.GetAdvantage(ChessConsole.isWhite);

            foreach (Tuple<int, int, int, int> validMove in board.GetValidMoves(isWhite))
            {
                if (currentDepth <= Depth)
                {
                    ChessBoard copiedBoard = board.Clone();
                    copiedBoard.Move(validMove.Item1, validMove.Item2, validMove.Item3, validMove.Item4, isWhite, false);
                    int nextAdvantage = SearchDepth(copiedBoard, currentDepth + 1, !isWhite);
                    if (nextAdvantage < advantage)
                    {
                        advantage = nextAdvantage;
                    }
                }
            }

            return advantage;
        }

        public void Move(ChessBoard board, bool isWhite)
        {
            Tuple<int,int,int,int> move = FindMove(board, isWhite);

            ChessConsole._board.Move(move.Item1, move.Item2, move.Item3, move.Item4, isWhite, false);
            ChessConsole._board.DrawBoard();
        }
    }
}
