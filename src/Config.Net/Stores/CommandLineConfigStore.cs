﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Config.Net.Stores
{
   class CommandLineConfigStore : IConfigStore
   {
      private readonly Dictionary<string, string> _nameToValue = new Dictionary<string, string>();
      private static readonly char[] ArgPrefixes = new[] { '-', '/' };
      private static readonly string[] ArgDelimiters = new[] { ":", "=" };

      public bool CanRead => true;

      public bool CanWrite => false;

      public string Name => "Command Line Arguments";

      public CommandLineConfigStore(string[] args)
      {
         Parse(args ?? Environment.GetCommandLineArgs());
      }

      public void Dispose()
      {
      }

      public string Read(string key)
      {
         if (key == null) return null;

         string value;
         _nameToValue.TryGetValue(key, out value);
         return value;
      }

      public void Write(string key, string value)
      {
         throw new NotSupportedException();
      }

      private void Parse(string[] args)
      {
         if (args == null) return;

         var pairs = args
            .Where(a => a != null)
            .Select(a => a.SplitByDelimiter(ArgDelimiters))
            .Where(kv => !string.IsNullOrEmpty(kv.Item2))
            .ToDictionary(kv => kv.Item1.TrimStart(ArgPrefixes), kv => kv.Item2);

         _nameToValue.AddRange(pairs);
      }
   }
}