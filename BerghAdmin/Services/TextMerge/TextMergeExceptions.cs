using System;

namespace BerghAdmin.Services.TextMerge
{
    [Serializable]
    public class TextMergeNoClosingTagException : Exception
    {
        public int Position { get; }

        public TextMergeNoClosingTagException() { }

        public TextMergeNoClosingTagException(string message)
            : base(message) { }

        public TextMergeNoClosingTagException(string message, Exception inner)
            : base(message, inner) { }

        public TextMergeNoClosingTagException(string message, int position)
            : this(message)
        {
            Position = position;
        }
    }

    [Serializable]
    public class TextMergeNoOpeningTagException : Exception
    {
        public int Position { get; }

        public TextMergeNoOpeningTagException() { }

        public TextMergeNoOpeningTagException(string message)
            : base(message) { }

        public TextMergeNoOpeningTagException(string message, Exception inner)
            : base(message, inner) { }

        public TextMergeNoOpeningTagException(string message, int position)
            : this(message)
        {
            Position = position;
        }
    }

    [Serializable]
    public class TextMergeNoMergeFieldException : Exception
    {
        public string MissingMergeFields { get; }

        public TextMergeNoMergeFieldException() { }

        public TextMergeNoMergeFieldException(string message)
            : base(message) { }

        public TextMergeNoMergeFieldException(string message, Exception inner)
            : base(message, inner) { }

        public TextMergeNoMergeFieldException(string message, string missingMergeFields)
            : this(message)
        {
            MissingMergeFields = missingMergeFields;
        }
    }
}
