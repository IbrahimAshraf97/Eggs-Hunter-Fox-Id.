using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    //Gameplay
    private float _chunkSpawnZ;
    private Queue<Chunk> _activeChunks = new Queue<Chunk>();  
    private List<Chunk> _chunkpool = new List<Chunk>();
    // Configurable fields
    [SerializeField] private int _firstChunkSpawnPosition = 5;
    [SerializeField] private int _chunkOnScreen = 5;
    [SerializeField] private float _despawnDistance = 5.0f;

    [SerializeField] private List<GameObject> _chunkPrefab;
    [SerializeField] private Transform _cameraTransform;
    

    private void Awake() {
        ResetWorld();
    }


    void Start()
    {
        // Check if we have an empty chunkPrefab list
        if (_chunkPrefab.Count == 0) {
            Debug.LogError("No chunk prefab found on the world generator, please assign some chunks!");
            return;
        }
        //Try to assign the cameraTransform
        if (!_cameraTransform) {
            _cameraTransform = Camera.main.transform;
            Debug.Log("We've assigned cameraTransform automaticly to the Camera.main.transform!");
        }
    }

    public void ScanPosition() {     
        float cameraZ = _cameraTransform.position.z;
        Chunk lastChunk = _activeChunks.Peek();

        if (cameraZ >= lastChunk.transform.position.z + lastChunk._chunkLength + _despawnDistance) {
            SpawnNewChunk();
            DeleteLastChunk();
        }
    }
    private void SpawnNewChunk() {
        // get a random index for which prefab to spawn
        int randomIndex = Random.Range(0,_chunkPrefab.Count);
        //does it already exist within our pool 
        Chunk chunk = _chunkpool.Find(x => !x.gameObject.activeSelf && x.name == (_chunkPrefab[randomIndex].name)+ "(Clone)");
        //create a chunk if were not able to find one to reuse
        if (!chunk) {
            GameObject go = Instantiate(_chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();
        }
        //place the object and show it 
        chunk.transform.position = new Vector3(0, 0, _chunkSpawnZ);
        _chunkSpawnZ += chunk._chunkLength;
        //Store the value to reuse in our pool
        _activeChunks.Enqueue(chunk);
        chunk.ShowChunk();
    }
    private void DeleteLastChunk() { 
        Chunk chunk = _activeChunks.Dequeue();
        chunk.HideChunk();
        _chunkpool.Add(chunk);
    }
    public void ResetWorld() {
        // reset the chunkspawnZ
        _chunkSpawnZ = _firstChunkSpawnPosition;

        for (int i = _activeChunks.Count; i != 0; i--)
            DeleteLastChunk();
        for (int i = 0; i < _chunkOnScreen; i++)
            SpawnNewChunk();
    }
}
