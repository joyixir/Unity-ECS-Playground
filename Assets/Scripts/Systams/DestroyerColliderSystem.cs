using DefaultNamespace.DataComponents;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace DefaultNamespace.Systams
{
    public class DestroyerColliderSystem : JobComponentSystem
    {
        private BeginInitializationEntityCommandBufferSystem _bufferSystem;
        private BuildPhysicsWorld _buildPhysicsWorld;
        private StepPhysicsWorld _stepPhysicsWorld;

        protected override void OnCreate()
        {
            _bufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            TriggerJob triggerJob = new TriggerJob
            {
                DestroyerEntity = GetComponentDataFromEntity<DestroyerTag>(),
                Buffer = _bufferSystem.CreateCommandBuffer(),
            };

            var asghar = triggerJob.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld,
                inputDeps);
            asghar.Complete();
            return asghar;
        }


        [BurstCompatible]
        private struct TriggerJob : ITriggerEventsJob
        {
            public ComponentDataFromEntity<DestroyerTag> DestroyerEntity;
            public EntityCommandBuffer Buffer;

            public void Execute(TriggerEvent triggerEvent)
            {
                CheckEntities(triggerEvent.EntityB, triggerEvent.EntityA);
            }


            private void CheckEntities(Entity entityA, Entity entityB)
            {
                if (DestroyerEntity.HasComponent(entityA))
                {
                    Buffer.DestroyEntity(entityB);
                }
            }
        }
    }
}