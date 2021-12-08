using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BerghAdmin.Services.TextMerge
{
    internal class MergeField 
    {
        public int StartPosition { get; set; }
        public int Length { get; set; }
    }
    
    public class TextMergeService : ITextMergeService
    {
        private readonly string OPENINGS_TAG = "&lt;&lt;";
        private readonly string CLOSING_TAG = "&gt;&gt;";

        public bool IsValidMergeText(string htmlText, Dictionary<string,string> mergeFieldValues)
        {
            throw new NotImplementedException();
        }

        public Stream Merge(Stream htmlText, Dictionary<string, string> mergeFieldValues)
        {
            throw new NotImplementedException();
        }

        public string Merge(string? htmlText, Dictionary<string, string>? mergeFieldValues)
        {
            if (string.IsNullOrEmpty(htmlText))
            {
                return string.Empty;
            } 
            if (! htmlText.Contains(OPENINGS_TAG) && ! htmlText.Contains(CLOSING_TAG))
            {
                return htmlText;
            }
            if (mergeFieldValues == null)
            {
                throw new TextMergeNoMergeFieldException($"Not any merge field values are specified");
            }

            var mergeFields = FindMergeFields(htmlText);
            if (!MergeFieldsHaveValues(mergeFields, mergeFieldValues, out string missingMergeValues))
            {
                throw new TextMergeNoMergeFieldException($"The following merge fields are unspecified: {missingMergeValues}");
            }

            var mergedText = PerformMerge(htmlText, mergeFields, mergeFieldValues);

            return mergedText;
        }

        private Dictionary<string, MergeField> FindMergeFields(string htmlText)
        {
            var foundTags = new Dictionary<string, MergeField>();

            var searchFromPosition = 0;
            while (true)
            {
                // start with a search for the openings tag
                var tagPosition = htmlText.IndexOf(OPENINGS_TAG, searchFromPosition);
                if (tagPosition < 0)
                {
                    // no openings tag maybe there is a faulty end tag
                    tagPosition = htmlText.IndexOf(CLOSING_TAG, searchFromPosition);
                    if (tagPosition >= 0)
                    {
                        throw new TextMergeNoOpeningTagException($"Closing tag found at position {tagPosition} without openingstag.", tagPosition);
                    }
                    break;
                }
                else
                {
                    var mergeField = new MergeField(){ StartPosition = tagPosition };
                    searchFromPosition = tagPosition + OPENINGS_TAG.Length;
                    tagPosition = htmlText.IndexOf(CLOSING_TAG, searchFromPosition); 
                    if (tagPosition < 0)
                    {
                        throw new TextMergeNoClosingTagException($"Mergefield starting at position {mergeField.StartPosition} has no closing tag.", mergeField.StartPosition);
                    }
                    mergeField.Length = tagPosition + CLOSING_TAG.Length - mergeField.StartPosition;
                    var name = htmlText.Substring(mergeField.StartPosition + OPENINGS_TAG.Length, mergeField.Length - OPENINGS_TAG.Length - CLOSING_TAG.Length);
                    searchFromPosition = tagPosition + CLOSING_TAG.Length;
                    
                    if (!foundTags.ContainsKey(name))
                    {
                        foundTags.Add(name, mergeField);
                    }
                }
            }

            return foundTags;
        }

        private static bool MergeFieldsHaveValues(Dictionary<string, MergeField> mergeFields, Dictionary<string, string> mergeFieldValues, out string missingMergeValues)
        {
            var missingMergeValuesArray = mergeFields.Keys
                .Where(key => !mergeFieldValues.ContainsKey(key))
                .ToArray();

            missingMergeValues = String.Join(", ", missingMergeValuesArray);

            return missingMergeValues.Length == 0;
        }

        private string PerformMerge(string htmlText, Dictionary<string, MergeField> mergeFields, Dictionary<string, string> mergeFieldValues)
        {
            var newText = htmlText;
            foreach(var mergeFieldKey in mergeFields.Keys)
            {
                newText = newText.Replace(OPENINGS_TAG + mergeFieldKey + CLOSING_TAG, mergeFieldValues[mergeFieldKey]);
            }

            return newText;
        }


    }

}
