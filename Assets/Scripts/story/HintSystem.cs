using System;
using Milestones;
using Player.Module;
using Player.Module.Upgrades;
using Player.UI;
using ScriptableObjects.Upgrade;
using UnityEngine;

namespace story
{
        public class HintSystem : ModuleBaseScript
        {
                //================================================================CLASSES
                public enum HintedActions
                {
                        BoulderDestruction,
                        MushroomBoulderPuzzle,
                        GeyserPuzzle,
                        MushroomDestruction,
                        MovablePuzzle,
                }
                
                [Serializable]
                public class Hint
                {
                        public int Remaining;
                        public ModuleUpgrades.Ups neededUpgrade;
                }
                //================================================================EDITOR VARIABLES
                
                [SerializeField] private Hint[] hintsCountDowns;
                //================================================================GETTER SETTER

                public int[] GerHintsRemaining()
                {
                        int[] rem = new int[hintsCountDowns.Length];
                        for (int i = 0; i < hintsCountDowns.Length; i++)
                        {
                                rem[i] = hintsCountDowns[i].Remaining;
                        }
                        return rem;
                }


                public void SetHintRemaining(int[] rem)
                {
                        for (int i = 0; i < Mathf.Min(hintsCountDowns.Length, rem.Length); i++)
                        {
                                hintsCountDowns[i].Remaining = rem[i];
                        }
                }
                //================================================================FUNCTIONALITY
                
                public void DecrementHint(int action)
                {
                        if (!ModuleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).GetUpgrades()[(int)hintsCountDowns[action].neededUpgrade]) return;
                        
                        hintsCountDowns[action].Remaining--;
                        if (hintsCountDowns[action].Remaining == 0)
                        {
                                UIController cont = ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript);
                                cont.BufferTransmission((int)action + "_Hint");
                        }
                }

                public void DeactivateHint(int action)
                {
                        hintsCountDowns[action].Remaining = -1;
                }
        }
}
