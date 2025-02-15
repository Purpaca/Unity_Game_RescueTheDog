using System;
using Newtonsoft.Json;

namespace Purpaca.AssetManagement.Version
{
    [Serializable]
    public class AssetInfo
    {
        [JsonProperty("Asset")]
        private string assetName;
        [JsonProperty("MD5")]
        private string md5;
        [JsonProperty("Size")]
        private long size;

        public AssetInfo(string name, string md5, long size)
        {
            this.assetName = name;
            this.md5 = md5;
            this.size = size;
        }

        [JsonIgnore]
        public string Name => assetName;
        [JsonIgnore]
        public string MD5 => md5;
        [JsonIgnore]
        public long Size => size;
    }
}