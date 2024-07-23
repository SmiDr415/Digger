using UnityEngine;

namespace MultiTool
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GroundItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private DropItem _dropItem;

        public void Start()
        {
            _dropItem = GameManager.Instance.DropItemDatabase.GetDropItemByNameEN(name);
            _spriteRenderer.sprite = _dropItem.Sprite;
            var collider = _spriteRenderer.GetComponent<BoxCollider2D>();
            collider.size = _spriteRenderer.sprite.bounds.size;

            var rb = GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * Random.Range(1, 10));
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
                if(InventoryManager.Instance.AddItem(_dropItem.NameItemEN, 1))
                {
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("НЕ собрал");
                }

            }
        }
    }
}
