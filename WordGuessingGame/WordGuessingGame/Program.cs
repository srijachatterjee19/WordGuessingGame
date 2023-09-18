using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TheWordGuessingGame
{
    public class Program
    {
        //
        // List with letters guessed by Player 2
        //
        public static List<char> lettersGuessed = new List<char>();
        //
        // Check if Player 2 wishes to keep track of word possibilities
        //
        public static bool wordPossibilities = false;
        //
        // Game level selected by Player 2
        //
        public static string gameLevel;
        //
        // Random word claimed by Player 1 to be the word selected at the end of the game 
        //
        public static string secretWord;

        public static void Main()
        {
            Start();
            Restart();
        }

        public static void Start()
        {
            WordFamily.GetDictionary();

            Console.Clear();
            Console.WriteLine("\n\n");
            Console.WriteLine("Welcome to the Word Guessing Game!");
            Console.Title = "Word Guessing Game";

            WordFamily.wordLength = WordFamily.WordLengthSelected(); // Get random word length selected by Player 1
            WordFamily.guesses = WordFamily.wordLength * 2; // Number of guesses allowed for Player 2
            PromptDisplayWordPossibilities(); // Ask Player 2 wishes to display number of words possible to guess
            PromptSelectGameLevel(); // Ask Player 2 the level of game they would like to play
            Console.Clear();

            Console.WriteLine("You selected Game Level {0}", gameLevel);
            Console.WriteLine("You have to guess a word consisting of {0} letters.", WordFamily.wordLength);
            Console.WriteLine("You have {0} guesses. Good Luck!!", WordFamily.guesses);

            // Initalise list containing attempted letters found in the word being guessed
            lettersGuessed.Clear();
            //Initialise word mask, add "_" equal to the word length selected by Player 1
            WordFamily.wordMask = InitialiseWordMask(WordFamily.wordLength);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();

            // Keep printing game screen until Player 2 has run out of guesses or all the letters have been guessed
            while (WordFamily.guesses > 0 || !WordFamily.wordMask.Contains('_'))
            {
                PrintGame();
            }
            // If Player 2 has guessed all the letters correctly congratulate them
            if (!WordFamily.wordMask.Contains("_"))
            {
                Console.WriteLine("\n\n");
                Console.WriteLine("Congratulations..You have guessed the word correctly!!");
            }
            else
            {
                // If Player 2 loses the game, reveal a random word selected by Player 1 from the last word family
                Console.WriteLine("\n\n");
                Console.WriteLine("You have LOST :(");
                Console.WriteLine("The word was {0}", secretWord);
            }
        }
        // If Player 2 wishes to play the game again, start game again
        public static void Restart()
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("Play Again?");
            Console.Write("Select y/n");

            char choice = Console.ReadKey().KeyChar;
            choice = Char.ToLower(choice);

            if (choice != 'y' && choice != 'n')
            {
                Console.WriteLine("\n");
                Console.WriteLine("Please enter y or n");
                Restart();
            }
            else if (choice == 'y')
            {
                Start(); // Restart the game
                Restart();
            }
            else if (choice == 'n')
            {
                Console.Clear();
                Console.WriteLine("Press any key to continue..");
                Console.ReadLine();
            }
        }



        // Ask Player 2 if they would like to see the number of words possible to guess during the game
        public static void PromptDisplayWordPossibilities()
        {
            Console.WriteLine("Do you want to know the number of words you can guess?");
            Console.WriteLine("\n");
            Console.WriteLine("Y or N ?");

            char choice = Console.ReadKey().KeyChar;
            choice = Char.ToLower(choice);

            if (choice != 'y' && choice != 'n')
            {
                Console.WriteLine("\n");
                Console.WriteLine("Enter either y or n");
                PromptDisplayWordPossibilities();
            }
            else if (choice == 'y')
            {
                wordPossibilities = true;
            }
        }


        // Ask Player 2 to select a game level
        public static void PromptSelectGameLevel()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Select Game Level...");
            Console.WriteLine("\n");
            // Display game level options
            Console.WriteLine("1.EASY");
            Console.WriteLine("2.DIFFICULT");

            string input = Console.ReadLine();
            int selectedOption;

            if (int.TryParse(input, out selectedOption))
            {
                switch (selectedOption)
                {
                    case 1:
                        gameLevel = "EASY";
                        break;
                    case 2:
                        gameLevel = "DIFFICULT";
                        break;
                }
            }
            else
            {
                Console.WriteLine("\n");
                Console.WriteLine("Enter either 1 or 2");
                PromptSelectGameLevel();
            }
        }

        public static void PrintGame()
        {
            Console.WriteLine("\n");
            Console.WriteLine("You have {0} guesses left!", WordFamily.guesses);
            Console.WriteLine("Guessed letters: {0}", string.Join(",", lettersGuessed));

            // Display word possibilities if Player 2 said yes
            if (wordPossibilities == true)
            {
                Console.WriteLine("Word possibilities: {0}", WordFamily.wordFamily.Count);
            }
            // Display word in masked form
            Console.WriteLine("Word: {0}", WordFamily.wordMask);

            //Prompt Player 2 to enter a letter and validate entry
            Console.Write("Enter guess: ");
            char userInput = Console.ReadKey().KeyChar;

            // Convert user entered letter to lowercase
            userInput = Char.ToLower(userInput);

            // Keep prompting Player 2 to enter a letter
            while (!char.IsLetter(userInput))
            {
                Console.WriteLine("\n");
                Console.WriteLine("Please enter an alphabet (a-z)");
                Console.Write("Enter guess: ");
                userInput = Console.ReadKey().KeyChar;
            }

            // Process user input according to game level
            if (gameLevel == "EASY")
            {

                //Console.WriteLine("DEBBUGGING: EASY LEVEL SELECTED");

                // Get Largest Word Family based on user entered letter 
                WordFamily.GetLargestWordFamily(userInput);

                // Return Largest Word Family count
                //Console.WriteLine("DEBBUGGING: Largest Word family has {0} words", WordFamily.largestFamily.Count());
                //Console.WriteLine("DEBBUGGING: Word Mask of largest Word family is {0} ", WordFamily.wordMask);

                // Assign Largest Word Family to list
                WordFamily.wordFamily = WordFamily.largestFamily;

                //Console.WriteLine("DEBBUGGING: New Word family count is: {0}", WordFamily.wordFamily.Count);

                /*
                  If one guess if remaining, select a word at random from the largest word family to claim
                  that's the word that was selected by the computer all along
                */
                if (WordFamily.guesses == 1)
                {
                    int randomInt = new Random().Next(WordFamily.wordFamily.Count);
                    secretWord = WordFamily.wordFamily[randomInt];
                    //Console.WriteLine("DEBBUGGING: Secret word is: {0}", secretWord);
                }
            }
            else if (gameLevel == "DIFFICULT")
            {
                //Console.WriteLine("DEBBUGGING: DIFFICULT LEVEL SELECTED");

                // Get the word family with the lowest number of letters revealed
                WordFamily.LetterCountWordFamilies(userInput);

                //Console.WriteLine("DEBBUGGING: Word family with least letters revealed has {0} words", WordFamily.largestFamily.Count());
                //Console.WriteLine("DEBBUGGING: Word Mask of Word family with least letters revealed is {0} ", WordFamily.wordMask);

                // Assign word family to largest word family for prcoessing next time
                WordFamily.wordFamily = WordFamily.smalledLetterCountFamily;

                //Console.WriteLine("DEBBUGGING: New Word family count is: {0}", WordFamily.wordFamily.Count);

                if (WordFamily.guesses == 1)
                {
                    int randomInt = new Random().Next(WordFamily.wordFamily.Count);
                    secretWord = WordFamily.wordFamily[randomInt];
                    //Console.WriteLine("DEBBUGGING:secret word is: {0}", secretWord);
                }
            }

            Console.WriteLine("\n");
            if (lettersGuessed.Contains(userInput))
            {
                Console.WriteLine("You have already guessed {0}", userInput);
            }

            //if largest word family has the letter, let user know letter was found and deduct a guess
            else if (WordFamily.wordMask.Contains(userInput))
            {
                int count = LettersCount(WordFamily.wordMask, userInput);

                // Correct guess
                Console.WriteLine("Yes, there is {0} letter {1} in the word", count, userInput);
                lettersGuessed.Add(userInput);

            }
            //if letter not found, report it to player
            else if (!lettersGuessed.Contains(userInput) && !WordFamily.wordMask.Contains(userInput))
            {
                lettersGuessed.Add(userInput);
                Console.WriteLine("Sorry, there are no copies of the letter {0} in the word", userInput);
                WordFamily.guesses--;
            }
        }


        // Used to count the number of letters in a masked word
        // The word mask _e_e for example will return the count 2 if player 2 guesses 'e'
        private static int LettersCount(string mask, char userChar)
        {
            int letterCount = 0;

            foreach (char c in mask)
            {
                if (c == userChar)
                    letterCount++;
            }
            return letterCount;
        }


        // Word mask is initialised with  same number of '_' as word length selected by Player 1
        public static string InitialiseWordMask(int length)
        {
            char[] wordMaskArray = new char[length];
            string mask;

            for (int i = 0; i < WordFamily.wordLength; i++)
            {
                wordMaskArray[i] = '_';
            }
            // Get word mask as a string
            mask = string.Join("", wordMaskArray);

            return mask;
        }
    }
}
