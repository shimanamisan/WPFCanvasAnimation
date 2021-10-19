using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFCanvasAnimation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //public partial class MainWindow : Window
    //{
    //    // 円の位置を格納する変数を初期化する。
    //    int myCercleX = 0;
    //    int myCercleY = 0;

    //    public MainWindow()
    //    {
    //        InitializeComponent();
    //    }
    //    private void printPos(UIElement el)
    //    {
    //        // elで指定した要素の座標を表示します。
    //        int x = (int)Canvas.GetLeft(el);
    //        int y = (int)Canvas.GetTop(el);

    //        // ツールバーに座標を表示します。
    //        textPosX.Text = string.Format("x:{0}", x);
    //        textPosY.Text = string.Format("y:{0}", y);
    //    }

    //    private void cmdPlay_Click(object sender, RoutedEventArgs e)
    //    {
    //        myCercle_move(20, 20);
    //    }

    //    private void myCercle_move(int x, int y)
    //    {
    //        // 円の位置を格納する変数を変更する
    //        myCercleX = myCercleX + x;
    //        myCercleY = myCercleY + y;

    //        // 円の位置を変更する
    //        Canvas.SetLeft(myCercle, myCercleX);
    //        Canvas.SetTop(myCercle, myCercleY);

    //        // ツールバーに座標を表示する
    //        printPos(myCercle);
    //    }

    //    private void cmdStop_Click(object sender, RoutedEventArgs e)
    //    {
    //        // 円の位置を格納する変数を初期化する。
    //        myCercleX = 0;
    //        myCercleY = 0;
    //    }

    //}

    public partial class MainWindow : Window
    {

        private int ellipseRadius = 10;     // 円の直径（楕円の高さと幅が同じ場合は円）
        private double accelerationX = 0;   // 移動速度を初期化
        private double accelerationY = 0;

        private bool rendering = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        //private void printPos(UIElement el)
        //{
        //    // elで指定した要素の座標を表示します。
        //    int x = (int)Canvas.GetLeft(el);
        //    int y = (int)Canvas.GetTop(el);

        //    // ツールバーに座標を表示します。
        //    textPosX.Text = string.Format("x:{0}", x);
        //    textPosY.Text = string.Format("y:{0}", y);
        //}

        private void printPos(int x, int y)
        {
            // ツールバーに座標を表示します。
            textPosX.Text = string.Format("x:{0}", x);
            textPosY.Text = string.Format("y:{0}", y);
        }

        private void cmdPlay_Click(object sender, RoutedEventArgs e)
        {

            if (!rendering)
            {
                ellipses.Clear();
                myCanvas.Children.Clear();

                // 移動速度を指定
                accelerationX = 1;
                accelerationY = 0;
                // アニメーションを実行します
                CompositionTarget.Rendering += RenderFrame;
                rendering = true;
            }
        }
        private void cmdPlayDown_Click(object sender, RoutedEventArgs e)
        {
            if (!rendering)
            {
                ellipses.Clear();
                myCanvas.Children.Clear();

                // 移動速度を指定
                accelerationX = 0;
                accelerationY = 1;
                // アニメーションを実行します
                CompositionTarget.Rendering += RenderFrame;
                rendering = true;
            }

        }

        private void RenderFrame(object sender, EventArgs e)
        {
            if (ellipses.Count == 0)
            {
                // 初期化：楕円（円)を作成します。

                // キャンパスサイズを確認し、円の大きさを引いておきます。
                //int maxWidth = (int)myCanvas.ActualWidth - ellipseRadius;
                //int maxHight = (int)myCanvas.ActualHeight - ellipseRadius;

                // Create the ellipse. 楕円（円）を作成します。
                Ellipse ellipse = new Ellipse();
                ellipse.Fill = Brushes.LimeGreen;
                ellipse.Width = ellipseRadius;
                ellipse.Height = ellipseRadius;

                // Place the ellipse. 楕円（円）を座標(0,0)に配置します。
                Canvas.SetLeft(ellipse, 0);
                Canvas.SetTop(ellipse, 0);
                myCanvas.Children.Add(ellipse);

                // Track the ellipse. 楕円を追跡します。
                EllipseInfo info = new EllipseInfo(ellipse, 0, 0);
                ellipses.Add(info);

                // ツールバーに座標を表示します。
                printPos((int)Canvas.GetLeft(ellipse), (int)Canvas.GetTop(ellipse));
            }

            else
            {
                // それぞれの楕円を移動します。

                for (int i = ellipses.Count - 1; i >= 0; i--)
                {
                    EllipseInfo info = ellipses[i];
                    // y軸方向の位置を移動します。
                    double top = Canvas.GetTop(info.Ellipse);
                    Canvas.SetTop(info.Ellipse, top + 1 * info.VelocityY);
                    // x軸方向の位置を移動します。
                    double left = Canvas.GetLeft(info.Ellipse);
                    Canvas.SetLeft(info.Ellipse, left + 1 * info.VelocityX);

                    // ツールバーに座標を表示します。
                    printPos((int)Canvas.GetLeft(info.Ellipse), (int)Canvas.GetTop(info.Ellipse));

                    if (top >= (myCanvas.ActualHeight - ellipseRadius))
                    {

                        // y軸方向の終了処理、Canvasの端で終了します。
                        // 少しはみ出るのは、端数のためだと思います。
                        ellipses.Remove(info);
                    }
                    else
                    {
                        // y軸方向の移動処理
                        info.VelocityY = accelerationY; // 等速移動
                    }

                    if (left >= (myCanvas.ActualWidth - ellipseRadius))
                    {
                        // x軸方向の終了処理、Canvasサイズの端で終了します。
                        // 少しはみ出るのは、端数のためだと思います。
                        ellipses.Remove(info);
                    }
                    else
                    {
                        // x軸方向の移動処理
                        info.VelocityX = accelerationX;
                    }

                    if (ellipses.Count == 0)
                    {
                        // 全ての楕円(円)に処理が終われば終了します。
                        // アニメーションを終了します。
                        StopRendering();
                    }
                }


            }
        }

        private void cmdStop_Click(object sender, RoutedEventArgs e)
        {
            StopRendering();
        }

        private void StopRendering()
        {
            CompositionTarget.Rendering -= RenderFrame;
            rendering = false;
        }

        public class EllipseInfo
        {
            public EllipseInfo(Ellipse ellipse, double velocityY, double velocityX)
            {
                VelocityY = velocityY;
                VelocityX = velocityX;
                Ellipse = ellipse;
            }

            public Ellipse Ellipse
            {
                // 楕円の参照
                get;
                set;
            }

            public double VelocityY
            {
                // 楕円のY方向速度
                get;
                set;
            }

            public double VelocityX
            {
                // 楕円のX方向速度
                get;
                set;
            }

        }

        private List<EllipseInfo> ellipses = new List<EllipseInfo>();


    }
}
