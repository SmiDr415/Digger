using UnityEngine;

namespace Digger
{
    public class InteractiveSprite : MonoBehaviour
    {
        private void OnMouseDown()
        {
            Vector3 topColliderPosition = GetTopColliderPosition();
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(topColliderPosition);
            GUIWindowManager.Instance.ShowWindowAbovePosition(screenPosition);
        }

        private Vector3 GetTopColliderPosition()
        {
            Collider2D collider = GetComponent<Collider2D>();
            if(collider != null)
            {
                Bounds bounds = collider.bounds;
                return new Vector3(bounds.center.x, bounds.max.y, 0);
            }
            return transform.position;
        }
    }

}