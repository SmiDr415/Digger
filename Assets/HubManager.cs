using UnityEngine;

namespace MultiTool
{
	public class HubManager : MonoBehaviour
	{
        private bool _playerInTrigger = false;
        private PlayerController _playerController;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                _playerInTrigger = true;
                _playerController = collision.GetComponent<PlayerController>();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                _playerInTrigger = false;
                _playerController = null;
            }
        }

        private void FixedUpdate()
        {
            if(_playerInTrigger && _playerController != null)
            {
                _playerController.Form.Repair();
            }
        }
    }

}