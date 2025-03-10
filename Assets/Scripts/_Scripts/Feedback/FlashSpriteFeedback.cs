using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSpriteFeedback : Feedback
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private float flashTime = 0.1f;

    [SerializeField]
    private Material flashMaterial = null;

    private Shader originalMaterialShader;
    private void Start()
    {
        originalMaterialShader = spriteRenderer.material.shader;
    }

    public override void CompletePreviousFeedback()
    {
        StopAllCoroutines();
        spriteRenderer.material.shader = originalMaterialShader;
    }

    public override void CreateFeedback()
    {
        if (spriteRenderer.material.HasProperty(ShaderConst.MADE_SOLID_COLOR)==false)
        {
            originalMaterialShader = flashMaterial.shader;
        }
        spriteRenderer.material.SetInt(ShaderConst.MADE_SOLID_COLOR, 1);
        StartCoroutine(WaitBeforeChangingBack());
    }
    IEnumerator WaitBeforeChangingBack()
    {
        yield return new WaitForSeconds(flashTime);
        if (spriteRenderer.material.HasProperty(ShaderConst.MADE_SOLID_COLOR))
        {
            spriteRenderer.material.SetInt(ShaderConst.MADE_SOLID_COLOR, 0);
        }
        else
        {
            originalMaterialShader = spriteRenderer.material.shader;
        }
    }
}
