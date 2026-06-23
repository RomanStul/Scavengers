using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Environment.Traps_and_puzzles
{
    public class Movable : SaveIDDistibutor
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [FormerlySerializedAs("rigidbody")] [SerializeField] private Rigidbody2D rb;
        //================================================================GETTER SETTER

        public Vector2 SetSavedPosition()
        {
            DestructionManager.MovableState saved = DestructionManager.instance.GetMovableState(Id);
            if (saved.position.x != Mathf.Infinity)
            {
                transform.position = Convertor.Vec2ToVec3(saved.position) - Vector3.forward ;
                rb.linearVelocity = saved.velocity;
                return saved.velocity;
            }
            return Vector2.zero;
        }

        public Vector2 GetPosition()
        {
            return Convertor.Vec3ToVec2(transform.position);
        }

        public Vector2 GetVelocity()
        {
            return rb.linearVelocity;
        }
        //================================================================FUNCTIONALITY

        public void SavePosition()
        {
            DestructionManager.instance.AddMovable(Id, Convertor.Vec3ToVec2(transform.position), rb.linearVelocity, this);
        }
}
}
