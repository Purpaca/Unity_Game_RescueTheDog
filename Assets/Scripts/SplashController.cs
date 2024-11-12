using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏应用开屏动画控制器
/// </summary>
public class SplashController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup m_icon;
    [SerializeField]
    private Image m_loadingImg;

    private bool isAnimationDone;
    public bool IsAnimationDone { get => isAnimationDone; }

    private void Start()
    {
        isAnimationDone = false;
        StartCoroutine(IconFadeInOutAnimation());
    }

    /// <summary>
    /// 开屏动画流程控制
    /// </summary>
    System.Collections.IEnumerator IconFadeInOutAnimation()
    {
        m_loadingImg.gameObject.SetActive(false);
        
        float fadeTime = 1.0f;      // Logo完成淡入或淡出渐变所需的时间
        float iconShowTime = 1.0f;  // Logo完成淡入渐变后停留展示的时间

        #region Logo动画控制
        float alpha = 0;

        // 淡入
        while (alpha < 1.0f)
        {
            alpha += 1.0f / fadeTime * Time.deltaTime;
            m_icon.alpha = alpha;
            yield return null;
        }

        // 保持显示
        yield return new WaitForSeconds(iconShowTime);

        // 淡出
        while (alpha > 0.0f)
        {
            alpha -= 1.0f / fadeTime * Time.deltaTime;
            m_icon.alpha = alpha;
            yield return null;
        }

        //持续显示空白屏一段时间
        yield return new WaitForSeconds(1.0f);
        #endregion

        isAnimationDone = true;     // 通知：开屏动画已经播放完毕

        #region 加载图标控制
        m_loadingImg.gameObject.SetActive(true);
        Vector3 loadingImgEular;
        while (true)
        {
            loadingImgEular = m_loadingImg.transform.eulerAngles;
            loadingImgEular.z -= 180.0f * Time.deltaTime;
            m_loadingImg.transform.eulerAngles = loadingImgEular;

            yield return null;
        }
        #endregion
    }
}