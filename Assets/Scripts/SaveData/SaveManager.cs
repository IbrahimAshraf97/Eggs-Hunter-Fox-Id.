using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return _instance; } }
    private static SaveManager _instance;
    //
    // Fields
    public SaveState _save;
    private const string _saveFileName = "data.sss";
    private BinaryFormatter _formatter;

    //Actions
    public Action<SaveState> OnLoad;
    public Action<SaveState> OnSave;

     
    private void Awake() {

        _instance = this;
        _formatter = new BinaryFormatter();
        //Try and load the previous save state
        Load();
    }

    public void Load() {
        try {
            FileStream file = new FileStream(Application.persistentDataPath + _saveFileName, FileMode.Open, FileAccess.Read);
            _save = (SaveState)_formatter.Deserialize(file);
            file.Close();
            OnLoad?.Invoke(_save);
        } catch {
            Debug.Log("Save file not found, let's create a new one!");
            Save();
        }
        
    }
    public void Save() {
        // if there's no previous state found, create new one!
        if (_save == null) {
            _save = new SaveState();
        }
        // Set the time at which we've tried saving
        _save.LastSaveTime = DateTime.Now;
        // Open a file on our system, and write on it
        FileStream file = new FileStream(Application.persistentDataPath + _saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
        _formatter.Serialize(file, _save);
        file.Close();
        OnSave?.Invoke(_save);
    }
}
