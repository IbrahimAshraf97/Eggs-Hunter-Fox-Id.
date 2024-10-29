using UnityEngine;

public abstract class GameState : MonoBehaviour
{
    protected GameManager _brain;

    protected virtual void Awake() {
        _brain = GetComponent<GameManager>();
    }
    public virtual void Constract() {
        Debug.Log("Constracting : " + this.ToString()); 
    }
    public virtual void Destract() { }
    public virtual void UpdateState() { }

}
