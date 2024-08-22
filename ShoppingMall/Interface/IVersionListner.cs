namespace ShoppingMall.Interface
{
    public interface IVersionListner
    {
        /// <summary>
        /// 初始化Version.json檔的監聽
        /// </summary>
        void Initialize();

        /// <summary>
        /// 釋放監聽資源
        /// </summary>
        void Dispose();
    }
}
