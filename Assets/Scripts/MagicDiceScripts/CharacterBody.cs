using UnityEngine;

public class CharacterBody : MonoBehaviour
{
    private Animator _animator;

    public void SetBody(Animator animatorPrefab)
    {
        RemoveBody();
        if (animatorPrefab != null)
        {
            _animator = Instantiate(animatorPrefab, transform);
        }
    }

    private void RemoveBody()
    {
        if (_animator == null) return;
        Destroy(_animator.gameObject);
        _animator = null;
    }
    public void Attack()
    {
        if (_animator == null)
        {
            return;
        }
        _animator.SetTrigger(Animator.StringToHash("Attack"));
    }

    public void Hurt()
    {
        if (_animator == null)
        {
            return;
        }
        _animator.SetTrigger(Animator.StringToHash("Hurt"));
    }
}