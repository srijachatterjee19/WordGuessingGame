# WordGuessingGame

<img width="400" alt="image" src="https://github.com/srijachatterjee19/WordGuessingGame/assets/84346422/1a0adf40-34c9-4079-a72a-b8bb997a155a">
A game where the computer always sets the word and the player has to guess the word - but in this case, there is a ‘twist’ – the computer can cheat – it has access to a dictionary and is able to change the chosen word on the fly during the game so as to avoid matches proposed by the human player trying to guess the word. This makes the computer very difficult to beat.


The game is build using C#. The `WordFamily.cs` is used to process the word families according to `EASY` and `DIFFICULT` levels of the game.The program starts with extracting all the words from the `dictionary.txt` file which consists of over 120,000 words. The computer i.e., Player 1 picks a random word length between 4 to 12. Player 2 is prompted to pick a difficulty level. The game executes without any runtime error.`GetWordFamily` method is used to extract the largest word families each time Player 2 guesses a letter. The words extracted from the dictionary are “masked” i.e. a word ELSE will be masked as E_ _ E when Player guesses the letter ‘e’; each word is masked in the word list then add to masked words list. For all the masked words extracted from the word list, the number of words is extracted those that match with each unique word mask in the entire word family using a string query.`Dictionary` data structures are used to extract the word masks as unique keys and all the list of words that exist for the respective word masks. The advantage of using Dictionary is the availability to hold word masks
(ELSE as E_ _ E if guess is letter ‘e’) as unique Keys making sure there are no duplicates and faster processing. Also, it has the advantage of keeping track of each type of word mask; for example, E_ _E and E_ E_ have two letters revealed each, but since each word mask is unique we don’t have to worry about errors for similar revealed letter counts. Processing a huge number of words (120,000+) has an advantage of being more efficient and quick.Then an `IGrouping` element returned when grouping the masked words using each word mask as unique keys by selecting the first mask for each set of words. A Dictionary is then used to extract the word masks as unique Keys and all the lists of words as Values that exist for the respective word masks (random order currently). The Dictionary elements then are ordered from highest to lowest number of words for each word mask and the highest element (with the greatest number of words as Values) is then picked to be the Largest Word Family.The EASY level of the game just picks the Largest Word Family each turn in the game. The DIFFICULT level takes a different approach. The goal is to pick word families that reveal the least number of letters with each guess.It starts by getting the Dictionary element with the word families ordered from highest to lowest, returned by the GetWordFamily method. Then, the number of letters revealed for each word mask is extracted using `LettersCount` method. The word families are then ordered according to the number of letters revealed and returned as a Dictionary element. The last Dictionary element is selected (since elements are ordered in descending order) and the Values which include the all the words with the Least Number of Letters Revealed
is returned as a list.

