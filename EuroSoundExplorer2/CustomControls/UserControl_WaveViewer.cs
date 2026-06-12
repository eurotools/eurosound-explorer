using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace sb_explorer.CustomControls
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class UserControl_WaveViewer : UserControl
    {
        #region declarations
        public float PenWidth { get; set; }
        public Bitmap currentWaveImage = null;


        public Bitmap CurrentWaveImage
        {
            get
            {
                return currentWaveImage;
            }
            set
            {
                currentWaveImage = value;
            }
        }

        public int RenderDelay = 10;

        public delegate void OnLineDrawHandler(Point point1, Point point2);

        public event OnLineDrawHandler OnLineDrawEvent;

        private Dictionary<Point, Point> ControlPoints = new Dictionary<Point, Point>();

        private readonly System.ComponentModel.Container components = null;
        private RawSourceWaveStream waveStream;
        private int samplesPerPixel = 128;
        private long startPosition;
        private int bytesPerSample;
        private Thread RenderThread;
        private volatile bool cancelRender;

        #endregion declarations

        //-------------------------------------------------------------------------------------------------------------------------------
        public UserControl_WaveViewer()
        {
            //This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            //use double buffer to avoid flickering
            DoubleBuffered = true;
            PenWidth = 1;
        }

        /// <summary>
        /// Resize to screen size
        /// </summary>
        public void InitControl()
        {
            if (waveStream == null)
            {
                return;
            }

            if (bytesPerSample > 0 && Width > 0)
            {
                int samples = (int)(waveStream.Length / bytesPerSample);
                startPosition = 0;
                SamplesPerPixel = samples / Width;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            InitControl();
        }

        /// <summary>
        /// sets the associated wavestream
        /// </summary>
        public RawSourceWaveStream WaveStream
        {
            get
            {
                return waveStream;
            }
            set
            {
                waveStream = value;
                if (waveStream != null)
                {
                    bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8) * waveStream.WaveFormat.Channels;
                }
                Invalidate();
            }
        }

        /// <summary>
        /// The zoom level, in samples per pixel
        /// </summary>
        public int SamplesPerPixel
        {
            get
            {
                return samplesPerPixel;
            }
            set
            {
                samplesPerPixel = Math.Max(1, value);
                Invalidate();
            }
        }

        /// <summary>
        /// Start position (currently in bytes)
        /// </summary>
        public long StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;
            }
        }

        /// <summary>
        /// Draw grid lines
        /// </summary>
        /// <param name="gfx"></param>
        private void DrawImageGrid(Graphics gfx)
        {
            int numOfCells = 30; int cellSize = (int)(gfx.ClipBounds.Height / 4);

            using (Pen gridPen = new Pen(Color.FromArgb(-8355712), 1))
            {
                for (int y = 0; y < numOfCells; ++y)
                {
                    gfx.DrawLine(gridPen, 0, y * cellSize, numOfCells * cellSize, y * cellSize);
                }

                for (int x = 0; x < numOfCells; ++x)
                {
                    gfx.DrawLine(gridPen, x * cellSize, 0, x * cellSize, numOfCells * cellSize);
                }
            }
        }

        /// <summary>
        /// Draw lines on paint event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //draw grid before drawing the wave
            DrawImageGrid(e.Graphics);

            if (waveStream != null && bytesPerSample > 0)
            {
                int bytesRead;
                byte[] waveData = new byte[samplesPerPixel * bytesPerSample];
                waveStream.Position = startPosition + (e.ClipRectangle.Left * bytesPerSample * samplesPerPixel);
                ControlPoints.Clear();// clear points

                using (Pen linePen = new Pen(Color.DarkBlue, PenWidth))
                {
                    for (int x = e.ClipRectangle.X; x < e.ClipRectangle.Right; x += 1)
                    {
                        short low = 0;
                        short high = 0;

                        bytesRead = waveStream.Read(waveData, 0, samplesPerPixel * bytesPerSample);

                        if (bytesRead == 0) { break; }

                        for (int n = 0; n < bytesRead; n += 2)
                        {
                            short sample = BitConverter.ToInt16(waveData, n);
                            if (sample < low) { low = sample; }
                            if (sample > high) { high = sample; }
                        }

                        //calculate min and max values for the current line
                        float lowPercent = (((float)low) - short.MinValue) / ushort.MaxValue;
                        float highPercent = (((float)high) - short.MinValue) / ushort.MaxValue;

                        Point point1 = new Point(x, (int)(Height * lowPercent));
                        Point point2 = new Point(x, (int)(Height * highPercent));

                        //if event is not hooked in, then render wave instantly
                        if (OnLineDrawEvent == null)
                        {
                            e.Graphics.DrawLine(linePen, point1, point2);
                        }
                        else
                        {
                            //save points to be used in the rendering thread
                            ControlPoints.Add(point1, point2);
                        }
                    }
                }

                if (OnLineDrawEvent != null)
                {
                    StopRenderThread();
                    //start rendering thread
                    RenderThread = new Thread(() => TriggerDrawing(new List<KeyValuePair<Point, Point>>(ControlPoints)))
                    {
                        IsBackground = true
                    };
                    RenderThread.Start();
                }
            }

            base.OnPaint(e);
        }

        private void TriggerDrawing(List<KeyValuePair<Point, Point>> controlPoints)
        {
            //check if the event is attached
            if (OnLineDrawEvent != null)
            {
                foreach (KeyValuePair<Point, Point> pointSet in controlPoints)
                {
                    if (cancelRender)
                    {
                        break;
                    }

                    OnLineDrawEvent(pointSet.Key, pointSet.Value);//trigger event and pass the points to the UI
                    Thread.Sleep(RenderDelay); //set custom delay
                }
            }
        }

        //Clean resources
        public void CallExit()
        {
            StopRenderThread();
            OnLineDrawEvent = null;
            ControlPoints = null;
        }

        private void StopRenderThread()
        {
            cancelRender = true;
            if (RenderThread != null && RenderThread.IsAlive)
            {
                RenderThread.Join(100);
            }

            cancelRender = false;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopRenderThread();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
