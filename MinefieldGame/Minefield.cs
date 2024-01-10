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
        public Difficulty Difficulty { get; private set; }
        public bool Mined { get; private set; }

        public int Rows
        {
            get
            {
                switch (Difficulty)
                {
                    case Difficulty.Easy:
                    default:
                        return EASY_ROWS;
                    case Difficulty.Medium:
                        return MEDIUM_ROWS;
                    case Difficulty.Hard:
                        return HARD_ROWS;
                }
            }
        }
        public int Columns
        {
            get
            {
                switch (Difficulty)
                {
                    case Difficulty.Easy:
                    default:
                        return EASY_COLS;
                    case Difficulty.Medium:
                        return MEDIUM_COLS;
                    case Difficulty.Hard:
                        return HARD_COLS;
                }
            }
        }
        public int TotalMines
        {
            get
            {
                switch (Difficulty)
                {
                    case Difficulty.Easy:
                    default:
                        return EASY_MINES;
                    case Difficulty.Medium:
                        return MEDIUM_MINES;
                    case Difficulty.Hard:
                        return HARD_MINES;
                }
            }
        }
        public int FlaggedSpaces { get => Field.Sum(x => x.Sum(y => y.Flagged ? 1 : 0)); }
        public int HiddenSpaces { get => Field.Sum(x => x.Sum(y => y.Revealed ? 0 : 1)); }
        public int RemainingMines { get => TotalMines - FlaggedSpaces; }

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

        public void SetMines()
        {
            if (Mined)
                return;

            if (HiddenSpaces < TotalMines)
                throw new Exception("Not enough spaces for mines.");

            // Add all the mines.
            int setMines = 0;
            Random rng = new Random();
            while(setMines < TotalMines)
            {
                int x = rng.Next(0, Rows - 1);
                int y = rng.Next(0, Columns - 1);

                var cell = Field[x][y];
                // Don't re-mine the same space or mine the first space the player clicked.
                if (!cell.HasMine && !cell.Revealed)
                {
                    cell.HasMine = true;
                    setMines++;
                }
            }

            // Set mine counts on empty spaces.
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    var cell = Field[i][j];
                    if(!cell.HasMine)
                    {
                        // Top-left space.
                        if (i > 0 && j > 0 && Field[i - 1][j - 1].HasMine)
                            cell.AdjacentMines++;
                        // Top space.
                        if (j > 0 && Field[i][j - 1].HasMine)
                            cell.AdjacentMines++;
                        // Top-right space.
                        if (i < Rows - 1 && j > 0 && Field[i + 1][j - 1].HasMine)
                            cell.AdjacentMines++;

                        // Left space.
                        if (i > 0 && Field[i - 1][j].HasMine)
                            cell.AdjacentMines++;
                        // Right space.
                        if (i < Rows - 1 && Field[i + 1][j].HasMine)
                            cell.AdjacentMines++;

                        // Bottom-left space.
                        if (i > 0 && j < Columns - 1 && Field[i - 1][j + 1].HasMine)
                            cell.AdjacentMines++;
                        // Bottom space.
                        if (j < Columns - 1 && Field[i][j + 1].HasMine)
                            cell.AdjacentMines++;
                        // Bottom-right space.
                        if (i < Rows - 1 && j < Columns - 1 && Field[i + 1][j + 1].HasMine)
                            cell.AdjacentMines++;
                    }
                }
            }

            Mined = true;
        }

        public void RevealSpace(int x, int y)
        {
            var startingCell = Field[x][y];
            // Don't reveal flagged spaces.
            if(startingCell.Flagged)
                return;

            startingCell.Revealed = true;

            // Mine the field if it hasn't been mined yet.
            if(!Mined)
            {
                // Top-left space.
                if (x > 0 && y > 0)
                    Field[x - 1][y - 1].Revealed = true;
                // Top space.
                if (y > 0)
                    Field[x][y - 1].Revealed = true;
                // Top-right space.
                if (x < Rows - 1 && y > 0)
                    Field[x + 1][y - 1].Revealed = true;

                // Left space.
                if (x > 0)
                    Field[x - 1][y].Revealed = true;
                // Right space.
                if (x < Rows - 1)
                    Field[x + 1][y].Revealed = true;

                // Bottom-left space.
                if (x > 0 && y < Columns - 1)
                    Field[x - 1][y + 1].Revealed = true;
                // Bottom space.
                if (y < Columns - 1)
                    Field[x][y + 1].Revealed = true;
                // Bottom-right space.
                if (x < Rows - 1 && y < Columns - 1)
                    Field[x + 1][y + 1].Revealed = true;
                SetMines();
            }
            // No other spaces will be revealed if this isn't a blank space.
            else if (startingCell.HasMine || startingCell.AdjacentMines > 0)
                return;

            // Reveal adjacent spaces.
            bool again = true;
            while(again)
            {
                again = false;
                for(int i = 0; i < Rows; i++)
                {
                    for(int j = 0; j < Columns; j++)
                    {
                        var cell = Field[i][j];
                        if(!cell.Revealed || cell.HasMine || cell.AdjacentMines > 0)
                            continue;

                        // Top-left space.
                        if (i > 0 && j > 0)
                        {
                            var otherCell = Field[i - 1][j - 1];
                            if (!otherCell.Revealed && !otherCell.HasMine)
                            {
                                otherCell.Revealed = true;
                                again = true;
                            }
                        }
                        // Top space.
                        if (j > 0)
                        {
                            var otherCell = Field[i][j - 1];
                            if (!otherCell.Revealed && !otherCell.HasMine)
                            {
                                otherCell.Revealed = true;
                                again = true;
                            }
                        }
                        // Top-right space.
                        if (i < Rows - 1 && j > 0)
                        {
                            var otherCell = Field[i + 1][j - 1];
                            if (!otherCell.Revealed && !otherCell.HasMine)
                            {
                                otherCell.Revealed = true;
                                again = true;
                            }
                        }

                        // Left space.
                        if (i > 0)
                        {
                            var otherCell = Field[i - 1][j];
                            if (!otherCell.Revealed && !otherCell.HasMine)
                            {
                                otherCell.Revealed = true;
                                again = true;
                            }
                        }
                        // Right space.
                        if (i < Rows - 1)
                        {
                            var otherCell = Field[i + 1][j];
                            if (!otherCell.Revealed && !otherCell.HasMine)
                            {
                                otherCell.Revealed = true;
                                again = true;
                            }
                        }

                        // Bottom-left space.
                        if (i > 0 && j < Columns - 1)
                        {
                            var otherCell = Field[i - 1][j + 1];
                            if (!otherCell.Revealed && !otherCell.HasMine)
                            {
                                otherCell.Revealed = true;
                                again = true;
                            }
                        }
                        // Bottom space.
                        if (j < Columns - 1)
                        {
                            var otherCell = Field[i][j + 1];
                            if (!otherCell.Revealed && !otherCell.HasMine)
                            {
                                otherCell.Revealed = true;
                                again = true;
                            }
                        }
                        // Bottom-right space.
                        if (i < Rows - 1 && j < Columns - 1)
                        {
                            var otherCell = Field[i + 1][j + 1];
                            if (!otherCell.Revealed && !otherCell.HasMine)
                            {
                                otherCell.Revealed = true;
                                again = true;
                            }
                        }
                    }
                }
            }
        }

        public void FlagSpace(int x, int y)
        {
            var cell = Field[x][y];
            if (!cell.Revealed)
                cell.Flagged = !cell.Flagged;
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
