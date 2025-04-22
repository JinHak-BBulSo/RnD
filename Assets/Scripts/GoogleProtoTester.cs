using UnityEngine;

public class GoogleProtoTester : MonoBehaviour
{
    private TestData testData_;
    void Start()
    {
        Init();
    }
    public void Init()
    {
        testData_ = new TestData(9999, 100, 10000, 5);
    }
}

public class TestData
{
    int id;
    int level;
    int hp;
    int speed;
    public TestData(int id_, int level_, int hp_, int speed_)
    {
        id = id_;
        level = level_;
        hp = hp_;
        speed = speed_;
    }
}
