using UnityEngine;
using UnityEditor;

public class AssetVersionControl
{
    [MenuItem("Rescue the Dog/Asset Version Control")]
    public static void OpenPanel()
    {
        var p = EditorWindow.GetWindow<AssetVersionControlPanel>();
        p.titleContent = new GUIContent("��Դ�汾��������");
        p.Show();
        p.Focus();
    }

    public class AssetVersionControlPanel : EditorWindow
    {
        private string input = string.Empty;

        private void OnBecameVisible()
        {
            input = "haloha";
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(15, 50, 145, 20), "Դ Assetbundle ��Ŀ¼");
            input = GUI.TextField(new Rect(155, 50, 350, 20), input);
            bool click = GUI.Button(new Rect(520, 50, 50, 20), "���");

            /*
            GUI.BeginScrollView(new Rect(15, 75, 505, 100), Vector2.zero, new Rect(0,0,505,100));
            GUI.Label(new Rect(15, 75, 505, 100), "haha");
            GUI.EndScrollView();
            */
        }
    }
}