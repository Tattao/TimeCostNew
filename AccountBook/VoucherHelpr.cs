using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCost
{
    public class VoucherHelpr
    {
        private List<Voucher> _data;

        public async void AddNew(Voucher item)
        {
            await Getdata();
            item.ID = Guid.NewGuid();
            this._data.Add(item);
        }
        public async Task<bool> LoadFromFile()
        {
            this._data = await StorageFileHelper.ReadAsync<List<Voucher>>("Voucher.dat");
            return (this._data != null);
        }

        public async void SaveToFile()
        {
            await StorageFileHelper.WriteAsync<List<Voucher>>(this._data, "Voucher.dat");
        }

        public async Task<List<Voucher>> Getdata()
        {
            if (this._data == null)
            {
                bool isExist = await LoadFromFile();
                if (!isExist)
                {
                    this._data = new List<Voucher>();
                }
            }
            return this._data;
        }

        public async void Remove(Voucher item)
        {   
            await Getdata();
            item.ID = Guid.NewGuid();
            _data.Remove(item);
        }
    }
}
