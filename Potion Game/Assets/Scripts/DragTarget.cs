using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragTarget : MonoBehaviour
{
    public LayerMask m_DragLayers;

    [Range(0.0f, 100.0f)]
    public float m_Damping = 1.0f;

    [Range(0.0f, 100.0f)]
    public float m_Frequency = 5.0f;

    public bool m_DrawDragLine = true;
    public Color m_Color = Color.cyan;

    private TargetJoint2D m_TargetJoint;

    private Rigidbody2D _rb;

    private void Awake()
    {
       _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Calculate the world position for the mouse.
        var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 1.0f;

        if (Input.GetMouseButtonDown(0))
        {
            // Fetch the first collider.
            // NOTE: We could do this for multiple colliders.
            var collider = Physics2D.OverlapPoint(worldPos, m_DragLayers);
            if (!collider)
                return;

            // Fetch the collider body.
            var body = collider.attachedRigidbody;
            if (!body)
                return;

            // Add a target joint to the Rigidbody2D GameObject.
            if(body.GetComponent<TargetJoint2D>() == null)
            {
                this.m_TargetJoint = body.gameObject.AddComponent<TargetJoint2D>();
                Debug.Log(m_TargetJoint.name);
            }
            else
            {
                return;
            }

            if (m_TargetJoint != null)
            {
                m_TargetJoint.dampingRatio = m_Damping;
                m_TargetJoint.frequency = m_Frequency;
                _rb.gravityScale = 0;
                // Attach the anchor to the local-point where we clicked. 
                m_TargetJoint.anchor = this.m_TargetJoint.transform.InverseTransformPoint(worldPos);
            }
            
        }
        
        else if (Input.GetMouseButtonUp(0))
        {
            if(gameObject.GetComponent<TargetJoint2D>() != null)
            {
                Destroy(this.gameObject.GetComponent<TargetJoint2D>());
            }
            _rb.gravityScale = 1;
            Destroy(m_TargetJoint);
            m_TargetJoint = null;
            return;
        }

        // Update the joint target.
        if (m_TargetJoint)
        {
            m_TargetJoint.target = worldPos;

            // Draw the line between the target and the joint anchor.
            if (m_DrawDragLine)
                Debug.DrawLine(m_TargetJoint.transform.TransformPoint(m_TargetJoint.anchor), worldPos, m_Color);
        }

        if(this.transform.position.y < -10 || this.transform.position.y > 100)
        {
            Destroy(this.gameObject);
        }
    }
}
