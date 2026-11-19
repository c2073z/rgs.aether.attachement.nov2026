// =====================================================================
//  SORTLE  -  ScoreKeeper.cs
//  OWNER: Siqing
// =====================================================================
//
//  This file runs the RULES of the game: the clock, the levels, and how
//  well the player is doing. It doesn't know anything about sorting.
//  It only hears "the player got it right" or "the player got it wrong".
//
//  THE GAME RULES (from our design)
//  --------------------------------
//  * The player starts with 60 seconds on the clock.
//  * The clock slowly runs down. When it reaches 0, the game is over.
//  * There are 4 levels. The player must get 5 problems CORRECT in a
//    level to move on to the next level.
//  * Bigger levels mean longer lists:
//
//      Level | List length | Correct answer | Wrong answer
//      ------+-------------+----------------+-------------
//        1   |   3 - 5     |   +5 seconds   |  -5 seconds
//        2   |   7 - 10    |  +10 seconds   |  -5 seconds
//        3   |  10 - 15    |  +15 seconds   |  -7 seconds
//        4   |  15 - 20    |  +20 seconds   |  -7 seconds
//
//  * A wrong answer gives a time penalty and a NEW problem, but it does
//    NOT count toward the 5 correct problems for the level.
//  * 5 WRONG answers in the same level = game over, even if there is
//    still time on the clock. The wrong count starts again at 0 on each
//    new level.
//  * Mastery = how many levels the player cleared before the game ended
//    (or before they beat level 4).
//
//  WHAT TO DO
//  ----------
//  Do the steps IN ORDER (1 -> 5). Each step has a method with a
//  "throw new NotImplementedException(...)" line. Delete that line and
//  write your own code in its place.
//
//  After each step, check your work by running this in the terminal:
//
//      dotnet run -- check score
//
//  [PASS] means the step works. [FAIL] tells you what went wrong.
//  [TODO] means you haven't started that step yet.
//
//  RULES
//  -----
//  * Only change the code INSIDE the method bodies { ... }.
//  * Do NOT change method names, parameters, return types, or the
//    properties/constants at the top. The rest of the game (and the
//    website later on) uses them by name.
//
// =====================================================================

using System.ComponentModel;

namespace Sortle;

public class ScoreKeeper
{
    // ===== GAME SETTINGS (already done for you, don't change) =====
    public const double StartingTime = 60;   // seconds on the clock at the start
    public const int ProblemsPerLevel = 5;   // correct answers needed to level up
    public const int MaxWrongPerLevel = 5;   // wrong answers in one level that end the game
    public const int MaxLevel = 4;           // the last level

    // ===== GAME STATE (already done for you, don't change) =====
    // These are "properties". Anyone can READ them (get), but only code
    // inside THIS class can CHANGE them (private set). Your methods below
    // are inside this class, so you can change them like normal variables:
    //     TimeLeft = TimeLeft + 5;
    //     TotalCorrect++;

    public double TimeLeft { get; private set; }              // seconds left on the clock
    public int CurrentLevel { get; private set; }             // 1 to 4
    public int ProblemsSolvedThisLevel { get; private set; }  // correct answers this level (resets on level up)
    public int WrongThisLevel { get; private set; }           // wrong answers this level (resets on level up)
    public int TotalCorrect { get; private set; }             // correct answers in the whole game
    public int TotalWrong { get; private set; }               // wrong answers in the whole game
    public int LevelsCleared { get; private set; }            // 0 to 4
    public bool IsGameOver { get; private set; }              // true once the game has ended
    public bool HasWonGame { get; private set; }              // true only if level 4 is cleared

    // The "constructor". It runs once when a new game starts:  new ScoreKeeper()
    public ScoreKeeper()
    {
        TimeLeft = StartingTime;
        CurrentLevel = 1;
        ProblemsSolvedThisLevel = 0;
        WrongThisLevel = 0;
        TotalCorrect = 0;
        TotalWrong = 0;
        LevelsCleared = 0;
        IsGameOver = false;
        HasWonGame = false;
    }


    // -----------------------------------------------------------------
    //  STEP 1: How long is the list for each level?
    // -----------------------------------------------------------------
    //  Use the "List length" column of the table above.
    //  GetMinLength returns the SHORTEST list for that level.
    //  GetMaxLength returns the LONGEST list for that level.
    //  For any level that isn't 1-4, return 0.
    //
    //  Examples:  GetMinLength(1) -> 3     GetMaxLength(1) -> 5
    //             GetMinLength(4) -> 15    GetMaxLength(4) -> 20
    //             GetMinLength(5) -> 0     (there is no level 5)
    //
    //  HINTS
    //  * A chain of if / else if works.
    //  * A `switch` is neater:
    //        switch (level)
    //        {
    //            case 1: return 3;
    //            case 2: return ...;
    //            default: return 0;
    //        }
    // -----------------------------------------------------------------
    public int GetMinLength(int level)
    {
        switch (level)
        {
            case 1: return 3;
            case 2: return 7;
            case 3: return 10;
            case 4: return 15;
            default: return 0;
        }
    }

    public int GetMaxLength(int level)
    {
        switch (level)
        {
            case 1: return 5;
            case 2: return 10;
            case 3: return 15;
            case 4: return 20;
            default: return 0;
        }
    }


    // -----------------------------------------------------------------
    //  STEP 2: Time rewards and penalties
    // -----------------------------------------------------------------
    //  GetTimeBonus: seconds ADDED for a correct answer at that level.
    //  GetTimePenalty: seconds TAKEN AWAY for a wrong answer at that level.
    //  Return positive numbers from both (the penalty for level 1 is 5, not -5).
    //  For any level that isn't 1-4, return 0.
    //
    //  Examples:  GetTimeBonus(2) -> 10      GetTimePenalty(2) -> 5
    //             GetTimeBonus(4) -> 20      GetTimePenalty(4) -> 7
    //
    //  HINTS
    //  * Look at the bonus column: 5, 10, 15, 20. Do you see a pattern?
    //    You might not need a switch at all.
    //    (Still return 0 for levels outside 1-4.)
    //  * The penalty has only two values. Which levels get 5, and which get 7?
    // -----------------------------------------------------------------
    public int GetTimeBonus(int level)
    {
        if (CurrentLevel < 1 || CurrentLevel > 5)
        {
            return 0;
        }
        return level * 5;
    }

    public int GetTimePenalty(int level)
    {
        switch (level)
        {
            case 1:
            case 2:
                return 5;
            case 3:
            case 4:
                return 7;
            default:
                return 0;
        }
    }


    // -----------------------------------------------------------------
    //  STEP 3: Make the clock tick
    // -----------------------------------------------------------------
    //  The game calls this to say "this many seconds have passed".
    //  Take `seconds` away from TimeLeft.
    //
    //  * If TimeLeft drops to 0 or below, set TimeLeft to exactly 0
    //    (no negative time!) and set IsGameOver to true.
    //  * If the game is ALREADY over, do nothing at all.
    //
    //  Examples:  TimeLeft = 60, Tick(10)   -> TimeLeft = 50
    //             TimeLeft = 50, Tick(80)   -> TimeLeft = 0, IsGameOver = true
    //
    //  HINTS
    //  * "Do nothing" can be written as an early exit at the top:
    //        if (IsGameOver) { return; }
    // -----------------------------------------------------------------
    public void Tick(double seconds)
    {
        if (IsGameOver) { return ; }

        TimeLeft -= seconds;
        if (TimeLeft <= 0)
        {
            TimeLeft = 0;
            IsGameOver = true;
        }
    }


    // -----------------------------------------------------------------
    //  STEP 4: The player answered a problem  (THE MAIN ONE!)
    // -----------------------------------------------------------------
    //  The game calls this after every guess. isCorrect tells you whether
    //  the player guessed the right number of swaps.
    //
    //  If the game is already over, do nothing.
    //
    //  If the answer is CORRECT:
    //    1. Add the time bonus for the CURRENT level to TimeLeft.
    //    2. Add 1 to TotalCorrect and 1 to ProblemsSolvedThisLevel.
    //    3. If ProblemsSolvedThisLevel has reached ProblemsPerLevel (5),
    //       the level is cleared:
    //         - add 1 to LevelsCleared
    //         - if this was the LAST level (MaxLevel): the player has won!
    //           Set HasWonGame = true and IsGameOver = true.
    //         - otherwise: go up to the next level and reset BOTH
    //           ProblemsSolvedThisLevel and WrongThisLevel back to 0.
    //
    //  If the answer is WRONG:
    //    1. Add 1 to TotalWrong and 1 to WrongThisLevel.
    //    2. Take the time penalty for the CURRENT level away from TimeLeft.
    //       If that makes the clock hit 0, the game is over (same rules
    //       as Step 3).
    //    3. If WrongThisLevel has reached MaxWrongPerLevel (5), the game
    //       is over (IsGameOver = true), even if there is time left.
    //    ProblemsSolvedThisLevel does NOT change.
    //
    //  HINTS
    //  * Use the methods you already wrote:  GetTimeBonus(CurrentLevel)
    //  * For the wrong-answer penalty, could you just call Tick(...)?
    //    It already handles "don't go below 0" and "game over".
    //  * Work through this on paper: start a new game, answer correctly
    //    5 times. What should CurrentLevel, ProblemsSolvedThisLevel,
    //    LevelsCleared and TimeLeft be now? (Answer: 2, 0, 1, 85)
    //  * And: new game, answer WRONG 5 times. TimeLeft should be 35 and
    //    IsGameOver should be true (5 mistakes, not the clock).
    // -----------------------------------------------------------------
    public void SubmitAnswer(bool isCorrect)
    {
        if (IsGameOver) {return ;}

        if (isCorrect)
        {
            TimeLeft += GetTimeBonus(CurrentLevel);
            TotalCorrect++;
            ProblemsSolvedThisLevel++;
            if (CurrentLevel == MaxLevel)
            {
                HasWonGame = true;
                IsGameOver = true;
            }
            else
            {
                CurrentLevel++;
                ProblemsSolvedThisLevel = 0;
                WrongThisLevel = 0;
            }
        }
        else
        {
            TimeLeft -= GetTimePenalty(CurrentLevel);
            TotalWrong++;
            WrongThisLevel++;
            Tick(GetTimePenalty(CurrentLevel));
            if (WrongThisLevel == MaxWrongPerLevel)
            {
                IsGameOver = true;
            }
        }
    }


    // -----------------------------------------------------------------
    //  STEP 5: Mastery rating
    // -----------------------------------------------------------------
    //  Shown at the end of the game. Return a title based on LevelsCleared.
    //  Use EXACTLY these words (the checker compares the text):
    //
    //      LevelsCleared | Title
    //      --------------+----------------------
    //            0       | "Not Yet Sorted"
    //            1       | "Bubble Beginner"
    //            2       | "Swap Apprentice"
    //            3       | "Bubble Expert"
    //            4       | "Bubble Sort Master"
    //
    //  HINTS
    //  * A switch on LevelsCleared, like in Step 1.
    //  * Every path through the method must return something. What will
    //    you return in the `default:` case?
    // -----------------------------------------------------------------
    public string GetMasteryRating()
    {
        switch (LevelsCleared)
        {
            case 0: return "Not Yet Sorted";
            case 1: return "Bubble Beginner";
            case 2: return "Swap Apprentice";
            case 3: return "Bubble Expert";
            case 4: return "Bubble Sort Master";
            default: return "Not Yet Sorted";
        }
    }
}
