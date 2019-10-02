﻿using Dapper;
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
    public class CustomerRepository
    {
        private static string connString;
        private readonly SqlConnection conn;

        public CustomerRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["WineDB"].ConnectionString;
            }
            conn = new SqlConnection(connString);
        }

        public Customers GetCustomersId(string userName)
        {
            Customers customers;

            using (conn)
            {
                string sql = "SELECT * FROM Customers WHERE Account = @CustomerName";
                customers = conn.QueryFirstOrDefault<Customers>(sql, new { CustomerName = userName });
            }

            return customers;
        }

        public void UpdateCustomer(Customers customers)
        {
            using (conn)
            {
                int CustomerID = 0;

                string sql = @"select CustomerID from Customers
                                where Account = @Account";

                CustomerID = conn.QueryFirstOrDefault<int>(sql, new { customers.Account });

                sql = @"update Customers
                               set CustomerName = @CustomerName,
                               Account = @Account,
                               Gender = @Gender,
                               Birthday = @Birthday,
                               Email = @Email,
                               Phone = @Phone,
                               Address = @Address,
                               VIP = @VIP 
                               where  CustomerID = @CustomerID";
                conn.Execute(sql, new { customers.CustomerName, customers.Account, customers.Gender, customers.Birthday, customers.Email, customers.Phone, customers.Address, customers.VIP, CustomerID });
            }
        }

        public void UpdatePassword(Customers customers)
        {
            using (conn)
            {
                //int Password = 0;

                string sql = @"select Password from Customers
                                where Account = @Account";

                //Password = conn.QueryFirstOrDefault<int>(sql, new { customers.Account });

                sql = @"update Customers
                        set Password =  @newPassword
                        where Password =  @Password";
                conn.Execute(sql, new { customers.Password, customers.NewPassword });
            }
        }

        public List<Orders> OrderQuery(string Account)
        {
            List<Orders> orders;
            using (conn)
            {
                string sql = @"select o.OrderID, o.OrderDate, o.PayWay, SUM(od.Quantity * od.UnitPrice * od.Discount) as TotalAmount, o.Status
                               from Orders as o
                               INNER JOIN [Order Details] as od on od.OrderID = o.OrderID
                               where o.CustomerID = (select c.CustomerID from Customers c where Account = @Account)
                               group by o.OrderID, o.OrderDate, o.PayWay, o.Status";
                orders = conn.Query<Orders>(sql, new { Account = Account }).ToList();
            }

            return orders;
        }

        public List<ShopListsViewModel> QueryOrderDetails(int id)
        {
            List<ShopListsViewModel> shopLists;

            using (conn)
            {
                string sql = @"select p.ProductID, p.Picture, p.ProductName, p.Year, p.Origin, od.UnitPrice as Price,
                                od.Quantity, (od.UnitPrice * od.Quantity * od.Discount) as TotalCost
                                from [Order Details] as od
								INNER JOIN Orders as o on o.OrderID = od.OrderID
                                INNER JOIN Products as p on p.ProductID = od.ProductID
                                where o.OrderID = @id";
                shopLists = conn.Query<ShopListsViewModel>(sql, new { id }).ToList();
            }

            return shopLists;
        }
        
        public List<ShopListsViewModel> QueryPaywayInfo(string Account)
        {
            List<ShopListsViewModel> shopLists;

            using (conn)
            {
                string sql = @"select * from CreditCard
                               where CustomerID = (select CustomerID from Customers where Account = @Account)";
                shopLists = conn.Query<ShopListsViewModel>(sql, new { Account }).ToList();
            }

            return shopLists;
        }
    }
}