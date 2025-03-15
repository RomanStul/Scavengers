using System;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Player.Module.Drill
{
    public class DrillController : ModuleBaseScript
    {
        //================================================================
        [SerializeField] protected GameObject drillGameObject;
        [SerializeField] protected Player.Module.Drill.Drill drill;
        //================================================================
        public Vector2 DrillPosition { get; set; } = new Vector2(0,0);
        //================================================================

        private void Update()
        {
            //TODO make follow body of module
            //TODO make change animated
            drillGameObject.transform.LookAt(Convertor.Vec2ToVec3(DrillPosition- new Vector2(Screen.width/2, Screen.height/2)) + drillGameObject.transform.position);
        }

        public void UseDrill(bool isDrilling)
        {
            drill.Use(isDrilling);
        }
    }
}
