using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraFollow : MonoBehaviour
    {
        public static CameraFollow instance;

        public Entity BallEntity;

        public float3 offset;

        private EntityManager _entityManager;


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }


            instance = this;
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void LateUpdate()
        {
            if (BallEntity == null ) {return;}

            Translation translation = _entityManager.GetComponentData<Translation>(BallEntity);
            transform.position = translation.Value + offset;
        }
    }
}