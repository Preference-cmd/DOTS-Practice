using UnityEngine;
using Unity.Entities;
public class SpawnAuthoring : MonoBehaviour
{
    public GameObject cubePrefab;

    class Baker : Baker<SpawnAuthoring>
    {
        public override void Bake(SpawnAuthoring authoring)
        {
            // Because we don't need any transform data, we can use the none flag
            var entity = GetEntity(authoring, TransformUsageFlags.None);
            var spawner =  new CubeSpawner
            {
                cubePrefab = GetEntity(authoring.cubePrefab, TransformUsageFlags.Dynamic)
            };
            AddComponent(entity, spawner);
        }
    }
}

struct CubeSpawner : IComponentData
{
    public Entity cubePrefab;
}