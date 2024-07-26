using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


public class CannonBallsAuthoring : MonoBehaviour
{

}

struct CannonBalls : IComponentData
{
    public Entity ball;
}