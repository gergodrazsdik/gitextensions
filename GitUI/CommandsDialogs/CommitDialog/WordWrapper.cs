namespace GitUI.CommandsDialogs.CommitDialog
{
    public static class WordWrapper
    {
        private static IEnumerable<string> InternalWrapSingleLine(string line, int lineLimit)
        {
            WrapperState wrapper = new(lineLimit);
            foreach (string word in line.Split())
            {
                if (!wrapper.CanAddWord(word))
                {
                    yield return wrapper.GetLineAndReset();
                }

                wrapper.AddWord(word);
            }

            if (wrapper.HasWords)
            {
                yield return wrapper.GetLineAndReset();
            }
        }

        public static string WrapSingleLine(string text, int lineLimit)
        {
            IEnumerable<string> lines = InternalWrapSingleLine(text, lineLimit);
            return string.Join(Environment.NewLine, lines);
        }

        private class WrapperState
        {
            private readonly List<string> _wordList = new();
            private int _wordsLength;
            private readonly int _lineLimit;

            public bool HasWords { get; set; }

            public WrapperState(int lineLimit)
            {
                _lineLimit = lineLimit;
                Reset();
            }

            private void Reset()
            {
                _wordList.Clear();
                _wordsLength = 0;
                HasWords = false;
            }

            public bool CanAddWord(string word)
            {
                int newLength = _wordsLength + _wordList.Count + word.Length;
                return (newLength < _lineLimit) || _wordList.Any() == false;
            }

            public string GetLineAndReset()
            {
                string line = string.Join(" ", _wordList);
                Reset();
                return line;
            }

            public void AddWord(string word)
            {
                _wordList.Add(word);
                _wordsLength += word.Length;
                HasWords = true;
            }
        }
    }
}
