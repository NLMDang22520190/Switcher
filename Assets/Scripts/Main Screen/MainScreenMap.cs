using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenMap : MonoBehaviour
{
    [SerializeField] private Material skyMaterial;

    private Vector2 currentOffset;
    private Vector2 SunsetOffset = new Vector2(0.2f, 0f);
    private Vector2 NightOffset = new Vector2(0.4f, 0f);


    private void Start()
    {
        // reset material offset
        skyMaterial.mainTextureOffset = Vector2.zero;   
        currentOffset = Vector2.zero;
    }

    public void UpdateToSunset()
    {
        // linearly interpolate the material offset to the sunset position
        StartCoroutine(UpdateMaterialOffset(SunsetOffset, 1f));
    }

    public void UpdateToNight()
    {
        // linearly interpolate the material offset to the night position
        StartCoroutine(UpdateMaterialOffset(NightOffset, 1f));
    }

    public void UpdateToDay()
    {
        StartCoroutine(UpdateMaterialOffset(Vector2.zero, 1f));
    }

    private IEnumerator UpdateMaterialOffset(Vector2 targetOffset, float duration)
    {
        Vector2 startOffset = skyMaterial.mainTextureOffset;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            skyMaterial.mainTextureOffset = Vector2.Lerp(startOffset, targetOffset, elapsedTime / duration);
            yield return null;
        }
    }
}