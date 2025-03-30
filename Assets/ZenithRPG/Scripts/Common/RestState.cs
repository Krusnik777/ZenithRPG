using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class RestState : MonoSingleton<RestState>
    {
        [SerializeField] private Player m_player;

        private Animator m_animator;

        public void StartRest()
        {
            m_animator.SetTrigger("Appear");
            StartCoroutine(RestRoutine());
        }

        public void EndRest()
        {
            m_animator.SetTrigger("Disappear");
            StopCoroutine(RestRoutine());
        }

        private void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        private IEnumerator RestRoutine()
        {
            float hitPointsRecoveryTime = 0;
            float magicPointsRecoveryTime = 0;

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("ActiveState"));

            while(m_animator.GetCurrentAnimatorStateInfo(0).IsName("ActiveState"))
            {
                if (hitPointsRecoveryTime >= (m_player.Character.Stats as PlayerStats).HitPointsRecoveryRate)
                {
                    m_player.Character.Stats.ChangeCurrentHitPoints(this, 1);
                    hitPointsRecoveryTime = 0;
                }

                if (magicPointsRecoveryTime >= (m_player.Character.Stats as PlayerStats).MagicPointsRecoveryRate)
                {
                    m_player.Character.Stats.RecoverMagicPoints(this, 1);
                    magicPointsRecoveryTime = 0;
                }

                hitPointsRecoveryTime += Time.deltaTime;
                magicPointsRecoveryTime += Time.deltaTime;

                yield return null;
            }
        }

    }
}
