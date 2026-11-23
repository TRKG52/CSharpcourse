using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleArcadeGame
{
    public static class Game
    {
        // ==========================================
        // 게임 맵 및 상태 데이터
        // ==========================================
        private const int Width = 30;
        private const int Height = 16;

        // 뱀의 몸통 좌표 리스트 (0번 인덱스가 머리)
        private static List<(int X, int Y)> _snake = new List<(int X, int Y)>();
        private static (int X, int Y) _direction = (1, 0); // 초기 이동 방향: 오른쪽
        private static (int X, int Y) _apple;

        private static int _score = 0;
        private static bool _isRunning = true;
        private static Random _random = new Random();

        // ==========================================
        // 1. 메인 게임 루프
        // ==========================================
        public static void Run()
        {
            Console.Title = "콘솔 스네이크 게임";
            Console.CursorVisible = false;

            InitGame();

            while (_isRunning)
            {
                Input();     // 1. 방향 전환 입력
                Update();    // 2. 뱀 이동, 사과 섭취 및 충돌 판정
                Rendering(); // 3. 맵과 뱀, 사과 그리기

                Thread.Sleep(110); // 뱀 이동 속도 (숫자가 작을수록 빨라짐)
            }

            // 게임 오버 화면
            Console.Clear();
            Console.WriteLine("==================================");
            Console.WriteLine("           🐍 GAME OVER 🐍        ");
            Console.WriteLine($"         최종 점수 : {_score} 점       ");
            Console.WriteLine($"         최종 길이 : {_snake.Count} 칸       ");
            Console.WriteLine("==================================");
            Console.CursorVisible = true;
        }

        // 게임 초기 세팅 (뱀 길이 3으로 시작)
        private static void InitGame()
        {
            _snake.Clear();
            int startX = Width / 2;
            int startY = Height / 2;

            _snake.Add((startX, startY));         // 머리
            _snake.Add((startX - 1, startY));     // 몸통 1
            _snake.Add((startX - 2, startY));     // 꼬리

            SpawnApple();
        }

        // ==========================================
        // 2. Input: 방향키 입력 처리 (역방향 유턴 방지)
        // ==========================================
        private static void Input()
        {
            if (!Console.KeyAvailable) return;

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (_direction.Y != 1) _direction = (0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    if (_direction.Y != -1) _direction = (0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    if (_direction.X != 1) _direction = (-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    if (_direction.X != -1) _direction = (1, 0);
                    break;
                case ConsoleKey.Escape:
                    _isRunning = false;
                    break;
            }
        }

        // ==========================================
        // 3. Update: 뱀 이동 및 사과/충돌 판정
        // ==========================================
        private static void Update()
        {
            if (!_isRunning) return;

            // 1. 다음 머리 위치 계산
            var head = _snake[0];
            var nextHead = (X: head.X + _direction.X, Y: head.Y + _direction.Y);

            // 2. 외벽 충돌 판정
            if (nextHead.X <= 0 || nextHead.X >= Width - 1 ||
                nextHead.Y <= 0 || nextHead.Y >= Height - 1)
            {
                _isRunning = false;
                return;
            }

            // 3. 자기 몸통 충돌 판정
            if (_snake.Contains(nextHead))
            {
                _isRunning = false;
                return;
            }

            // 4. 새로운 머리 추가
            _snake.Insert(0, nextHead);

            // 5. 사과를 먹었는지 확인
            if (nextHead == _apple)
            {
                _score += 100;
                SpawnApple(); // 꼬리를 자르지 않으므로 길이가 1 늘어남
            }
            else
            {
                _snake.RemoveAt(_snake.Count - 1); // 사과를 안 먹었으면 꼬리 한 칸 제거
            }
        }

        // ==========================================
        // 4. Rendering: 화면 출력
        // ==========================================
        private static void Rendering()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"점수: {_score} 점 | 길이: {_snake.Count} | 종료: ESC   ");

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (y == 0 || y == Height - 1 || x == 0 || x == Width - 1)
                    {
                        Console.Write("#"); // 외벽
                    }
                    else if (x == _snake[0].X && y == _snake[0].Y)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("O"); // 뱀 머리
                        Console.ResetColor();
                    }
                    else if (_snake.Contains((x, y)))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("o"); // 뱀 몸통
                        Console.ResetColor();
                    }
                    else if (x == _apple.X && y == _apple.Y)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("@"); // 사과
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(" "); // 빈 공간
                    }
                }
                Console.WriteLine();
            }
        }

        // 뱀 몸통과 겹치지 않는 빈 자리에 사과 생성
        private static void SpawnApple()
        {
            do
            {
                _apple = (_random.Next(1, Width - 1), _random.Next(1, Height - 1));
            } while (_snake.Contains(_apple));
        }
    }
}
