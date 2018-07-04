using OrderBatch.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBatch
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 1)
            {
                Console.WriteLine("Usage: OrderBatch <Path_to_orders_file>");
                return;
            }

            if (!IsValidInputFile(args[0]))                            
                return;            

            try
            {
                var orderService = new OrderService();

                var comissionSummary = orderService.CalculateComission(args[0]);

                foreach (var comission in comissionSummary.OrderBy(p => p.BoutiqueId))
                {
                    Console.WriteLine($"{comission.BoutiqueId},{comission.TotalComission.ToString("#.00", CultureInfo.InvariantCulture)}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Behavior: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit..");
            Console.ReadLine();
        }

        static bool IsValidInputFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
            {
                Console.WriteLine($"Invalid path: {filePath}");
                return false;
            }
                        
            if (fileInfo.Extension != ".csv")
            {
                Console.WriteLine($"Please provide a .csv file");
                return false;
            }

            return true;
        }
    }
}
