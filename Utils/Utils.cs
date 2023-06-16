using Chess_Cabs.Game;
using ConsoleChess;

namespace Chess_Cabs.Utils
{
    public static class Utilities
    {

        public static Tuple<int, int, int, int> ConvertStringToTupleStockfish(string inputString)
        {

            if (inputString.Length < 4)
            {
                return Tuple.Create(-1, -1, -1, -1);
            }

            string char1 = inputString[..2];
            string char2 = inputString[2..];

            int item1 = char1[0] - 'a';
            int item2 = char1[1] - '1';
            int item3 = char2[0] - 'a';
            int item4 = char2[1] - '1';


            return Tuple.Create(item1, item2, item3, item4);

        }

        public static char ChangeColorOfPiece(char piece)
        {
            if (char.IsUpper(piece))
            {
                return char.ToLower(piece);
            }
            else
            {
                return char.ToUpper(piece);
            }
        }

        public static char GetRealChar(char piece)
        {
            switch(piece)
            {
                case '♔':
                    return 'k';

                case '♕':
                    return 'q';

                case '♖':
                    return 'r';

                case '♗':
                    return 'b';

                case '♘':
                    return 'n';
                    break;

                case '♙':
                    return 'p';
                    break;

                case '♚':
                    return 'K';

                case '♛':
                    return 'Q';

                case '♜':
                    return 'R';

                case '♝':
                    return 'B';

                case '♞':
                    return 'N';
                    break;

                case '♟':
                    return 'P';
                    break;
            }

            return piece;
        }
    }
}
