namespace AssetManagement.VersionControl
{
    /// <summary>
    /// 资源集的版本信息
    /// </summary>
    [System.Serializable]
    public class VersionInfo
    {
        /// <summary>
        /// 当前资源集的版本
        /// </summary>
        public string version;

        /// <summary>
        /// 资源集包含的所有资源的描述信息
        /// </summary>
        public AssetInfo[] assets;
    }
}