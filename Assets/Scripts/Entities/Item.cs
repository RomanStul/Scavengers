using System;
using ScriptableObjects.Item;
using UnityEngine;

namespace Entities
{
    public class Item : MonoBehaviour
    {
        //================================================================CLASSES
        [Serializable]
        public class ItemConstants
        {
            public float itemTravelSpeed;
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Collider2D detectTrigger, pickUpTrigger;
        [SerializeField] protected ItemSO itemData;
        [SerializeField] protected ItemConstants itemConstants;
        //================================================================GETTER SETTER
        public void SetItemData(ItemSO item)
        {
            itemData = item;
            spriteRenderer.sprite = item.image;
        }

        public ItemSO GetItemData()
        {
            return itemData;
        }
        //================================================================FUNCTIONALITY
        private bool _isCollecting = false;
        private Transform _pickUpTarget = null;

        public bool StartCollecting(Transform module)
        {
            bool result = _isCollecting;
            _isCollecting = true;
            _pickUpTarget = module;
            detectTrigger.enabled = false;
            pickUpTrigger.enabled = true;
            return result;
        }

        private void Update()
        {
            if (_pickUpTarget != null)
            {
                transform.Translate((_pickUpTarget.position-transform.position) * (itemConstants.itemTravelSpeed * Time.deltaTime), Space.World);
            }
        }
    }
}
