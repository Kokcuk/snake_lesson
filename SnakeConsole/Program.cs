using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace SnakeConsole
{
    class Program
    {
        //TODO: Draw food
        //TODO: Implement eating food
        //TODO: Draw score
        
        static int maxX = 100;
        static int maxY = 20;
        static List<Block> blocks = new List<Block>();
        static Random rnd = new Random();
        static MoveDirection MoveDirection = MoveDirection.Up;
        static int Score = 1;
        
        static void Main(string[] args)
        {
            //Убираем мигающий курсор
            Console.CursorVisible = false;
            //Заполняем забор и начальную позицию змеи
            SetDefaultBlocks();
            
            // Основной цикл
            while (true)
            {             
                WriteScore();
                
                //Читаем ввод
                GetUserInput();
                
                //Рисуем блоки на экран
                Render();
                
                //Пауза программы на 500мс
                Thread.Sleep(500);
                
                //Перенос координат змеи
                MoveSnake();
                
                //Проверяем пересечение координат змени и забора
                var collision = CheckCollision();
                if(collision)
                    break;
            }

            WriteStatus("Pizdec!");
            
            Console.ReadLine();
        }

        static void GetUserInput()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    MoveDirection = MoveDirection.Up;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    MoveDirection = MoveDirection.Down;
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    MoveDirection = MoveDirection.Right;
                }
                
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    MoveDirection = MoveDirection.Left;
                }
            }
        }
        
        static bool CheckCollision()
        {
            //Перебираем все блок
            foreach (var block in blocks)
            {
                //Если тип блока змея
                if (block.Type == BlockType.SnakeBody)
                {
                    //Ищем не пересекается ли она с забором
                    foreach (var fenceBlock in blocks)
                    {
                        if (fenceBlock.Type == BlockType.Fence && fenceBlock.X == block.X && fenceBlock.Y == block.Y)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        static void WriteStatus(string text)
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, maxY + 2);
            Console.WriteLine(text);
        }
        
        static void WriteScore()
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, maxY + 3);
            Console.WriteLine("Score: " + Score);
        }
        
        /// <summary>
        /// Модифицируем координаты змеи согласно направлению
        /// </summary>
        static void MoveSnake()
        {
            foreach (var block in blocks)
            {
                if (block.Type == BlockType.SnakeBody)
                {
                    if (MoveDirection == MoveDirection.Down)
                    {
                        block.Y = block.Y + 1;
                    }
                    
                    if (MoveDirection == MoveDirection.Up)
                    {
                        block.Y = block.Y - 1;
                    }
                    
                    if (MoveDirection == MoveDirection.Right)
                    {
                        block.X = block.X + 1;
                    }
                    
                    if (MoveDirection == MoveDirection.Left)
                    {
                        block.X = block.X - 1;
                    }
                }
            }
        }
        
        /// <summary>
        /// Заполняем начальные позиции блоков
        /// </summary>
        static void SetDefaultBlocks()
        {
            for (int x = 0; x <= maxX; x++)
            {
                for (int y = 0; y <= maxY; y++)
                {
                    if(x == 0)
                        blocks.Add(new Block
                        {
                            X = x,
                            Y = y,
                            Type = BlockType.Fence
                        });
                    
                    else if(y == 0)
                        blocks.Add(new Block
                        {
                            X = x,
                            Y = y,
                            Type = BlockType.Fence
                        });
                    
                    else if (y == maxY)
                        blocks.Add(new Block
                        {
                            X = x,
                            Y = y,
                            Type = BlockType.Fence
                        });
                    else if (x == maxX)
                        blocks.Add(new Block
                        {
                            X = x,
                            Y = y,
                            Type = BlockType.Fence
                        });
                }
            }

            var snakeX = rnd.Next(1, maxX - 1);
            var snakeY = rnd.Next(1, maxY - 1);
            
            blocks.Add(new Block
            {
                X = snakeX,
                Y = snakeY,
                Type = BlockType.SnakeBody
            });
        }

        
        /// <summary>
        /// Рисуем блоки из памяти в консоль
        /// </summary>
        static void Render()
        {
            Console.Clear();
            
            foreach (var block in blocks)
            {
                if (block.Type == BlockType.Fence)
                    DrawFence(block.X, block.Y);
                if (block.Type == BlockType.SnakeBody)
                    DrawSnakeBody(block.X, block.Y);
                if (block.Type == BlockType.SnakeHead)
                    DrawSnakeHead(block.X, block.Y);
                if (block.Type == BlockType.Food)
                    DrawFood(block.X, block.Y);
            }
        }
        
        static void DrawFence(int x, int y)
        {
            DrawColoredBlock(ConsoleColor.Black, x, y);
        }
        
        static void DrawSnakeBody(int x, int y)
        {
            DrawColoredBlock(ConsoleColor.Green, x, y);
        }
        
        static void DrawSnakeHead(int x, int y)
        {
            DrawColoredBlock(ConsoleColor.Red, x, y);
        }
        
        static void DrawFood(int x, int y)
        {
            DrawColoredBlock(ConsoleColor.Yellow, x, y);
        }

        static void DrawColoredBlock(ConsoleColor color, int x, int y)
        {
            Console.BackgroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
            Console.ResetColor();
        }
    }
}