using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get { return instance; } }
    private static GameStats instance;

    // Score
    public float _score;
    public float _highScore;
    public float _distanceModifier = 1.5f;

    // fish
    public float _totalFish;
    public int _fishCollectedThisSession;
    public float _pointePerFish = 10f;

    // Internal Cooldown
    private float _lastScoreUpdate;
    public float _scoreUpdateDelta = .2f;

    // Action
    public Action<int> OnCollectFish;
    public Action<float> OnScoreChange;

    private void Awake() {
        instance = this;
    }

    public void CollectFish() {
        _fishCollectedThisSession++;
        OnCollectFish?.Invoke(_fishCollectedThisSession);
    }

    // Update is called once per frame
    void Update()
    {
        float s = GameManager.Instance._motor.transform.position.z * _distanceModifier;
        s += _fishCollectedThisSession * _pointePerFish;

        if (s > _score) {
            _score = s;
            if (Time.time - _lastScoreUpdate > _scoreUpdateDelta) {
                _lastScoreUpdate = Time.time;
                OnScoreChange?.Invoke(_score);
            }
        }
    }

    public void ResetSession() {
        _score = 0;
        _fishCollectedThisSession = 0;
        OnCollectFish?.Invoke(_fishCollectedThisSession);
        OnScoreChange?.Invoke(_score);
    }

    public String ScoreToText() {
        return _score.ToString("0000000");
    }
    public String FishToText() {
        return _fishCollectedThisSession.ToString("000");
    }
}
