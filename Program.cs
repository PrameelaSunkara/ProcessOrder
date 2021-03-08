using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessOrder
{
    public class Program
    {
        public static List<Product> Products { get; set; }
        static void Main(string[] args)
        {
            //Build Product details into List<T>
            Products = BuildProducts();
            
            Console.WriteLine("---------- Welcome to Order Processing ----------");
            
            bool ProductExists = false;
            string prodtry = "y";
            string ProductId = string.Empty;
            int Qty = 0;

            while (ProductExists == false & prodtry.ToLower() == "y")
            {
                Console.WriteLine("Enter Product ID you want to purchase:");
                ProductId = Console.ReadLine();
                Console.WriteLine("Enter Quantity you want to purchase:");
                Qty = Convert.ToInt32(Console.ReadLine());

                //Check Inventory
                ProductExists = CheckInventory(ProductId, Qty);

                if (ProductExists == false)
                {
                    Console.WriteLine("Do you want to try again? (Y/N)");
                    prodtry = Console.ReadLine();
                }
            }

            if (ProductExists)
            {
                Console.WriteLine("---------- Product Quantity you want to purchase is availiable ----------");
                string usertry = "y";
                while (usertry.ToLower() == "y")
                {
                    Console.WriteLine("Enter Credit Card Number to process Order.");
                    string CreditCardNumber = Console.ReadLine();
                    decimal amount = 0.5M;

                    //Credit Card validation
                    bool ValidCC = ValidCreditCard(CreditCardNumber);

                    //PaymentFee
                    if (ValidCC)
                    {
                        //Charge Payment Fee
                        bool chargepay = ChargePayment(CreditCardNumber, amount);
                        if (!chargepay)
                        {
                            Console.WriteLine("\nAn unexpected error while charging fee on credit card.");
                        }

                        //Send Email
                        string Department = "Shipping Department";
                        string Message = "Please proceed with shipping the product " + ProductId + " of quantity " + Qty.ToString() + " to the customer bearing Credit card # " + CreditCardNumber + ".";
                        bool EmailSent = SendEmail(Department, Message);
                        usertry = "N";
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Invalid credit card details.");
                        Console.WriteLine("Do you want to try again? (Y/N)");
                        usertry = Console.ReadLine();
                    }
                }
            }
            
        }

        public static bool SendEmail(string department, string message)
        {
            Console.WriteLine("\n---------- Email to the Shipping department ----------");
            Console.WriteLine("Hello " + department + "\n" + message + "\nThank you.");
            return true;
        }

        public static bool ValidCreditCard(string creditCardNumber)
        {
            
            if (creditCardNumber.Length >=13 & creditCardNumber.Length <= 19)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool ChargePayment(string creditCardNumber, decimal amount)
        {
            Console.WriteLine("\nA Fee of " + amount.ToString() + " has been charged to process the payment on " + creditCardNumber);
            return true;
        }

        public static List<Product> BuildProducts()
        {
            List<Product> PList = new List<Product>();
            try
            {
                PList.Add(new Product { ProductID = "P1", Qty = 10 });
                PList.Add(new Product { ProductID = "P2", Qty = 15 });
                PList.Add(new Product { ProductID = "P3", Qty = 20 });
                PList.Add(new Product { ProductID = "P4", Qty = 25 });
                PList.Add(new Product { ProductID = "P5", Qty = 30 });
            }
            catch (Exception)
            {
                throw;
            }
            return PList;
        }

        public static bool CheckInventory(string productID, int quantity)
        {
            Product avblqty = Products.FirstOrDefault(p => p.ProductID == productID);

            if (avblqty == null) return false;

            if(quantity <= avblqty.Qty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
