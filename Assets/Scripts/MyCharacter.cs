using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public int Health = 100;                             //체력선언(변수)
    public float Timer = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = Health + 100;                     //첫 시작할때 100이 체력을 추가한다.
    }

    // Update is called once per frame
    void Update()
    {
        Timer = Timer - Time.deltaTime;              //시간 실수를 매 프레임마다 감소 시킨다.

        if(Timer <=0)                            //만약 타이머의 수치가 0 이하로 내려갈 경우
        {
            Timer = 1.0f;                                  //다시 1초로 변경시켜줌
            Health = Health - 20;                          //체력을 20 감소시킴
        }

        if(Input.GetKeyDown(KeyCode.Space))                 //space바를 누르면 체력이 2만큼 찬다
        {
            Health = Health + 2;
        }

        if(Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
