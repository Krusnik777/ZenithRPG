using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    [RequireComponent(typeof(Player))]
    public class PlayerInputController : MonoBehaviour, IDependency<ControlsManager>
    {
        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        private Controls _controls;
        private Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void OnEnable()
        {
            if (_controls == null) _controls = m_controlsManager.Controls;

            _controls.Gameplay.Enable();

            _controls.Gameplay.RotateLeft.performed += OnRotateLeft;
            _controls.Gameplay.RotateRight.performed += OnRotateRight;
            _controls.Gameplay.RotateBack.performed += OnRotateBack;

            _controls.Gameplay.Jump.performed += OnJump;

            _controls.Gameplay.Use.performed += OnUse;

            _controls.Gameplay.Attack.performed += OnAttack;

            _controls.Gameplay.Block.started += OnBlockStarted;
            _controls.Gameplay.Block.performed += OnBlockHolded;
            _controls.Gameplay.Block.canceled += OnBlockCanceled;

            _controls.Gameplay.UseMagic.performed += OnUseMagic;

            _controls.Gameplay.Rest.performed += OnRest;
            _controls.Gameplay.Inventory.performed += OnCheckInventory;
            _controls.Gameplay.UseItem.performed += OnUseItem;
            _controls.Gameplay.LeftItem.performed += OnLeftItem;
            _controls.Gameplay.MiddleItem.performed += OnMiddleItem;
            _controls.Gameplay.RightItem.performed += OnRightItem;

            _controls.Gameplay.Pause.performed += OnPause;
        }

        private void OnDisable()
        {
            _controls.Gameplay.RotateLeft.performed -= OnRotateLeft;
            _controls.Gameplay.RotateRight.performed -= OnRotateRight;
            _controls.Gameplay.RotateBack.performed -= OnRotateBack;

            _controls.Gameplay.Jump.performed -= OnJump;

            _controls.Gameplay.Use.performed -= OnUse;

            _controls.Gameplay.Attack.performed -= OnAttack;

            _controls.Gameplay.Block.started -= OnBlockStarted;
            _controls.Gameplay.Block.performed -= OnBlockHolded;
            _controls.Gameplay.Block.canceled -= OnBlockCanceled;

            _controls.Gameplay.UseMagic.performed -= OnUseMagic;

            _controls.Gameplay.Rest.performed -= OnRest;
            _controls.Gameplay.Inventory.performed -= OnCheckInventory;
            _controls.Gameplay.UseItem.performed -= OnUseItem;
            _controls.Gameplay.LeftItem.performed -= OnLeftItem;
            _controls.Gameplay.MiddleItem.performed -= OnMiddleItem;
            _controls.Gameplay.RightItem.performed -= OnRightItem;

            _controls.Gameplay.Pause.performed -= OnPause;

            _controls.Gameplay.Disable();
        }

        private void Update()
        {
            OnMovement();
        }

        private void OnMovement()
        {
            if (!player.IsGrounded || player.State != Player.PlayerState.Active) return;

            Vector2 inputDirection = _controls.Gameplay.Movement.ReadValue<Vector2>();

            if (inputDirection == Vector2.zero) return;

            player.Move(inputDirection);
        }

        private void OnRotateLeft(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.Turn(-90);
        }

        private void OnRotateRight(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.Turn(90);
        }

        private void OnRotateBack(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.Turn(180);
        }

        private void OnJump(InputAction.CallbackContext obj)
        {
            if (!player.IsGrounded || player.State != Player.PlayerState.Active) return;

            player.Jump();
        }

        private void OnUse(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.Inspect();
        }

        private void OnAttack(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.Attack();
        }

        private void OnBlockStarted(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.Block("BlockStart");
        }

        private void OnBlockHolded(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.Block("BlockHold");
        }

        private void OnBlockCanceled(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.Block("BlockEnd");
        }

        private void OnUseMagic(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.UseMagic();
        }

        private void OnRest(InputAction.CallbackContext obj)
        {
            player.ChangeRestState();
        }

        private void OnCheckInventory(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active || !player.ActionsIsAvailable) return;

            m_controlsManager.SetPlayerControlsActive(false);

            m_controlsManager.SetInventoryControlsActive(true);
        }

        private void OnUseItem(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.UseActiveItem();
        }

        private void OnLeftItem(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.ChooseLeftActiveItem();
        }

        private void OnMiddleItem(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.ChooseMiddleActiveItem();
        }

        private void OnRightItem(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active) return;

            player.ChooseRightActiveItem();
        }

        private void OnPause(InputAction.CallbackContext obj)
        {
            if (player.State != Player.PlayerState.Active || !player.ActionsIsAvailable) return;

            PauseMenu.Instance.ShowPauseMenu();
        }
    }
}
