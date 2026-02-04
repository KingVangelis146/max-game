using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections; // Needed for IEnumerator

public class PlayerFlashOnThorns : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Tilemap tilemap;
    public TileBase thornTile;
    public Color flashColor = Color.white; // Flash pure white
    public float flashDuration = 0.2f;

    private Color originalColor;
    private bool isFlashing = false;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (spriteRenderer == null || tilemap == null || thornTile == null)
        {
            return;
        }

        Vector3 feet = transform.position + (Vector3.down * spriteRenderer.bounds.size.y / 2f);
        feet.z = tilemap.transform.position.z;

        Vector3Int cell = tilemap.WorldToCell(feet);
        TileBase tile = tilemap.GetTile(cell);

        if (tile == thornTile && !isFlashing)
        {
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        isFlashing = true;
        spriteRenderer.color = flashColor; // Turn white
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor; // Restore original color
        isFlashing = false;
    }
}