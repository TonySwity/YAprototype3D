using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


[System.Serializable]
public class ProgressData
{
    public int Coin;
    public bool IsAuth;
}
public class YandexProvider : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Auth();
    
    [DllImport("__Internal")]
    private static extern void SaveYG(string date);

    [DllImport("__Internal")]
    private static extern void LoadYG();
    
    [DllImport("__Internal")]
    private static extern void SaveToLocalStorage(string key, string value);
    
    [DllImport("__Internal")]
    private static extern string LoadFromLocalStorage(string key);
    
    [DllImport("__Internal")]
    private static extern int HasKeyInLocalStorage(string key);
    
    [DllImport("__Internal")]
    private static extern void RemoveFromLocalStorage(string key);

    [DllImport("__Internal")]
    private static extern void RateGame();

    [DllImport("__Internal")]
    private static extern void ReplayRateGame();

    [DllImport("__Internal")]
    private static extern void FullscreenAdv();

    [DllImport("__Internal")]
    private static extern void RewardedVideo();

    [DllImport("__Internal")]
    private static extern void SaveLeaderBoards(int value);

    [DllImport("__Internal")]
    private static extern void LoadLeaderboards();

    [DllImport("__Internal")]
    private static extern void CheckAuth();

    [DllImport("__Internal")]
    
    private static extern void GetPlayerData();// name and photo

    public static YandexProvider Instance;
    
    [SerializeField] private TextMeshProUGUI _playerDataCoin;

    private int _coin;
    private bool _isAuth;
    [Header("Auth")]
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private RawImage _playerPhoto;
    
    private readonly string[] _sizePhoto = { "small", "medium", "large" };

    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin()
    {
        _coin++;
        _playerDataCoin.text = _coin.ToString();
    }
    
    public void LoadPlayerProgressData()
    {
        LoadYG();
    }

    public void SavePlayerProgressData()
    {
        ProgressData progressData = new ProgressData();
        progressData.Coin = _coin;

        string jsonString = JsonUtility.ToJson(progressData);
        print(jsonString);
        SaveYG(jsonString);
    }

    public void SetPlayerProgressData(string date)
    {
        print(date);
        ProgressData progressData = new ProgressData();
        progressData = JsonUtility.FromJson<ProgressData>(date);
        _coin = progressData.Coin;
        _playerDataCoin.text = _coin.ToString();
    }

    public void Authorization()
    {
        Auth();
    }

    public void CheckAuthorization()
    {
        CheckAuth();
    }
    
    public void SaveLocal()
    {
        Debug.Log("Save Local");
// #if !UNITY_EDITOR
        ProgressData progressData = new ProgressData();
        progressData.Coin = this._coin;
        progressData.IsAuth = this._isAuth;
        SaveToLocalStorage("savesData", JsonUtility.ToJson(progressData));
// #endif
    }
    
    public void LoadLocal()
    {
        Debug.Log("Load Local");

        if (!HasKey("savesData"))
        {
            ResetSaveProgress();
        }
        else
        {
            ProgressData progressData = JsonUtility.FromJson<ProgressData>(LoadFromLocalStorage("savesData"));
            _coin = progressData.Coin;
            _isAuth = progressData.IsAuth;
        }
        _playerDataCoin.text = _coin.ToString();
    }

    public static bool HasKey(string key)
    {
        try
        {
            return HasKeyInLocalStorage(key) == 1;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }

    }
    
    public void ResetSaveProgress()
    {
        Debug.Log("Reset Save Progress");
        ProgressData progressData = new ProgressData();
        _coin = progressData.Coin;
        _isAuth = progressData.IsAuth;
    }

    public void Rate()
    {
        RateGame();
    }

    public void ReRate()
    {
        ReplayRateGame();
    }

    public void FullScreenAdv()
    {
        FullscreenAdv();
    }

    public void RewardedAdv()
    {
        RewardedVideo();
    }

    public void LoadLeaderBoard()
    {
        LoadLeaderboards();
    }

    public void SaveLeaderboard()
    {
        
       SaveLeaderBoards(_coin);
    }

    public void GetNameDisplay()
    {
        GetPlayerData();
    }
    
    
    public void SetName(string playerName)
    {
        _playerName.text = playerName;
    }
    
    public void SetPhoto(string url)
    {
        StartCoroutine(DownloadImage(url));
    }
    
    private IEnumerator DownloadImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
            _playerPhoto.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }

}
