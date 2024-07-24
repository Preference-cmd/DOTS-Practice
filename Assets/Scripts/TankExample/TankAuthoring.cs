using Unity.Entities;
using UnityEngine;

public class TankAuthoring : MonoBehaviour
{
    // From authoring to runtime
    public GameObject turret;
    public GameObject cannon;

    class Baker : Baker<TankAuthoring>
    {
        public override void Bake(TankAuthoring authoring)
        {
            // Get the entity from the baked GameObject
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            // Add Tank component to entity
            AddComponent(entity, new Tank
            {
                turret = GetEntity(authoring.turret, TransformUsageFlags.Dynamic),
                cannon = GetEntity(authoring.cannon, TransformUsageFlags.Dynamic),
            });
        }
    }
}

struct Tank : IComponentData
{
    public Entity turret;
    public Entity cannon;
}
