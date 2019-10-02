using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using Team7MVC.Models;
using Team7MVC.ViewModels;

namespace Team7MVC.Repositories
{
    public class AdminRepository
    {
        private static string connString;
        private SqlConnection conn;

        public AdminRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["WineDB"].ConnectionString;
            }

            conn = new SqlConnection(connString);
        }

        #region Product

        public List<Products> GetAllProducts()
        {
            List<Products> products;
            using (conn)
            {
                string sql = "select * from Products";
                products = conn.Query<Products>(sql).ToList();
            }

            return products;
        }

        public List<Products> GetAllProducts(string SortBy)
        {
            List<Products> products;
            using (conn)
            {
                string sql = $"select * from Products order by {SortBy}";
                products = conn.Query<Products>(sql).ToList();
            }

            return products;
        }

        public Products GetProduct(int id)
        {
            Products product;

            using (conn)
            {
                string sql = "select * from Products where ProductID = @ProductId";
                product = conn.QueryFirstOrDefault<Products>(sql, new { ProductId = id });
            }

            return product;
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

        public void CreateProduct(Products product)
        {
            using (conn)
            {
                string sql = @"insert into Products (ProductName,Origin,Year,Capacity,UnitPrice,Stock,Grade,Variety,Area,Picture,Introduction,CategoryID,Status)
                                values(@ProductName,@Origin,@Year,@Capacity,@UnitPrice,@Stock,@Grade,@Variety,@Area,@Picture,@Introduction,@CategoryID,@Status);";
                conn.Execute(sql, new { product.ProductName, product.Origin, product.Year, product.Capacity, product.UnitPrice, product.Stock, product.Grade, product.Variety, product.Area, product.Picture, product.Introduction, product.CategoryID, product.Status });
            }
        }

        public void UpdateProduct(Products product)
        {
            using (conn)
            {
                string sql = @"update Products
                                set ProductName = @ProductName,
                                Origin = @Origin,
                                Year = @Year,
                                Capacity = @Capacity,
                                UnitPrice = @UnitPrice,
                                Stock = @Stock,
                                Grade = @Grade,
                                Variety = @Variety,
                                Area = @Area,
                                Picture = @Picture,
                                Introduction = @Introduction,
                                CategoryID = @CategoryID,
                                Status = @Status
                                where ProductID = @ProductID";
                conn.Execute(sql, new { product.ProductName, product.Origin, product.Year, product.Capacity, product.UnitPrice, product.Stock, product.Grade, product.Variety, product.Area, product.Picture, product.Introduction, product.CategoryID, product.ProductID, product.Status });
            }
        }

        public void DeleteProduct(int Id)
        {
            using (conn)
            {
                string sql = @"delete from Products
                                where ProductID = @ProductId";
                conn.Execute(sql, new { ProductId = Id });
            }
        }
        #endregion

        #region Order
        public List<AdminOrdersViewModel> GetAllOrders()
        {
            List<AdminOrdersViewModel> orders;
            using (conn)
            {
                string sql = "select * from Orders";
                orders = conn.Query<AdminOrdersViewModel>(sql).ToList();
            }

            return orders;
        }

        public List<AdminOrdersViewModel> GetAllOrders(string SortBy)
        {
            List<AdminOrdersViewModel> orders;
            using (conn)
            {
                string sql = $"select * from Orders order by {SortBy}";
                orders = conn.Query<AdminOrdersViewModel>(sql).ToList();
            }

            return orders;
        }

        public Orders GetOrder(int id)
        {
            Orders order;

            using (conn)
            {
                string sql = "select * from Orders where OrderID = @OrderId";
                order = conn.QueryFirstOrDefault<Orders>(sql, new { OrderId = id });
            }

            return order;
        }

        public Orders GetOrderById(int id)
        {
            Orders order;

            using (conn)
            {
                string sql = "select * from Orders where OrderID = @OrderId";
                order = conn.QueryFirstOrDefault<Orders>(sql, new { OrderId = id });
            }
            return order;
        }



        public void CreateOrder(Orders order)
        {
            using (conn)
            {
                string sql = @"insert into Orders (CustomerID,OrderDate,RequiredDate,ShippedDate,ShipperID,ShipName,ShipAddress,Freight,PayWay,PayDate,ShipPhone,ShipCity,BillAddress,BillName,BillCity,BillPhone,Status)
                                values(@CustomerID,@OrderDate,@RequiredDate,@ShippedDate,@ShipperID,@ShipName,@ShipAddress,@Freight,@PayWay,@PayDate,@ShipPhone,@ShipCity,@BillAddress,@BillName,@BillCity,@BillPhone,@Status);";
                conn.Execute(sql, new { order.CustomerID,order.OrderDate, order.RequiredDate, order.ShippedDate, order.ShipperID, order.ShipName, order.ShipAddress, order.Freight, order.PayWay, order.PayDate, order.ShipPhone, order.ShipCity, order.BillAddress, order.BillName, order.BillCity, order.BillPhone, order.Status });
            }
        }

        public void UpdateOrder(Orders order)
        {
            using (conn)
            {
                string sql = @"update Orders
                                set CustomerID = @CustomerID,
                                OrderDate = @OrderDate,
                                RequiredDate = @RequiredDate,
                                ShippedDate = @ShippedDate,
                                ShipperID = @ShipperID,
                                ShipName = @ShipName,
                                ShipAddress = @ShipAddress,
                                Freight = @Freight,
                                PayWay = @PayWay,
                                PayDate = @PayDate,
                                ShipPhone = @ShipPhone,
                                ShipCity = @ShipCity,
                                BillAddress = @BillAddress,
                                BillName = @BillName,
                                BillCity = @BillCity,
                                BillPhone = @BillPhone,
                                Status = @Status
                                where OrderID = @OrderID";
                conn.Execute(sql, new { order.CustomerID, order.OrderDate, order.RequiredDate, order.ShippedDate, order.ShipperID, order.ShipName, order.ShipAddress, order.Freight, order.PayWay, order.PayDate, order.ShipPhone, order.ShipCity, order.BillAddress, order.BillName, order.BillCity, order.BillPhone, order.Status, order.OrderID });
            }
        }

        public void DeleteOrder(int Id)
        {
            using (conn)
            {
                string sql = @"delete from Orders
                                where OrderID = @OrderId";
                conn.Execute(sql, new { OrderId = Id });
            }
        }

        #endregion

        #region Customer

        public List<AdminCustomersViewModel> GetAllCustomers()
        {
            List<AdminCustomersViewModel> customers;
            using (conn)
            {
                string sql = @"select c.CustomerID, c.Account,c.CustomerName, c.Gender, c.Email,
                                c.Address, c.Phone,
                                SUM(ISNULL(od.UnitPrice * od.Quantity,0)) as TotalCost
                                from Customers as c
                                LEFT OUTER JOIN Orders as o on o.CustomerID = c.CustomerID
                                LEFT OUTER JOIN [Order Details] as od on od.OrderID = o.OrderID
                                group by c.CustomerID, c.Account, c.CustomerName, c.Gender, c.Email,
                                c.Address, c.Phone";
                customers = conn.Query<AdminCustomersViewModel>(sql).ToList();
            }

            return customers;
        }

        public List<AdminCustomersViewModel> GetAllCustomers(string SortBy)
        {
            List<AdminCustomersViewModel> customers;
            using (conn)
            {
                string sql = $"select c.CustomerID, c.Account,c.CustomerName, c.Gender, c.Email,c.Address, c.Phone,SUM(ISNULL(od.UnitPrice * od.Quantity,0)) as TotalCost from Customers as c LEFT OUTER JOIN Orders as o on o.CustomerID = c.CustomerID LEFT OUTER JOIN [Order Details] as od on od.OrderID = o.OrderID group by c.CustomerID, c.Account, c.CustomerName, c.Gender, c.Email, c.Address, c.Phone order by {SortBy}";
                customers = conn.Query<AdminCustomersViewModel>(sql).ToList();
            }

            return customers;
        }

        public Customers GetCustomer(int id)
        {
            Customers customer;

            using (conn)
            {
                string sql = "select * from Customers where CustomerID = @CustomerId";
                customer = conn.QueryFirstOrDefault<Customers>(sql, new { CustomerId = id });
            }

            return customer;
        }

        public AdminCustomersViewModel GetCustomerById(int id)
        {
            AdminCustomersViewModel customer;

            using (conn = new SqlConnection(connString))
            {
                string sql = "select * from Customers where CustomerID = @CustomerId";
                customer = conn.QueryFirstOrDefault<AdminCustomersViewModel>(sql, new { CustomerId = id });
            }
            return customer;
        }

        public void CreateCustomer(Customers customer)
        {
            using (conn)
            {
                string sql = @"insert into Customers (Account,Password,CustomerName,Gender,Birthday,Email,Address,Phone,VIP,Picture,City)
                                values(@Account,@Password,@CustomerName,@Gender,@Birthday,@Email,@Address,@Phone,@VIP,@Picture,@City);";
                conn.Execute(sql, new { customer.Account, customer.Password, customer.CustomerName, customer.Gender, customer.Birthday, customer.Email, customer.Address, customer.Phone, customer.VIP, customer.Picture, customer.City });
            }
        }

        public void UpdateCustomer(Customers customer)
        {
            using (conn)
            {
                string sql = @"update Customers
                                set Account=@Account,
                                Password=@Password,
                                CustomerName=@CustomerName,
                                Gender=@Gender,
                                Birthday=@Birthday,
                                Email=@Email,
                                Address=@Address,
                                Phone=@Phone,
                                VIP=@VIP,
                                Picture=@Picture,
                                City=@City
                                where CustomerID = @CustomerID";
                conn.Execute(sql, new { customer.Account, customer.Password, customer.CustomerName, customer.Gender, customer.Birthday, customer.Email, customer.Address, customer.Phone, customer.VIP, customer.Picture, customer.City ,customer.CustomerID });
            }
        }

        public void DeleteCustomer(int Id)
        {
            using (conn)
            {
                string sql = @"delete from Customers
                                where CustomerID = @CustomerId";
                conn.Execute(sql, new { CustomerId = Id });
            }
        }

        #endregion

        #region Message
        public List<AdminMessagesViewModel> GetAllMessages()
        {
            List<AdminMessagesViewModel> messages;
            using (conn)
            {
                string sql = "select * from Messages";
                messages = conn.Query<AdminMessagesViewModel>(sql).ToList();
            }

            return messages;
        }

        public List<AdminMessagesViewModel> GetAllMessages(string SortBy)
        {
            List<AdminMessagesViewModel> messages;
            using (conn)
            {
                string sql = $"select * from Messages order by {SortBy}";
                messages = conn.Query<AdminMessagesViewModel>(sql).ToList();
            }

            return messages;
        }

        public Messages GetMessageById(int id)
        {
            Messages message;

            using (conn)
            {
                string sql = "select * from Messages where Id = @MessageId";
                message = conn.QueryFirstOrDefault<Messages>(sql, new { MessageId = id });
            }
            return message;
        }

        #endregion

        #region DashBoard資料

        public MonthSaleViewModel GetMonthSale()
        {
            MonthSaleViewModel monthSaleViewModels = new MonthSaleViewModel();

            using (conn = new SqlConnection(connString))
            {
                decimal[] amount = new decimal[12];
                string sql = @"select ISNULL(SUM(od.Quantity * od.UnitPrice * od.Discount),0) as Amount from Orders as o
                            INNER JOIN [Order Details] as od on od.OrderID = o.OrderID
                            where MONTH(OrderDate) = @Month";

                for (int i = 1; i <= 12; i++)
                {
                    amount[i - 1] = conn.QueryFirstOrDefault<decimal>(sql, new { Month = i });
                }
                monthSaleViewModels.Amount = amount;
                monthSaleViewModels.Month = new string[] { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
            }

            return monthSaleViewModels;
        }

        public decimal GetYearSale()
        {
            decimal result = 0;
            using (conn = new SqlConnection(connString))
            {
                string sql = @"select ISNULL(SUM(od.Quantity * od.UnitPrice * od.Discount),0) as Amount from Orders as o
                                INNER JOIN [Order Details] as od on od.OrderID = o.OrderID
                                where Year(OrderDate) = @year";

                result = conn.QueryFirstOrDefault<decimal>(sql, new { Year = DateTime.Now.Year });
            }

            return result;
        }

        public int GetCustomer_Count()
        {
            int result = 0;
            using (conn = new SqlConnection(connString))
            {
                string sql = "select COUNT(*) from Customers";

                result = conn.QueryFirstOrDefault<int>(sql);
            }

            return result;
        }

        public int GetOrder_Count()
        {
            int result = 0;
            using (conn = new SqlConnection(connString))
            {
                string sql = @"select COUNT(*) from Orders
                                where MONTH(OrderDate) = @Month";

                result = conn.QueryFirstOrDefault<int>(sql, new { Month = DateTime.Now.Month });
            }

            return result; 
        }

        public HotOrigin GetOrigin_Count()
        {
            HotOrigin hotOrigin;
            using (conn = new SqlConnection(connString))
            {
                string sql = @"select top 1 p.Origin, SUM(od.Quantity) as OriginsTotal from [Order Details] as od
                                INNER JOIN Products as p on p.ProductID = od.ProductID
                                group by p.Origin
                                order by OriginsTotal desc";

                hotOrigin = conn.QueryFirstOrDefault<HotOrigin>(sql);
            }

            return hotOrigin;
        }

        #endregion

    }
}