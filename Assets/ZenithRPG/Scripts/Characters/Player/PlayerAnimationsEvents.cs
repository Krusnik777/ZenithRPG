using UnityEngine;

namespace DC_ARPG
{
    public class PlayerAnimationsEvents : CharacterAnimationsEvents
    {
        private Player _player;

        public void OnAttackWindowOpen()
        {
            if (_player == null) return;

            _player.ComboIsAvailable = true;
        }

        public void OnAttackWindowClose()
        {
            if (_player == null) return;

            _player.ComboIsAvailable = false;
        }

        private void Start()
        {
            if (m_characterAvatar is not Player)
            {
                Debug.LogError("Character Avatar is not Player");

                return;
            }

            _player = m_characterAvatar as Player;
        }
    }
}
