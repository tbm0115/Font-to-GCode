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
      public Point Previous { get; set; }
      public Point Current { get; set; }

      

    }
    public class DirectionModel
    {
      public enum Direction { NW, N, NE, W, E, SW, S, SE}
      public Dictionary<Point, Direction> History { get; set; }


      public class CrawlerMatrix
      {
        //---------------------
        //|  NW  |  N   |  NE |
        //---------------------
        //|  W   |  C   |  E  |
        //---------------------
        //|  SW  |  S   |  SE |
        //---------------------


        private Point NW = new Point(-1, -1);
        private Point N = new Point(0, -1);
        private Point NE = new Point(1, -1);
        private Point W = new Point(-1,0);
        private Point Center = new Point(0, 0);
        private Point E = new Point(1, 0);
        private Point SW = new Point(-1, 1);
        private Point S = new Point(0, 1);
        private Point SE = new Point(1, 1);

        public Dictionary<Point, CrawlerPoint> Matrix { get; set; }

        public CrawlerMatrix(Bitmap image, int x, int y)
        {
          this.Matrix = new Dictionary<Point, CrawlerPoint>(9)
          {
            {NW, null },
            {N, null },
            {NE, null },
            {W, null },
            {Center, null },
            {E, null },
            {SW, null },
            {S, null },
            {SE, null }
          };
          
          if (((x-1) >= 0) && ((x+1) < image.Width) && ((y-1) >= 0) && ((y+1) < image.Height))
          {
            foreach (Point item in this.Matrix.Keys)
            {
              this.Matrix[item] = new CrawlerPoint(image, x + item.X, y + item.Y);
            }
          }
        }

        public Direction GetDirection(double threshold)
        {
          // Update here;  
          return Direction.N;
        }

        public class CrawlerPoint
        {
          public Point Point { get; set; }
          public double Brightness { get; set; }

          public CrawlerPoint(Bitmap image, int x, int y)
          {
            if (image == null)
              throw new NullReferenceException("Image cannot be null!");
            if (x >= 0 && y >= 0 && x < image.Width && y < image.Height)
            {
              this.Point = new Point(x, y);
              this.Brightness = image.GetPixel(x, y).GetBrightness();
            }else
            {
              throw new IndexOutOfRangeException("X or Y positions are outside the bounds of the image provided!");
            }
          }
        }
      }

    }

  }

}
