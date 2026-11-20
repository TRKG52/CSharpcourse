using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

//조건문, 반복문 

namespace Todo
{
    internal class upgradeLoop
    {
        static void Main(string[] args)
        {
            int gold = 100;
            int grade = 0;
            int bestGrade = 0;
            int wincount = 10;
            bool running = true;
            string message = string.Empty;

            Random random = new Random();


            while(running)
            {
                int cost = (grade + 1) * 10;
                int chance = 90 - grade * 8;
                int sellPrice = grade * 20;

                // Render
                Console.Clear();
                Console.WriteLine("=== 무기 강화 ===");
                Console.WriteLine($"골드: {gold} | 무기: +{grade} | 최고: +{bestGrade}");
                Console.WriteLine($"강화 비용: {cost} | 성공 확률: {chance}%");
                Console.WriteLine($"판매 가격: {sellPrice} | 실패하면 무기는 +0으로 돌아갑니다.");
                Console.WriteLine("최고 등급 +10 무기를 만들어 보세요!");
                Console.WriteLine(message);
                Console.WriteLine("	[1] 강화 [2] 팔기 [0] 종료");

                if(grade >= 10)
                {
                    Console.WriteLine("+10 달성! 승리했습니다.");
                    break;
                }
                
                if(gold < 10 && grade == 0) //파산조건
                {
                    Console.WriteLine($"파산! 최고 기록: +{bestGrade}");
                    break;
                }

                // 입력
                string? Choice = Console.ReadLine();

                if (Choice == "1")
                {
                    if (gold < cost)
                    {
                        message = ("강화 비용이 부족합니다. 무기를 팔아 보세요.");
                    }
                    else
                    {
                        gold = gold - cost; //강화성공
                        if (random.Next(100) < chance)
                        {
                            grade = grade + 1;
                            if (grade > bestGrade) bestGrade = grade;
                            message = $"강화 성공! +{grade}";
                        }
                        else
                        {
                            grade = 0;
                            message = "	강화 실패! 새 +0 무기로 다시 시작합니다.";
                        }
                    }

                }
                else if(Choice == "2")
                {
                    if (grade == 0)
                    {
                        message = "+0 무기는 팔 수 없습니다.";
                    }
                    else
                    {
                        gold = gold + sellPrice;
                        grade = 0;
                        message = $"{sellPrice}골드에 판매했습니다. 새 +0 무기를 받았습니다.";
                    }
                }
                else if(Choice == "0" || Choice == null)
                {
                    running = false;
                }
                else
                {
                    Console.WriteLine("1, 2, 0 중 하나를 입력하세요.");
                }
            }

        }
    }
}
