namespace NashFridayStore.API.Extensions;

public static class NumberExtension
{
    public static decimal NormalizeRating(this decimal number)
    {
        return Math.Round(number, 1);
    }
}
