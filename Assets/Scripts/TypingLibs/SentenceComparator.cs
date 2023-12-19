using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingComparator
{
    public class Sentence
    {
        #region Props

        private char[] _ownSentence;
        public char[] OwnSentence
        {
            get { return _ownSentence; }
        }

        private char[] _originalSentence;

        private bool _hasBegun = false;

        public bool HasBegun
        {
            get { return _hasBegun; }
        }


        private bool _isDone = false;
        public bool IsDone
        {
            get { return _isDone; }
        }

        private int _actualIndex = 0;

        private List<int> _wordStartIndexes;

        private Stopwatch _stopwatch;

        private int _wordCount;

        private int _mistakes = 0;

        private int _characterCount;

        private bool _toLower;

        #endregion



        /// <summary>
        /// Sentence object creator
        /// </summary>
        /// <param name="sentence">input sentence as a string</param>
        /// <param name="toLower">should the sentence be in lowercase, or as inputted</param>
        public Sentence(string sentence, bool toLower)
        {
            _toLower = toLower;

            string sentenceToGet = _toLower ? sentence.ToLower() : sentence;

            _originalSentence = sentenceToGet.ToCharArray();
            _ownSentence = (char[])_originalSentence.Clone(); // Clone the array to avoid reference issues

            _wordStartIndexes = new List<int>();

            int indexToNote = 0;

            _wordStartIndexes.Add(indexToNote);

            foreach (char c in _originalSentence)
            {
                if (c == Convert.ToChar(" "))
                {
                    _wordStartIndexes.Add(indexToNote + 1);
                }
                indexToNote++;
            }

            _wordStartIndexes.Reverse();

            _stopwatch = new Stopwatch();

            _wordCount = _wordStartIndexes.Count + 1;

            _characterCount = _ownSentence.Count();
        }
        /// <summary>
        /// Rebuilds the whole sentence as it originally was
        /// <para>Resets the stopwatch</para>
        /// </summary>
        public void ResetSentence()
        {
            _ownSentence = (char[])_originalSentence.Clone(); // Clone again when refilling
            _isDone = false;
            _actualIndex = 0;
            _hasBegun = false;
            _stopwatch.Reset();
            _mistakes = 0;

        }
        /// <summary>
        /// Gets rid of as much chars from the start of the sentence as specified
        /// </summary>
        /// <param name="numberOfChars">Numbers the char to get rid of</param>
        public void ClearChars(int numberOfChars)
        {
            if (numberOfChars < _ownSentence.Length)
            {
                _ownSentence = _ownSentence.Skip(numberOfChars).ToArray();
                for (int i = 0; i < numberOfChars; i++)
                {
                    _actualIndex++;
                }
            }
            else
            {
                BeDone(); // Mark as done if the number of chars to clear is equal or exceeds the length
            }
        }
        /// <summary>
        /// Sets the isDone flag to true;
        /// </summary>
        private void BeDone()
        {
            _stopwatch.Stop();
            _isDone = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>wordcount of sentence</returns>
        public int WordCount()
        {
            return _wordCount;
        }
        /// <summary>
        /// Takes char input and compares it with the next char in the sentence (chararray)
        /// <para>If this is the first char typed, turns the hasBegun flag to true, and starts the _stopWatch</para>
        /// </summary>
        /// <param name="typedChar">char input</param>
        /// <returns>true on good input, false on wrong input</returns>
        public bool TypeIn(char typedChar)
        {
            if (_ownSentence.Length > 0 && typedChar == _ownSentence[0])
            {
                StartStopWatchAndBegin();
                return true;
            }
            else if (_ownSentence.Length > 0 && typedChar != _ownSentence[0])
            {
                IncrementMistakes();
                return false;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// If the word processing has begun, increase the mistake count by one
        /// </summary>
        private void IncrementMistakes()
        {
            if (_hasBegun)
            {
                _mistakes++;
            }
        }
        /// <summary>
        /// If hasBegun is false, it sets it to true and starts the stopwatch
        /// </summary>
        public void StartStopWatchAndBegin()
        {
            if (!HasBegun)
            {
                _hasBegun = true;
                _stopwatch.Start();
            }
        }
        /// <summary>
        /// Rebuilds last word that has been partially typed (On wrong input for example)
        /// </summary>
        public void RebuildLastWord()
        {
            foreach (int index in _wordStartIndexes)
            {
                if (index <= _actualIndex)
                {
                    _actualIndex = index;
                    _ownSentence = _originalSentence.Skip(_actualIndex).ToArray();
                    break; // Exit the loop once the correct start index is found
                }
            }
        }
        public string ShowAchieved()
        {
            string toReturn = "";
            string wholeSentence = new string(_originalSentence);
            toReturn += wholeSentence.Substring(0, _actualIndex);
            return toReturn;
        }
        /// <summary>
        /// Returns the sentence as is right now
        /// </summary>
        /// <returns></returns>
        public string ShowRemaining()
        {
            return new string(_ownSentence);
        }
        /// <summary>
        /// </summary>
        /// <returns>double representing the time in seconds to type the whole sentence</returns>
        public double ElapsedTime()
        {
            return _stopwatch.Elapsed.TotalSeconds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Words per minute for the currently typed words</returns>
        public int WordsPerMinute()
        {
            return Convert.ToInt32(_wordCount / ((float)ElapsedTime() / 60));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Actual precision of the typing as an int</returns>
        public int TypePrecision()
        {
            if (_characterCount > 0)
            {
                return _mistakes > 0 ? Convert.ToInt32((((float)(_characterCount - _mistakes) / _characterCount) * 100)) : 100;
            }
            else
            {
                return 0; // or some other default value, because precision is not defined for 0 total characters
            }
        }

    }
}