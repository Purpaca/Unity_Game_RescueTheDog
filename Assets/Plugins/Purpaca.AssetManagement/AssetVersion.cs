using System;
using Newtonsoft.Json;

namespace Purpaca.AssetManagement.Version
{
    [Serializable]
    public class AssetVersion
    {
        [JsonProperty("Version")]
        private string name;
        [JsonProperty("Assets")]
        private AssetInfo[] assetInfos;

        public AssetVersion(string name, AssetInfo[] assetInfos)
        {
            this.name = name;
            this.assetInfos = assetInfos;
        }

        [JsonIgnore]
        public string Name => name;
        [JsonIgnore]
        public AssetInfo[] Assets => assetInfos;
    }
}