using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private PlayerStatusInfo playerStatusInfo;

    private void Start()
    {
        playerStatusInfo = GetComponentInParent<Player>().PlayerStatusInfo;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            playerStatusInfo.GetHurt(1);
        }
        else if (other.CompareTag("EnemyDamager"))
        {
            if (other.TryGetComponent<IDestroySelf>(out var destroyer))
            {
                other.transform.SetParent(transform);
                destroyer.DestroySelf();
            }
            if (other.TryGetComponent<IDamager>(out var damager))
            {
                playerStatusInfo.GetHurt(damager.Damage);
            }
        }
    }
}
