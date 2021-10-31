using System;
using System.Collections.Generic;
using System.IO;

namespace BerghAdmin.Services.TextMerge
{
    public interface ITextMergeService
    {
        bool IsValidMergeText(string htmlText, Dictionary<string,string> mergeFieldValues);
        string Merge(string htmlText, Dictionary<string,string> mergeFieldValues);
        Stream Merge(Stream htmlText, Dictionary<string,string> mergeFieldValues);
    }
}
