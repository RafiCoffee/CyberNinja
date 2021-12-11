using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraBoss : MonoBehaviour
{
    private bool same = true;
    private bool isSame;

    public Slider barraBoss;
    private Slider barraBossBajando;

    // Start is called before the first frame update
    void Start()
    {
        barraBossBajando = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (barraBoss.value == barraBossBajando.value)
        {
            same = true;
        }

        if (isSame)
        {
            barraBossBajando.value = barraBoss.value;
            isSame = false;
        }

        if (barraBoss.value < barraBossBajando.value && same)
        {
            same = false;
            StartCoroutine(BajarBarra());
        }
    }

    IEnumerator BajarBarra()
    {
        yield return new WaitForSeconds(1);
        do
        {
            if (!same)
            {
                barraBossBajando.value = barraBossBajando.value - 0.1f;
                yield return new WaitForSeconds(0.01f);
            }
        } while (barraBoss.value < barraBossBajando.value);
        isSame = true;
    }
}
