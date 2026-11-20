# Sortle

**A game that teaches bubble sort.**

🎮 **Play it here:** <https://c2073z.github.io/rgs.aether.attachement.nov2026/>

The game is hosted on **GitHub Pages**. It updates automatically every time new code is pushed to `main`.

## Credits

**Game Developers:**
- Cai Yuhan (RGS 409 '26)
- Cham Si Qing (RGS 403 '26)

**Mentored by:** Ahmad Farhan, Cephas Koh

This game was developed during a 1-week work attachment at **RSAF RAiD AETHER-SUTD**.

The core game engine is fully built by the game developers. The game user experience and user design was done by the game developers in Canva.

Due to the short time frame, an AI Agent was deployed to only:
1. Convert the front end to HTML
2. Write the script to host C# on HTML

<p>
  <img src="SortleWeb/wwwroot/img/logo-raid.png" alt="RAiD" height="24" />&nbsp;&nbsp;
  <img src="SortleWeb/wwwroot/img/logo-aether.png" alt="AETHER" height="40" />&nbsp;&nbsp;
  <img src="SortleWeb/wwwroot/img/logo-raffles.png" alt="Raffles Girls' School" height="40" />
</p>

---

## About the game

The game shows you a list of random numbers. You guess how many **swaps** bubble sort will make to sort the list from smallest to biggest. Right answers add time to the clock, and wrong answers take time away. Get 1 right to pass a level, and get level 4 right to win before the clock runs out.

The main logic of the game was written by hand in C# by the game developers. The website version (in `SortleWeb/`, made from our Canva design) runs that same code in the browser.

### Rule change (Draft 3)

The team updated the rules. The checker now tests these:

| Level | List length | Correct answer | Wrong answer |
|---|---|---|---|
| 1 | 3 | +10 s | -5 s |
| 2 | 4 - 5 | +20 s | -5 s |
| 3 | 6 - 7 | +30 s | -7 s |
| 4 | 8 - 10 | game ends (you win!) | -7 s |

- **1 right answer** passes a level.
- There is **no mistake limit**. The game ends when the clock reaches 0, or when you get level 4 right.

**Both of you:** get the new checker into your branch before you carry on:
```bash
git pull origin main
```

---

## Who does what

| File | Owner | What it does |
|---|---|---|
| [`StudentWork/BubbleSorter.cs`](StudentWork/BubbleSorter.cs) | **Yuhan** | Makes the random list and works out the correct answer: how many swaps bubble sort makes. |
| [`StudentWork/ScoreKeeper.cs`](StudentWork/ScoreKeeper.cs) | **Siqing** | Runs the game rules: the clock, the levels, time bonuses and penalties, and the mastery rating. |
| `GameEngine/` | (provided) | The terminal version of the game, plus the checker that tests your code. **You don't need to edit anything in here.** |

Each file has **5 steps**. Do them in order. Every step has a comment above it that explains what to build, gives an example, and has hints.

Each of you only edits your own file, so you won't overwrite each other's work.

---

## One-time setup

1. **Install the .NET 8 SDK** from <https://dotnet.microsoft.com/download/dotnet/8.0>. Pick the installer for your computer (macOS Arm64 for newer Macs, x64 for Windows).
   Check that it worked by opening a terminal and typing:
   ```bash
   dotnet --version
   ```
   You should see a number starting with `8.`.
2. **Install VS Code** and its **C# Dev Kit** extension. The extension colours your code and underlines mistakes.
3. **Clone this repo:**
   ```bash
   git clone https://github.com/c2073z/rgs.aether.attachement.nov2026.git Sortle
   ```
   ```bash
   cd Sortle
   ```
4. **Switch to your own branch.** Each of you has a branch named after you, so your work stays separate until it's ready:

   | Who | Branch | Command |
   |---|---|---|
   | Yuhan | `yuhan` | `git switch yuhan` |
   | Siqing | `siqing` | `git switch siqing` |

   Check you're on the right one with `git branch`. Your branch has a `*` next to it.
5. Open the `Sortle` folder in VS Code.

---

## How to work

1. Open your file in `StudentWork/`.
2. Find the next step. Delete the line `throw new NotImplementedException(...)` and write your code in its place.
3. Check your work in the terminal:

   ```bash
   dotnet run -- check sort
   ```
   That one is for Yuhan. Siqing runs:
   ```bash
   dotnet run -- check score
   ```
   To check both files at once:
   ```bash
   dotnet run -- check
   ```

   What the results mean:
   - `[PASS]`: the step works 🎉
   - `[FAIL]`: your code runs but gives a wrong answer. The message tells you what it got and what it expected.
   - `[TODO]`: you haven't written this step yet, or it needs an earlier step to be done first.

4. Once **both** files are finished and merged into `main` (see below), play the full game:
   ```bash
   dotnet run
   ```
   On your own branch, your teammate's file is still unfinished, so the game stops and tells you which step is missing. That's expected.

### Saving your work (git)

Always work on **your own branch**, never directly on `main`. When a step passes, save it to GitHub:

```bash
git add StudentWork
```
```bash
git commit -m "BubbleSorter: finished step 2"
```
```bash
git push
```

Your work goes to your branch on GitHub (`yuhan` or `siqing`), so your teacher can see your progress.

### When your file is finished

When `dotnet run -- check` shows `[PASS]` for all 5 of your steps:
1. Push your last commit.
2. On GitHub, open a **pull request** from your branch into `main`.
3. Your teacher reviews it and merges it.

When both pull requests are merged, everyone switches to `main` and pulls to get the full game:
```bash
git switch main
```
```bash
git pull
```

---

## The website (SortleWeb/)

**Play online (hosted on GitHub Pages):** <https://c2073z.github.io/rgs.aether.attachement.nov2026/>

Every push to `main` rebuilds and republishes the website automatically (see `.github/workflows/pages.yml`). The workflow runs the checker first, so a broken student file stops a bad version going live.

The `SortleWeb/` folder is the website version of the game, built from our Canva design. It runs **your** two files from `StudentWork/` in the browser. You don't edit anything in it.

To try it, once both of your files pass the checker:
```bash
cd SortleWeb
```
```bash
dotnet run
```
Then open the link it prints (for example `http://localhost:5000`). If a step isn't written yet, the website shows an "Almost there!" screen that says which step is missing.

---

## Tips

- **Work it out on paper first.** Take `[5, 1, 4, 2]` and do bubble sort by hand, counting the swaps. Then write the code.
- **Read the error message.** `IndexOutOfRangeException` almost always means a loop went one step too far.
- **Only change code inside the `{ }` of the methods.** Don't rename methods or change what they take in or give back. The website will call them by these exact names later.
- Stuck? Use `Console.WriteLine(...)` to print values and see what your code is really doing.
