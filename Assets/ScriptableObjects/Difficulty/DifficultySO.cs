using UnityEngine;

namespace ScriptableObjects.Difficulty
{
    [CreateAssetMenu(fileName = "DifficultySO", menuName = "ScriptableObjects/DifficultySO")]
    public class DifficultySO : ScriptableObject
    {
        public float damageTakenMultiplier;
        public float dayLengthMultiplier;
        public float frictionMultiplier;
    }
}
