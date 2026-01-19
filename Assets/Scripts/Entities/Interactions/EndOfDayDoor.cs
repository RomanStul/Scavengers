using System.Reflection;
using Player.Module;
using UnityEngine;
using Module = Player.Module.Module;

namespace Entities.Interactions
{
    public class EndOfDayDoor : MonoBehaviour
    {
        public void StartEndOfDayAnimation(Module moduleRef)
        {
            if (((Storage)moduleRef.GetScript<Storage>(Module.ScriptNames.StorageScript)).Currency >= 0)
                moduleRef.moduleAnimator.SetTrigger("endOfDay");
            else
                Debug.Log("not enough money");
        }
        
    }
}
