using UnityEngine;

namespace ParticleBoom
{
    [RequireComponent(typeof(Rigidbody))]
    public class ParticleAttractor : MonoBehaviour
    {
        [Header("RequiredObjects")]
        [SerializeField] private Transform m_GravityObject;
        
        [Header("GravityValues")]
        [SerializeField] private float m_GravityValue;

        [SerializeField] private float m_MaxGravityValue;
    
        private Rigidbody m_Rigidbody;

        public Transform GravityObject
        {
            get => m_GravityObject;
            set => m_GravityObject = value;
        }
        public float GravityValue
        {
            get => m_GravityValue;
            set => m_GravityValue = value;
        }
        public float MaxGravityValue
        {
            get => m_MaxGravityValue;
            set => m_MaxGravityValue = value;
        }

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (m_GravityObject == null) return;
            Vector3 direction = m_GravityObject.position - transform.position;
            m_Rigidbody.AddForce(m_GravityValue * direction);

            if (m_Rigidbody.velocity.magnitude > m_MaxGravityValue)
                m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * m_MaxGravityValue;
        }
    }
}
