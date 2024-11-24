#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RESCUETHEDOG.Test
{
    public class 测试资源加载框架 : MonoBehaviour
    {
        /// <summary>
        /// 未管理的异步资源加载 托管协程
        /// </summary>
        /// <typeparam name="T">要加载的资源的类型</typeparam>
        /// <param name="request">由资源加载框架返回的AssetBundleRequest请求</param>
        /// <param name="callback">资源加载请求结束时的回调</param>
        private IEnumerator UnmanagedAsyncLoad<T>(AssetBundleRequest request, UnityAction<T> callback) where T : Object
        {
            while (!request.isDone)
            {
                Debug.Log($"正在加载 {typeof(T).Name} 类型的资源，加载进度：{request.progress}");
                yield return null;
            }

            callback?.Invoke(request.asset as T);
        }

        private void Start()
        {
            #region 加载狗头相关

            /*
            //  同步加载狗头资源（通过）
            var dog = Instantiate(AssetManager.Instance.LoadDogPrefab("dog_vanilla"));
            dog.GetComponent<DogController>().SetDogeSimulated(false);
            */

            /*
            //  异步加载狗头资源，未托管（通过）
            StartCoroutine(UnmanagedAsyncLoad<GameObject>(AssetManager.Instance.LoadDogPrefabAsync("dog_vanilla"), (asset) => {
                var dog = Instantiate(asset);
                dog.GetComponent<DogController>().SetDogeSimulated(false);
            }));
            */

            /*
            //  异步加载狗头资源，带回调（通过）
            AssetManager.Instance.LoadDogPrefabAsync("dog_vanilla", (asset) => 
            {
                var dog = Instantiate(asset);
                dog.GetComponent<DogController>().SetDogeSimulated(false);
            });
            */
            #endregion
        }
    }
}
#endif