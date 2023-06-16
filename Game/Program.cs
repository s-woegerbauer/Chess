using System.Text;
using ConsoleChess;
using Chess_Cabs.Utils;

namespace Chess_Cabs.Game
{
    public class Program
    {
        static void Main(string[] args)
        {
            int engineSkillLevel = GetEngineSkillLevel();
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.Unicode;
            DisableConsoleQuickEdit.Go();
            ChessConsole.Start(engineSkillLevel);
        }

        public static int GetEngineSkillLevel()
        {
            int engine = -1;

            while (engine == -1)
            {
                Console.Write("Do you want to play against? Another Player (0), an easy Chess Engine (1), Stockfish 15.1 (2): ");
                if(!int.TryParse(Console.ReadLine(), out engine))
                {
                    engine = -1;
                }

                if (engine > 2 || engine < 0)
                {
                    engine = -1;
                }
            }

            return engine;
        }
    }
}


