using BehavioralAlgorithms.Interfaces;
using BehavioralAlgorithms.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BehavioralAlgorithms.Behaviors
{
    public class DFSBehavior
    {
        private bool[,] visited;
        private readonly Stack<Point> stack = new Stack<Point>();
        private int[,] cloneMap;

        public List<Point> BuildMap(Point headPosition)
        {
            List<Point> wave = new List<Point>();
            int nx = headPosition.X;
            int ny = headPosition.Y;

            //заполнение значений достижимости(длинны пути) для конечной точки
            cloneMap = (int[,])MapInformation.map.Clone();
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

                        if (ny >= 0 && ny < MapInformation.height && nx >= 0 && nx < MapInformation.width)
                        {
                            if (cloneMap[nx, ny] == -1)
                            {
                                wave.Add(new Point(nx, ny));
                                cloneMap[nx, ny] = nstep;
                            }
                        }
                    }
                }

                if(wave.Count == 0)
                {
                    return oldWave;
                }
                else
                {
                    oldWave = new List<Point>(wave);
                }
            }

            return new List<Point>();
        }

        public bool FindPath(Point startPosition, Point endPosition)
        {
            stack.Push(startPosition);

            if (startPosition.X < 0 || startPosition.Y < 0 || startPosition.X >= MapInformation.width || startPosition.Y >= MapInformation.height || visited[startPosition.X, startPosition.Y] || cloneMap[startPosition.X, startPosition.Y] == MapInformation.barrier)
            {
                stack.Pop();
                return false;
            }

            if (startPosition.X == endPosition.X && startPosition.Y == endPosition.Y)
            {
                return true;
            }

            visited[startPosition.X, startPosition.Y] = true;

            if (FindPath(new Point(startPosition.X, (startPosition.Y - 1)), endPosition))
            {
                return true;
            }

            if (FindPath(new Point(startPosition.X, (startPosition.Y + 1)), endPosition))
            {
                return true;
            }

            if (FindPath(new Point((startPosition.X - 1), startPosition.Y), endPosition))
            {
                return true;
            }

            if (FindPath(new Point((startPosition.X + 1), startPosition.Y), endPosition))
            {
                return true;
            }

            stack.Pop();
            return false;
        }

        // Т.к. данный алгоритм не эффективен в построении пути и в производительности, он используется только для затягивания времени когда не найден путь к фрукту
        public MoveDirection Move(Point headPosition)
        {
            visited = new bool[MapInformation.width, MapInformation.height];

            var farCells = BuildMap(headPosition);

            var targetCell = farCells.FirstOrDefault();

            if (FindPath(headPosition, targetCell))
            {
                var nextCell = stack.SkipLast(1).LastOrDefault();

                int translationX = headPosition.X - nextCell.X;
                int translationY = headPosition.Y - nextCell.Y;

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

            return new MoveDirection { Move = "right", Taunt = "Moving right" };
        }
    }
}
