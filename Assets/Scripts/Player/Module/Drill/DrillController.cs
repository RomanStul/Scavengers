using System;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Module.Drill
{
    public class DrillController : ModuleBaseScript
    {
        [Serializable]
        public class DrillControllerConstants
        {
            public float RotationSpeed;
        }
        //================================================================
        [SerializeField] protected DrillControllerConstants drillControllerConstants;
        [SerializeField] protected GameObject drillGameObject;
        [SerializeField] protected Player.Module.Drill.Drill drill;
        //================================================================
        public Vector2 DrillTargetPosition { get; set; } = new Vector2(0,0);
        //================================================================

        private void Update()
        {
            //TODO make follow body of module
            Vector3 targetPosition = Convertor.Vec2ToVec3(DrillTargetPosition - new Vector2(Screen.width / 2, Screen.height / 2));
            Vector3 relative = targetPosition;
            Quaternion rotation = Quaternion.LookRotation(relative, Vector3.forward);
            drillGameObject.transform.rotation = Quaternion.Lerp( drillGameObject.transform.rotation, rotation, Time.deltaTime * drillControllerConstants.RotationSpeed);
            
        }

        public void UseDrill(bool isDrilling)
        {
            drill.Use(isDrilling);
        }
    }
}
