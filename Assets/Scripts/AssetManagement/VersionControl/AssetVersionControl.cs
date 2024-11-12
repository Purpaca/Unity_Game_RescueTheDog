using System;
using System.IO;
using System.Security.Cryptography;

namespace AssetManagement.VersionControl
{
    public class AssetVersionControl
    {
        #region Public 方法
        /// <summary>
        /// 获取指定资源文件的MD5码
        /// </summary>
        /// <param name="assetPath">资源文件所在的绝对路径</param>
        public static string GetAssetMD5(string assetPath)
        {
            if (!File.Exists(assetPath))
            {
                return null;
            }

            byte[] bytes = File.ReadAllBytes(assetPath);
            return GetAssetMD5(bytes);
        }

        /// <summary>
        /// 获取指定资源文件的MD5码
        /// </summary>
        /// <param name="data">资源文件的二进制数据</param>
        public static string GetAssetMD5(byte[] data)
        {
            byte[] hash;

            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(data);
            }
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /*
        /// <summary>
        /// 生成资源集版本信息
        /// </summary>
        /// <param name="relativePath">资源在根资源存储目录下的相对路径</param>
        /// <param name="version">资源集的版本</param>
        public static VersionInfo CreateVersionConfig(string relativePath, string version)
        {
            string path = Path.Combine(AssetUtility.AssetsRootPath, relativePath);
            if (!Directory.Exists(path))
            {
                Debug.LogWarning($"GetVersionConfig(string, string): Directory that pointed to by path({path}) does not exist.");
                return null;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] fileInfos = directoryInfo.GetFiles();

            if (fileInfos.Length <= 0)
            {
                Debug.LogWarning($"GetVersionConfig(string, string): There is no assets under the specific directory {path}");
                return null;
            }

            VersionConfig config = new VersionConfig();
            config.version = version;
            config.path = relativePath;

            foreach (FileInfo file in fileInfos)
            {
                AssetInformation assetInfo = new AssetInformation();
                assetInfo.name = file.Name;
                assetInfo.md5 = GetAssetMD5(file.FullName);

                config.assets.Add(assetInfo);
            }

            return config;
        }


        /// <summary>
        /// 比对检查给定的资源集版本信息实例与其指向的资源是否匹配
        /// </summary>
        /// <param name="config">资源集版本信息</param>
        /// <param name="unmatchedAssets">不匹配的资源的名称</param>
        /// <returns>资源是否与资源版本信息匹配？</returns>
        public static bool CheckAssetsVersionMatchConfig(VersionConfig config, out string[] unmatchedAssets)
        {
            unmatchedAssets = null;
            string path = Path.Combine(AssetUtility.AssetsRootPath, config.path);

            List<string> unmatches = new List<string>();

            foreach (AssetInformation assetInfo in config.assets) 
            {
                string assetPath = Path.Combine(path, assetInfo.name);

                if (!File.Exists(assetPath) || GetAssetMD5(Path.Combine(assetPath, assetInfo.name)) != assetInfo.md5)
                {
                    unmatches.Add(assetInfo.name);
                }
            }

            if(unmatches.Count > 0) 
            {
                unmatchedAssets = unmatches.ToArray();
                return false;
            }

            return true;
        }*/
        #endregion
    }
}