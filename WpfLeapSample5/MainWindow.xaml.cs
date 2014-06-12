using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfLeapSample5
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller leap = new Controller(); 
        DrawingAttributes touchIndicator = new DrawingAttributes(); 
        StylusPoint touchPoint;

        float windowWidth = 1920;
        float windowHeight = 1080;
        double x;
        double y;
        double tx;
        double ty; 

        int FingersCount;
        string Message;
        int Index;

        public MainWindow()
        {
            InitializeComponent();

            //AddHandler CompositionTarget.Rendering, AddressOf Update
            CompositionTarget.Rendering += Update;
            touchIndicator.Width = 20;
            touchIndicator.Height = 20;
            touchIndicator.StylusTip = StylusTip.Ellipse;
            TextBlock1.Foreground = new SolidColorBrush(Colors.White);
        }

        
        protected void Update(object sender, EventArgs e)
        {
            paintCanvas.Strokes.Clear();

            // ここで画面サイズを設定する
            windowWidth = (float)this.Width;
            windowHeight = (float)this.Height;

            // フレーム・データを取得する
            // このフレームでのLeap.FrameオブジェクトおよびLeap.InteractionBoxオブジェクトを取得する。
            Leap.Frame frame = leap.Frame();

            // leap.Frame().InteractionBox は Leap Motionで認識できる可動範囲
            InteractionBox interactionBox = leap.Frame().InteractionBox;

            // PointableListオブジェクトをループで順に処理する
            foreach (Pointable pointable in leap.Frame().Pointables)
            {
                // ここで、伸ばしてない指をスルーする
                if (pointable.IsExtended == false)
                {
                    continue;
                }


                // Leap.InteractionBox.NormalizePoint()メソッドにLeap.Pointable.StabilizedTipPositionプロパティを渡してポインターの画面上の位置を取得する。
                Leap.Vector normalizedPosition = interactionBox.NormalizePoint(pointable.StabilizedTipPosition);

                // このサイトの説明よくわかる
                // http://www.buildinsider.net/small/leapmotioncs/05
                //
                // Intersect Point を利用した座標変換
                //Leap.Vector normalizedPosition = locatedScreen.Intersect( pointable, true );
                // Projection Point を利用した座標変換
                //Leap.Vector normalizedPosition = locatedScreen.Project( pointable.TipPosition, false );

                // 画面サイズ上でのポインターの位置が「0.0」～「1.0」の間で表される。
                // これを実際の画面サイズで割り出すが、Leap Motionのスクリーン座標系の原点が左下であるため
                // Y座標は高さの値から算出した値を引くことで、左上を原点とする座標にしている。
                float tx = normalizedPosition.x * windowWidth;
                float ty = windowHeight - normalizedPosition.y * windowHeight;

                StylusPoint touchPoint = new StylusPoint(tx, ty);
                StylusPointCollection tips = new StylusPointCollection(new StylusPoint[] { touchPoint });
                Stroke touchStroke = new Stroke(tips, touchIndicator);
                paintCanvas.Strokes.Add(touchStroke);

                touchIndicator.Color = Colors.Navy;
                x = touchPoint.X;
                y = touchPoint.Y;
                FingersCount = leap.Frame().Fingers.Count; // 指の数を取得する

                // 伸びている指の数が返る
                FingersCount = leap.Frame().Fingers.Count((f) => f.IsExtended);


                // TouchDistanceプロパティが「1」であれば、TouchZoneプロパティは「ZONENONE」
                // TouchDistanceプロパティが「1」以下、かつ「0」より大きい値であれば、TouchZoneプロパティは「ZONEHOVERING」
                // TouchDistanceプロパティが「0」以下、かつ「-1」より大きい値であれば、TouchZoneプロパティは「ZONETOUCHING」

                if (pointable.TouchDistance > 0 && pointable.TouchZone != Pointable.Zone.ZONENONE)
                {
                    touchIndicator.Color = Colors.Navy;
                    x = touchPoint.X;
                    y = touchPoint.Y;

                    FingersCount = leap.Frame().Fingers.Count; // 指の数を取得する

                    // 伸びている指の数が返る
                    FingersCount = leap.Frame().Fingers.Count((f) => f.IsExtended);

                    TextBlock1.Text = "伸ばしている指の数 = " + FingersCount.ToString();

                }
                // タッチ状態
                else if (pointable.TouchDistance <= 0)
                {
                    touchIndicator.Color = Colors.Red;

                    if ((x > (double)redButton.GetValue(Canvas.LeftProperty)) && (x < (double)redButton.GetValue(Canvas.LeftProperty) + redButton.Width) && (y > (double)redButton.GetValue(Canvas.TopProperty)) && (y < (double)redButton.GetValue(Canvas.TopProperty) + redButton.Height)) {
                        Index = 1;
                    }

                    else if ((x > (double)blueButton.GetValue(Canvas.LeftProperty)) && (x < (double)blueButton.GetValue(Canvas.LeftProperty) + blueButton.Width) && (y > (double)blueButton.GetValue(Canvas.TopProperty)) && (y < (double)blueButton.GetValue(Canvas.TopProperty) + blueButton.Height)) {
                    	Index = 2;
                    }

                    else if ((x > (double)greenButton.GetValue(Canvas.LeftProperty)) && (x < (double)greenButton.GetValue(Canvas.LeftProperty) + greenButton.Width) && (y > (double)greenButton.GetValue(Canvas.TopProperty)) && (y < (double)greenButton.GetValue(Canvas.TopProperty) + greenButton.Height)) {
                    	Index = 3;
                    }

                    else if ((x > (double)yellowButton.GetValue(Canvas.LeftProperty)) && (x < (double)yellowButton.GetValue(Canvas.LeftProperty) + yellowButton.Width) && (y > (double)yellowButton.GetValue(Canvas.TopProperty)) && (y < (double)yellowButton.GetValue(Canvas.TopProperty) + yellowButton.Height)) {
                    	Index = 4;
                    }

                    else if ((x > (double)blackButton.GetValue(Canvas.LeftProperty)) && (x < (double)blackButton.GetValue(Canvas.LeftProperty) + blackButton.Width) && (y > (double)blackButton.GetValue(Canvas.TopProperty)) && (y < (double)blackButton.GetValue(Canvas.TopProperty) + blackButton.Height)) {
                    	Index = 5;
                    }
                    else
                    {
                        Index = 0; // ここでクリアする　でないと以前の状態を覚えている事になってしまう
                    }
                    if (FingersCount == 1) {
                        switch (Index) {
                            case 1:
                                //ShowArea.Background = new SolidColorBrush(Colors.Red);
                                //Message = "背景色は赤です。";

                                // クリックイベントを発生させても、ボタンを押した感じにならないな～
                                this.redButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));


                                break;
                            case 2:
                                ShowArea.Background = new SolidColorBrush(Colors.Blue);
                                Message = "背景色は青です。";
                                break;
                            case 3:
                                ShowArea.Background = new SolidColorBrush(Colors.Green);
                                Message = "背景色は緑です。";
                                break;
                            case 4:
                                ShowArea.Background = new SolidColorBrush(Colors.Gold);
                                Message = "背景色は黄です。";
                                break;
                            case 5:
                                ShowArea.Background = new SolidColorBrush(Colors.Black);
                                Message = "背景色は黒です。";
                                break;
                            default :
                                break;
                        }

                        TextBlock1.Text = Message;
                    } else if (FingersCount == 5) {

                        // ５本指で画面を白にする ... 無くてもいいと思う
                        ShowArea.Background = new SolidColorBrush(Colors.White);
                    }
                    

                }
                // タッチ対象外
                else
                {
                    touchIndicator.Color = Colors.Gold;
                }
            }
        }

        private void blackButton_Click(object sender, RoutedEventArgs e)
        {
            ShowArea.Background = new SolidColorBrush(Colors.Black);
            Message = "背景色は黒です。";
        }

        private void blueButton_Click(object sender, RoutedEventArgs e)
        {
            ShowArea.Background = new SolidColorBrush(Colors.Blue);
            Message = "背景色は青です。";

        }

        private void greenButton_Click(object sender, RoutedEventArgs e)
        {
            ShowArea.Background = new SolidColorBrush(Colors.Green);
            Message = "背景色は緑です。";
        }

        private void yellowButton_Click(object sender, RoutedEventArgs e)
        {
            ShowArea.Background = new SolidColorBrush(Colors.Gold);
            Message = "背景色は黄です。";
        }

        private void redButton_Click(object sender, RoutedEventArgs e)
        {
            ShowArea.Background = new SolidColorBrush(Colors.Red);
            Message = "背景色は赤です。";
        }

     }
}
