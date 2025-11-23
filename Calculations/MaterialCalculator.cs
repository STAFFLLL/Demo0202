using NewTechnology.Data;
using System;
using System.Linq;

namespace NewTechnology.Calculations
{
    public static class MaterialCalculator
    {
        /// <summary>
        /// Расчет количества материала для производства продукции
        /// </summary>
        /// <param name="productTypeId">Идентификатор типа продукции</param>
        /// <param name="materialTypeId">Идентификатор типа материала</param>
        /// <param name="requiredProductQuantity">Требуемое количество продукции</param>
        /// <param name="productInStock">Количество продукции на складе</param>
        /// <param name="param1">Параметр продукции 1</param>
        /// <param name="param2">Параметр продукции 2</param>
        /// <returns>Количество необходимого материала или -1 при ошибке</returns>
        public static int CalculateRequiredMaterial(
            int productTypeId,
            int materialTypeId,
            int requiredProductQuantity,
            int productInStock,
            double param1,
            double param2)
        {
            try
            {
                var context = new DZ0202Entities();
                double defectPercentage = context.TypesMaterial.Where(x => x.Id == productTypeId).FirstOrDefault().PercentageLoss;
                double productTypeCoefficient = context.TypeProducts.Where(x => x.Id == productTypeId).FirstOrDefault().ProductTC;

                // Проверка входных параметров
                if (productTypeId <= 0 || materialTypeId <= 0 ||
                    requiredProductQuantity < 0 || productInStock < 0 ||
                    param1 <= 0 || param2 <= 0 ||
                    productTypeCoefficient <= 0 || defectPercentage < 0 || defectPercentage >= 100)
                {
                    return -1;
                }

                // Расчет необходимого количества продукции для производства
                int productionNeeded = requiredProductQuantity - productInStock;
                if (productionNeeded <= 0)
                {
                    return 0; // Не нужно производить
                }

                // Расчет материала на одну единицу продукции
                double materialPerUnit = param1 * param2 * productTypeCoefficient;

                // Учет брака материала
                double adjustedMaterialPerUnit = materialPerUnit / (1 - defectPercentage / 100.0);

                // Общее количество материала
                double totalMaterial = productionNeeded * adjustedMaterialPerUnit;

                // Округление в большую сторону до целого числа
                return (int)Math.Ceiling(totalMaterial);
            }
            catch
            {
                return -1;
            }
        }
    }
}