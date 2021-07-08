using DefaultNamespace.DataComponents;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace DefaultNamespace.Systams
{
    public class ChangeMeshColorSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var random = new Random();
            random.InitState();
            Entities
                .WithoutBurst()
                .WithAll<ChangeCubeColorData>()
                .ForEach((RenderMesh mesh) =>
                
            {
                mesh.material.color = new Color(random.NextFloat(0f,1f),random.NextFloat(0f,1f),random.NextFloat(0f,1f),1);
            }).Run();
            return default;
        }
    }
}