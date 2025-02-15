using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadFileResumable : MonoBehaviour
{
    public string fileUrl = "http://your-file-url.com/file.zip";
    public string savePath = "C:/Downloads/file.zip";
    public int maxRetries = 3; // 最大重试次数
    public Button pauseButton; // 暂停按钮
    public Button resumeButton; // 恢复按钮

    private long currentBytes = 0; // 当前已下载字节数
    private bool isDownloading = false;
    private UnityWebRequest currentRequest;
    private string tempPath;

    void Start()
    {
        // 初始化按钮事件
        pauseButton.onClick.AddListener(PauseDownload);
        resumeButton.onClick.AddListener(ResumeDownload);

        // 开始下载
        StartCoroutine(StartDownloadWithRetry());
    }

    // 带重试机制的下载入口
    IEnumerator StartDownloadWithRetry(int retryCount = 0)
    {
        if (retryCount >= maxRetries)
        {
            Debug.LogError($"下载失败，已达最大重试次数: {maxRetries}");
            yield break;
        }

        isDownloading = true;
        yield return StartCoroutine(DownloadFileCoroutine());

        // 如果下载未完成且未被取消，自动重试
        if (isDownloading)
        {
            Debug.LogWarning($"尝试重试 ({retryCount + 1}/{maxRetries})...");
            yield return new WaitForSeconds(2); // 等待2秒后重试
            yield return StartDownloadWithRetry(retryCount + 1);
        }
    }

    /*
    private IEnumerator Download() 
    {
        string savedFullPath = "C:/Download/file.ab";
        string tmpPath = savedFullPath + ".download";
        long downloadedSize;

        if (!File.Exists(tmpPath)) 
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        else
        {
            FileInfo file = new FileInfo(tmpPath);
            downloadedSize = file.Length;
        }

        if (File.Exists(tempPath))
        {
            // 获取已下载的临时文件大小
            FileInfo fileInfo = new FileInfo(tempPath);
            currentBytes = fileInfo.Length;
            Debug.Log($"检测到未完成下载，已下载 {currentBytes} 字节");
        }
        else
        {
            currentBytes = 0;
            EnsureDirectoryExists(tempPath);
        }




        yield return null;
    }
    */

    // 核心下载协程
    IEnumerator DownloadFileCoroutine()
    {
        // 检查本地文件并初始化断点
        InitializeResume();

        // 创建带Range头的请求
        currentRequest = new UnityWebRequest(fileUrl, UnityWebRequest.kHttpVerbGET);
        currentRequest.SetRequestHeader("Range", $"bytes={currentBytes}-");

        // 配置下载处理器
        tempPath = savePath + ".tmp"; // 临时文件
        DownloadHandlerFile downloadHandler = new DownloadHandlerFile(tempPath, true);
        downloadHandler.removeFileOnAbort = false; // 避免中断时删除临时文件
        currentRequest.downloadHandler = downloadHandler;

        // 发送请求
        currentRequest.SendWebRequest();

        // 实时更新进度
        while (!currentRequest.isDone)
        {
            if (currentRequest.result == UnityWebRequest.Result.InProgress)
            {
                // 获取文件总大小
                long totalFileSize = currentBytes + GetContentLength(currentRequest);

                // 计算已下载的字节数
                long downloadedBytes = currentBytes + (long)currentRequest.downloadedBytes;

                // 计算下载进度
                float progress = totalFileSize > 0 ? (float)downloadedBytes / totalFileSize : 0f;
                Debug.Log($"下载进度: {progress * 100:F1}%");
            }
            yield return null;
        }

        // 处理结果
        if (currentRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"下载失败: {currentRequest.error}");
            yield break;
        }

        // 合并临时文件到最终路径
        MergeTempFile(tempPath);
        Debug.Log("下载完成！文件已保存至: " + savePath);
        isDownloading = false;
    }

    // 获取文件总大小
    long GetContentLength(UnityWebRequest request)
    {
        if (request.GetResponseHeaders().ContainsKey("Content-Length"))
        {
            string contentLength = request.GetResponseHeaders()["Content-Length"];
            if (long.TryParse(contentLength, out long length))
            {
                return length;
            }
        }
        return 0; // 如果无法获取 Content-Length，返回 0
    }

    // 初始化断点续传
    void InitializeResume()
    {
        tempPath = savePath + ".tmp";

        if (File.Exists(tempPath))
        {
            // 获取已下载的临时文件大小
            FileInfo fileInfo = new FileInfo(tempPath);
            currentBytes = fileInfo.Length;
            Debug.Log($"检测到未完成下载，已下载 {currentBytes} 字节");
        }
        else
        {
            currentBytes = 0;
            EnsureDirectoryExists(tempPath);
        }
    }

    // 合并临时文件
    void MergeTempFile(string tempPath)
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath); // 删除旧文件（如果有）
        }
        File.Move(tempPath, savePath);
    }

    // 确保目录存在
    void EnsureDirectoryExists(string path)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    // 暂停下载
    public void PauseDownload()
    {
        if (currentRequest != null && isDownloading)
        {
            currentRequest.Abort(); // 中止当前请求
            isDownloading = false;
            Debug.Log("下载已暂停");
        }
    }

    // 恢复下载
    public void ResumeDownload()
    {
        if (!isDownloading)
        {
            StartCoroutine(StartDownloadWithRetry());
            Debug.Log("下载已恢复");
        }
    }

    // 停止下载（可选）
    public void CancelDownload()
    {
        isDownloading = false;
        if (currentRequest != null)
        {
            currentRequest.Abort();
        }
        StopAllCoroutines();
    }
}