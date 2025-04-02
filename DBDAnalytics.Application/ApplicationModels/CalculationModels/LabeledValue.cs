namespace DBDAnalytics.Application.ApplicationModels.CalculationModels
{
    public class LabeledValue
    {
        /// <summary>
        /// Отображение название элемента. 
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Значение для конвертеров. Опционально. 
        /// </summary>
        public double? ConverterValue { get; set; }

        /// <summary>
        /// Значение для отображение и расчетов.
        /// </summary>
        public double Value { get; set; }
    }
}