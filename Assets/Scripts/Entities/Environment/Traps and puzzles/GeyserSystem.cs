using System;
using UnityEngine;

namespace Entities.Environment.Traps_and_puzzles
{
    public class GeyserSystem : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES

        [SerializeField] private PermanentGeyser[] attachedGeysers;

        [SerializeField] private float totalPressure;
        
        //================================================================GETTER SETTER
        

        //================================================================FUNCTIONALITY
        private int oppenedGeysers = 0;
        
        
        private void Awake()
        {
            foreach (PermanentGeyser geyser in attachedGeysers)
            {
                if (!geyser.Blocked)
                {
                    oppenedGeysers++;
                }
            }

            foreach (PermanentGeyser geyser in attachedGeysers)
            {
                geyser.SetReferences();
                if (!geyser.Blocked)
                {
                    geyser.SetParticleSpeed(totalPressure/oppenedGeysers);
                    geyser.SetParticleEmissionRate((int)((totalPressure*10) / oppenedGeysers));
                    geyser.StartEruption();
                }
            }
        }

        public void OpenedGeyser(PermanentGeyser geyser)
        {
            oppenedGeysers++;

            foreach (PermanentGeyser g in attachedGeysers)
            {
                
                g.SetParticleSpeed(totalPressure/oppenedGeysers);
                g.SetParticleEmissionRate((int)((totalPressure*20) / oppenedGeysers));
            }
            
            geyser.StartEruption();

        }
    }
}
