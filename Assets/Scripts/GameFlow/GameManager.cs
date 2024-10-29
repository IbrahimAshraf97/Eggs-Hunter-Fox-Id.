using UnityEngine;

public enum GameCamera {
    Init = 0,
    Game = 1,
    Shop = 2,
    Respawn = 3, 
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    public PlayerMotor _motor;
    public WorldGeneration _worldGeneration;
    public SceneChunkGeneration _sceneChunkGeneration;

    public GameObject[] _cameras;
    private GameState _state;

    private void Start() {
        _instance = this;
        _state = gameObject.GetComponent<GameStateInit>();
        _state.Constract();
    }

    private void Update() {
        _state.UpdateState();
    }

    public void ChangeState(GameState s) {
        _state.Destract();
        _state = s;
        _state.Constract();
    }

    public void ChangeCamera(GameCamera c) {
        foreach (GameObject go in _cameras)
            go.SetActive(false);
        _cameras[(int)c].SetActive(true);
    }
}
