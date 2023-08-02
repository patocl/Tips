using System;
using System.IO;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

class Program
{
    class Options
    {
        [Value(0, Required = true, MetaName = "baseName1", HelpText = "The base name of the first .reg file")]
        public string BaseName1 { get; set; }

        [Value(1, Required = true, MetaName = "baseName2", HelpText = "The base name of the second .reg file")]
        public string BaseName2 { get; set; }

        [Value(2, Required = true, MetaName = "option", HelpText = "Comparison option: keys, values, merge, both")]
        public string Option { get; set; }

        [Option('k', "search-key", MetaValue = "KEY", HelpText = "Search for a key in baseName1.reg")]
        public string SearchKey { get; set; }

        [Option('v', "search-value", MetaValue = "VALUE", HelpText = "Search for a value in baseName1.reg")]
        public string SearchValue { get; set; }

        [Usage(ApplicationAlias = "RegFileTool")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Compare and analyze .reg files", new Options { BaseName1 = "file1", BaseName2 = "file2", Option = "both" });
                yield return new Example("Search for a key in a .reg file", new Options { BaseName1 = "file1", Option = "search-key", SearchKey = "KeyName" });
            }
        }
    }

    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
          .WithParsed(RunOptions);
    }

    static void RunOptions(Options options)
    {
        string baseName1 = options.BaseName1;
        string baseName2 = options.BaseName2;
        string option = options.Option;

        Dictionary<string, string> keysAndValues1 = ParseRegFile($"{baseName1}.reg");
        Dictionary<string, string> keysAndValues2 = ParseRegFile($"{baseName2}.reg");

        bool filesAreEqual = CompareFiles(keysAndValues1, keysAndValues2);

        if (filesAreEqual)
        {
            Console.WriteLine("The files are identical.");
            return;
        }

        if (option == "keys" || option == "both")
        {
            CompareMissingKeys(keysAndValues1, keysAndValues2);
        }

        if (option == "values" || option == "both")
        {
            CompareDifferentValues(keysAndValues1, keysAndValues2);
        }

        if (option == "merge" || option == "both")
        {
            GenerateMergedRegFile(baseName1, baseName2, keysAndValues1, keysAndValues2);
        }

        if (!string.IsNullOrEmpty(options.SearchKey))
        {
            SearchByKey(keysAndValues1, options.SearchKey);
        }

        if (!string.IsNullOrEmpty(options.SearchValue))
        {
            SearchByValue(keysAndValues1, options.SearchValue);
        }
    }

    static Dictionary<string, string> ParseRegFile(string filePath)
    {
        var keysAndValues = new Dictionary<string, string>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            string currentKey = null;

            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("["))
                {
                    currentKey = line;
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    int equalIndex = line.IndexOf('=');
                    if (equalIndex != -1)
                    {
                        string valueName = line.Substring(0, equalIndex).Trim();
                        string valueData = line.Substring(equalIndex + 1).Trim();
                        keysAndValues[currentKey + "\\" + valueName] = valueData;
                    }
                }
            }
        }

        return keysAndValues;
    }

    static bool CompareFiles(Dictionary<string, string> dict1, Dictionary<string, string> dict2)
    {
        // Compare the number of keys and values first
        if (dict1.Count != dict2.Count)
        {
            Console.WriteLine("The files have different numbers of keys and values.");
            return false;
        }

        // Compare each key and value
        foreach (var kvp1 in dict1)
        {
            if (!dict2.TryGetValue(kvp1.Key, out var value2) || value2 != kvp1.Value)
            {
                Console.WriteLine($"Difference found - Key: {kvp1.Key}, Value1: {kvp1.Value}, Value2: {value2}");
                return false;
            }
        }

        return true;
    }

    static void CompareMissingKeys(Dictionary<string, string> keysAndValues1, Dictionary<string, string> keysAndValues2)
    {
        foreach (var keyValuePair1 in keysAndValues1)
        {
            if (!keysAndValues2.ContainsKey(keyValuePair1.Key))
            {
                Console.WriteLine($"Key not present in {keyValuePair1.Key}.reg");
            }
        }

        foreach (var keyValuePair2 in keysAndValues2)
        {
            if (!keysAndValues1.ContainsKey(keyValuePair2.Key))
            {
                Console.WriteLine($"Key not present in {keyValuePair2.Key}.reg");
            }
        }
    }

    static void CompareDifferentValues(Dictionary<string, string> keysAndValues1, Dictionary<string, string> keysAndValues2)
    {
        foreach (var keyValuePair1 in keysAndValues1)
        {
            if (keysAndValues2.ContainsKey(keyValuePair1.Key) && keysAndValues2[keyValuePair1.Key] != keyValuePair1.Value)
            {
                Console.WriteLine($"Different value in {keyValuePair1.Key}.reg - Value1: {keyValuePair1.Value}, Value2: {keysAndValues2[keyValuePair1.Key]}");
            }
        }
    }

    static void GenerateMergedRegFile(string baseName1, string baseName2, Dictionary<string, string> keysAndValues1, Dictionary<string, string> keysAndValues2)
    {
        string mergedFilePath = $"{baseName1}_{baseName2}_merged.reg";

        using (StreamWriter writer = new StreamWriter(mergedFilePath))
        {
            // Write the merged .reg file header
            writer.WriteLine("Windows Registry Editor Version 5.00");

            // Write keys and values from both dictionaries
            foreach (var kvp1 in keysAndValues1)
            {
                writer.WriteLine($"{kvp1.Key}={kvp1.Value}");
            }

            foreach (var kvp2 in keysAndValues2)
            {
                if (!keysAndValues1.ContainsKey(kvp2.Key))
                {
                    writer.WriteLine($"{kvp2.Key}={kvp2.Value}");
                }
            }
        }

        Console.WriteLine($"Merged .reg file generated: {mergedFilePath}");
    }

    static void SearchByKey(Dictionary<string, string> keysAndValues, string searchTerm)
    {
        foreach (var kvp in keysAndValues)
        {
            if (kvp.Key.Contains(searchTerm))
            {
                Console.WriteLine($"Found key: {kvp.Key} - Value: {kvp.Value}");
            }
        }
    }

    static void SearchByValue(Dictionary<string, string> keysAndValues, string searchTerm)
    {
        foreach (var kvp in keysAndValues)
        {
            if (kvp.Value.Contains(searchTerm))
            {
                Console.WriteLine($"Found key: {kvp.Key} - Value: {kvp.Value}");
            }
        }
    }
}
