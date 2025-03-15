using System;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Player.Module.Drill
{
    public class DrillController : ModuleBaseScript
    {
        //================================================================
        [SerializeField] protected Player.Module.Module.DrillConstants drillConstants;
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
            drillGameObject.transform.rotation = Quaternion.Lerp( drillGameObject.transform.rotation, rotation, Time.deltaTime * drillConstants.RotationSpeed);
            
        }

        public void UseDrill(bool isDrilling)
        {
            drill.Use(isDrilling);
        }
    }
}
