using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

public class PickUpSystem : JobComponentSystem
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
            SpeedEntity = GetComponentDataFromEntity<SpeedData>(),
            enityToDelete = GetComponentDataFromEntity<DeleteTag>(),
            Buffer = _bufferSystem.CreateCommandBuffer(),
        };

         var asghar = triggerJob.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps); 
         asghar.Complete();
         return asghar;
    }
    
    
    [BurstCompatible]
    private struct TriggerJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<SpeedData> SpeedEntity;
        [ReadOnly] public ComponentDataFromEntity<DeleteTag> enityToDelete;
        public EntityCommandBuffer Buffer;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            CheckEntities(triggerEvent.EntityA, triggerEvent.EntityB);
            CheckEntities(triggerEvent.EntityB, triggerEvent.EntityA);
        }
        

        private void CheckEntities(Entity entityA, Entity entityB)
        {
            if (SpeedEntity.HasComponent(entityA))
            {
                if (enityToDelete.HasComponent(entityB)) return;
                {
                    Buffer.AddComponent(entityB, new DeleteTag());
                }
            }
        }
    }
}
