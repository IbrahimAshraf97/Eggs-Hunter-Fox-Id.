using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour,IUnityAdsInitializationListener
{

    public static AdManager Instance { get { return instance; } }

    private static AdManager instance;

    [SerializeField] private string _gameID;
    [SerializeField] public string _rewardedVideoPlacementId;
    [SerializeField] private bool _testMode;


    private void Awake() {
        instance = this;
        InitializeAds();
    }
    public void InitializeAds() {
        if (!Advertisement.isInitialized && Advertisement.isSupported) {
            Advertisement.Initialize(_gameID, _testMode, this);
        }
    }
    /*public void ShowRewardedAd() {
        ShowOptions so = new ShowOptions();
        Advertisement.Show(_rewardedVideoPlacementId, so);
    }*/

    public void OnInitializationComplete() {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
