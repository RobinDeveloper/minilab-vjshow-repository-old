using Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class SpotlightManager : MonoBehaviour
    {
        [Header("Wall")]
        [SerializeField] private GameObject m_EmmisionWall;
        [SerializeField] private float m_MultiplyIntensity = 1;
    
        [Header("Lights")]
        [SerializeField] private GameObject[] m_Lights;

        [Header("Inputs")] 
        [SerializeField] private InputAction m_WallEmmision;
        [SerializeField] private InputAction m_WallColourWhite;
        [SerializeField] private InputAction m_WallColourRed;
        [SerializeField] private InputAction m_SetLightThemeOne;
        [SerializeField] private InputAction m_SetLightThemeTwo;
        [SerializeField] private InputAction m_SetLightThemeThree;

        [Header("ColourThemes")] 
        [SerializeField] private Color[] m_ColourAccentOne;
        [SerializeField] private Color[] m_ColourAccentTwo;
        [SerializeField] private Color[] m_ColourAccentThree;
    
    
        private AudioData m_AudioData;
        private Material m_WallMaterial;
        private Color m_CurrentWallColour = Color.white;
        private float m_Value;
        private static readonly int EmissiveColor = Shader.PropertyToID("_EmissiveColor");

        private void OnEnable()
        {
            m_WallEmmision.performed += ChangeIntensity;
            m_WallColourWhite.performed += _context => ChangeColour(_context, Color.white);
            m_WallColourRed.performed += _context => ChangeColour(_context, Color.red);
            m_SetLightThemeOne.performed += _context => ChangeThene(_context, m_ColourAccentOne);
            m_SetLightThemeTwo.performed += _context => ChangeThene(_context, m_ColourAccentTwo);
            m_SetLightThemeThree.performed += _context => ChangeThene(_context, m_ColourAccentThree);
        
        
            m_WallColourWhite.Enable();
            m_WallColourRed.Enable();
            m_SetLightThemeOne.Enable();
            m_SetLightThemeTwo.Enable();
            m_SetLightThemeThree.Enable();
            m_WallEmmision.Enable();
        }

        private void ChangeThene(InputAction.CallbackContext _context, Color[] _colourAccentOne)
        {
            for (int i = 0; i < m_Lights.Length; i++)
            {
                m_Lights[i].GetComponent<Light>().color = _colourAccentOne[i];
            }
        }

        private void ChangeColour(InputAction.CallbackContext _obj, Color _colour)
        {
            m_CurrentWallColour = _colour;
        }

        private void ChangeIntensity(InputAction.CallbackContext _obj)
        {
            m_Value = m_MultiplyIntensity * _obj.ReadValue<float>();
        }

        private void OnDisable()
        {
            m_WallEmmision.performed -= ChangeIntensity;
        
            // ReSharper disable once EventUnsubscriptionViaAnonymousDelegate
            m_WallColourWhite.performed -= _context => ChangeColour(_context, Color.white);;
            // ReSharper disable once EventUnsubscriptionViaAnonymousDelegate
            m_WallColourRed.performed -= _context => ChangeColour(_context, Color.red);
            // ReSharper disable once EventUnsubscriptionViaAnonymousDelegate
            m_SetLightThemeOne.performed -= _context => ChangeThene(_context, m_ColourAccentOne);
            // ReSharper disable once EventUnsubscriptionViaAnonymousDelegate
            m_SetLightThemeTwo.performed -= _context => ChangeThene(_context, m_ColourAccentTwo);
            // ReSharper disable once EventUnsubscriptionViaAnonymousDelegate
            m_SetLightThemeThree.performed -= _context => ChangeThene(_context, m_ColourAccentThree);
        
        
            m_WallColourWhite.Disable();
            m_WallColourRed.Disable();
            m_SetLightThemeOne.Disable();
            m_SetLightThemeTwo.Disable();
            m_SetLightThemeThree.Disable();
            m_WallEmmision.Disable();
        }

        private void Awake()
        {
            m_WallMaterial = m_EmmisionWall.GetComponent<Renderer>().material;
            m_AudioData = FindObjectOfType<AudioData>();
        }

        private void Update()
        {
            float[] bands = m_AudioData.GetFrequencybands;

            for (int i = 0; i < m_Lights.Length; i++)
            {
                m_Lights[i].SetActive(bands[i] >= 0.1f);
            }
        
            float emissiveIntensity = bands[6] * m_Value;
            Color emissiveColor = m_CurrentWallColour;
            m_WallMaterial.SetColor(EmissiveColor, emissiveColor * emissiveIntensity);
        }
    }
}
