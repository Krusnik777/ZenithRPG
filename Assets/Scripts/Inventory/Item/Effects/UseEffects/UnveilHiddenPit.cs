using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "UnveilHiddenPit", menuName = "ScriptableObjects/UseEffects/UnveilHiddenPit")]
    public class UnveilHiddenPit : UseEffect
    {
        public override void Use(IItem item)
        {
            var player = LevelState.Instance.Player;

            if (player == null)
            {
                Debug.LogError("On Use Item - Not Found Player");
                return;
            }

            UISounds.Instance.PlayItemUsedSound(m_useSound);

            var potentialPit = player.CheckForwardGridForInspectableObject();

            if (potentialPit is Pit)
            {
                var pit = potentialPit as Pit;

                if (pit.TrapFloor != null)
                {
                    ShortMessage.Instance.ShowMessage("����� ���!");
                    pit.UnveilHiddenPit();
                    item.Amount--;
                    return;
                }
                else
                {
                    ShortMessage.Instance.ShowMessage("��� ���� � ���.");
                    item.Amount--;
                    return;
                }
            }

            if (player.CheckForwardGridIsEmpty() == true)
            {
                ShortMessage.Instance.ShowMessage("��� ������� ���.");
            }
            else
            {
                ShortMessage.Instance.ShowMessage("��� �������� �� ����������� � ���� �� ����.");
                player.Character.Stats.ChangeCurrentHitPoints(item, -1);
            }
        }
    }
}
