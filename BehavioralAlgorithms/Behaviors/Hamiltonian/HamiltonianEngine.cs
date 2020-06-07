using BehavioralAlgorithms.Models;
using BehavioralAlgorithms.Models.Hamiltonian;
using System.Collections.Generic;
using System.Linq;

namespace BehavioralAlgorithms.Behaviors.Hamiltonian
{
    public static class HamiltonianEngine
    {
        public static string Move(MoveState state)
        {
            var snake = state.Snakes.First(s => s.Id == state.You);
            var head = new int[] { snake.HeadPosition.Y, snake.HeadPosition.X };

            var points = state.Snakes.SelectMany(s => s.Coords).Select(p => new[] { p.Y, p.X }).ToArray();

            var node = HamiltonianEngine.Start(state.Height, state.Width, head, points);

            return Util.MapHamiltonToDirection(head, node);
        }

        private static ArrayNode Start(int height, int width, int[] start, int[][] walls)
        {
            var map = MapNodes(height, width, walls);

            map = SetNeberhuds(height, width, map);

            return FindHamiltonianPath(map, height, width, start);
        }

        /// Создает карту с узлами, и стенами
        private static ArrayNode[,] MapNodes(int height, int width, int[][] walls)
        {
            var nodes = new ArrayNode[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    nodes[y, x] = new ArrayNode
                    {
                        X = x,
                        Y = y,
                        Blocked = walls?.Any(w => w[0] == y && w[1] == x) == true
                    };
                }
            }

            return nodes;
        }

        /// Задает связи между узлами
        private static ArrayNode[,] SetNeberhuds(int height, int width, ArrayNode[,] map)
        {
            // Тут небольшая неразбериха с X и Y, но в целом, так и должно быть
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x + 1 < width)
                    {
                        map[y, x].Right = map[y, x + 1];
                    }
                    if (x - 1 >= 0)
                    {
                        map[y, x].Left = map[y, x - 1];
                    }
                    if (y + 1 < height)
                    {
                        map[y, x].Bottom = map[y + 1, x];
                    }
                    if (y - 1 >= 0)
                    {
                        map[y, x].Top = map[y - 1, x];
                    }
                }
            }

            return map;
        }

        /// Находит самый длинный путь
        /// Это не полная реализация алгоритма, поиска Гамильтоновых цыклов,
        /// т.к. полная реализация не укладывается по времени, и поэтому поиск, идет до
        /// ближайшего отрезка. Т.е. строится не полный путь.
        private static ArrayNode FindHamiltonianPath(ArrayNode[,] map, int height, int width, int[] start)
        {
            var queue = new Queue<ArrayNode>();

            var yStart = start[0];
            var xStart = start[1];

            queue.Enqueue(map[yStart, xStart]);

            // Добавим узлы в очередь
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    if (x == xStart && y == yStart || map[y, x].Blocked)
                    {
                        continue;
                    }

                    queue.Enqueue(map[y, x]);
                }
            }

            var work = true;
            while (work)
            {
                // Проверяем, есть ли ребро между узлами
                if (CheckEdge(queue.ElementAt(0), queue.ElementAt(1)))
                {
                    // Если ребро есть, то первый узел двигаем в самый низ
                    var top = queue.Dequeue();
                    queue.Enqueue(top);
                }
                else
                {
                    // Если узла нет, то ищем ближайший от 1 узла с ребром
                    var i = 2;

                    for (; i < queue.Count; i++)
                    {
                        if (CheckEdge(queue.ElementAt(0), queue.ElementAt(i)))
                        {
                            break;
                        }
                    }

                    // Переворачиваем очередь от 1 узла и до i
                    Swap(1, i, queue);
                }

                var topElement = queue.ElementAt(0);
                work = !(topElement.X == start[1] && topElement.Y == start[0]);
            }

            return queue.Last();
        }

        /// Переворачивает узлы в очереди, от start до stop
        private static void Swap(int start, int stop, Queue<ArrayNode> q)
        {
            var nodes = new List<ArrayNode>();

            var head = q.Dequeue();

            // Выбрали то что нужно перевернуть
            for (int x = 0; x < stop; x++)
            {
                nodes.Add(q.Dequeue());
            }

            // Перевернули
            nodes.Reverse();

            // Добавим все остальное
            for (int y = 0; y < q.Count; y++)
            {
                nodes.Add(q.Dequeue());
            }

            //nodes.Reverse();

            q.Enqueue(head);

            // Вставляем все обратно в очередь
            for (int y = 0; y < nodes.Count; y++)
            {
                q.Enqueue(nodes[y]);
            }
        }

        /// Проверяет, есть ли ребро между узлами
        private static bool CheckEdge(ArrayNode left, ArrayNode right)
        {
            if (left.Top == right || left.Right == right || left.Bottom == right || left.Left == right)
            {
                return true;
            }

            if (right.Top == left || right.Right == left || right.Bottom == right || right.Left == left)
            {
                return true;
            }

            return false;
        }
    }
}
