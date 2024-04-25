using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] Sprite[] playerLightsSprite;
    [SerializeField] Sprite[] playerAlarmLightsSprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite deadMouse;
    
    bool mustDie = false;
    bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
       StartCoroutine(LoopLights());
    }

    IEnumerator LoopLights()
    {
        int index = 0;
        while (true)
        {
            spriteRenderer.sprite = playerLightsSprite[index];
            index = (index + 1) % playerLightsSprite.Length;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public void AlarmLights()
    {
        if (isDead) return;
        StopAllCoroutines();
        StartCoroutine(AlarmLightsCoroutine());
    }
    
    IEnumerator AlarmLightsCoroutine()
    {

        int index = 0;
        for (int i = 0; i < 5 * playerAlarmLightsSprites.Length; i++)
        {
            spriteRenderer.sprite = playerAlarmLightsSprites[index];
            index = (index + 1) % playerAlarmLightsSprites.Length;
            yield return new WaitForSeconds(0.2f);
        }
        Debug.Log("AlarmLightsCoroutine completed" + mustDie);
        StartCoroutine(LoopLights());
    }
    
    public void Die()
    {
        isDead = true;
        StopAllCoroutines();

        spriteRenderer.sprite = deadMouse;
    }
}
