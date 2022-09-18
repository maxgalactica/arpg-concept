using UnityEngine;
using UnityEngine.AI;

namespace Unity.AI.Navigation.Samples
{
    /// <summary>
    /// Use physics raycast hit from mouse click to set agent destination 
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class ClickToMove : MonoBehaviour
    {
        [SerializeField] Camera pCam;

        NavMeshAgent m_Agent;
        RaycastHit m_HitInfo = new RaycastHit();
    
        void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
        }
    
        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_Agent.destination = transform.position;
            }

            if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                var ray = pCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
                    m_Agent.destination = m_HitInfo.point;
            }
        }
    }
}