using UnityEngine;
using TMPro;

public class MyTask_BattleLogic : MonoBehaviour
{
    private MyTask_CritSystem critSystem;

    [Header("Enemy Info")]
    public int maxHP = 300;
    private int currentHP;

    [Header("Item Probabilities (%)")]
    private float[] baseProbs = { 50f, 30f, 15f, 5f }; // 일반, 고급, 희귀, 전설 기본값
    private float[] currentProbs = new float[4];
    private int[] inventory = new int[4];

    [Header("UI Text Mesh Pro")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI critText;
    public TextMeshProUGUI probText;
    public TextMeshProUGUI invenText;

    void Start()
    {
        critSystem = GetComponent<MyTask_CritSystem>();
        currentHP = maxHP;

        // 확률 배열 초기화
        for (int i = 0; i < baseProbs.Length; i++)
            currentProbs[i] = baseProbs[i];

        UpdateDisplay();
    }

    // 공격 버튼에 연결할 함수
    public void ClickAttack()
    {
        bool isCrit = critSystem.RollCrit();
        int damage = isCrit ? 60 : 30; // 치명타 시 2배 데미지

        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            UpdateDisplay(); // 0인 상태를 한 번 보여줌
            GetReward();     // 보상 획득
            currentHP = maxHP; // 새 적 등장
        }

        UpdateDisplay();
    }

    void GetReward()
    {
        float dice = Random.value * 100f;
        float cumulative = 0f;
        int selected = -1;

        for (int i = 0; i < currentProbs.Length; i++)
        {
            cumulative += currentProbs[i];
            if (dice <= cumulative)
            {
                selected = i;
                break;
            }
        }

        inventory[selected]++;

        // 전설(index 3) 획득 여부에 따른 확률 보정
        if (selected == 3)
        {
            // 전설 획득 시 확률 초기화
            for (int i = 0; i < baseProbs.Length; i++)
                currentProbs[i] = baseProbs[i];
        }
        else
        {
            // 전설 못 얻으면 전설 확률 +1.5%, 나머지는 각각 -0.5%
            currentProbs[3] += 1.5f;
            currentProbs[0] -= 0.5f;
            currentProbs[1] -= 0.5f;
            currentProbs[2] -= 0.5f;
        }
    }

    void UpdateDisplay()
    {
        // 체력 표시
        hpText.text = $"체력 : <color=red>{currentHP}/{maxHP}</color>";

        // 치명타 통계
        float realRate = (critSystem.totalHits == 0) ? 0 : (float)critSystem.critHits / critSystem.totalHits * 100f;
        critText.text = $"전체 공격 회수 : {critSystem.totalHits}\n" +
                        $"발생한 치명타 회수 : {critSystem.critHits}\n" +
                        $"설정된 치명타 확률 : {critSystem.targetRate * 100:F2}%\n" +
                        $"실제 치명타 확률 : {realRate:F2}%";

        // 아이템 확률
        probText.text = $"현재 아이템 확률\n" +
                        $"일반 : {currentProbs[0]:F1}%\n" +
                        $"고급 : {currentProbs[1]:F1}%\n" +
                        $"희귀 : {currentProbs[2]:F1}%\n" +
                        $"전설 : {currentProbs[3]:F1}%";

        // 인벤토리
        invenText.text = $"현재 드롭된 아이템\n\n" +
                         $"일반 : {inventory[0]}\n" +
                         $"고급 : {inventory[1]}\n" +
                         $"희귀 : {inventory[2]}\n" +
                         $"전설 : {inventory[3]}";
    }
}