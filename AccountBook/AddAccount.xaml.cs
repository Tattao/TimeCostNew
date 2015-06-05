using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TimeCost
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddAccount : Page
    {
        public AddAccount()
        {
            this.InitializeComponent();
            AddListPickerItems();
        }

        private void AddListPickerItems()
        {
            listPickerExpenses.Items.Add("用餐");
            listPickerExpenses.Items.Add("购物");
            listPickerExpenses.Items.Add("零食");
            listPickerExpenses.Items.Add("网购");
            listPickerExpenses.Items.Add("充值");
            listPickerExpenses.Items.Add("房租");
            listPickerExpenses.Items.Add("服装");
            listPickerExpenses.Items.Add("娱乐");
            listPickerExpenses.Items.Add("餐饮");
            listPickerExpenses.Items.Add("交通");
            listPickerExpenses.Items.Add("其他");
            listPickerExpenses.SelectedIndex = 0;

            listPickerIncome.Items.Add("修电脑");
            listPickerIncome.Items.Add("打麻将");
            listPickerIncome.Items.Add("斗地主");
            listPickerIncome.Items.Add("工资");
            listPickerIncome.Items.Add("兼职");
            listPickerIncome.Items.Add("其他");
            listPickerIncome.SelectedIndex = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if(e.Parameter!=null)
            {
                if(e.Parameter.ToString()=="0")
                {
                    pivot.SelectedIndex = 0;
                }
                else
                {
                    pivot.SelectedIndex = 1;
                }
            }
        }

        private async void appbar_buttonAdd_Click(object sender, RoutedEventArgs e)
        {

            pivot.Focus(FocusState.Pointer);
            await SaveVoucher();
        }

        private async void appbar_buttonFinish_Click(object sender, RoutedEventArgs e)
        {
            if (await SaveVoucher())
            {
                Frame.GoBack();
            }
        }

        private void appbar_buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        

        private async Task<bool> SaveVoucher()
        {
            string erro = "";
            try
            {
                if (pivot.SelectedIndex == 0)
                {
                    if (this.textBox_Income.Text.Trim() == "")
                    {
                        await new MessageDialog("金额不能为空").ShowAsync();
                        return false;
                    }
                    else
                    {
                        Voucher voucher = new Voucher
                        {
                            Money = double.Parse(this.textBox_Income.Text),
                            Desc = this.textBox_IncomeDesc.Text,
                            DT = DatePickerIncome.Date.Date.Add(TimePickerIncome.Time),
                            Category = listPickerIncome.SelectedItem.ToString(),
                            Type = 0
                        };
                        App.voucherHelper.AddNew(voucher);
                    }
                }
                else
                {
                    if (this.textBox_Expenses.Text.Trim() == "")
                    {
                        await new MessageDialog("金额不能为空").ShowAsync();
                        return false;
                    }
                    else
                    {
                        Voucher voucher = new Voucher
                        {
                            Money = double.Parse(this.textBox_Expenses.Text),
                            Desc = this.textBox_ExpensesDesc.Text,
                            DT = DatePickerExpenses.Date.Date.Add(TimePickerExpenses.Time),
                            Category = listPickerExpenses.SelectedItem.ToString(),
                            Type = 1
                        };
                        App.voucherHelper.AddNew(voucher);
                    }
                }
            }
            catch (Exception ee)
            {
                erro = ee.Message;
            }
            if (erro!="")
            {
                await new MessageDialog(erro).ShowAsync();
                return false;
            }
            else
            {
                App.voucherHelper.SaveToFile();
                await new MessageDialog("保存成功").ShowAsync();
                return true;
            }    
        }
    }
}
