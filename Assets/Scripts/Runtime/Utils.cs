using UnityEngine;

public class Utils
{
    /// <summary>
    /// 获取主相机
    /// （如果主相机当前不存在或未激活，会创建并初始化主相机）
    /// </summary>
    public static Camera GetMainCamera()
    {
        if (Camera.main == null)
        {
            GameObject go = new GameObject("Main Camera");
            Camera cam = go.AddComponent<Camera>();
            go.AddComponent<AudioListener>();
            go.tag = "MainCamera";

            cam.transform.position = new Vector3(0.0f, 0.0f, -10.0f);

            AdaptiveCamera2D adaptor = cam.gameObject.AddComponent<AdaptiveCamera2D>();
            adaptor.ContentSize = new Vector2(6, 10.66667f);
            adaptor.AdaptedDisplayAxis = AdaptiveCamera2D.AdaptedAxis.Both;

            return cam;
        }

        return Camera.main;
    }

    /// <summary>
    /// 将给定的以矩形Rect表示的安全区转换为以屏幕中心为原点在上、下、左和右四个方向上的偏移值。（偏移值越大，安全区在此方向上的边界越远离屏幕中心点。值为1则安全区在屏幕此方向上的最边缘）
    /// </summary>
    /// <param name="rect">给定的以矩形Rect表示的安全区</param>
    /// <param name="safeAreaOffsetUp">安全区以屏幕中心为原点，向上方的偏移值</param>
    /// <param name="safeAreaOffsetDown">安全区以屏幕中心为原点，向下方的偏移值</param>
    /// <param name="safeAreaOffsetLeft">安全区以屏幕中心为原点，向左方的偏移值</param>
    /// <param name="safeAreaOffsetRight">安全区以屏幕中心为原点，向右方的偏移值</param>
    public static void SafeAreaRectToOffsetValues(Rect rect, out float safeAreaOffsetUp, out float safeAreaOffsetDown, out float safeAreaOffsetLeft, out float safeAreaOffsetRight)
    {
        safeAreaOffsetUp = (rect.y + rect.height - Screen.height / 2) / (Screen.height / 2);
        safeAreaOffsetDown = (Screen.height / 2 - rect.y) / (Screen.height / 2);
        safeAreaOffsetLeft = (Screen.width / 2 - rect.x) / (Screen.width / 2);
        safeAreaOffsetRight = (rect.x + rect.width - Screen.width / 2) / (Screen.width / 2);
    }
}