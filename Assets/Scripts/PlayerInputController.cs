using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    [RequireComponent(typeof(Player))]
    public class PlayerInputController : MonoBehaviour
    {
        private Controls _controls;
        private Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
            _controls = new Controls();
        }

        private void OnEnable()
        {
            _controls.Gameplay.Enable();

            _controls.Gameplay.RotateLeft.performed += OnRotateLeft;
            _controls.Gameplay.RotateRight.performed += OnRotateRight;
            _controls.Gameplay.RotateBack.performed += OnRotateBack;
            _controls.Gameplay.Jump.performed += OnJump;

            _controls.Gameplay.Use.performed += OnUse;
            _controls.Gameplay.Attack.performed += OnAttack;
            _controls.Gameplay.Block.performed += OnBlock;
            _controls.Gameplay.Rest.performed += OnRest;
            _controls.Gameplay.Inventory.performed += OnCheckInventory;
            _controls.Gameplay.UseItem.performed += OnUseItem;
            _controls.Gameplay.NextItem.performed += ChangeItem;
            _controls.Gameplay.PreviousItem.performed += ChangeItem;
        }

        private void OnDisable()
        {
            _controls.Gameplay.RotateLeft.performed -= OnRotateLeft;
            _controls.Gameplay.RotateRight.performed -= OnRotateRight;
            _controls.Gameplay.RotateBack.performed -= OnRotateBack;
            _controls.Gameplay.Jump.performed -= OnJump;

            _controls.Gameplay.Use.performed -= OnUse;
            _controls.Gameplay.Attack.performed -= OnAttack;
            _controls.Gameplay.Block.performed -= OnBlock;
            _controls.Gameplay.Rest.performed -= OnRest;
            _controls.Gameplay.Inventory.performed -= OnCheckInventory;
            _controls.Gameplay.UseItem.performed -= OnUseItem;
            _controls.Gameplay.NextItem.performed -= ChangeItem;
            _controls.Gameplay.PreviousItem.performed -= ChangeItem;

            _controls.Disable();
        }

        private void Update()
        {
            OnMovement();
        }

        private void OnMovement()
        {
            Vector2 directionV2 = _controls.Gameplay.Movement.ReadValue<Vector2>();

            if (directionV2 == Vector2.zero) return;

            Vector3 direction = new Vector3(0, 0, 0);

            if (directionV2.x != 0)
            {
                direction = player.transform.right * Mathf.Sign(directionV2.x);
            }

            if (directionV2.y != 0)
            {
                direction = player.transform.forward * Mathf.Sign(directionV2.y);
            }

            player.Move(direction);
        }

        private void OnRotateLeft(InputAction.CallbackContext obj)
        {
            player.Turn(-90);
        }

        private void OnRotateRight(InputAction.CallbackContext obj)
        {
            player.Turn(90);
        }

        private void OnRotateBack(InputAction.CallbackContext obj)
        {
            player.Turn(180);
        }

        private void OnJump(InputAction.CallbackContext obj)
        {
            player.Jump();
        }

        private void OnUse(InputAction.CallbackContext obj)
        {
            player.Inspect();
        }

        private void OnAttack(InputAction.CallbackContext obj)
        {
            player.Attack();
        }

        private void OnBlock(InputAction.CallbackContext obj)
        {
            player.Block();
        }

        private void OnRest(InputAction.CallbackContext obj)
        {
            player.Rest();
        }

        private void OnCheckInventory(InputAction.CallbackContext obj)
        {
            player.CheckInventory();
        }

        private void OnUseItem(InputAction.CallbackContext obj)
        {
            player.UseActiveItem();
        }

        private void ChangeItem(InputAction.CallbackContext obj)
        {
            player.ChangeActiveItem();
        }
    }
}
