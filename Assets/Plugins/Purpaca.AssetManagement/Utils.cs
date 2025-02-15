using System.Security.Cryptography;
using System;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Purpaca.AssetManagement.Version
{
    public static class Utils
    {
        public static string GetMD5(byte[] data)
        {
            byte[] hash;

            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(data);
            }
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public static void CreateListFile(AssetVersion version, string outputPath) 
        {
            string json = JsonConvert.SerializeObject(version);
            if (!Directory.Exists(outputPath))
            {
                Debug.LogError("Output Directory Does Not Exist");
                return;
            }

            string path = Path.Combine(Path.GetFullPath(outputPath), "version.json");
            File.WriteAllText(path, json);
        }
    }
}