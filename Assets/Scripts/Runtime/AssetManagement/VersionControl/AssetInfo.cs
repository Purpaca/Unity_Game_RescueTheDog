namespace AssetManagement.VersionControl
{
    /// <summary>
    /// 资源文件的描述信息
    /// </summary>
    [System.Serializable]
    public class AssetInfo
    {
        /// <summary>
        /// 资源文件于资源集根目录下的相对路径
        /// </summary>
        public string name;

        /// <summary>
        /// 资源文件的MD5值
        /// </summary>
        public string md5;

        /// <summary>
        /// 资源文件以字节为单位的大小
        /// </summary>
        public long size;
    }
}