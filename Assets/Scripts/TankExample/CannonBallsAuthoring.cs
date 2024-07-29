using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;


public class CannonBallsAuthoring : MonoBehaviour
{
    class Baker : Baker<CannonBallsAuthoring>
    {
        public override void Bake(CannonBallsAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
            
            // Add CannonBall component to the entity
            // the velocity of CannonBall will be initialized as zero by default
            AddComponent<CannonBall>(entity);
            // Add material component to the entity
            AddComponent<URPMaterialPropertyBaseColor>(entity);
        }
    }
}

public struct CannonBall : IComponentData
{
    public float3 velocity;
}