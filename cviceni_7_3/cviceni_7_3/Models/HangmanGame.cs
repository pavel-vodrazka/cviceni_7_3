using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;

namespace cviceni_7_3
{
    public class HangmanGame : INotifyPropertyChanged
    {
        public int MaxMisses { get; private set; }
        public int Misses
        {
            get => misses;
            private set
            {
                misses = value;
                OnPropertyChanged($"{nameof(Misses)}");
            }
        }
        public char CurrentGuess
        {
            get => currentGuess;
            set
            {
                currentGuess = value;
                OnPropertyChanged($"{nameof(CurrentGuess)}");
            }
        }

        public string WordDrawn
        {
            get => string.Concat(wordDrawn);
        }

        public string MaskedWord
        {
            get => maskedWord;
            private set
            {
                if (maskedWord != value)
                {
                    maskedWord = value;
                    OnPropertyChanged($"{nameof(MaskedWord)}");
                }
            }
        }
        public HangmanGameState State
        {
            get => state;
            private set
            {
                if (state != value)
                {
                    state = value;
                    OnPropertyChanged($"{nameof(State)}");
                }
            }
        }
        public int WordsInBag { get => bagOfWords.Count; }
        public char MaskChar { get; private set; }

        private static string[] defaultWords = new string[]
            {
                "stůl", "stolek", "židle", "knížka", "pero", "vybavení", "auto", "stavení", "dům", "dveře",
                "okno", "počítač", "obraz", "květina", "hora", "řeka", "les", "kočka", "pes", "pták",
                "krabice", "klíč", "mikina", "rukavice", "boty", "kabelka", "peněženka", "hračka", "hřeben", "kartáč",
                "hřebík", "traktor", "stroj", "slunečnice", "vítr", "slunce", "dešť", "mrak", "kámen", "pláž",
                "letadlo", "raketa", "nůž", "vidlička", "lžíce", "sklenice", "talíř", "hrnec", "lampa", "kniha",
                "slovo", "věta", "písmeno", "číslo", "město", "vesnice", "vlak", "loď", "přístav", "kolo",
                "motorka", "kánoe", "mobil", "hodinky", "náramek", "prsten", "brýle", "šperk", "brýle", "hodinky",
                "peníze", "banka", "krabice", "sako", "košile", "klobouk", "džíny", "mikina", "sukně", "halenka",
                "batoh", "klíčenka", "náramek", "parfém", "šátek", "krabice", "pouzdro", "tlačítko", "kapesník", "sluchátka",
                "plášť", "televize", "vysavač", "stan", "duha", "klávesnice", "plakát", "papír", "páka", "klacek"
            };
        private static char defaultMaskChar = '_';

        private string[] ListOfWords { get; set; }
        private List<string> bagOfWords;
        private Random generator;
        private List<char> wordDrawn;
        private List<bool> mask;
        private string maskedWord;
        private int misses;
        private char currentGuess;
        private HangmanGameState state;

        public void Draw()
        {
            if (WordsInBag < 1)
                throw new AllWordsTriedException();

            wordDrawn = new List<char>();
            int numberDrawn = generator.Next(bagOfWords.Count);
            wordDrawn.AddRange(bagOfWords[numberDrawn]);
            bagOfWords.RemoveAt(numberDrawn);
            mask = wordDrawn.ConvertAll((char x) => true);
            MaskedWord = GetMaskedWord();
            Misses = 0;
            State = HangmanGameState.Ready;
        }

        public HangmanGameState GuessChar(char guess)
        {
            if ((State | HangmanGameState.GameOver) == HangmanGameState.GameOver)
                throw new GameAlreadyOverException();

            CurrentGuess = guess; // consider making it lowercase

            int[] indexes = Enumerable.Range(0, wordDrawn.Count).ToArray();

            int[] guessedIndexes = indexes.Where((x) => wordDrawn[x] == CurrentGuess).ToArray();

            if (guessedIndexes.Length == 0)
            {
                Misses++;
                State = HangmanGameState.BadGuess;
            }
            else
            {
                if (guessedIndexes.Select((x) => mask[x]).All((x) => x == true))
                {
                    mask = indexes.Select((x) => guessedIndexes.Contains(x) ? false : mask[x]).ToList(); //maybe it will be necessary to split into 2 steps
                    MaskedWord = GetMaskedWord();
                    State = HangmanGameState.GoodGuess;
                }
                else if (guessedIndexes.Select((x) => mask[x]).All((x) => x == false))
                    State = HangmanGameState.AlreadyGuessed; // do nothing if character is already guessed
                else
                    throw new InconsistentStateException();
            }

            if (Misses > MaxMisses)
                State = HangmanGameState.Lost;

            if (mask.All((x) => x == false))
                State = HangmanGameState.Won;

            return State;
        }

        private string GetMaskedWord()
        {
            return string.Concat(wordDrawn.Zip(mask, (character, characterMask) => characterMask ? MaskChar : character));
        }

        public HangmanGame(int maxMisses) : this(maxMisses, defaultMaskChar, defaultWords) { }

        public HangmanGame(int maxMisses, char maskChar) : this(maxMisses, maskChar, defaultWords) { }

        public HangmanGame(int maxMisses, string[] listOfWords) : this(maxMisses, defaultMaskChar, listOfWords) { }

        public HangmanGame(int maxMisses, char maskChar, string[] listOfWords)
        {
            MaxMisses = maxMisses;
            MaskChar = maskChar;
            ListOfWords = listOfWords;
            bagOfWords = new List<string>(ListOfWords);
            generator = new Random();
            Draw();
        }

        public HangmanGame Reset()
        {
            return new HangmanGame(MaxMisses, MaskChar, ListOfWords);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class AllWordsTriedException : Exception { }

    public class GameAlreadyOverException : Exception { }

    public class InconsistentStateException : Exception { }

    [Flags]
    public enum HangmanGameState
    {
        Unknown = 0,
        Ready = 1,
        GoodGuess = 2,
        BadGuess = 4,
        AlreadyGuessed = 8,
        Won = 16,
        Lost = 32,
        Playing = Ready | GoodGuess | BadGuess | AlreadyGuessed,
        GameOver = Won | Lost
    }
}
