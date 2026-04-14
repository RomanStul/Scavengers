using UnityEngine;

namespace Entities.Environment.Traps_and_puzzles
{
    public class Movable : Destructible
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private Rigidbody2D rigidbody;
        //================================================================GETTER SETTER

        public Vector2 SetSavedPosition()
        {
            DestructionManager.MovableState saved = DestructionManager.instance.GetMovableState(destructibleId);
            if (saved.position.x != Mathf.Infinity)
            {
                transform.position = Convertor.Vec2ToVec3(saved.position);
                rigidbody.linearVelocity = saved.velocity;
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
            return rigidbody.linearVelocity;
        }
        //================================================================FUNCTIONALITY


        public override void Awake()
        {

        }

        public void SavePosition()
        {
            DestructionManager.instance.AddMovable(destructibleId, Convertor.Vec3ToVec2(transform.position), rigidbody.linearVelocity, this);
        }
}
}
