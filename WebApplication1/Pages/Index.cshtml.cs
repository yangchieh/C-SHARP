using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGetProductInfo(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return Content("請輸入產品編號！");
            }

            string result;

            try
            {
                // 從 appsettings.json 取得 Oracle 連接字串
                string connectionString = _configuration.GetConnectionString("OracleDb");

                using (var conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT PRODUCT_NAME, PRICE FROM PRODUCTS WHERE PRODUCT_ID = :productId";

                    using (var cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("productId", productId));

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string productName = reader["PRODUCT_NAME"].ToString();
                                string price = reader["PRICE"].ToString();

                                result = $"<strong>產品名稱：</strong> {productName} <br />" +
                                         $"<strong>價格：</strong> {price}";
                            }
                            else
                            {
                                result = "找不到相符的產品！";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = $"查詢時發生錯誤：{ex.Message}";
            }

            return Content(result, "text/html");
        }
    }
}