using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCost
{
    public class Common
    {

        public static async Task<double> GetLimitOf(string ItemName)
        {
            IEnumerable<Budget> source = from c in await App.budgetHelper.Getdata()
                                         where c.ItemName == ItemName
                                         select c;
            if (source.Count<Budget>() > 0)
            {
                return (double)source.FirstOrDefault<Budget>().Limit;
            }
            return -1.0;
        }

        public static async Task<IEnumerable<Voucher>> GetAllRecords()
        {
            return (from c in await App.voucherHelper.Getdata() select c);
        }

        public static async Task<IEnumerable<Voucher>> GetThisMonthAllRecords(int month, int year)
        {
            return (from c in await App.voucherHelper.Getdata()
                    where (c.DT.Month == month) && (c.DT.Year == year)
                    select c);
        }

        public static async Task<IEnumerable<Voucher>> GetThisDayAllRecords(int day, int month, int year)
        {
            return (from c in await App.voucherHelper.Getdata()
                    where (c.DT.Day == day) && (c.DT.Month == month) && (c.DT.Year == year)
                    select c);
        }

        public static async Task<IEnumerable<Voucher>> GetThisYearAllRecords(int year)
        {
            return (from c in await App.voucherHelper.Getdata()
                    where c.DT.Year == year
                    select c);
        }

        public static  async Task<double> GetThisMonthSummaryOf(string ItemName, short type)
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where ((c.DT.Month == DateTime.Now.Month) && (c.DT.Year == DateTime.Now.Year)) && (c.Type == type) && (c.Category == ItemName)
                                          select c.Money)).Sum();
        }

        public static  async Task<double> GetThisMonthSummaryOf(Category category)
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where ((c.DT.Month == DateTime.Now.Month) && (c.DT.Year == DateTime.Now.Year)) && (c.Type == category.Type) && (c.Category == category.Name)
                                          select c.Money)).Sum();
        }

        public static async Task<double> GetSummaryExpenses()
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where c.Type == 1
                                          select c.Money)).Sum();
        }

        public static async Task<double> GetThisYearSummaryExpenses()
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where ((c.DT.Year == DateTime.Now.Year)) && (c.Type == 1)
                                          select c.Money)).Sum();
        }

        public static async Task<double> GetYearSummaryExpenses(int year)
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where ((c.DT.Year == year)) && (c.Type == 1)
                                          select c.Money)).Sum();
        }

        public static async Task<double> GetThisMouthSummaryExpenses()
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where ((c.DT.Year == DateTime.Now.Year)) && ((c.DT.Month == DateTime.Now.Month)) && (c.Type == 1)
                                          select c.Money)).Sum();
        }

        public static async Task<double> GetMouthSummaryExpenses(int mouth, int year)
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where ((c.DT.Year == year)) && ((c.DT.Month == mouth)) && (c.Type == 1)
                                          select c.Money)).Sum();
        }

        public static async Task<double> GetSummaryIncome()
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where c.Type == 0
                                          select c.Money)).Sum();
        }

        public static async Task<double> GetThisYearSummaryIncome()
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where ((c.DT.Year == DateTime.Now.Year)) && (c.Type == 0)
                                          select c.Money)).Sum();
        }

        public static async Task<double> GetYearSummaryIncome(int year)
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where ((c.DT.Year == year)) && (c.Type == 0)
                                          select c.Money)).Sum();
        }

        public static  async Task<double> GetThisMouthSummaryIncome()
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where ((c.DT.Year == DateTime.Now.Year)) && ((c.DT.Month == DateTime.Now.Month)) && (c.Type == 0)
                                          select c.Money)).Sum();
        }

        public static async Task<double> GetMouthSummaryIncome(int mouth, int year)
        {
            return ((IEnumerable<double>)(from c in await App.voucherHelper.Getdata()
                                          where ((c.DT.Year == year)) && ((c.DT.Month == mouth)) && (c.Type == 0)
                                          select c.Money)).Sum();
        }

        public static async Task<IEnumerable<Voucher>> Search(DateTime? begin, DateTime? end, string keyWords)
        {
            if (keyWords == "")
            {
                return (from c in await App.voucherHelper.Getdata()
                        where c.DT >= begin && c.DT <= end
                        select c);
            }
            else
            {
                return (from c in await App.voucherHelper.Getdata()
                        where c.DT >= begin && c.DT <= end && c.Desc.IndexOf(keyWords) >= 0
                        select c);
            }
        }
    }
}
