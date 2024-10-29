using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;


public class GameStateDeath : GameState , IUnityAdsLoadListener, IUnityAdsShowListener {
    public GameObject _deathUI;
    [SerializeField] private TextMeshProUGUI _hightScore;
    [SerializeField] private TextMeshProUGUI _currentScore;
    [SerializeField] private TextMeshProUGUI _fishTotal;
    [SerializeField] private TextMeshProUGUI _fishCurrent;

    // Completion circle fields
    [SerializeField] private Image _completionCircle;
    public float _timeToDecision = 2.5f;
    private float _deathTime;


    private void Start() {

    }

    public override void Constract() {
        base.Constract();
        GameManager.Instance._motor.PausePlayer();
        _deathTime = Time.time;

        _deathUI.SetActive(true);

        // Prior to save, set the highscore if needed
        if (SaveManager.Instance._save.Highscore < (int)GameStats.Instance._score) 
            {
            SaveManager.Instance._save.Highscore = (int)GameStats.Instance._score;
            _currentScore.color = Color.green;
        }else
            _currentScore.color = Color.white;


        SaveManager.Instance._save.Fish += GameStats.Instance._fishCollectedThisSession;
        SaveManager.Instance.Save();

        _hightScore.text = "Hight Score: " + SaveManager.Instance._save.Highscore;
        _currentScore.text = GameStats.Instance.ScoreToText();
        _fishTotal.text = "Total fish: "+ SaveManager.Instance._save.Fish; ;
        _fishCurrent.text = GameStats.Instance.FishToText(); ;
    }

    public override void Destract() {
        _deathUI.SetActive(false);
    }

    public override void UpdateState() {
        float ratio = (Time.time - _deathTime) / _timeToDecision;
        _completionCircle.color = Color.Lerp(Color.green, Color.red, ratio);
        _completionCircle.fillAmount = 1 - ratio;
         
        if (ratio > 1) {
            _completionCircle.gameObject.SetActive(false);
        }
    }

    public void ToMenu() {
        _brain.ChangeState(GetComponent<GameStateInit>());

        GameManager.Instance._motor.ResetPlayer();
        GameManager.Instance._worldGeneration.ResetWorld();
        GameManager.Instance._sceneChunkGeneration.ResetWorld();
    }
    public void EnableRevive() {
        _completionCircle.gameObject.SetActive(true); 
    }

    public void TryResumeGame() {
        ShowAd();
    }

    public void ResumeGame() {
        _brain.ChangeState(GetComponent<GameStateGame>());

        GameManager.Instance._motor.RespawnPlayer();
    }

    public void LoadAd() {
        Advertisement.Load(AdManager.Instance._rewardedVideoPlacementId,this);
    }

    public void OnUnityAdsAdLoaded(string placementId) {
        Debug.Log("Loading Ad: " + placementId);
    }

    public void ShowAd() {

        Advertisement.Show(AdManager.Instance._rewardedVideoPlacementId, this);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        _completionCircle.gameObject.SetActive(false);

        switch (showCompletionState) {
            case UnityAdsShowCompletionState.UNKNOWN:
                ToMenu();
                break;
            case UnityAdsShowCompletionState.COMPLETED:
                ResumeGame();
                break;
            default:
                break;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        Debug.Log(message);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        Debug.Log(message);
    }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }

    void OnDestroy() {
        // Clean up the button listeners:
        _completionCircle.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
