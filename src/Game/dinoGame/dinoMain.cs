dinosourGame game = new dinosourGame();
game.Run();

public class dinosourGame
{
    // 화면 및 지형 설정
    private const int Width = 60;
    private const int Height = 15;
    private const int GroundY = 12;

    // 공룡 상태
    private const int DinoX = 6;
    private float dinoY;
    private float velocityY;
    private const float Gravity = 0.8f;
    private const float JumpForce = -3.2f;
    private bool isJumping;

    // 장애물 및 점수 상태
    private List<int> obstacles;
    private Random random;
    private int spawnTimer;
    private int score;
    private bool isGameOver;
    private bool isRunning;

    public dinosourGame()
    {
        Console.CursorVisible = false;
        obstacles = new List<int>();
        random = new Random();
        Init();
    }

    private void Init()
    {
        dinoY = GroundY;
        velocityY = 0;
        isJumping = false;
        obstacles.Clear();
        spawnTimer = 0;
        score = 0;
        isGameOver = false;
        isRunning = true;
        Console.Clear();
    }

    public void Run()
    {
        while (isRunning)
        {
            Input();
            Update();
            Rendering();
            Thread.Sleep(40); // 약 25 FPS 유지
        }
    }

    private void Input()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // 게임 오버 상태일 때 R키로 재시작, ESC로 종료
            if (isGameOver)
            {
                if (keyInfo.Key == ConsoleKey.R)
                {
                    Init();
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isRunning = false;
                }
                return;
            }

            // 점프 입력 (스페이스바 또는 위쪽 방향키)
            if ((keyInfo.Key == ConsoleKey.Spacebar || keyInfo.Key == ConsoleKey.UpArrow) && !isJumping)
            {
                velocityY = JumpForce;
                isJumping = true;
            }
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        // 1. 공룡 물리(점프 및 중력) 처리
        if (isJumping)
        {
            dinoY += velocityY;
            velocityY += Gravity;

            if (dinoY >= GroundY)
            {
                dinoY = GroundY;
                velocityY = 0;
                isJumping = false;
            }
        }

        // 2. 장애물 생성
        spawnTimer--;
        if (spawnTimer <= 0)
        {
            obstacles.Add(Width - 2);
            spawnTimer = random.Next(18, 35); // 다음 장애물까지 랜덤 간격
        }

        // 3. 장애물 이동 및 충돌 판정
        int currentDinoY = (int)Math.Round(dinoY);

        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            obstacles[i]--;

            // 충돌 체크 (X좌표와 Y좌표가 겹칠 때)
            if (obstacles[i] == DinoX && currentDinoY == GroundY)
            {
                isGameOver = true;
            }

            // 화면 왼쪽 끝을 벗어난 장애물 제거 및 점수 획득
            if (obstacles[i] < 0)
            {
                obstacles.RemoveAt(i);
                score += 10;
            }
        }
    }

    private void Rendering()
    {
        // 화면 깜빡임(Flickering) 방지를 위해 커서를 원점으로 이동
        Console.SetCursorPosition(0, 0);

        char[,] buffer = new char[Height, Width];

        // 1. 빈 공간 및 바닥 채우기
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (y == GroundY + 1)
                    buffer[y, x] = '='; // 바닥
                else
                    buffer[y, x] = ' ';
            }
        }

        // 2. 공룡 그리기
        int drawDinoY = (int)Math.Round(dinoY);
        if (drawDinoY >= 0 && drawDinoY < Height)
        {
            buffer[drawDinoY, DinoX] = 'R'; // 공룡 (Rex)
        }

        // 3. 장애물(선인장) 그리기
        foreach (int obsX in obstacles)
        {
            if (obsX >= 0 && obsX < Width)
            {
                buffer[GroundY, obsX] = '#'; // 선인장
            }
        }

        // 4. 버퍼 내용을 한 번에 문자열로 변환하여 출력
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine($"SCORE: {score:D5}   [SPACE / UP]: Jump");

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                sb.Append(buffer[y, x]);
            }
            sb.AppendLine();
        }

        if (isGameOver)
        {
            sb.AppendLine("============================================================");
            sb.AppendLine("         GAME OVER!  Press 'R' to Restart, 'ESC' to Exit    ");
            sb.AppendLine("============================================================");
        }
        else
        {
            // 게임 오버 메시지 잔상 지우기용 공백 라인
            sb.AppendLine(new string(' ', 60));
            sb.AppendLine(new string(' ', 60));
            sb.AppendLine(new string(' ', 60));
        }

        Console.Write(sb.ToString());
    }
}