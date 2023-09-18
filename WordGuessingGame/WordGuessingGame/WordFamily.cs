using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TheWordGuessingGame
{
    public class WordFamily
    {
        //
        // Add words from dictionary to List
        //
        public static List<string> wordsList = new List<string>();
        //
        // List to hold word families
        //
        public static List<string> wordFamily = new List<string>();
        //
        // Word length randomly selected by Player 1 
        //
        public static int wordLength = 0;
        //
        // Number of guesses Player 2 can make
        //
        public static int guesses = 0;
        //
        // List to hold the words masked using the letters guessed by Player 2
        //
        public static List<string> maskedWords = new List<string>();
        //
        // Testing query to add all the words from word family that match the word mask
        //
        public static IEnumerable<string> MaskedWordsQuery;
        //
        // List to hold the largest word family
        //
        public static List<string> largestFamily = new List<string>();
        //
        // List to hold the largest word family
        //
        public static List<string> smalledLetterCountFamily = new List<string>();
        //
        // The masked form of the a word example: for words DEAL and SEAL, letter 'e' guessed
        // by Player 2 will have the word mask _e__
        //
        public static string wordMask;


        // Get all words from the Dictionary text file and add them to a list
        public static void GetDictionary()
        {
            wordsList = File.ReadAllLines("dictionary.txt").ToList();
        }
        /*
         Player 1 randomly picks a word of random length between 4-12 and initialises 
         word family with words of selected length
        */
        public static int WordLengthSelected()
        {
            int randomLength = new Random().Next(4, 12);
            foreach (string word in wordsList)
            {
                if (word.Length == randomLength)
                    wordFamily.Add(word);
            }

            wordsList.Clear(); // Clear wordList since it is no longer required 

            return randomLength;
        }

        // Return word families using a dictionary element, the key being the Word Mask and Values
        // include the all the words that share the same word mask
        public static Dictionary<string, List<string>> GetWordFamily(char userChar)
        {

            foreach (string word in wordFamily)
            {
                // Mask each word in the word family using the letter entered by Player 2,then add to masked words to a list
                maskedWords.Add(MaskWord(word, userChar));

            }

            // For all the masked words extracted from the word family, extract the number of words that match each unique mask
            // in the entire word family
            foreach (string word in wordFamily)
            {
                // Extract all the words that match every unique word mask using a string query element
                MaskedWordsQuery = maskedWords.Where(x => x.Equals(MaskWord(word, userChar)));

            }

            //Console.WriteLine("DEBUGGING: Getting Word families..");

            // IGrouping element using each word mask as unique keys and selecting the first masked word for each set, since word mask
            // is the same for those words
            var groupByMaskedWordsMatch =
                from word in wordFamily
                group word by maskedWords.Where(x => x.Equals(MaskWord(word, userChar))).First();

            // Dictionary is used to extract the word masks as unique keys and all the words that exist for the respective words masks (not ordered)
            Dictionary<string, List<string>> MaskedWordsDictionary = groupByMaskedWordsMatch.ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());


            /*Console.WriteLine("\n");
            foreach (KeyValuePair<string, List<string>> look in MaskedWordsDictionary)
            {
                Console.WriteLine("DEBUGGING: Masked word " + look.Key + " has words " + look.Value.Count());
            }*/

            // Ordered masked words in order of highest to lowest number of words that exist for that word mask
            var ordered = MaskedWordsDictionary.OrderByDescending(x => x.Value.Count()).ToDictionary(x => x.Key, x => x.Value);

            return ordered;
        }
        // Used to mask a word using the letter entered by PLayer 2
        // If player 2 enters the letter 'e' for a word else then the word mask would be e__e

        public static void GetLargestWordFamily(char userChar)
        {
            // Get the word families in order of highest to lowest count of words
            var ordered = GetWordFamily(userChar);

            // Pick the first value and respective word mask since it's has the highest number of words  and add them to a list
            largestFamily = ordered.Values.First();
            wordMask = ordered.Keys.First();

            //Console.WriteLine("DEBBUGING: Ordered letters highest to lowest according to word family count for each word mask");
            //foreach (KeyValuePair<string, List<string>> look in ordered)
            //{
            //   Console.WriteLine("DEBBUGING : Masked word " + look.Key + " has words " + look.Value.Count());
            //
        }

        // Pick word families that reveal the least number of letters and
        // another dictionary to keep add keys (the  masked words) and the count of letters 
        public static void LetterCountWordFamilies(char userChar)
        {
            // Get the word families in order of highest to lowest count of words
            var ordered = GetWordFamily(userChar);

            //Console.WriteLine("DEBBUGING: Ordered letters highest to lowest according to word family count for each word mask");
            //foreach (KeyValuePair<string, List<string>> look in ordered)
            //{
            //    Console.WriteLine("DEBBUGING: Masked word " + look.Key + " has words " + look.Value.Count());
            //}

            //Console.WriteLine("DEBBUGING: Number of Letters revealed for each word mask");

            // Extract the count of letters for each word mask
            foreach (KeyValuePair<string, List<string>> look in ordered)
            {
                int lettersCount = LettersCount(look.Key, userChar);
                //Console.WriteLine("DEBBUGING: Masked word " + look.Key + " has " + lettersCount + " letters");
            }

            // Order the word masks in a new dictionary element according to the count of letters revealed in them

            var lowestNumberOfLetters = ordered.OrderByDescending(x => LettersCount(x.Key, userChar)).ToDictionary(x => x.Key, x => x.Value);

            //Console.WriteLine("DEBBUGING: Ordered letters according to number of letters revealed in word mask from highest to lowest letters revealed");

            foreach (KeyValuePair<string, List<string>> look in lowestNumberOfLetters)
            {
                int lettersCount = LettersCount(look.Key, userChar);
                //Console.WriteLine("DEBBUGING: Masked word " + look.Key + " has " + lettersCount + " letters");
            }

            // Get the Word Family with the least number of letters revealed and add them to a list
            // Select the last element for the rexpective values and word mask since ordering was done in descending order

            smalledLetterCountFamily = lowestNumberOfLetters.Values.Last();
            wordMask = lowestNumberOfLetters.Keys.Last();

            int smalledLetterCount = smalledLetterCountFamily.Count();
            //Console.WriteLine("\n");
            //Console.WriteLine("DEBBUGING: Word family has {0} words currently..", smalledLetterCount);

        }
        public static string MaskWord(string word, char ch)
        {
            // List of chars to hold each letter in a word
            List<char> maskedChars = new List<char>();

            foreach (char c in word)
            {
                maskedChars.Add(c);
            }

            // If the letter entered by Player 2 is not found in the word, then replace it with '_' 
            for (int i = 0; i < maskedChars.Count; i++)
            {
                if (maskedChars[i] == ch)
                {
                    maskedChars[i] = ch;
                }
                else
                {
                    maskedChars[i] = '_';
                }
            }

            // Get word mask as a string
            string maskedWord = new string(maskedChars.ToArray());

            return maskedWord;

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

    }
}
