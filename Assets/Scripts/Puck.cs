using UnityEngine;

public class Puck : MonoBehaviour
{
    private Material originalMaterial;    
    private Material glowingMaterial;
    private MeshRenderer meshRenderer;

    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.material;
        }
        glowingMaterial = GameManager.instance.glowingPuckMaterialListSO.materials[GameManager.instance.puckMaterialID];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Table"))
        {
            audioSource.Play();
        }

        if(collision.gameObject.CompareTag("Table") || collision.gameObject.CompareTag("Hockey"))
        {
            meshRenderer.material = glowingMaterial;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        meshRenderer.material = originalMaterial;
    }
}
