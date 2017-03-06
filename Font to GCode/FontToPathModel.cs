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


    public FontToPathModel(string fontFile, string[] characterList)
    {
      this.FontFamily = fontFile;
      this.Font = new Font(FontFamily, Properties.Settings.Default.FontSize);
      this.Characters = characterList;
      this._charLib = new List<CharacterPath>();
      foreach (string item in this.Characters)
      {
        this.Add(item, this.Font);
      }
    }

    public CharacterPath Add(CharacterPath cp)
    {
      if (!this.Contains(cp.Character))
      {
        this._charLib.Add(cp);
        return _charLib[_charLib.Count - 1];
      }
      else
      {
        throw new DuplicateWaitObjectException();
      }
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
    public IEnumerable<MotionPoint> MotionPath
    {
      get
      {
        return this._motionPath.ToArray();
      }
    }
    public IEnumerable<ImagePoint> ImagePoints
    {
      get
      {
        return this._imagePoints.ToArray();
      }
    }

    private List<MotionPoint> _motionPath { get; set; }
    private List<ImagePoint> _imagePoints { get; set; }

    public CharacterPath(string Character, Font font)
    {
      this.Character = Character;
      this.Font = font;
      this._imagePoints = new List<ImagePoint>();
      this._motionPath = new List<MotionPoint>();
      this.Draw();
    }

    public ImagePoint Add(ImagePoint pt)
    {
      if (!this.Contains(pt))
      {
        this._imagePoints.Add(pt);
        return this._imagePoints[this._imagePoints.Count - 1];
      }
      else
      {
        throw new DuplicateWaitObjectException();
      }
    }
    public ImagePoint Add(double x, double y)
    {
      return this.Add(new ImagePoint(x, y));
    }
    public ImagePoint Add(int x, int y, Size controlSize)
    {
      return this.Add(new ImagePoint(x, y, controlSize));
    }

    public int IndexOf(double x, double y)
    {
      for (int i = 0; i < this._imagePoints.Count; i++)
      {
        if (x == this._imagePoints[i].PercentX && y == this._imagePoints[i].PercentY)
        {
          return i;
        }
      }
      return -1;
    }
    public int IndexOf(ImagePoint pt)
    {
      return this.IndexOf(pt.PercentX, pt.PercentY);
    }
    public bool Contains(ImagePoint pt)
    {
      return this.Contains(pt.PercentX, pt.PercentY);
    }
    public bool Contains(double x, double y)
    {
      return this.IndexOf(x, y) >= 0;
    }

    public Bitmap Draw(bool includePoints = false)
    {
      if (this.Image != null)
        this.Image.Dispose();
      this.Image = new Bitmap(Properties.Settings.Default.FontSize * 2, Properties.Settings.Default.FontSize * 2, PixelFormat.Format32bppPArgb);
      using (Graphics g = Graphics.FromImage(this.Image))
      {
        g.Clear(Color.White);
        g.DrawString(Character, this.Font, Brushes.Black, 0, 0);
        if (includePoints && this._imagePoints != null && this._imagePoints.Count > 0)
        {
          ImagePoint cur = this._imagePoints[0];
          cur.DrawPoint(g, this.Image.Size);
          if (this._imagePoints.Count > 1)
          {
            for (int i = 1; i < this._imagePoints.Count; i++)
            {
              ImagePoint thisPt = this._imagePoints[i];
              thisPt.DrawPoint(g, this.Image.Size);
              DrawLine(cur, thisPt, g, this.Image.Size);
              cur = thisPt;
            }
          }
        }
      }
      return this.Image;
    }

    public void DrawLine(ImagePoint pt1, ImagePoint pt2, Graphics g, Size imageSize)
    {
      var x1 = (int)(pt1.PercentX * imageSize.Width);
      var y1 = (int)(pt1.PercentY * imageSize.Height);
      var x2 = (int)(pt2.PercentX * imageSize.Width);
      var y2 = (int)(pt2.PercentY * imageSize.Height);
      g.DrawLine(new Pen(Color.YellowGreen, 2), new Point(x1, y1), new Point(x2, y2));
    }

  }

  public class MotionPoint
  {
    public IEnumerable<AxisPoint> AxisPoints { get; set; }
    private List<AxisPoint> _axisPoints;

    public MotionPoint()
    {
      _axisPoints = new List<AxisPoint>();
    }
    public MotionPoint(AxisPoint[] axisPoints) : base()
    {
      _axisPoints.AddRange(axisPoints);
    }

  }
  public class AxisPoint
  {
    public string Name { get; set; }
    public double Value { get; set; }

    public AxisPoint()
    {
      this.Name = "";
      this.Value = 0;
    }
    public AxisPoint(string Name, double Value) : base()
    {
      this.Name = Name;
      this.Value = Value;
    }

    public static AxisPoint CreateAPoint(string name, double value)
    {
      return new AxisPoint(name, value);
    }
  }
  public class ImagePoint
  {
    public double PercentX { get; set; }
    public double PercentY { get; set; }

    public override string ToString()
    {
      return "{X: " + PercentX.ToString() + ", Y: " + PercentY.ToString() + "}";
    }

    public ImagePoint()
    {
      this.PercentX = 0;
      this.PercentY = 0;
    }
    public ImagePoint(double x, double y) : base()
    {
      this.PercentX = x;
      this.PercentY = y;
    }
    public ImagePoint(int x, int y, Size controlSize) : base()
    {
      this.PercentX = (double)x / (double)controlSize.Width;
      this.PercentY = (double)y / (double)controlSize.Height;
    }

    public MotionPoint ConvertToMotionPoint(string name, Rectangle machineCharacterBounds, Rectangle imageSize)
    {
      double macX = (double)(this.PercentX * machineCharacterBounds.Width);
      double macY = (double)(this.PercentY * machineCharacterBounds.Height);

      return new MotionPoint(new AxisPoint[] { AxisPoint.CreateAPoint("X", macX), AxisPoint.CreateAPoint("Y", macY) });
    }

  }
  public static class FontToPathMethods
  {
    public static void DrawPoint(this ImagePoint pt, Graphics g, Size imageSize)
    {
      int x = (int)(pt.PercentX * imageSize.Width);
      int y = (int)(pt.PercentY * imageSize.Height);
      g.DrawEllipse(new Pen(Color.LightBlue, 2), new Rectangle(x - 5, y - 5, 10, 10));
    }
  }

  public class AutoEdge
  {
    public Bitmap Image { get; set; }
    public AutoEdge(Bitmap bmp)
    {
      this.Image = bmp;

    }

    public class Crawler
    {
      public enum Direction { NW, N, NE, W, E, SW, S, SE, Center }
      public Dictionary<CrawlerMatrix, Direction> History { get; set; }
      public CrawlerMatrix Current { get; set; }
      

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
        private CrawlPointModel _model { get; set; }
        private LastCrawlerPoint _last { get; set; }

        public CrawlerMatrix(Bitmap image, int x, int y)
        {
          this.Matrix = new Dictionary<Direction, CrawlerPoint>(9)
          {
            {Direction.NW, null },
            {Direction.N, null },
            {Direction.NE, null },
            {Direction.W, null },
            {Direction.Center, null },
            {Direction.E, null },
            {Direction.SW, null },
            {Direction.S, null },
            {Direction.SE, null }
          };

          if (((x - 1) >= 0) && ((x + 1) < image.Width) && ((y - 1) >= 0) && ((y + 1) < image.Height))
          {
            foreach (Direction dir in this.Matrix.Keys)
            {
              this.Matrix[dir] = new CrawlerPoint(image, x + DirectionGrid[dir].X, y + DirectionGrid[dir].Y);
            }
          }
          
        }

        public Direction Move(double threshold)
        {
          // Update here;
          double curAvg = this.Matrix[Direction.Center].Brightness;
          if (this.Matrix[last] != null)
            curAvg = (curAvg + this.Matrix[last].Brightness) / 2;

          foreach (KeyValuePair<Direction,CrawlerPoint> item in this.Matrix)
          {
            
          }
          return Direction.N;
        }

        public class CrawlerPoint
        {
          public Point Point { get; set; }
          public double Brightness { get; set; }
          private bool _IsNull { get; set; }

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

        public class CrawlPointModel {
          public SortedList<Direction, int> ScoreMatrix { get; set; }

          public CrawlPointModel(Direction last)
          {
            switch (last)
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
                this.LoadMatrix(new int[] { 12, 12, 12, 0, 12, 12, 12, 12, 12 });
                break;
              default:
                this.LoadMatrix(new int[] { 12, 12, 12, 0, 12, 12, 12, 12, 12 });
                break;
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
            }else
            {
              throw new Exception("Size not valid. Array should be a length of 9 to match matrix model");
            }
          }
        }

        public struct LastCrawlerPoint
        {
          public CrawlerPoint Point { get; set; }
          public Direction MotionDirection { get; set; }

          public Direction ReverseDirection()
          {
            switch (this.MotionDirection)
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
        }
      }

    }

  }

}
