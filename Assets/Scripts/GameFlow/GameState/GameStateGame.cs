using TMPro;
using UnityEngine;

public class GameStateGame : GameState {
    public GameObject _gameUI;
    [SerializeField] private TextMeshProUGUI _fishCount;
    [SerializeField] private TextMeshProUGUI _scoreCount;

    public override void Constract() {
        base.Constract();
        GameManager.Instance._motor.ResumePlayer();

        GameManager.Instance.ChangeCamera(GameCamera.Game);

        GameStats.Instance.OnCollectFish += OnCollectFish;
        GameStats.Instance.OnScoreChange += OnScoreChange;

        _gameUI.SetActive(true);
    }

    private void OnCollectFish(int amnCollected) {
        _fishCount.text = GameStats.Instance.FishToText();
    }

    private void OnScoreChange(float score) {
        _scoreCount.text = GameStats.Instance.ScoreToText();
    }

    public override void Destract() {
        _gameUI.SetActive(false);

        GameStats.Instance.OnCollectFish -= OnCollectFish;
        GameStats.Instance.OnScoreChange -= OnScoreChange;
    }

    public override void UpdateState() {
        GameManager.Instance._worldGeneration.ScanPosition();
        GameManager.Instance._sceneChunkGeneration.ScanPosition();
    }
}
