using UnityEngine;

public class Hockey : MonoBehaviour
{
    public int score;

    private Material originalMaterial;
    private Material glowingMaterial;
    private MeshRenderer meshRenderer;
    private AudioSource audioSource;
    public void IncreaseScore()
    {
        score++;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.material;
        }
    }

    public void SetGlowMaterial(Material material)
    {
        glowingMaterial = material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Puck"))
        {
            audioSource.Play();

            meshRenderer.material = glowingMaterial;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        meshRenderer.material = originalMaterial;
    }
}
