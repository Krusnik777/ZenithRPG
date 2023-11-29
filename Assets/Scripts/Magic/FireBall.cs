using UnityEngine;

namespace DC_ARPG
{
    public class FireBall : MonoBehaviour
    {
        [SerializeField] protected float m_velocity;
        [SerializeField] protected float m_lifeTime;
        [SerializeField] protected int m_damage;

        private void Start()
        {
            Destroy(gameObject, m_lifeTime);
        }


        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.forward*100, m_velocity * Time.deltaTime); 
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Player>() == true) return;

            if (collision.gameObject.TryGetComponent(out EnemyCharacter enemyCharacter))
            {
                enemyCharacter.EnemyStats.ChangeCurrentHitPoints(-m_damage);
            }

            Destroy(gameObject);
        }

    }
}
