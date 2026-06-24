using System;
using System.Security.Cryptography;
using Entities.Environment;
using Random = UnityEngine.Random;

namespace Player.Module.Tools
{
    public class GravityAnchor : ModuleTool
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private int id;
        
        public override void Use(Module m)
        {
            DestructionManager.instance.AddPermanentObject(gameObject, DestructionManager.PermanentObject.permanentObjectType.GravityAnchor);
        }
    }
}
