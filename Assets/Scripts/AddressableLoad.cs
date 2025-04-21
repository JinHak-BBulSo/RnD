using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressableLoad : MonoBehaviour
{
    //AWS S3 프리 티어
    //RemotePath : https://kjhbbulso.s3.eu-north-1.amazonaws.com/StandaloneWindows64

    private static readonly string SizeTextFormat = "[{0} / {1}]";
    [SerializeField]
    TextMeshProUGUI _sizeText;
    [SerializeField]
    TextMeshProUGUI _percentText;
    [SerializeField]
    Slider slider;
    [SerializeField]
    Button _downloadBtn;
    private string LABEL = "Test";
    private string _path1 = "Assets/TestAssets/TestPrefab/SampleImage.prefab";
    private string _path2 = "Assets/TestAssets/TestPrefab/SampleImage2.prefab";
    private string _path3 = "Assets/TestAssets/TestPrefab/SampleImage3.prefab";
    private string _path4 = "Assets/TestAssets/TestPrefab/SampleImage4.prefab";
    private long _size;
    private string _sizeStr;
    private long _received;

    private void Start()
    {
        InitAddressables();
    }

    private void InitAddressables()
    {
        var initHandle = Addressables.InitializeAsync(false);
        initHandle.Completed += (handle) =>
        {
            Addressables.Release(handle);
        };
    }
    public void Download()
    {
        StartCoroutine(DownloadProgress());
        _downloadBtn.interactable = false;
        //GetDownloadFileSize();
    }

    //public void GetDownloadFileSize()
    //{
    //    Addressables.GetDownloadSizeAsync(LABEL).Completed +=
    //        (AsyncOperationHandle<long> handle) =>
    //        {
    //            string size = string.Concat(handle.Result, " byte");
    //            _sizeText.text = size;

    //            Addressables.Release(handle);
    //        };
    //}

    IEnumerator GetDownloadFileSize()
    {
        var sizeHandle = Addressables.GetDownloadSizeAsync(LABEL);
        yield return sizeHandle;

        if (sizeHandle.Status == AsyncOperationStatus.Succeeded)
        {
            _size = sizeHandle.Result;
            _sizeStr = FormatBytes(_size);
            _sizeText.text = string.Format(SizeTextFormat, 0, _sizeStr);
        }
        Addressables.Release(sizeHandle);
    }

    private string FormatBytes(long bytes)
    {
        int scale = 1024;
        string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
        long max = (long)Mathf.Pow(scale, orders.Length - 1);

        foreach (string order in orders)
        {
            if (bytes > max)
                return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

            max /= scale;
        }
        return "0Bytes";
    }

    IEnumerator DownloadProgress()
    {
        yield return GetDownloadFileSize();

        var downloadHandle = Addressables.DownloadDependenciesAsync(LABEL);
        StartCoroutine(TrackDownloadProgress(downloadHandle));
    }

    IEnumerator TrackDownloadProgress(AsyncOperationHandle handle)
    {
        while (!handle.IsDone)
        {
            long delta = handle.GetDownloadStatus().DownloadedBytes - _received;
            float ratio = (float)_received / (float)_size;

            _received += delta;
            _sizeText.text = string.Format(SizeTextFormat, FormatBytes(_received), _sizeStr);

            float percent = handle.PercentComplete * 100f;
            string percentStr = percent.ToString("N2") + "%";
            slider.value = handle.PercentComplete;
            _percentText.text = percentStr;
            yield return null;
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            slider.value = 1.0f;
            _percentText.text = 100 + "%";
            Debug.Log("다운로드 성공");
        }
        else
        {
            Debug.LogError("다운로드 실패");
        }

        Addressables.Release(handle);
    }

    [SerializeField]
    private GameObject canvas;
    private List<GameObject> _spawnObjs = new List<GameObject>();
    public void SpawnObject()
    {
        if (_spawnObjs.Count > 0)
        {
            foreach (GameObject obj in _spawnObjs)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
        }
        _spawnObjs.Clear();
        string[] pathArr = new string[4] { _path1, _path2, _path3, _path4 };
        for (int i = 0; i < pathArr.Length; i++)
        {
            Addressables.LoadResourceLocationsAsync(pathArr[i]).Completed +=
            (handle) =>
            {
                var locations = handle.Result;
                Addressables.InstantiateAsync(locations[0], canvas.transform).Completed += (objHandle) =>
                {
                    GameObject result = objHandle.Result;
                    _spawnObjs.Add(result);
                };
            };
        }
    }
}