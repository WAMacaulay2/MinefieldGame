namespace MinefieldGame
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public class Minefield
    {
        private const int EASY_ROWS = 8;
        private const int EASY_COLS = 8;
        private const int EASY_MINES = 10;

        private const int MEDIUM_ROWS = 16;
        private const int MEDIUM_COLS = 16;
        private const int MEDIUM_MINES = 40;

        private const int HARD_ROWS = 30;
        private const int HARD_COLS = 16;
        private const int HARD_MINES = 99;

        public MinefieldSpace[][] Field { get; set; }
        private Difficulty Difficulty { get; set; }
        private bool Mined { get; set; }

        public int Row { get => Field.Length; }
        public int Columns { get => Field[0].Length; }

        public Minefield(Difficulty difficulty)
        {
            Difficulty = difficulty;
            switch(difficulty)
            {
                case Difficulty.Easy:
                default:
                    Field = new MinefieldSpace[EASY_ROWS][];
                    for (int i = 0; i < EASY_ROWS; i++)
                    {
                        Field[i] = new MinefieldSpace[EASY_COLS];
                        for (int j = 0; j < EASY_COLS; j++)
                            Field[i][j] = new MinefieldSpace();
                    }
                    break;
                case Difficulty.Medium:
                    Field = new MinefieldSpace[MEDIUM_ROWS][];
                    for (int i = 0; i < MEDIUM_ROWS; i++)
                    {
                        Field[i] = new MinefieldSpace[MEDIUM_COLS];
                        for (int j = 0; j < MEDIUM_COLS; j++)
                            Field[i][j] = new MinefieldSpace();
                    }
                    break;
                case Difficulty.Hard:
                    Field = new MinefieldSpace[HARD_ROWS][];
                    for (int i = 0; i < HARD_ROWS; i++)
                    {
                        Field[i] = new MinefieldSpace[HARD_COLS];
                        for (int j = 0; j < HARD_COLS; j++)
                            Field[i][j] = new MinefieldSpace();
                    }
                    break;
            }
        }
    }

    public class MinefieldSpace
    {
        public bool Revealed { get; set; }
        public bool HasMine { get; set; }
        public bool Flagged { get; set; }
        public int AdjacentMines { get; set; }
    }
}
