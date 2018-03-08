using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stock
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            var twse = GetTwse();
            var stocks = GetStocks(twse);

            Content = new ListView()
            {
                ItemsSource = stocks.Select(p => $"{p.證券名稱} {p.證券代號}" )
            };
		}

        private TwseModel GetTwse()
        {
            using (var client = new HttpClient())
            {
                var responseJson = client.GetStringAsync("http://www.twse.com.tw/exchangeReport/BWIBBU_d?response=json&selectType=ALL&date").Result;
                var responseObject = JsonConvert.DeserializeObject<TwseModel>(responseJson);

                return responseObject;
            }
        }
        
        private StockModel[] GetStocks(TwseModel twse)
        {
            var stocks = twse.Data.Select(p => new StockModel() { 證券代號 = p[0].ToString(), 證券名稱 = p[1].ToString() });

            return stocks.ToArray();
        }
        
	}

    public class StockModel
    {
        public string 證券代號 { get; set; }
        public string 證券名稱 { get; set; }
    }

    public class TwseModel
    {
        public string Stat { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string[] Fields { get; set; }
        public object[][] Data { get; set; }
        public string SelectType { get; set; }
        public string[] Notes { get; set; }
    }
}
