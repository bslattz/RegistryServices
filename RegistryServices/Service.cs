using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace RegistryServices
{
    public static class Service
    {
        public static RegistryKey FindKey(RegistryKey root, object keys)
        {
            if (keys == null || root == null) return root;
            var keysList = keys is List<string> 
                ? (List<string>)keys : new List<string> {keys as string};
            var rootKey = root;
            var remainingKeys = new List<string>(keysList);
            foreach (var keyName in keysList)
            {
                var foundKey = rootKey.OpenSubKey(keyName);
                if (foundKey == null)
                {
                    var subKeyList = rootKey.GetSubKeyNames();
                    foreach (var k in subKeyList)
                    {
                        foundKey = FindKey(rootKey.OpenSubKey(k), remainingKeys);
                        if (foundKey != null) break;
                    }
                }
                if ((rootKey = foundKey) == null) break;
                remainingKeys.Remove(keyName);
            }
            return rootKey;
        }

        public static string QueryRegistry(RegistryKey root, string path)
        {
            return path.Split(Path.DirectorySeparatorChar)
                .Aggregate(root, (r, k) =>
                {
                    var key = r?.OpenSubKey(k);
                    r?.Close();
                    return key;
                }).GetValue(null).ToString();
        }
    }
}
