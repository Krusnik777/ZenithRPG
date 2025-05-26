using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public abstract class EnemyStateTransition
    {
        [SerializeField] protected EnemyState m_trueState;
        [SerializeField] protected EnemyState m_falseState;
    }
}
