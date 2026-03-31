using UnityEngine;

public class DiceSimulator : MonoBehaviour
{
    int[] counts = new int[6];

    public int trials = 100;
   
    
    void Start()
    {
        for(int i =0; i < trials; i++)
        {
            int result = Random.Range(1, 7);
            counts[result - 1]++;
        }

        for(int i = 0; i < counts.Length; i++)
        {
            float percent = (float)counts[i] / trials * 100f;
            Debug.Log($"{i + 1}:{counts[i]}회 ({percent:F2}%");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
