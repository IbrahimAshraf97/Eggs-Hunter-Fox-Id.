using TMPro;
using UnityEngine;

public class GameStateInit : GameState
{
    public GameObject _menuUI;
    [SerializeField] private TextMeshProUGUI _hiScoreText;
    [SerializeField] private TextMeshProUGUI _fishCountText;


    public override void Constract() {
        GameManager.Instance.ChangeCamera(GameCamera.Init);

        _hiScoreText.text = "Highscore: " + SaveManager.Instance._save.Highscore.ToString();
        _fishCountText.text = SaveManager.Instance._save.Fish.ToString("000");

        _menuUI.SetActive(true);
    }

    public override void Destract() {
        _menuUI.SetActive(false);
    }

    public void OnPlayClick() {
        _brain.ChangeState(GetComponent<GameStateGame>());
        GameStats.Instance.ResetSession();
        GetComponent<GameStateDeath>().EnableRevive();
    }
    public void OnShopClick() {
        _brain.ChangeState(GetComponent<GameStateShop>());
    }

}
