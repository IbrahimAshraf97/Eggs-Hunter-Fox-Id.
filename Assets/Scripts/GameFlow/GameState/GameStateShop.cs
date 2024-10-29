using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateShop : GameState
{
    public GameObject _shopUI;
    public TextMeshProUGUI _totalFish;
    public TextMeshProUGUI _currentHatName;
    public HatLogic _hatLogic;
    private bool _isInit = false;
    private int _hatCount;
    private int _unlockedHatCount;


    // Shop Item
    public GameObject _hatPrefab;
    public Transform _hatContainer;
    private Hat[] _hats;

    // Completion Circle
    public Image _completionCircle;
    public TextMeshProUGUI _completionText;


    public override void Constract() {
        GameManager.Instance.ChangeCamera(GameCamera.Shop);
        _hats = Resources.LoadAll<Hat>("Hat/");
        _shopUI.SetActive(true);

        if (!_isInit) {
            _totalFish.text = "Fish: " + SaveManager.Instance._save.Fish.ToString("000");
            _currentHatName.text = _hats[SaveManager.Instance._save.CurrentHatIndex].ItemName;
            PopulateShop(); 
            _isInit = true;
        }
        ResetCompletionCircle();
    }

    public override void Destract() {
        _shopUI.SetActive(false);
    }

    private void PopulateShop() {

        for (int i = 0; i < _hats.Length; i++) {
            int _index = i;
            GameObject go = Instantiate(_hatPrefab, _hatContainer) as GameObject ;
            // Button
            go.GetComponent<Button>().onClick.AddListener(() => OnHatClick(_index));
            //Thumbnail
            go.transform.GetChild(0).GetComponent<Image>().sprite = _hats[_index].Thumbnail;
            //ItemName
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _hats[_index].ItemName;
            //price
            if (SaveManager.Instance._save.UnlockedHatFlag[i] == 0)
                go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _hats[_index].ItemPrice.ToString();
            else 
            {

                go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = " ";
                _unlockedHatCount++;
            }
        }

    }
    private void OnHatClick(int i) {
        if (SaveManager.Instance._save.UnlockedHatFlag[i] == 1) 
        {
            SaveManager.Instance._save.CurrentHatIndex = i;
            _currentHatName.text = _hats[i].ItemName;
            _hatLogic.SelectHat(i);
            SaveManager.Instance.Save();
        }
        // if we dont have it can we buy it ?
        else if (_hats[i].ItemPrice <= SaveManager.Instance._save.Fish) {

            SaveManager.Instance._save.Fish -= _hats[i].ItemPrice;
            SaveManager.Instance._save.UnlockedHatFlag[i] = 1;
            SaveManager.Instance._save.CurrentHatIndex = i;
            _currentHatName.text = _hats[i].ItemName;
            _hatLogic.SelectHat(i);
            _totalFish.text = "Fish: " + SaveManager.Instance._save.Fish.ToString("000");
            SaveManager.Instance.Save();
            _hatContainer.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = " ";

            _unlockedHatCount++;
            ResetCompletionCircle();
        }
        //Dont have it cant buy it
        else {
            Debug.Log("Not Enough Fish");
        }
    }

    private void ResetCompletionCircle() {
        _hatCount = _hats.Length -1;
        int currentlyUnlockedCount = _unlockedHatCount - 1;
        _completionCircle.fillAmount = (float)currentlyUnlockedCount / (float)_hatCount;
        _completionText.text = currentlyUnlockedCount+ "/" +_hatCount;
    }

    public void OnHomeClick() {
        _brain.ChangeState(GetComponent<GameStateInit>());
    }
}
