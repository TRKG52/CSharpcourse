// 주석 comment 

/* 범위 주석 
 * 2026-11-18
 * 키워드 : namespace , internal, class , static , void , string 문법
 * 
 * Todo : 키워드 문법에 따라서 결정된 이름 
 * Program : 클래스. 
 * Main( ) / WriteLine( ) 
 */

 /* 
  * 게임에서 제목 타이틀이 나오고 그 아래에 플레이가 가능한 플레이어 이름 만드는거 구현 해보세요 
  * 
  * 제목 문자열 
  * 
  * <입출력>
  * step1자연어를 프로그래밍 언어로 바꾸고 > 실행 가능한가 ?
  * step2. 효율적인가? 왜 이런 이유로 선택했는지 근거를 말할 수 있는가? - 알고리즘 
  * 
  *[모험가 길드 ver] [1.0.1]
  */


namespace Todo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 텍스트, 실수, 정수 
            // string 문자열 "....." / "" 없으면 에러가 뜬다. -> 컨텍스트가 없음 이라고 뜬다.
            // 변수 -> 누가 타입을 정의 했는가  

            string title = "슬레이더 스파이어";
            float version = 1.01f;

            int hp = 10 - 1;
            
            // C# 콘솔에서 출력을 할것인데 타이틀 이름은 슬레이더 스파이어 이고 버젼은 1.01로 출력하는 코드
           
            Console.WriteLine($"{title} {version}");
            Console.WriteLine(title); // 문자열 , string
            Console.WriteLine(hp); // 정수 . int 
            Console.WriteLine(1.1f); // 1이 나온다 , 1.0 적었을때는 실수가 안나오네 ? 어떻게 해야 나타낼수있을까 
            Console.WriteLine(false);

            Console.ReadLine(); // 작성한걸 읽어오는것 
        }
    }
}
