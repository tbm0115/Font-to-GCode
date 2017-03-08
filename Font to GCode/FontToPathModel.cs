using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Font_to_GCode
{
  class FontToPathModel
  {
    public string FontFamily { get; set; }
    public Font Font { get; set; }
    public string[] Characters { get; }
    public IEnumerable<CharacterPath> CharacterLibrary
    {
      get
      {
        return this._charLib.ToArray();
      }
    }

    private List<CharacterPath> _charLib { get; set; }


    public FontToPathModel(string fontFamily, string[] characterList)
    {
      this.FontFamily = fontFamily;
      this.Font = new Font(FontFamily, Convert.ToInt32(Properties.Settings.Default.FontSize));
      Console.WriteLine("Font Size: " + this.Font.Size.ToString());
      this.Characters = characterList;
      this._charLib = new List<CharacterPath>();
      foreach (string item in this.Characters)
      {
        this.Add(item, this.Font);
      }
    }

    public CharacterPath Add(CharacterPath cp)
    {
      this._charLib.Add(cp);
      return _charLib[_charLib.Count - 1];
      //if (!this.Contains(cp.Character))
      //{
      //}
      //else
      //{
      //  throw new DuplicateWaitObjectException();
      //}
    }
    public CharacterPath Add(string character, Font font)
    {
      return this.Add(new CharacterPath(character, font));
    }

    public int IndexOf(string character)
    {
      for (int i = 0; i < this._charLib.Count; i++)
      {
        if (this._charLib[i].Character == character)
        {
          return i;
        }
      }
      return -1;
    }
    public int IndexOf(CharacterPath cpath)
    {
      return this.IndexOf(cpath.Character);
    }
    public bool Contains(string character)
    {
      return this.IndexOf(character) >= 0;
    }
    public bool Contains(CharacterPath cpath)
    {
      return this.IndexOf(cpath) >= 0;
    }
    public CharacterPath Item(int index)
    {
      if (this._charLib.Count > 0)
      {
        if (index >= 0 && index < this._charLib.Count)
        {
          return this._charLib[index];
        }
        else
        {
          throw new IndexOutOfRangeException();
        }
      }
      else
      {
        throw new NullReferenceException("No items currently in CharacterPath array.");
      }
    }
  }
  public class CharacterPath
  {
    public string Character { get; set; }
    public Font Font { get; set; }
    public Bitmap Image { get; set; }
    public AutoEdge EdgeDetector { get; set; }

    public CharacterPath(string Character, Font font)
    {
      this.Character = Character;
      this.Font = font;
      this.Draw();
    }

    public Bitmap Draw(bool includePoints = false)
    {
      if (this.Image != null)
        this.Image.Dispose();
      SizeF sz = GetImageSize();
      this.Image = new Bitmap((int)sz.Width, (int)sz.Height);
      using (Graphics g = Graphics.FromImage(this.Image))
      {
        g.Clear(Color.White);
        g.DrawString(this.Character, this.Font, Brushes.Black, 0, 0);
      }
      if (includePoints)
      {
        this.EdgeDetector = new AutoEdge(this.Image);
        this.Image = this.EdgeDetector.Image;
      }
      return this.Image;
    }

    private SizeF GetImageSize()
    {
      SizeF sz;
      var bmp = new Bitmap(100, 100);
      using (Graphics g = Graphics.FromImage(bmp))
      {
        sz = g.MeasureString(this.Character, this.Font);
      }
      return sz;
    }
    
    public Point[][] GetPaths()
    {
      List<Point[]> ptPaths = new List<Point[]>();
      foreach (AutoEdge.Crawler item in this.EdgeDetector.EdgeCrawler)
      {
        ptPaths.Add(item.GetPointPath());
      }
      return ptPaths.ToArray();
    }
    public Point[][] GetIncrementalPaths()
    {
      List<Point[]> ptPaths = new List<Point[]>();
      foreach (AutoEdge.Crawler item in this.EdgeDetector.EdgeCrawler)
      {
        ptPaths.Add(item.GetPointPath_Incremental(this.Image.Size));
      }
      return ptPaths.ToArray();
    }
  }

  public class AutoEdge
  {
    public Bitmap Image { get; set; }
    public List<Crawler> EdgeCrawler { get; set; }
    public AutoEdge(Bitmap bmp)
    {
      this.EdgeCrawler = new List<Crawler>();
      this.Image = bmp;
      // Find first pixel;
      bool foundPix = false;
      int x = 0;
      int y = 0;
      for (int i = 0; i < this.Image.Height; i++)
      {
        y = i;
        for (int j = 0; j < this.Image.Width; j++)
        {
          if (j > 0 && this.Image.GetPixel(j, i).GetBrightness() <= 0.5 && this.Image.GetPixel(j - 1, i).GetBrightness() >= 0.5)
          {
            x = j;
            if (!ContainedInPath(x, y))
            {
              this.EdgeCrawler.Add(new Crawler(bmp, x, y));
              do
              {
                //Console.WriteLine(this.EdgeCrawler.Current.ToString());
              } while (this.EdgeCrawler[this.EdgeCrawler.Count - 1].Move() != Crawler.Direction.Center);
            }else
            {
              //Console.WriteLine("Contained in Path already! X=" + x.ToString() + ", Y=" + y.ToString());
            }
          }
        }
      }
      List<Point> pts = new List<Point>();
      foreach (Crawler item in this.EdgeCrawler)
      {
        pts.AddRange(item.GetPointPath());
      }
      if (pts.Count> 0)
      {
        using (Graphics g = Graphics.FromImage(bmp))
        {
          g.DrawLines(new Pen(Color.ForestGreen, 2), pts.ToArray());// this.EdgeCrawler.GetPointPath());
        }
      }
    }
    private bool ContainedInPath(int x, int y)
    {
      Point tmp = new Point(x, y);
      foreach (Crawler item in this.EdgeCrawler)
      {
        Point[] pts = item.GetPointPath();
        if (pts.Contains(tmp)){
          return true;
        }
      }
      return false;
    }
    
    public double GetShortestDistance()
    {
      double val = double.MaxValue;
      foreach (Crawler item in EdgeCrawler)
      {
        Point[] cur = item.GetPointPath();
        foreach (Crawler other in EdgeCrawler)
        {
          if (!item.Equals(other))
          {
            Point[] nxt = other.GetPointPath();
            foreach (Point cpt in cur)
            {
              foreach (Point opt in nxt)
              {
                double dist = Math.Sqrt(Math.Pow(opt.X - cpt.X, 2) + Math.Pow(opt.Y - cpt.Y, 2));
                if (dist < val)
                  val = dist;
              }
            }
          }
        }
      }
      return val;
    }

    public class Crawler
    {
      private Bitmap _bmp { get; set; }
      public enum Direction { NW, N, NE, W, E, SW, S, SE, Center }
      public Dictionary<CrawlerMatrix, Direction> History { get; set; }
      public CrawlerMatrix Current { get; set; }

      public Crawler(Bitmap bmp, int startX =0, int startY=0)
      {
        this._bmp = bmp;
        this.Current = new CrawlerMatrix(this._bmp, startX, startY);
        this.History = new Dictionary<CrawlerMatrix, Direction>();
      }

      public Direction Move()
      {
        CrawlPointModel model;
        if (this.History.Count > 0)
        {
          model = new CrawlPointModel(Reverse(this.History.Last().Value));
        }else
        {
          model = new CrawlPointModel(Direction.Center);
        }
        CrawlerMatrix.CrawlerPoint[] hist = this.History.Select(item => item.Key.Matrix[Direction.Center]).ToArray();
        Direction nextDir = model.GetHighestScore(this.Current, hist);
        this.History.Add(this.Current, nextDir);
        this.Current = new CrawlerMatrix(this._bmp, this.Current.Matrix[nextDir].Point.X, this.Current.Matrix[nextDir].Point.Y);
        return nextDir;
      }

      public Direction Reverse(Direction dir)
      {
        switch (dir)
        {
          case Direction.NW:
            return Direction.SE;
          case Direction.N:
            return Direction.S;
          case Direction.NE:
            return Direction.SW;
          case Direction.W:
            return Direction.E;
          case Direction.E:
            return Direction.W;
          case Direction.SW:
            return Direction.NE;
          case Direction.S:
            return Direction.N;
          case Direction.SE:
            return Direction.NW;
          default:
            return Direction.Center;
        }
      }

      public Point[] GetPointPath()
      {
        List<Point> pts = new List<Point>();
        foreach (CrawlerMatrix item in this.History.Keys)
        {
          pts.Add(item.Matrix[Direction.Center].Point);
        }
        return pts.ToArray();
      }
      public Point[] GetPointPath_Incremental(Size ImageSize)
      {
        List<Point> pts = new List<Point>();
        Point[] regPts = GetPointPath();
        Point cur = new Point(0,ImageSize.Height);
        for (int i = 0; i < regPts.Length; i++)
        {
          Point tmp = regPts[i];
          pts.Add(new Point(tmp.X - cur.X, (tmp.Y - cur.Y)*-1));
          cur = tmp;
        }
        //pts.Add(new Point(0 - cur.X, ImageSize.Height - cur.Y));
        return CleanPointPath_Incremental(pts.ToArray());
      }
      private Point[] CleanPointPath_Incremental(Point[] pts)
      {
        int origin = pts.Length;
        int last = pts.Length;
        int cur = 0;
        do
        {
          last = pts.Length;
          pts = Clean_Dups(pts);
          pts = Clean_Thresh(pts, 0.015);
          cur = pts.Length;
        } while (cur != last);
        //Console.WriteLine("Lengths Original=" + origin.ToString() + "\tNew=" + cur.ToString());
        return  pts;
      }
      private Point[] Clean_Dups(Point[] pts)
      {
        List<Point> nwpts = new List<Point>();
        Point cur = new Point();//last
        // Clean by concatenating duplicate entries (straight lines);
        //last = pts[0];
        int conX = 0;//last.X;
        int conY = 0;//last.Y;
        for (int i = 0; i < pts.Length; i++)
        {
          if (i == 0)
          {
            cur = pts[i];
            conX = cur.X;
            conY = cur.Y;
            continue;
          }
          if (pts[i].Equals(cur))// last))
          {
            conX += pts[i].X;
            conY += pts[i].Y;
          }
          else
          {
            cur.X = conX;
            cur.Y = conY;
            nwpts.Add(cur);
            cur = pts[i];
            conX = cur.X;
            conY = cur.Y;
          }
        }
        cur.X = conX;
        cur.Y = conY;
        nwpts.Add(cur);

        return nwpts.ToArray();
      }
      private Point[] Clean_Thresh(Point[] pts, double inchThreshold)
      {
        List<Point> nwpts = new List<Point>();
        Point cur = new Point();
        // Clean by concatenating duplicate entries (straight lines);
        //int conX = 0;
        //int conY = 0;
        double ratHeight = Convert.ToDouble(Properties.Settings.Default.GCodeHeight);
        double ratWidth = (this._bmp.Width * ratHeight) / this._bmp.Height;
        double imgHeight = this._bmp.Height;
        double imgWidth = this._bmp.Width;
        for (int i = 0; i < pts.Length; i++)
        {
          double inx = (pts[i].X / imgWidth) * ratWidth;
          double iny = (pts[i].Y / imgHeight) * ratHeight;
          //Console.WriteLine("X=" + inx.ToString() + "\tY=" + iny.ToString() + "\tThreshold=" + inchThreshold.ToString());
          if ((inx == 0 && iny < inchThreshold) && (inx == 0 && iny > -inchThreshold) || (inx < inchThreshold && iny == 0) && (inx > -inchThreshold && iny == 0))
          {
            cur.X += pts[i].X;
            cur.Y += pts[i].Y;
          }
          else
          {
            if (cur.X != 0 || cur.Y != 0)
            {
              cur.X += pts[i].X;
              cur.Y += pts[i].Y;
              nwpts.Add(cur);
              cur = new Point();
            }else{
              nwpts.Add(pts[i]);
            }
          }
        }
        if (cur.X > 0 && cur.Y > 0)
          nwpts.Add(cur);

        return nwpts.ToArray();
      }


      public class CrawlPointModel
      {
        public SortedList<Direction, int> ScoreMatrix { get; set; }
        private Direction _last { get; set; }
        public CrawlPointModel(Direction last)
        {
          this._last = last;
          switch (this._last)
          {
            case Direction.NW:
              this.LoadMatrix(new int[] { 0, 0, 10, 0, 0, 30, 10, 30, 20 });
              break;
            case Direction.N:
              this.LoadMatrix(new int[] { 0, 0, 0, 0, 0, 0, 25, 50, 25 });
              break;
            case Direction.NE:
              this.LoadMatrix(new int[] { 10, 0, 0, 30, 0, 0, 20, 30, 10 });
              break;
            case Direction.W:
              this.LoadMatrix(new int[] { 0, 0, 25, 0, 0, 50, 0, 0, 25 });
              break;
            case Direction.E:
              this.LoadMatrix(new int[] { 25, 0, 0, 50, 0, 0, 25, 0, 0 });
              break;
            case Direction.SW:
              this.LoadMatrix(new int[] { 10, 30, 20, 0, 0, 30, 0, 0, 10 });
              break;
            case Direction.S:
              this.LoadMatrix(new int[] { 25, 50, 25, 0, 0, 0, 0, 0, 0 });
              break;
            case Direction.SE:
              this.LoadMatrix(new int[] { 20, 30, 10, 30, 0, 0, 10, 0, 0 });
              break;
            case Direction.Center:
              this.LoadMatrix(new int[] { 12, 12, 12, 12, 0, 12, 12, 12, 12 });
              break;
            default:
              this.LoadMatrix(new int[] { 12, 12, 12, 12, 0, 12, 12, 12, 12 });
              break;
          }
        }

        public Direction GetHighestScore(CrawlerMatrix matrix,CrawlerMatrix.CrawlerPoint[] history)
        {
          Direction curDir= Direction.Center;
          double curScore = -1;
          foreach (KeyValuePair<Direction,int> item in this.ScoreMatrix)
          {
            if (matrix.Matrix[item.Key].IsNull() == false)
            {
              if (matrix.Matrix[item.Key].Brightness == 0)
              {
                // Get adjacent cells to check for open space;
                Direction[] dirs = GetAdjacent(item.Key);
                foreach (Direction dir in dirs)
                {
                  if (matrix.Matrix[dir].Brightness == 1)
                  {
                    if (curScore < item.Value && !history.Contains(matrix.Matrix[item.Key]) && item.Key != this._last)
                    {
                      curScore = item.Value;
                      curDir = item.Key;
                    }
                  }
                }
              }
            }
          }
          if (curDir == Direction.Center)
          {
            curDir = Direction.Center;
          }
          return curDir;
        }

        public Direction[] GetAdjacent(Direction dir)
        {
          switch (dir)
          {
            case Direction.NW:
              return new Direction[] { Direction.N, Direction.W };
            case Direction.N:
              return new Direction[] { Direction.NW, Direction.NE };
            case Direction.NE:
              return new Direction[] { Direction.N, Direction.E };
            case Direction.W:
              return new Direction[] { Direction.NW, Direction.SW };
            case Direction.E:
              return new Direction[] { Direction.NE, Direction.SE };
            case Direction.SW:
              return new Direction[] { Direction.S, Direction.W };
            case Direction.S:
              return new Direction[] { Direction.SW, Direction.SE };
            case Direction.SE:
              return new Direction[] { Direction.S, Direction.E };
            default:
              return new Direction[] { Direction.N, Direction.E, Direction.S, Direction.W };
          }
        }

        private void LoadMatrix(int[] arr)
        {
          this.ScoreMatrix = new SortedList<Direction, int>();
          if (arr.Length == 9)
          {
            this.ScoreMatrix.Add(Direction.NW, arr[0]);
            this.ScoreMatrix.Add(Direction.N, arr[1]);
            this.ScoreMatrix.Add(Direction.NE, arr[2]);
            this.ScoreMatrix.Add(Direction.W, arr[3]);
            this.ScoreMatrix.Add(Direction.Center, arr[4]);
            this.ScoreMatrix.Add(Direction.E, arr[5]);
            this.ScoreMatrix.Add(Direction.SW, arr[6]);
            this.ScoreMatrix.Add(Direction.S, arr[7]);
            this.ScoreMatrix.Add(Direction.SE, arr[8]);
          }
          else
          {
            throw new Exception("Size not valid. Array should be a length of 9 to match matrix model");
          }
        }
      }


      public class CrawlerMatrix
      {
        /*
        ---------------------
        |  NW  |  N   |  NE |
        ---------------------
        |  W   |  C   |  E  |
        ---------------------
        |  SW  |  S   |  SE |
        ---------------------*/
        public SortedList<Direction, Point> DirectionGrid
        {
          get
          {
            return (new SortedList<Direction, Point> {
        { Direction.NW, new Point(-1, -1) },
        { Direction.N, new Point(0, -1) },
        { Direction.NE,  new Point(1, -1) },
        { Direction.W, new Point(-1, 0) },
        { Direction.Center, new Point(0, 0) },
        { Direction.E, new Point(1, 0) },
        { Direction.SW, new Point(-1, 1) },
        { Direction.S, new Point(0, 1) },
        { Direction.SE, new Point(1, 1) } });
          }
        }
        
        public Dictionary<Direction, CrawlerPoint> Matrix { get; set; }
        public CrawlerPoint this[Direction dir] {
          get
          {
            return this.Matrix[dir];
          }
          set
          {
            this.Matrix[dir] = value;
          }
        }

        public override string ToString()
        {
          var outstr = new StringBuilder(this.Matrix[Direction.Center].Point.ToString() + "{");
          foreach (KeyValuePair<Direction, CrawlerPoint> item in this.Matrix)
          {
            outstr.Append(item.Key.ToString() + ": " + item.Value.Brightness.ToString() + ", ");
          }
          outstr.Append("}");
          return outstr.ToString();
        }
        public CrawlerMatrix(Bitmap image, int x, int y)
        {
          this.Matrix = new Dictionary<Direction, CrawlerPoint>(9)
          {
            { Direction.NW, null },
            { Direction.N, null },
            { Direction.NE, null },
            { Direction.W, null },
            { Direction.Center, null },
            { Direction.E, null },
            { Direction.SW, null },
            { Direction.S, null },
            { Direction.SE, null }
          };

          for (int i = this.Matrix.Count - 1; i >= 0; i--)
          {
            Direction dir = this.Matrix.Keys.ToArray()[i];
            //if (((x + DirectionGrid[dir].X) >= 0) && ((x + DirectionGrid[dir].X) < image.Width) && ((y + DirectionGrid[dir].Y) >= 0) && ((y + DirectionGrid[dir].Y) < image.Height))
            //{
              this.Matrix[dir] = new CrawlerPoint(image, x + DirectionGrid[dir].X, y + DirectionGrid[dir].Y);
            //}
          }
          
        }


        public class CrawlerPoint
        {
          public Point Point { get; set; }
          public double Brightness { get; set; }
          private bool _IsNull { get; set; }

          public override bool Equals(object obj)
          {
            if (obj is CrawlerPoint)
            {
              return ((CrawlerPoint)obj).Point.Equals(this.Point);
            }else
            {
              throw new InvalidCastException("Can only compare CrawlerPoint to type CrawlerPoint");
            }
          }
          public override int GetHashCode()
          {
            return this.Point.GetHashCode();
          }
          public override string ToString()
          {
            return "{X=" + this.Point.X.ToString() + ",Y=" + this.Point.Y.ToString() + ",B=" + this.Brightness.ToString() + "}";
          }

          public CrawlerPoint(Bitmap image, int x, int y)
          {
            if (image == null)
              throw new NullReferenceException("Image cannot be null!");
            if (x >= 0 && y >= 0 && x < image.Width && y < image.Height)
            {
              this.Point = new Point(x, y);
              this.Brightness = image.GetPixel(x, y).GetBrightness();
              this._IsNull = false;
            }
            else
            {
              this.Point = new Point(-1, -1);
              this._IsNull = true;
            }
          }

          public bool IsNull()
          {
            if (this.Point != null && this.Point.X>=0 && this.Point.Y >= 0)
            {
              this._IsNull = false;
            }
            return this._IsNull;
          }
        }
        
      }

    }

  }

}
