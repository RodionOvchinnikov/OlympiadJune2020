using BehavioralAlgorithms.Interfaces;
using BehavioralAlgorithms.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BehavioralAlgorithms.Behaviors
{
    public class BFSBehavior : IBehavior
    {
        private List<Point> FindPath(Point headPosition, Point fruitPosition, int[,] map, int Width, int Height)
        {
            List<Point> wave = new List<Point>();
            int x = headPosition.X;
            int y = headPosition.Y;
            int nx = fruitPosition.X;
            int ny = fruitPosition.Y;

            //заполнение значений достижимости(длинны пути) для конечной точки
            int[,] cloneMap = (int[,])map.Clone();
            List<Point> oldWave = new List<Point>();
            oldWave.Add(new Point(nx, ny));
            int nstep = 0;
            cloneMap[nx, ny] = nstep;

            int[] dx = { 0, 1, 0, -1 };
            int[] dy = { -1, 0, 1, 0 };

            while (oldWave.Count > 0)
            {
                ++nstep;
                wave.Clear();
                foreach (Point i in oldWave)
                {
                    for (int d = 0; d < 4; d++)
                    {
                        nx = i.X + dx[d];
                        ny = i.Y + dy[d];

                        if (ny >= 0 && ny < Height && nx >= 0 && nx < Width)
                        {
                            if (cloneMap[nx, ny] == -1)
                            {
                                wave.Add(new Point(nx, ny));
                                cloneMap[nx, ny] = nstep;
                            }
                        }
                    }
                }
                oldWave = new List<Point>(wave);
            }

            wave.Clear();
            while (cloneMap[x, y] != 0)
            {
                //поиск пути от точки отправления
                bool flag = true;
                for (int d = 0; d < 4; d++)
                {
                    nx = x + dx[d];
                    ny = y + dy[d];

                    if (ny >= 0 && ny < Height && nx >= 0 && nx < Width)
                    {
                        if (cloneMap[x, y] - 1 == cloneMap[nx, ny])
                        {
                            x = nx;
                            y = ny;
                            wave.Add(new Point(x, y));
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    break;
                }
            }

            return wave;
        }

        public MoveDirection Move(MoveState state)
        {
            // Наша змея
            Snake mainSnake = state.Snakes.Single(x => x.Id == state.You);
            // Противники
            IEnumerable<Snake> enemies = state.Snakes.Where(x => x.Id != state.You);

            // информация о карте
            MapInformation.map = new int[state.Width, state.Height];
            MapInformation.height = state.Height;
            MapInformation.width = state.Width;

            // Заполняем карту служебной имнформацией
            for (int i = 0; i < state.Width; ++i)
            {
                for(int j = 0; j < state.Height; ++j)
                {
                    MapInformation.map[i, j] = -1;
                }
            }

            // заполняем карту препятствиями
            foreach (var enemy in enemies)
            {
                foreach (var cell in enemy.Coords)
                {
                    MapInformation.map[cell.X, cell.Y] = MapInformation.barrier;
                }

                if (enemy.Coords.Length >= mainSnake.Coords.Length)
                {
                    int x = 0;
                    int y = 0;

                    // Змеи находятся напротив друг друга вертикально
                    if (Math.Abs(enemy.HeadPosition.X - mainSnake.HeadPosition.X) == 2 && (enemy.HeadPosition.Y - mainSnake.HeadPosition.Y) == 0)
                    {
                        if (enemy.HeadPosition.X > mainSnake.HeadPosition.X)
                        {
                            x = enemy.HeadPosition.X - 1;
                        }
                        else
                        {
                            x = enemy.HeadPosition.X + 1;
                        }

                        y = enemy.HeadPosition.Y;

                        MapInformation.map[x, y] = MapInformation.barrier;
                    }

                    // Змеи находятся напротив друг друга горизонтально
                    if ((enemy.HeadPosition.X - mainSnake.HeadPosition.X) == 0 && Math.Abs(enemy.HeadPosition.Y - mainSnake.HeadPosition.Y) == 2)
                    {
                        if (enemy.HeadPosition.Y > mainSnake.HeadPosition.Y)
                        {
                            y = enemy.HeadPosition.Y - 1;
                        }
                        else
                        {
                            y = enemy.HeadPosition.Y + 1;
                        }

                        x = enemy.HeadPosition.X;

                        MapInformation.map[x, y] = MapInformation.barrier;
                    }

                    // Змеи находятся в друг от друга по диагонали, но в одном шаге
                    if (Math.Abs(enemy.HeadPosition.X - mainSnake.HeadPosition.X) == 1 && Math.Abs(enemy.HeadPosition.Y - mainSnake.HeadPosition.Y) == 1)
                    {
                        MapInformation.map[mainSnake.HeadPosition.X, enemy.HeadPosition.Y] = MapInformation.barrier;
                        MapInformation.map[enemy.HeadPosition.X, mainSnake.HeadPosition.Y] = MapInformation.barrier;
                    }
                }
            }

            foreach (var cell in mainSnake.Coords)
            {
                MapInformation.map[cell.X, cell.Y] = MapInformation.barrier;
            }

            // Будем считать голову свободной ячейкой для удобства расчетов
            MapInformation.map[mainSnake.HeadPosition.X, mainSnake.HeadPosition.Y] = -1;

            // Находим путь до каждого фрукта
            List<List<Point>> paths = new List<List<Point>>();
            foreach (var fruit in state.Food)
            {
                var path = FindPath(mainSnake.HeadPosition, fruit, MapInformation.map, state.Width, state.Height);
                // Если нашли путь, добавляем его в список
                if(path.Count > 0)
                {
                    paths.Add(path);
                }
            }

            List<List<Point>> checkedPaths = new List<List<Point>>();
            
            foreach (var path in paths)
            {
                Point fruit = path.Last();
                bool dangerousPath = false;

                if (enemies.Count() > 0)
                {
                    foreach (var enemy in enemies)
                    {
                        // Если вражеская змея в шаге от фрукта
                        if ((Math.Abs(enemy.HeadPosition.X - fruit.X) == 1 && (enemy.HeadPosition.Y - fruit.Y) == 0)
                            || ((enemy.HeadPosition.X - fruit.X) == 0 && Math.Abs(enemy.HeadPosition.Y - fruit.Y) == 1))
                        {
                            // Если нам идти более чем 1 шаг или вражеская змея большем либо равна нашей, то исключаем этот фрукт из выборки
                            if (enemy.Coords.Length >= mainSnake.Coords.Length)
                            {
                                dangerousPath = true;
                            }
                        }
                    }

                    if (!dangerousPath)
                    {
                        checkedPaths.Add(path);
                    }
                }
                else
                {
                    checkedPaths.Add(path);
                }
            }

            List<Point> pathToNearestFruit = checkedPaths.OrderBy(x => x.Count).FirstOrDefault();

            // Если нашли путь до фрукта
            if (pathToNearestFruit != null)
            {
                // Берем первую ячейку пути, в которую нам и надо шагнуть
                var nextCell = pathToNearestFruit[0];

                int translationX = mainSnake.HeadPosition.X - nextCell.X;
                int translationY = mainSnake.HeadPosition.Y - nextCell.Y;

                if (translationX == 0)
                {
                    if (translationY > 0)
                    {
                        return new MoveDirection { Move = "up", Taunt = "Moving up" };
                    }
                    else
                    {
                        return new MoveDirection { Move = "down", Taunt = "Moving down" };
                    }
                }

                if (translationX > 0)
                {
                    return new MoveDirection { Move = "left", Taunt = "Moving left" };
                }
                else
                {
                    return new MoveDirection { Move = "right", Taunt = "Moving right" };
                }
            }
            // Если не смогли найти путь до фрукта, то тянем время с помощью движения в сторону самой удаленной доступной ячейки с помощью пути найденного поиском в глубину
            else
            {
                DFSBehavior dfs = new DFSBehavior();

                return dfs.Move(mainSnake.HeadPosition);
            }
        }
    }
}
