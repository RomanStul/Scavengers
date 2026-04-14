using System;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI
{
    public class Bar : MonoBehaviour
    {
        //================================================================CLASSES
    
        //================================================================EDITOR VARIABLES
        [SerializeField] protected float value, maxValue;
        [SerializeField] private Image fillImage;
        [SerializeField] private Texture color, mask, background;
        [SerializeField] protected float maxFill, minFill;

        [SerializeField] protected RectTransform.Axis axis = RectTransform.Axis.Vertical;
        //================================================================GETTER SETTER
        public virtual void SetValue(float barValue)
        {
            value = barValue;
            UpdateFill();
        }

        public virtual void SetMaxValue(float barValue)
        {
            maxValue = barValue;
        }
        //================================================================FUNCTIONALITY
        
        private Material instancedMaterial;
        
        protected virtual void Awake()
        {
            instancedMaterial = Instantiate(fillImage.material);
            fillImage.material = instancedMaterial;
            fillImage.material.SetTexture("_mask", mask);
            fillImage.material.SetTexture("_overlay", background);
            fillImage.material.SetTexture("_colorTexture", color);
            if (maxValue != 0)
            {
                UpdateFill();
            }
        }

        protected virtual void UpdateFill()
        {
            float offset = minFill - ((minFill - maxFill) * (value/maxValue));
            if (instancedMaterial != null)
            {
                fillImage.material = instancedMaterial;
                if (axis == RectTransform.Axis.Horizontal)
                {
                    instancedMaterial.SetVector("_offset", new Vector4(offset, 0));
                }
                else
                {
                    instancedMaterial.SetVector("_offset", new Vector4(0, offset));
                }
            }

        }
    }
}
