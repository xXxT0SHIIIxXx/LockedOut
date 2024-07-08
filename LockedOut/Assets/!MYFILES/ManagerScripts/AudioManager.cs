using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioSource fxSrc;

    [SerializeField] AudioClip[] fxClips;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PlayInteractSound(float delay, int state)
    {
        float counter = 0;
        float waitTime = delay;
        while (counter < (waitTime))
        {
            counter += Time.deltaTime;
            yield return null;
        }

        if (state == 0)
        {
            fxSrc.PlayOneShot(fxClips[state]);
        }
        else if (state == 1)
        {

            fxSrc.PlayOneShot(fxClips[state]);
        }
    }

    public void InteractionSound(Component sender, object data)
    {
        if(data is HouseData)
        {
            HouseData result = (HouseData)data;

            StartCoroutine(PlayInteractSound(1f,result.knockRing));
        }
    }
}
