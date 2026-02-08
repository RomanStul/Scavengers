using System.Reflection;
using Menu;
using Player.Module;
using story;
using UnityEngine;
using Module = Player.Module.Module;

namespace Entities.Interactions
{
    public class EndOfDayDoor : MonoBehaviour
    {
        public void StartEndOfDayAnimation(Module moduleRef)
        {
            ((ModuleAnimations)moduleRef.GetScript<ModuleAnimations>(Module.ScriptNames.AnimationFunctionsScript)).StartEndOfDayAnimation();
        }
        
    }
}
