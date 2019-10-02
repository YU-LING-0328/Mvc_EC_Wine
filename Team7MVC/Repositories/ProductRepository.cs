using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Team7MVC.Models;
using Team7MVC.ViewModels;

namespace Team7MVC.Repositories
{
    public class ProductRepository
    {
        private static string connString;
        private SqlConnection conn;
        private SqlConnection _conn;

        public ProductRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["WineDB"].ConnectionString;
            }

            conn = new SqlConnection(connString);
            //_conn = new SqlConnection(connString);
        }

        public List<Products> GetProducts()
        {
            List<Products> products;

            using (conn)
            {
                string sql = "select * from Products";

                products = conn.Query<Products>(sql).ToList();
            }

            return products;
        }

        public List<Products> GetNewProducts()
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                            where Year = (select top 1 Year from Products order by Year desc)";

                products = conn.Query<Products>(sql).ToList();
            }

            return products;
        }

        public List<Products> GetHotProducts()
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"with t1 (ProductId, Total)
                            as
                            (
	                            select top 6 ProductID, SUM(Quantity) as Total 
	                            from [Order Details]
	                            group by ProductID
	                            order by Total desc
                            )
                            select p.ProductID, p.ProductName, p.Origin, p.Year, p.Capacity,
		                    p.UnitPrice, p.Stock, p.Grade, p.Variety, p.Area, p.Picture, p.Introduction, p.CategoryID
                            from t1 as t
                            INNER JOIN Products as p on p.ProductID = t.ProductId";

                products = conn.Query<Products>(sql).ToList();
            }

            return products;
        }

        public List<Products> GetExpensiveProducts()
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                            where UnitPrice >= 10000
                            order by UnitPrice desc";

                products = conn.Query<Products>(sql).ToList();
            }

            return products;
        }

        public List<Products> GetProducts(string SearchStr)
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                                where ProductName LIKE @ProductName";

                products = conn.Query<Products>(sql, new { ProductName = "%" + SearchStr + "%" }).ToList();
            }

            return products;
        }

        public List<Products> GetProducts(int? Year_s, int? Year_e)
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                                where Year between @Year_s and @Year_e";

                products = conn.Query<Products>(sql, new { Year_s, Year_e }).ToList();
            }

            return products;
        }

        public List<Products> GetProducts(int? Year_s)
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                                where Year >= @Year_s";

                products = conn.Query<Products>(sql, new { Year_s }).ToList();
            }

            return products;
        }

        public List<Products> GetProducts(decimal? Price_s)
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                                where UnitPrice >= @Price_s";

                products = conn.Query<Products>(sql, new { Price_s }).ToList();
            }

            return products;
        }

        public List<Products> GetProducts(decimal? Price_s, decimal? Price_e)
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                                where UnitPrice between @Price_s and @Price_e";

                products = conn.Query<Products>(sql, new { Price_s, Price_e }).ToList();
            }

            return products;
        }

        public List<Products> GetProducts(string[] Array, int temp)
        {
            List<Products> products = new List<Products>();
            List<Products> tempList;

            using (conn)
            {
                if (temp == 1)
                {
                    foreach (var item in Array)
                    {
                        string sql = @"select * from Products
                                where Origin = @Origin";

                        tempList = conn.Query<Products>(sql, new { Origin = item }).ToList();

                        foreach (var item_t in tempList)
                        {
                            products.Add(item_t);
                        }
                    }
                }
                else
                {
                    foreach (var item in Array)
                    {
                        string sql = @"select * from Products as p
                                        INNER JOIN Categories as c on c.CategoryID = p.CategoryID
                                        where CategoryName = @Category";

                        tempList = conn.Query<Products>(sql, new { Category = item }).ToList();

                        foreach (var item_t in tempList)
                        {
                            products.Add(item_t);
                        }
                    }
                }

            }

            return products;
        }

        public Products GetProductById(int id)
        {
            Products product;

            using (conn)
            {
                string sql = "select * from Products where ProductID = @ProductId";
                product = conn.QueryFirstOrDefault<Products>(sql, new { ProductId = id });
            }

            return product;
        }

        public void CreateShoppingCartData(string Account, int ProductId, int buyQty)
        {
            int OrderQty = 0;
            using (conn)
            {
                string sql = @"SELECT s.Quantity 
                               FROM ShopLists s
                               INNER JOIN Customers c ON c.CustomerID = s.CustomerID
                               WHERE  c.Account = @Account AND s.ProductID = @ProductId";
                OrderQty = conn.QueryFirstOrDefault<int>(sql, new { Account, ProductId });

                if (OrderQty == 0)
                {
                    sql = @"insert into ShopLists (CustomerID, ProductID, Price, Quantity)
                            values((select CustomerID from Customers where Account = @Account), @ProductID, (select UnitPrice from Products where ProductID = @Product2ID),@Quantity)";
                    conn.Execute(sql, new { Account, ProductID = ProductId, Product2ID = ProductId, Quantity = buyQty });
                }
                else
                {
                    OrderQty = OrderQty + buyQty;

                    sql = @"UPDATE ShopLists 
                            SET Quantity = @OrderQty
                            FROM ShopLists s
                            INNER JOIN Customers c ON c.CustomerID = s.CustomerID
                            WHERE  c.Account = @Account AND ProductID = @ProductId";
                    conn.Execute(sql, new { OrderQty, Account, ProductId });
                }
            }
        }

        public List<ShopListsViewModel> ShopList(string CustomerID)
        {
            List<ShopListsViewModel> shopLists;


            using (conn)
            {
                string sql = @"select p.ProductID, p.Picture, p.ProductName, p.Year, p.Origin, sh.Price,
                                p.stock, sh.Quantity, (sh.Price * sh.Quantity) as TotalCost
                                from ShopLists as sh
                                INNER JOIN Products as p on p.ProductID = sh.ProductID
                                INNER JOIN Customers as c on c.CustomerID = sh.CustomerID
                                where c.Account = @CustomerID";
                shopLists = conn.Query<ShopListsViewModel>(sql, new { CustomerID }).ToList();
            }

            return shopLists;
        }

        public void DeleteShopListProduct(int CustomerID, int ProductID)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = @"delete from ShopLists
                                where CustomerID = @CustomerID and ProductID = @ProductID";

                conn.Execute(sql, new { CustomerID, ProductID });
            }
        }

        public void Payment(PaymentViewModel paymentViewModel, int CustomerID)
        {
            List<ShopLists> shopLists;
            int OrderID = 0;
            //decimal Freight = 100;
            using (conn = new SqlConnection(connString))
            {

                string sql = @"insert into Orders( CustomerID,OrderDate,ShipName,ShipCity,ShipPhone,
					         ShipperID, ShipAddress, Freight, PayWay, PayDate,BillName,BillAddress,BillCity,BillPhone)
                             values((select CustomerID from Customers where CustomerID = @CustomerID),
                             @OrderDate,@ShipName,@ShipperID,@ShipCity,@ShipPhone,@ShipAddress,@Freight, @PayWay, @PayDate,@BillName,@BillAddress,@BillCity,@BillPhone)";
                conn.Execute(sql, new
                {
                    CustomerID,
                    paymentViewModel.customerPayment.OrderDate,
                    paymentViewModel.customerPayment.ShipName,
                    paymentViewModel.customerPayment.ShipCity,
                    paymentViewModel.customerPayment.ShipPhone,
                    paymentViewModel.customerPayment.ShipperID,
                    paymentViewModel.customerPayment.ShipAddress,
                    paymentViewModel.customerPayment.Freight,
                    paymentViewModel.customerPayment.PayWay,
                    paymentViewModel.customerPayment.PayDate,
                    paymentViewModel.customerPayment.BillName,
                    paymentViewModel.customerPayment.BillAddress,
                    paymentViewModel.customerPayment.BillCity,
                    paymentViewModel.customerPayment.BillPhone
                });

                sql = @"select OrderID 
                        from Orders as o
                        INNER JOIN Customers as c on c.CustomerID = o.CustomerID
                        where o.CustomerID = @CustomerID
                        order by OrderID Desc";
                OrderID = conn.QueryFirstOrDefault<int>(sql, new { CustomerID });

                if (!string.IsNullOrEmpty(paymentViewModel.customerPayment.CreditCardNo) && !string.IsNullOrEmpty(paymentViewModel.customerPayment.CreditCardDate) && paymentViewModel.customerPayment.CreditCardCSC != null && !string.IsNullOrEmpty(paymentViewModel.customerPayment.IdentityCard))
                {
                    sql = @"insert into CreditCard(CustomerID,CreditCardNo,CreditCardDate,CreditCardCSC,IdentityCard)
                        values(@CustomerID,@CreditCardNo,@CreditCardDate,@CreditCardCSC,@IdentityCard)";
                    conn.Execute(sql, new { CustomerID, paymentViewModel.customerPayment.CreditCardNo, paymentViewModel.customerPayment.CreditCardDate, paymentViewModel.customerPayment.CreditCardCSC, paymentViewModel.customerPayment.IdentityCard });
                }

                sql = @"select sh.CustomerID, ProductID, Price, Quantity 
                        from ShopLists as sh
                        INNER JOIN Customers as c on c.CustomerID = sh.CustomerID
                        where c.CustomerID = @CustomerID";

                shopLists = conn.Query<ShopLists>(sql, new { CustomerID }).ToList();
                
                sql = @"insert into [Order Details] (OrderID, ProductID, UnitPrice, Quantity,
                        Discount)
                        values(@OrderID, @ProductID, @Price, @Quantity, 1)";
                foreach (var item in shopLists)
                {
                    conn.Execute(sql, new { OrderID, item.ProductID, item.Price, item.Quantity });
                }
            }
        }

        public int GetCustomerID(string Account)
        {
            int CustomerId;

            using (conn = new SqlConnection(connString))
            {
                string sql = @"select CustomerID 
                                from Customers
                                where Account = @Account";

                CustomerId = conn.QueryFirstOrDefault<int>(sql, new { Account });
            }

            return CustomerId;
        }
    }
}