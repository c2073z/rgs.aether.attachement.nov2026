// =====================================================================
//  SORTLE  -  Program.cs   (PROVIDED: students don't need to edit this)
// =====================================================================
//  This is a simple text version of the game that runs in the terminal.
//  It uses BubbleSorter.cs and ScoreKeeper.cs, so you can play the real
//  game as soon as both files are finished.
//  The website version (made from the Canva design) comes later and
//  will use the same two files.
//
//    dotnet run                  -> play the game
//    dotnet run -- check         -> test both student files
//    dotnet run -- check sort    -> test BubbleSorter.cs only
//    dotnet run -- check score   -> test ScoreKeeper.cs only
// =====================================================================

using System.Diagnostics;

namespace Sortle;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length > 0 && args[0].Equals("check", StringComparison.OrdinalIgnoreCase))
        {
            string which = args.Length > 1 ? args[1].ToLowerInvariant() : "all";
            return SelfCheck.Run(which);
        }

        try
        {
            PlayGame();
            return 0;
        }
        catch (NotImplementedException ex)
        {
            Console.WriteLine();
            Console.WriteLine("Oops! The game can't run yet because this part isn't written:");
            Console.WriteLine($"    {ex.Message}");
            Console.WriteLine();
            Console.WriteLine("Run  dotnet run -- check  to see which steps are still to do.");
            return 1;
        }
    }

    private static void PlayGame()
    {
        var random = new Random();
        var score = new ScoreKeeper();
        var stopwatch = new Stopwatch();

        PrintTitle();

        while (!score.IsGameOver)
        {
            int level = score.CurrentLevel;

            // Build a new puzzle for this level.
            int length = random.Next(score.GetMinLength(level), score.GetMaxLength(level) + 1);
            int[] puzzle = BubbleSorter.GenerateRandomList(length, random);
            int correctAnswer = BubbleSorter.CountSwaps(puzzle);

            Console.WriteLine();
            Console.WriteLine($"=== Level {level} of {ScoreKeeper.MaxLevel}  |  Correct {score.ProblemsSolvedThisLevel}/{ScoreKeeper.ProblemsPerLevel}  |  Mistakes {score.WrongThisLevel}  |  Time left: {score.TimeLeft:0.0}s ===");
            Console.WriteLine($"  {FormatList(puzzle)}");
            Console.Write("How many swaps will bubble sort make? > ");

            // The clock runs while the player is thinking.
            stopwatch.Restart();
            int guess = ReadNumber();
            stopwatch.Stop();
            score.Tick(stopwatch.Elapsed.TotalSeconds);

            if (score.IsGameOver)
            {
                Console.WriteLine("Time's up! The clock ran out while you were thinking.");
                break;
            }

            bool isCorrect = guess == correctAnswer;
            score.SubmitAnswer(isCorrect);

            if (isCorrect)
            {
                Console.WriteLine($"Correct! +{score.GetTimeBonus(level)}s");
            }
            else
            {
                Console.WriteLine($"Not quite. The answer was {correctAnswer}. -{score.GetTimePenalty(level)}s");
                ShowReplay(puzzle);
            }

            if (score.HasWonGame)
            {
                Console.WriteLine();
                Console.WriteLine($"*** You cleared all {ScoreKeeper.MaxLevel} levels! ***");
            }
            else if (score.IsGameOver)
            {
                Console.WriteLine("Time's up!");
            }
            else if (score.CurrentLevel > level)
            {
                Console.WriteLine();
                Console.WriteLine($">>> Level {level} cleared! On to level {score.CurrentLevel}: longer lists, bigger rewards. <<<");
            }
        }

        PrintResults(score);
    }

    // Replays the sort swap-by-swap after a wrong answer.
    // Uses the Step 5 challenge method. If it isn't written yet, we just skip it.
    private static void ShowReplay(int[] puzzle)
    {
        const int MaxStepsToShow = 12;
        try
        {
            List<int[]> steps = BubbleSorter.GetSortSteps(puzzle);
            if (steps.Count == 0 || steps.Count > MaxStepsToShow)
            {
                return;
            }

            Console.WriteLine("Here's how bubble sort did it:");
            Console.WriteLine($"  start:   {FormatList(puzzle)}");
            for (int i = 0; i < steps.Count; i++)
            {
                Console.WriteLine($"  swap {i + 1,2}: {FormatList(steps[i])}");
            }
        }
        catch (NotImplementedException)
        {
            // Step 5 (challenge) not done yet. That's fine.
        }
    }

    private static int ReadNumber()
    {
        while (true)
        {
            string? input = Console.ReadLine();
            if (input == null)
            {
                // Input closed (e.g. Ctrl+D). Treat as a wrong answer.
                return -1;
            }

            if (int.TryParse(input.Trim(), out int value) && value >= 0)
            {
                return value;
            }

            Console.Write("Please type a whole number (0 or more) > ");
        }
    }

    private static string FormatList(int[] numbers)
    {
        return "[" + string.Join(", ", numbers) + "]";
    }

    private static void PrintTitle()
    {
        Console.WriteLine("=========================================");
        Console.WriteLine("                 SORTLE");
        Console.WriteLine("=========================================");
        Console.WriteLine("Guess how many SWAPS bubble sort needs to");
        Console.WriteLine("sort each list from smallest to biggest.");
        Console.WriteLine($"You start with {ScoreKeeper.StartingTime:0} seconds. Right answers add");
        Console.WriteLine("time, wrong answers take it away.");
        Console.WriteLine($"Get {ScoreKeeper.ProblemsPerLevel} right to pass a level. The game ends");
        Console.WriteLine($"when the clock hits 0. Can you clear all {ScoreKeeper.MaxLevel} levels?");
        Console.WriteLine("Press Enter to start...");
        Console.ReadLine();
    }

    private static void PrintResults(ScoreKeeper score)
    {
        Console.WriteLine();
        Console.WriteLine("=============== GAME OVER ===============");
        Console.WriteLine($"  Levels cleared : {score.LevelsCleared} / {ScoreKeeper.MaxLevel}");
        Console.WriteLine($"  Correct        : {score.TotalCorrect}");
        Console.WriteLine($"  Wrong          : {score.TotalWrong}");
        Console.WriteLine($"  Time left      : {score.TimeLeft:0.0}s");
        Console.WriteLine($"  Mastery        : {score.GetMasteryRating()}");
        Console.WriteLine("=========================================");
    }
}
