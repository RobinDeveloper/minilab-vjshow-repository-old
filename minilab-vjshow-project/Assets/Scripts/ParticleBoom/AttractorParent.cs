using System;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParticleBoom
{
    public class AttractorParent : MonoBehaviour
    {
        [Header("RequiredGameObjects")]
        [SerializeField] private GameObject m_Particle;
        [SerializeField] private GameObject m_Attractor;
        [SerializeField] private AudioData m_AudioData;
         
        [Header("GizmoValues")]
        [SerializeField] private Gradient m_Gradient;

        [Header("AudioValues")] 
        [SerializeField] private float m_AudioScaleMultiplier;

        [Header("SpawnValues")] 
        [SerializeField] private bool m_UseGravity;
        [SerializeField] private int[] m_AttractorPoints; 
        [SerializeField] private Vector3 m_AttractorSpawnDirection;
        [SerializeField] private Vector2 m_ParticleScaleMinMax;
        [SerializeField] private float m_GravityStrength;
        [SerializeField] private float m_MaxGravity;
        [SerializeField] private float m_RandomPositionDistance;
        [Range(0,1)] [SerializeField] private float m_SpacingAttractors;
        [Range(0,1)][SerializeField] private float m_AttractorScale;
        [Range(1,512)][SerializeField] private int m_ParticlesPerAttractor;

        private GameObject[] m_Attractors;
        private GameObject[] m_Particles;

        private float[] m_ParticleScaleSet;

        public float AudioScaleMultiplier
        {
            get => m_AudioScaleMultiplier;
            set => m_AudioScaleMultiplier = value;
        }

        public float GravityStrength
        {
            get => m_GravityStrength;
            set => m_GravityStrength = value;
        }

        public float MaxGravity
        {
            get => m_MaxGravity;
            set => m_MaxGravity = value;
        }
        
        private void Start()
        {
            m_Attractors = new GameObject[m_AttractorPoints.Length];
            m_Particles = new GameObject[m_AttractorPoints.Length * m_ParticlesPerAttractor];
            
            m_ParticleScaleSet = new float[m_AttractorPoints.Length * m_ParticlesPerAttractor];

            int particleCount = 0;
            
            for (int i = 0; i < m_AttractorPoints.Length; i++)
            {
                GameObject attractor = Instantiate(m_Attractor) as GameObject;
                m_Attractors[i] = attractor;
                Debug.Log("attractor Counts: " + i.ToString());

                Vector3 position = transform.position;
                attractor.transform.position = new Vector3(
                    position.x + (m_SpacingAttractors * i * m_AttractorSpawnDirection.x),
                    position.y + (m_SpacingAttractors * i * m_AttractorSpawnDirection.y),
                    position.z + (m_SpacingAttractors * i * m_AttractorSpawnDirection.z));

                attractor.transform.parent = transform.parent;
                attractor.transform.localScale = new Vector3(m_AttractorScale, m_AttractorScale, m_AttractorScale);

                for (int j = 0; j < m_ParticlesPerAttractor; j++)
                {
                    GameObject particle = Instantiate(m_Particle) as GameObject;
                    m_Particles[particleCount] = particle;
                    ParticleAttractor particleAttractor = particle.GetComponent<ParticleAttractor>();
                    particleAttractor.GravityObject = m_Attractors[i].transform;
                    particleAttractor.GravityValue = m_GravityStrength;
                    particleAttractor.MaxGravityValue = m_MaxGravity;
                    
                    particle.GetComponent<Rigidbody>().useGravity = m_UseGravity;

                    particle.transform.position = new Vector3(
                        m_Attractors[i].transform.position.x + Random.Range(-m_RandomPositionDistance, m_RandomPositionDistance),
                        m_Attractors[i].transform.position.y + Random.Range(-m_RandomPositionDistance, m_RandomPositionDistance),
                        m_Attractors[i].transform.position.z + Random.Range(-m_RandomPositionDistance, m_RandomPositionDistance));

                    float randomScale = Random.Range(m_ParticleScaleMinMax.x, m_ParticleScaleMinMax.y);
                    m_ParticleScaleSet[particleCount] = randomScale;
                    particle.transform.localScale = new Vector3(m_ParticleScaleSet[particleCount], m_ParticleScaleSet[particleCount], m_ParticleScaleSet[particleCount]);

                    particle.transform.parent = transform.parent;
                    
                    particleCount++;
                }
            }
        }

        private void Update()
        {
            int particleCount = 0;
            for (int i = 0; i < m_AttractorPoints.Length; i++)
            {
                for (int j = 0; j < m_ParticlesPerAttractor; j++)
                {
                    m_Particles[particleCount].transform.localScale = new Vector3(m_ParticleScaleSet[particleCount] + m_AudioData.GetFrequencybands[m_AttractorPoints[i]] * m_AudioScaleMultiplier,
                                                                                    m_ParticleScaleSet[particleCount] + m_AudioData.GetFrequencybands[m_AttractorPoints[i]] * m_AudioScaleMultiplier,
                                                                                    m_ParticleScaleSet[particleCount] + m_AudioData.GetFrequencybands[m_AttractorPoints[i]] * m_AudioScaleMultiplier);
                    particleCount++;
                }
            }
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < m_AttractorPoints.Length; i++)
            {
                float evaluateStep = 0.125f;
                Color color = m_Gradient.Evaluate(Mathf.Clamp(evaluateStep * m_AttractorPoints[i], 0,7));
                Gizmos.color = color;

                Vector3 position = transform.position;
                Vector3 pos = new Vector3(
                    position.x + (m_SpacingAttractors * i * m_AttractorSpawnDirection.x),
                    position.y + (m_SpacingAttractors * i * m_AttractorSpawnDirection.y),
                    position.z + (m_SpacingAttractors * i * m_AttractorSpawnDirection.z));
                
                Gizmos.DrawSphere(pos, m_AttractorScale);
            }
        }
    }
}
