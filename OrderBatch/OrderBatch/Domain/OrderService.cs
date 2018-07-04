using OrderBatch.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBatch.Domain
{
    public class OrderService
    {
        Dictionary<string, decimal> subjectBoutiqueOrders = new Dictionary<string, decimal>();

        internal List<ComissionSummary> CalculateComission(string filePath)
        {
            var highestOrderValue = FindHighestOrderPrice(filePath);

            FillSubjectBoutiqueOrders(filePath, highestOrderValue);

            return subjectBoutiqueOrders.Select(p => new ComissionSummary
            {
                BoutiqueId = p.Key,
                TotalSubjectOrders = p.Value
            })
            .ToList();
        }

        void FillSubjectBoutiqueOrders(string filePath, decimal highestOrderValue)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                while (!streamReader.EndOfStream)
                {
                    var orderSummary = ParseOrderLine(streamReader.ReadLine(), true);

                    if (orderSummary == null) continue;

                    if (!subjectBoutiqueOrders.ContainsKey(orderSummary.BoutiqueId))
                        subjectBoutiqueOrders.Add(orderSummary.BoutiqueId, 0);

                    if (orderSummary.TotalValue < highestOrderValue)
                        subjectBoutiqueOrders[orderSummary.BoutiqueId] += orderSummary.TotalValue;
                }
            }
        }

        decimal FindHighestOrderPrice(string filePath)
        {
            decimal highestOrderValue = 0;

            using (var streamReader = new StreamReader(filePath))
            {
                while (!streamReader.EndOfStream)
                {
                    var orderSummary = ParseOrderLine(streamReader.ReadLine());

                    if (orderSummary == null) continue;

                    highestOrderValue = highestOrderValue > orderSummary.TotalValue ? highestOrderValue : orderSummary.TotalValue;
                }
            }

            return highestOrderValue;
        }

        OrderSummary ParseOrderLine(string line, bool logErrors = false)
        {
            var columns = line.Split(',');

            if (columns.Length != 3)
            {
                Log(logErrors, "Invalid line format", line);
                return null;
            }

            try
            {
                var totalOrderPrice = Decimal.Parse(columns[2], NumberStyles.Currency, CultureInfo.InvariantCulture);

                return new OrderSummary
                {
                    BoutiqueId = columns[0],
                    OrderId = columns[1],
                    TotalValue = totalOrderPrice
                };
            }
            catch (Exception)
            {
                Log(logErrors, "Invalid total order price format", line);
                return null;
            }
        }

        void Log(bool logErrors, string errorMessage, string line)
        {
            if (logErrors)
                Console.WriteLine($"{errorMessage} : {line}");
        }
    }
}
