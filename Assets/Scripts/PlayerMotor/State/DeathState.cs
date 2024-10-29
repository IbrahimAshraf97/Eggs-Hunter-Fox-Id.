using UnityEngine;

public class DeathState : BaseState
{
    [SerializeField] private Vector3 _knockbackForce = new Vector3(0, 4, -3);

    private Vector3 _currentKnockback;

    public override void Construct() {
        _motor._anim?.SetTrigger("Death");
        _currentKnockback = _knockbackForce;
    }
    public override Vector3 ProcessMotion() {
        Vector3 m = _currentKnockback;

        _currentKnockback = new Vector3(0,
            _currentKnockback.y -= _motor._gravity * Time.deltaTime,
            _currentKnockback.z += 2.0f * Time.deltaTime);

        if (_currentKnockback.z > 0) {
            _currentKnockback.z = 0;
            GameManager.Instance.ChangeState(GameManager.Instance.GetComponent<GameStateDeath>());
        }

        return _currentKnockback;
    }
}
