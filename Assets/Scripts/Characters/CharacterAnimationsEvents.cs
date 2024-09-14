using UnityEngine;

namespace DC_ARPG
{
    public class CharacterAnimationsEvents : MonoBehaviour
    {
        [SerializeField] private CharacterAvatar m_characterAvatar;
        [SerializeField] protected CharacterSFX m_characterSFX;

        public void OnFootstepAnimation()
        {
            m_characterSFX.PlayFootstepSound();
        }

        public void OnJumpStartAnimation()
        {
            m_characterSFX.PlayJumpSound();
        }

        public void OnLandAfterJumpAnimation()
        {
            m_characterSFX.PlayLandSound();
            m_characterAvatar.LandAfterJump();
        }

        public void OnLandAfterFallAnimation()
        {
            m_characterAvatar.LandAfterFall();
        }

        public void OnAttackAnimation()
        {
            m_characterSFX.PlayAttackSound();
        }

        public void OnDeathAnimation()
        {
            m_characterSFX.PlayDeathSFX(m_characterAvatar.transform.position);
        }
    }
}
