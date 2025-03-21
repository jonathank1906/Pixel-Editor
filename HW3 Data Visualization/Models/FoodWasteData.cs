namespace HW3_Data_Visualization.Models;

public class FoodWasteData
{
    public string Country { get; set; } = string.Empty;
    public int Year { get; set; }
    public string FoodCategory { get; set; } = string.Empty;
    public double TotalWasteTons { get; set; }
    public double EconomicLoss { get; set; }
    public double AvgWastePerCapita { get; set; }
    public double Population { get; set; }
    public double HouseholdWastePercentage { get; set; }
}