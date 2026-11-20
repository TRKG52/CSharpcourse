// 반복문 
// 반복 표현식을 줄일수있는 방법 

// for 반복문

// while 반복문 
//9x9단 


    for (int i = 2; i <= 9; i++)
    {
        for (int j = 1; j <= 9; j++)
        {
            Console.WriteLine(i + " x " + j + " = " + (i * j));
        }

        Console.WriteLine();
    }
// for반복문은, 반복횟수 눈에 보일때 쓰면 효율적이다 

//while반복문
//무한 반복문
//게임을 종료할 조건
// while 반복문은 언제 끝날지 모를떄 쓰면 좋다 

int grade = 0;
bool gameFinish = grade >= 10;

while (true)

{
    grade++;
    Console.WriteLine($"현재등급 :{grade}");


    if (grade >= 10)
    {
        break;
    }
}