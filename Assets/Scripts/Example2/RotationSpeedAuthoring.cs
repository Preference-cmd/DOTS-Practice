using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct RotationSpeed : IComponentData
{
    // runtime data 
    public float radiansPerSecond;
}

public class RotationSpeedAuthoring : MonoBehaviour
{
    // authoring data that will be spelified in the subscene
    public float degreesPerSecond = 360f;
}

class RotationSpeedAuthoringBaker : Baker<RotationSpeedAuthoring>
{
    public override void Bake(RotationSpeedAuthoring authoring)
    {
        // convert the authoring data into runtime data
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        var rotationSpeed = new RotationSpeed
        { 
            radiansPerSecond = math.radians(authoring.degreesPerSecond) 
        };

        AddComponent(entity, rotationSpeed);
    }
}
