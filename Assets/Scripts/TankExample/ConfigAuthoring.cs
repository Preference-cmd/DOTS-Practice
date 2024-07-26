using UnityEngine;
using Unity.Entities;

public class ConfigAuthoring : MonoBehaviour
{
    public GameObject tankPrefab;
    public GameObject cannonBallPrefab; 
    public int TankCount;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            // Config does not need transform, so we use TransformUsageFlags.None
            var entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new Config{
                tankPrefab = GetEntity(authoring.tankPrefab, TransformUsageFlags.Dynamic),
                cannonBallPrefab = GetEntity(authoring.cannonBallPrefab, TransformUsageFlags.Dynamic),
                tankCount = authoring.TankCount,
            });
        }
    }
}

struct Config : IComponentData
{
    public Entity tankPrefab;
    public Entity cannonBallPrefab;
    public int tankCount;
}
