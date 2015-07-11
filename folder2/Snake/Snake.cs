using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Snake
{
    class Snake
    {
        public enum EDirection //Перечислення напрямків, куди йти
        {
            N, S, E, W
        }

        private const int ScoreForFood = 10;

        private readonly Dictionary<EDirection, Point> _dictionary = new Dictionary<EDirection, Point> //Словник, переводить напрямов у відносні кординати 
                                                           {
                                                               {EDirection.N, new Point(0, -1)}, //Північ
                                                               {EDirection.S, new Point(0, 1)},  //Південь
                                                               {EDirection.W, new Point(-1, 0)}, //Захід
                                                               {EDirection.E, new Point(1, 0)},  //Схід
                                                           };

        private struct Segment //Структура сегментів. Фактично той же Point, тільки з перезавантаженням декількох параметрів, для того, щоб можна було легко складати відносні кординати.
        {
            public readonly int X;
            public readonly int Y;


            public Segment(int x, int y)
                : this()
            {
                X = x;
                Y = y;
            }

            public Segment(Point p) : this(p.X, p.Y) { }

            public static Segment operator +(Segment lhs, Segment rhs)
            {
                return new Segment(lhs.X + rhs.X, lhs.Y + rhs.Y);
            }

            #region Point<->Segment implementation

            public static implicit operator Point(Segment segment)
            {
                return new Point(segment.X, segment.Y);
            }

            public static implicit operator Segment(Point p)
            {
                return new Segment(p.X, p.Y);
            }

            #endregion
        }

        private readonly Queue<Segment> _snake; //Сама змійка
        private Segment _head;                  //Голова
        private readonly Size _size;            //Розміри карти
        private static Point Food { get; set; } //Координати  клітинки з їжею
        public static uint Score { get; private set; }
        public Snake(int n, Size mapSize)              //Конструктор, визначае розміри карти, і кількість сегментів змії
        {
            _snake = new Queue<Segment>();
            _size = mapSize;
            var p = new Point(_size.Width / 2 - n / 2, _size.Height / 2); //Ставим змійку по центру карти
            for (int i = 0; i < n; i++, p.X++)
                _snake.Enqueue(new Segment(p));
            p.X--;
            _head = p;
            PlaceFood();
        }

        public List<Point> GetCoords()
        {
            return _snake.Select(segment => (Point)segment).ToList(); //Повертаємо список координат змії
        }

        public bool Move(EDirection direction, out List<Visualizer.Terrain> coordsToRepaint)  //Рухаємо змію. Якщо вона себе кусає, повертаємо true;
        {
            coordsToRepaint = new List<Visualizer.Terrain> { new Visualizer.Terrain(Visualizer.TerrainInfo.Backcolor, _snake.Peek()),
                                                             new Visualizer.Terrain(Visualizer.TerrainInfo.Snake, _head) };
            var s = _head + _dictionary[direction];
            if (_snake.Contains(s) || _head.X <= 0 || _head.X >= _size.Width - 1 || _head.Y <= 0 || _head.Y >= _size.Height - 1)
                return true;
            _head = s;
            _snake.Enqueue(_head);
            coordsToRepaint.Add(new Visualizer.Terrain(Visualizer.TerrainInfo.Head, _head));
            if (_head == Food) //Якщо змійка потрапила на клітинку з їжею, ми заповнюемо випадкову клітинку їжею.
            {
                Score += ScoreForFood;
                PlaceFood();
            }
            else _snake.Dequeue(); //Інакше, щоб змія не збільшилась, прибираемо останній сегмент
            coordsToRepaint.Add(new Visualizer.Terrain(Visualizer.TerrainInfo.Food, Food));
            return false; //Ми себе не вкусили, значить false
        }
        private void PlaceFood()
        {
            var r = new Random();
            var p = new Point();
            do
            {
                p.X = r.Next(_size.Width);
                p.Y = r.Next(_size.Height);
            } while (_snake.Contains(p));
            Food = p;
        }

    }
}


