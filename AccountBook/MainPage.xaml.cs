using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace TimeCost
{
    public sealed partial class MainPage : Page,INotifyPropertyChanged
    {
        
        private string summaryIncome;
        public string SummaryIncome
        {
            get
            {
                return summaryIncome;
            }
            set
            {
                summaryIncome = value;
                OnPropertyChanged("SummaryIncome");
            }
        }

        private string summaryExpenses;
        public string SummaryExpenses
        {
            get
            {
                return summaryExpenses;
            }
            set
            {
                summaryExpenses = value;
                OnPropertyChanged("SummaryExpenses");
            }
        }
     
        public MainPage()
        {
            this.InitializeComponent();
            _1.DataContext = this;
            Loaded += MainPage_Loaded;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: 准备此处显示的页面。
            
            // TODO: 如果您的应用程序包含多个页面，请确保
            // 通过注册以下事件来处理硬件“后退”按钮:
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed 事件。
            // 如果使用由某些模板提供的 NavigationHelper，
            // 则系统会为您处理该事件。
        }      

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //设置总收入
            SummaryIncome = (await Common.GetSummaryIncome()).ToString();
            //设置总支出
            SummaryExpenses = (await Common.GetSummaryExpenses()).ToString();

            var items = await Common.GetThisDayAllRecords(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            cvs1.Source = items;         
        }

        private void p1_AddItems_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StackPanel stackAdd = sender as StackPanel;
            RotateTransform rotate = stackAdd.RenderTransform as RotateTransform;
            DoubleAnimation anima = new DoubleAnimation();
            anima.From = 0;
            anima.To = 90;
            anima.Duration = new Duration(TimeSpan.FromSeconds(0.6));
            Storyboard.SetTarget(anima, rotate);
            Storyboard.SetTargetProperty(anima, "Angle");
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(anima);
            storyboard.Begin();

            storyboard.Completed+=add_Completed;
        }

        //完成旋转之后，进入添加页面
        private void add_Completed(object sender, object e)
        {
            Frame.Navigate(typeof(AddAccount));
        }
     
        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = await Common.GetThisDayAllRecords(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            cvs1.Source = items;
            
            Voucher voucher =cvs1.Source as Voucher;     
            if (voucher != null && voucher is Voucher)
            {
                App.voucherHelper.Remove(voucher);
            }
        }

        private async void ListView_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var items = await Common.GetThisDayAllRecords(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            cvs1.Source = items;
            Voucher voucher =cvs1.Source as Voucher;
          
            App.voucherHelper.AddNew(voucher);
            Frame.Navigate(typeof(AddAccount));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        
    }
}
