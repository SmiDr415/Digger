using UnityEngine;

namespace MultiTool
{
    public class InteractiveObject : MonoBehaviour
    {
        [SerializeField] private GameObject _helpInfo;

        //Включение интерактивного окна про клюку по объекту
        //private void OnMouseDown()
        //{
        //    Vector3 topColliderPosition = GetTopColliderPosition();
        //    Vector3 screenPosition = Camera.main.WorldToScreenPoint(topColliderPosition);
        //    GUIWindowManager.Instance.ShowWindowAbovePosition(screenPosition);
        //}

        public Vector3 GetTopColliderPosition()
        {
            if(TryGetComponent<Collider2D>(out var collider))
            {
                Bounds bounds = collider.bounds;
                var pos = Camera.main.WorldToScreenPoint(new Vector3(bounds.center.x, bounds.max.y, 0));
                return pos;
            }
            return transform.position;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Interactible"))
            {
                if(_helpInfo != null)
                {
                    _helpInfo.SetActive(true);
                }

                PlayerController.Instance.ActivateReadyInteractible(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.CompareTag("Interactible"))
            {
                if(_helpInfo != null)
                {
                    _helpInfo.SetActive(false);
                }

                PlayerController.Instance.ActivateReadyInteractible(null);

            }
        }

        private void OnMouseDown()
        {
            PlayerController.Instance.ReadyInteractible();
        }
    }

}