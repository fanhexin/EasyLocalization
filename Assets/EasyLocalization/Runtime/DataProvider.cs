using System.Collections.Generic;
using UnityEngine;

namespace EasyLocalization
{
    public abstract class DataProvider : ScriptableObject
    {
        public abstract IEnumerable<string> Keys { get; }
        public abstract IEnumerable<string> Languages { get; }
        public abstract string GetValue(string language, string key);
    }
}