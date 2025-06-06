using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DC_ARPG
{
    public class CharacterAnimationsEvents : MonoBehaviour
    {
        [SerializeField] protected CharacterAvatar m_characterAvatar;
        [SerializeField] private CharacterSFX m_characterSFX;
        //[SerializeField] private RigBuilder m_rigBuilder;
        [SerializeField] private GameObject m_kickEffect;

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
            if (!m_characterAvatar.IsFallingOrFallen) m_characterSFX.PlayLandSound();
            m_characterAvatar.LandAfterJump();
        }

        public void OnLandAfterFallAnimation()
        {
            m_characterSFX.PlayLandSound(); // Maybe another sound? Or without it
            m_characterAvatar.LandAfterFall();

            //m_rigBuilder.layers[0].active = false;
            //m_rigBuilder.layers[1].active = false;
        }

        public void OnAttackAnimation()
        {
            m_characterSFX.PlayAttackSound();

            var opponent = m_characterAvatar.CheckForwardGridForOpponent();

            if (opponent != null) m_characterAvatar.Character.DamageOpponent(opponent);
        }

        public void OnAttackAnimationEnd()
        {
            if (m_characterAvatar is not Enemy) return;

            (m_characterAvatar as Enemy).StopAttack();
        }

        public void OnKickAnimation()
        {
            m_characterSFX.PlayKickSound();

            var kickable = m_characterAvatar.CheckForwardGridForKickableObject();

            if (kickable != null)
            {
                m_kickEffect.SetActive(true);

                kickable.OnKicked(m_characterAvatar.transform.forward);
            }
        }

        public void OnKickAnimationEnd()
        {
            m_characterAvatar.RecoverAfterKick();

            m_kickEffect.SetActive(false);
        }

        public void OnDeathAnimation()
        {
            m_characterSFX.PlayDeathSFX(m_characterAvatar.transform.position);
        }

        private void Start()
        {
            m_characterAvatar.EventOnFallStart += OnFallAnimation;
        }

        private void OnDestroy()
        {
            m_characterAvatar.EventOnFallStart -= OnFallAnimation;
        }

        private void OnFallAnimation()
        {
            //m_rigBuilder.layers[0].active = true;
            //m_rigBuilder.layers[1].active = true;
        }
    }
}
